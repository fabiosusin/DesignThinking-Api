using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Business.API.General.Files.Word
{
    public class OpenXMLWord : IDisposable
    {
        private MemoryStream stream;
        private WordprocessingDocument document;
        private Func<string, byte[]> ImageHandler { get; set; }
        private const string _patternToGetImage = "__imagehandlerxplay__=";

        public OpenXMLWord(byte[] file) => LoadOpenXmlData(file);

        public void DoReplaces(Dictionary<string, string> replaces)
        {
            var documentParagraphs = document.MainDocumentPart.Document.Body.Descendants<Paragraph>().ToList();

            //parágrafo contém tags
            var taggedParagraphs = documentParagraphs.Where(q =>
            q.InnerText != null &&
            Regex.IsMatch(q.InnerText, "##", RegexOptions.IgnoreCase)).ToList();


            foreach (var paragraph in taggedParagraphs)
            {
                var tag = GetNextTag(paragraph.Descendants<Text>().ToList(), 0);

                while (tag != null)
                {
                    var matchReplaces = replaces.Where(q => Regex.IsMatch(tag.MatchText, q.Key, RegexOptions.IgnoreCase));

                    foreach (var replace in matchReplaces)
                    {
                        if (replace.Value != null && replace.Value.Contains(_patternToGetImage))
                        {
                            var parent = tag.MatchElements[0].Parent;

                            for (int j = 0; j < tag.MatchElements.Count; j++)
                                tag.MatchElements[j].Parent.RemoveChild(tag.MatchElements[j]);

                            var id = replace.Value.Split('=')[1];
                            if (ImageHandler != null)
                            {
                                var imageArray = ImageHandler(id);
                                if (imageArray != null && imageArray.Length > 0)
                                {
                                    InsertAPicture(parent, imageArray, id);
                                }
                            }
                        }
                        else
                        {
                            var replaceValue = replace.Value == null ? string.Empty : replace.Value.Replace("$", "$$");
                            tag.MatchText = Regex.Replace(tag.MatchText, replace.Key, replaceValue, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 5));

                            DoTagReplace(tag);
                        }

                    }

                    var lastTag = tag;
                    tag = GetNextTag(paragraph.Descendants<Text>().ToList(), tag.MatchIndex);

                    if (tag != null && tag.MatchIndex == lastTag.MatchIndex && tag.MatchText == lastTag.MatchText)
                    {
                        //não conseguiu substituir a última tag, então ignora ela
                        tag = GetNextTag(paragraph.Descendants<Text>().ToList(), tag.MatchIndex + 1);
                    }
                }
            }

            document.MainDocumentPart.Document.Save();
        }


        public void DoTableReplaces(List<Dictionary<string, string>> replacesList, bool replaceChildTable = false)
        {
            if (replacesList.Count == 0)
                return;

            var documentTables = document.MainDocumentPart.Document.Body.Descendants<Table>().ToList();
            if (replaceChildTable)
                documentTables = documentTables.Where(x => !(x.Parent is Body)).ToList();

            foreach (var table in documentTables)
            {
                //procura linha dos códigos
                TableRow matchRow = null;
                foreach (var replace in replacesList[0])
                {
                    matchRow = table.Descendants<TableRow>().FirstOrDefault(q => q.InnerText != null && Regex.IsMatch(q.InnerText, replace.Key, RegexOptions.IgnoreCase));
                    if (matchRow != null) break;
                }

                if (matchRow == null) continue;

                //substituir os códigos
                for (int i = 0; i < replacesList.Count; i++)
                {
                    TableRow destRow = null;

                    if (i == replacesList.Count - 1)
                        destRow = matchRow;
                    else
                    {
                        destRow = new TableRow(matchRow.OuterXml);
                        destRow = table.InsertBefore(destRow, matchRow);
                    }

                    foreach (var cell in destRow.Descendants<TableCell>())
                    {
                        var tag = GetNextTag(cell.Descendants<Text>().ToList(), 0);
                        if (tag == null)
                            continue;

                        while (tag != null)
                        {
                            var matchReplaces = replacesList[i].Where(q => Regex.IsMatch(tag.MatchText, q.Key, RegexOptions.IgnoreCase)).ToList();

                            foreach (var replace in matchReplaces)
                            {
                                if (replace.Value != null && replace.Value.Contains(_patternToGetImage))
                                {
                                    var parent = tag.MatchElements[0].Parent;

                                    for (int j = 0; j < tag.MatchElements.Count; j++)
                                        tag.MatchElements[j].Parent.RemoveChild(tag.MatchElements[j]);

                                    var id = replace.Value.Split('=')[1];
                                    if (ImageHandler != null)
                                    {
                                        var imageArray = ImageHandler(id);
                                        if (imageArray != null && imageArray.Length > 0)
                                        {
                                            InsertAPicture(parent, imageArray, id);
                                        }
                                    }
                                }
                                else
                                {
                                    var replaceValue = replace.Value == null ? string.Empty : replace.Value.Replace("$", "$$");
                                    tag.MatchText = Regex.Replace(tag.MatchText, replace.Key, replaceValue, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 5));

                                    DoTagReplace(tag);
                                }
                            }

                            var lastTag = tag;
                            tag = GetNextTag(cell.Descendants<Text>().ToList(), tag.MatchIndex);

                            if (tag != null && tag.MatchIndex == lastTag.MatchIndex && tag.MatchText == lastTag.MatchText)
                            {
                                //não conseguiu substituir a última tag, então ignora ela
                                tag = GetNextTag(cell.Descendants<Text>().ToList(), tag.MatchIndex + 1);
                            }
                        }
                    }
                }
            }
        }


        public void CloseDocument()
        {
            document.Close();
            stream?.Close();
        }

        public void SaveAs(string fileName) => File.WriteAllBytes(fileName, stream.ToArray());

        public void Dispose()
        {
            if (document != null) document.Dispose();
            if (stream != null) stream.Dispose();
        }

        private bool LoadOpenXmlData(byte[] bytes)
        {
            stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);

            document = WordprocessingDocument.Open(stream, true, new OpenSettings() { AutoSave = true });

            return true;
        }

        private void InsertAPicture(OpenXmlElement parent, byte[] imageData, string dataId)
        {
            var mainPart = document.MainDocumentPart;
            var imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            using (var writer = new BinaryWriter(imagePart.GetStream()))
            {
                writer.Write(imageData);
                writer.Flush();
            }

            AddImageToElement(parent, mainPart.GetIdOfPart(imagePart), dataId);
        }

        private static void AddImageToElement(OpenXmlElement parent, string relationshipId, string dataId)
        {
            //parent.ExtendedAttributes.

            var element = new Drawing(
                //new DW.Inline(new DW.Extent() { Cx = 990000L, Cy = 792000L },
                new DW.Inline(new DW.Extent() { Cx = 600000L, Cy = 600000L },
                new DW.EffectExtent()
                {
                    LeftEdge = 0L,
                    TopEdge = 0L,
                    RightEdge = 0L,
                    BottomEdge = 0L
                },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Foto_" + dataId
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(new PIC.NonVisualDrawingProperties()
                                     {
                                         Id = (UInt32Value)0U,
                                         Name = "Foto_" + dataId + ".jpg"
                                     }, new PIC.NonVisualPictureDrawingProperties()), // Fecha NonVisualPictureProperties
                                     new PIC.BlipFill(new A.Blip(new A.BlipExtensionList(new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" }))
                                     {
                                         Embed = relationshipId,
                                         CompressionState = A.BlipCompressionValues.Print
                                     },
                                         new A.Stretch(new A.FillRectangle())), // Fecha BlipFill
                                         new PIC.ShapeProperties(new A.Transform2D(new A.Offset() { X = 0L, Y = 0L }, new A.Extents() { Cx = 600000L, Cy = 600000L }),
                                            new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }) // Fecha ShapeProperties
                                 ) // Fecha Picture
                             ) // Fecha GraphicData
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                     ) // Fecha Graphic
                     )
                {
                    DistanceFromTop = 0U,
                    DistanceFromBottom = 0U,
                    DistanceFromLeft = 0U,
                    DistanceFromRight = 0U
                });

            //Append the reference to body. Parent should be a run element.
            if (parent is Run)
            {
                parent.AppendChild(element);
            }
            else if (parent is Paragraph)
            {
                var aux = new Run(element);
                parent.AppendChild(aux);
            }
            else
            {
                var aux = new Paragraph(new Run(element));
                parent.AppendChild(aux);
            }
        }

        private static Match GetNextTag(List<Text> elementTexts, int currentIndex)
        {
            //caminhamento em Ordem
            var textList = new List<Text>();

            for (int i = currentIndex; i < elementTexts.Count; i++)
            {
                textList.Add(elementTexts[i]);

                var joinedText = String.Join("", textList.Select(q => q.InnerText));

                if (Regex.IsMatch(joinedText, "#", RegexOptions.IgnoreCase))
                {
                    if (Regex.Matches(joinedText, "##", RegexOptions.IgnoreCase).Count > 1)
                        return new Match() { MatchIndex = i - (textList.Count - 1), MatchText = joinedText, MatchElements = new List<Text>(textList.ToArray()) };
                }
                else
                    textList.Clear();
            }

            return null;
        }

        private static void DoTagReplace(Match tag)
        {
            if (tag == null || tag.MatchElements == null)
                return;

            //agrupa tudo no Primeiro elemento
            for (int j = 1; j < tag.MatchElements.Count; j++)
            {
                if (tag.MatchElements[j].Parent != null)
                    tag.MatchElements[j].Parent.RemoveChild(tag.MatchElements[j]);
            }

            if (tag.MatchText.Contains(Environment.NewLine) || tag.MatchText.Contains("\r\n") || tag.MatchText.Contains("\n"))
            {
                //quebras de linha
                var delimiters = new String[] { Environment.NewLine, "\r\n", "\n" };
                var parts = tag.MatchText.Split(delimiters, StringSplitOptions.None);

                tag.MatchElements[0].Text = parts[0];
                OpenXmlElement lastElement = tag.MatchElements[0];

                for (int i = 1; i < parts.Length; i++)
                {
                    var breakElement = new Break();
                    lastElement.Parent.InsertAfter(breakElement, lastElement);
                    lastElement = breakElement;

                    var textElement = new Text(parts[i]);
                    lastElement.Parent.InsertAfter(textElement, lastElement);
                    lastElement = textElement;
                }
            }
            else
                tag.MatchElements[0].Text = tag.MatchText;
        }

        private class Match
        {
            public int MatchIndex { get; set; }
            public string MatchText { get; set; }
            public List<Text> MatchElements { get; set; }
        }
    }
}