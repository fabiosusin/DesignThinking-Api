using System.Xml.Serialization;

namespace DTO.Hub.Integration.NFSe.Output
{
    [XmlRoot(ElementName = "confirmaLote")]
    public class SendNfseOutput
    {
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "cLote")]
        public string Lot { get; set; }

        [XmlElement(ElementName = "dhRecbto")]
        public string DhRecbto { get; set; }
        
        [XmlElement(ElementName = "sit")]
        public string Situation { get; set; }

        [XmlElement(ElementName = "mot")]
        public string Reason { get; set; }
    }
}
