using System.Xml.Serialization;

namespace DTO.Hub.Integration.NFSe.Output
{

	[XmlRoot(ElementName = "resPedidoLoteNFSePNG")]
	public class GetNfseImageOutput
    {
		[XmlElement(ElementName = "CNPJ")]
		public string CNPJ { get; set; }

		[XmlElement(ElementName = "dhRecbto")]
		public string DhRecbto { get; set; }

		[XmlElement(ElementName = "NFS-ePNG")]
		public string NFSePNG { get; set; }

		[XmlElement(ElementName = "numeroPaginas")]
		public string NumeroPaginas { get; set; }

		[XmlElement(ElementName = "sit")]
		public string Situation { get; set; }

		[XmlElement(ElementName = "mot")]
		public string Mot { get; set; }

		[XmlAttribute(AttributeName = "versao")]
		public string Versao { get; set; }
    }
}