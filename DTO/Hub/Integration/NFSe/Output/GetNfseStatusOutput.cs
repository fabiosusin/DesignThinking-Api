using System.Xml.Serialization;

namespace DTO.Hub.Integration.NFSe.Output
{
    [XmlRoot(ElementName = "resultadoLote")]
    public class GetNfseStatusOutput
    {
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "dhRecbto")]
        public string DhRecbto { get; set; }

        [XmlElement(ElementName = "sit")]
        public string Situation { get; set; }

        [XmlElement(ElementName = "NFSe")]
        public CaxiasDoSulNFSeStatusOutput Nfse { get; set; }
    }

    [XmlRoot(ElementName = "NFSe")]
    public class CaxiasDoSulNFSeStatusOutput
    {
        [XmlElement(ElementName = "chvAcessoNFSe")]
        public string AccessKey { get; set; }
        [XmlElement(ElementName = "sit")]
        public string Sit { get; set; }
        [XmlElement(ElementName = "motivos")]
        public CaxiasDoSulReasonsOutput Reasons { get; set; }
    }

    [XmlRoot(ElementName = "motivos")]
    public class CaxiasDoSulReasonsOutput
    {
        [XmlElement(ElementName = "mot")]
        public string Mot { get; set; }
    }
}
