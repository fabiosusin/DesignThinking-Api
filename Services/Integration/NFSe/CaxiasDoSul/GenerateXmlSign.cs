using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Services.Integration.NFSe.CaxiasDoSul
{
    internal class GenerateXmlSign
    {
        public static string SignXML(X509Certificate2 X509Cert, string xmlString)
        {
            if (X509Cert == null)
                return xmlString;

            var xmlDoc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            xmlDoc.LoadXml(xmlString);

            var reference = new Reference
            {
                Uri = "",
                DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1"
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform
            {
                Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315"
            });

            var kiData = new KeyInfoX509Data(X509Cert);
            kiData.AddSubjectName(X509Cert.Subject);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(kiData);

            var signedXml = new SignedXml(xmlDoc)
            {
                SigningKey = X509Cert.PrivateKey,
                KeyInfo = keyInfo
            };
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            var xmlDigitalSignature = signedXml.GetXml();
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            return xmlDoc.OuterXml;
        }
    }
}
