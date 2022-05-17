using System.Xml.Serialization;

namespace DTO.Hub.Integration.NFSe.Output
{

    [XmlRoot(ElementName = "resPedidoNFSe")]
    public class GetNfseDetailsOutput
    {
        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "chvAcessoNFS-e")]
        public string AccessKey { get; set; }

        [XmlElement(ElementName = "dhRecbto")]
        public string ReceiptDate { get; set; }

        [XmlElement(ElementName = "NFS-e")]
        public NFSeDetailsOutput Nfse { get; set; }

        [XmlElement(ElementName = "sit")]
        public string Situation { get; set; }

        [XmlElement(ElementName = "mot")]
        public string Reason { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "NFS-e")]
    public class NFSeDetailsOutput
    {
        [XmlElement(ElementName = "infNFSe")]
        public NfseDetailsInfNFSe InfNFSe { get; set; }
    }

	[XmlRoot(ElementName = "Id")]
	public class NfseDetailsId
	{
		[XmlElement(ElementName = "cNFS-e")]
		public string CNFSe { get; set; }

		[XmlElement(ElementName = "mod")]
		public string Mod { get; set; }

		[XmlElement(ElementName = "serie")]
		public string Serie { get; set; }

		[XmlElement(ElementName = "nNFS-e")]
		public string NNFSe { get; set; }

		[XmlElement(ElementName = "dEmi")]
		public string DEmi { get; set; }

		[XmlElement(ElementName = "hEmi")]
		public string HEmi { get; set; }

		[XmlElement(ElementName = "tpNF")]
		public string TpNF { get; set; }

		[XmlElement(ElementName = "refNF")]
		public string RefNF { get; set; }

		[XmlElement(ElementName = "tpEmis")]
		public string TpEmis { get; set; }

		[XmlElement(ElementName = "cancelada")]
		public string Cancelada { get; set; }

		[XmlElement(ElementName = "canhoto")]
		public string Canhoto { get; set; }

		[XmlElement(ElementName = "ambienteEmi")]
		public string AmbienteEmi { get; set; }

		[XmlElement(ElementName = "formaEmi")]
		public string FormaEmi { get; set; }

		[XmlElement(ElementName = "empreitadaGlobal")]
		public string EmpreitadaGlobal { get; set; }
	}

	[XmlRoot(ElementName = "end")]
	public class NfseDetailsEnd
	{
		[XmlElement(ElementName = "xLgr")]
		public string XLgr { get; set; }

		[XmlElement(ElementName = "nro")]
		public string Nro { get; set; }

		[XmlElement(ElementName = "xBairro")]
		public string XBairro { get; set; }

		[XmlElement(ElementName = "cMun")]
		public string CMun { get; set; }

		[XmlElement(ElementName = "xMun")]
		public string XMun { get; set; }

		[XmlElement(ElementName = "UF")]
		public string UF { get; set; }

		[XmlElement(ElementName = "CEP")]
		public string CEP { get; set; }

		[XmlElement(ElementName = "cPais")]
		public string CPais { get; set; }

		[XmlElement(ElementName = "xPais")]
		public string XPais { get; set; }
	}

	[XmlRoot(ElementName = "prest")]
	public class NfseDetailsPrest
	{
		[XmlElement(ElementName = "CNPJ")]
		public string CNPJ { get; set; }

		[XmlElement(ElementName = "xNome")]
		public string XNome { get; set; }

		[XmlElement(ElementName = "IM")]
		public string IM { get; set; }

		[XmlElement(ElementName = "xEmail")]
		public string XEmail { get; set; }

		[XmlElement(ElementName = "xSite")]
		public string XSite { get; set; }

		[XmlElement(ElementName = "end")]
		public NfseDetailsEnd End { get; set; }

		[XmlElement(ElementName = "regimeTrib")]
		public string RegimeTrib { get; set; }
	}

	[XmlRoot(ElementName = "ender")]
	public class NfseDetailsEnder
	{
		[XmlElement(ElementName = "xLgr")]
		public string XLgr { get; set; }

		[XmlElement(ElementName = "nro")]
		public string Nro { get; set; }

		[XmlElement(ElementName = "xBairro")]
		public string XBairro { get; set; }

		[XmlElement(ElementName = "cMun")]
		public string CMun { get; set; }

		[XmlElement(ElementName = "xMun")]
		public string XMun { get; set; }

		[XmlElement(ElementName = "UF")]
		public string UF { get; set; }

		[XmlElement(ElementName = "CEP")]
		public string CEP { get; set; }

		[XmlElement(ElementName = "cPais")]
		public string CPais { get; set; }

		[XmlElement(ElementName = "xPais")]
		public string XPais { get; set; }
	}

	[XmlRoot(ElementName = "TomS")]
	public class NfseDetailsTomS
	{
		[XmlElement(ElementName = "CNPJ")]
		public string CNPJ { get; set; }

		[XmlElement(ElementName = "xNome")]
		public string XNome { get; set; }

		[XmlElement(ElementName = "ender")]
		public NfseDetailsEnder Ender { get; set; }
	}

	[XmlRoot(ElementName = "serv")]
	public class NfseDetailsServ
	{
		[XmlElement(ElementName = "cServ")]
		public string CServ { get; set; }

		[XmlElement(ElementName = "cLCServ")]
		public string CLCServ { get; set; }

		[XmlElement(ElementName = "xServ")]
		public string XServ { get; set; }

		[XmlElement(ElementName = "localTributacao")]
		public string LocalTributacao { get; set; }

		[XmlElement(ElementName = "localVerifResServ")]
		public string LocalVerifResServ { get; set; }

		[XmlElement(ElementName = "uTrib")]
		public string UTrib { get; set; }

		[XmlElement(ElementName = "qTrib")]
		public string QTrib { get; set; }

		[XmlElement(ElementName = "vUnit")]
		public string VUnit { get; set; }

		[XmlElement(ElementName = "vServ")]
		public string VServ { get; set; }

		[XmlElement(ElementName = "vDesc")]
		public string VDesc { get; set; }

		[XmlElement(ElementName = "vDed")]
		public string VDed { get; set; }

		[XmlElement(ElementName = "vBCISS")]
		public string VBCISS { get; set; }

		[XmlElement(ElementName = "pISS")]
		public string PISS { get; set; }

		[XmlElement(ElementName = "vISS")]
		public string VISS { get; set; }

		[XmlElement(ElementName = "vBCINSS")]
		public string VBCINSS { get; set; }

		[XmlElement(ElementName = "pRetINSS")]
		public string PRetINSS { get; set; }

		[XmlElement(ElementName = "vRetINSS")]
		public string VRetINSS { get; set; }

		[XmlElement(ElementName = "vRed")]
		public string VRed { get; set; }

		[XmlElement(ElementName = "vBCRetIR")]
		public string VBCRetIR { get; set; }

		[XmlElement(ElementName = "pRetIR")]
		public string PRetIR { get; set; }

		[XmlElement(ElementName = "vRetIR")]
		public string VRetIR { get; set; }

		[XmlElement(ElementName = "vBCCOFINS")]
		public string VBCCOFINS { get; set; }

		[XmlElement(ElementName = "pRetCOFINS")]
		public string PRetCOFINS { get; set; }

		[XmlElement(ElementName = "vRetCOFINS")]
		public string VRetCOFINS { get; set; }

		[XmlElement(ElementName = "vBCCSLL")]
		public string VBCCSLL { get; set; }

		[XmlElement(ElementName = "pRetCSLL")]
		public string PRetCSLL { get; set; }

		[XmlElement(ElementName = "vRetCSLL")]
		public string VRetCSLL { get; set; }

		[XmlElement(ElementName = "vBCPISPASEP")]
		public string VBCPISPASEP { get; set; }

		[XmlElement(ElementName = "pRetPISPASEP")]
		public string PRetPISPASEP { get; set; }

		[XmlElement(ElementName = "vRetPISPASEP")]
		public string VRetPISPASEP { get; set; }

		[XmlElement(ElementName = "totalAproxTribServ")]
		public string TotalAproxTribServ { get; set; }
	}

	[XmlRoot(ElementName = "det")]
	public class NfseDetailsDet
	{
		[XmlElement(ElementName = "nItem")]
		public string NItem { get; set; }

		[XmlElement(ElementName = "serv")]
		public NfseDetailsServ Serv { get; set; }
	}

	[XmlRoot(ElementName = "Ret")]
	public class NfseDetailsRet
	{
		[XmlElement(ElementName = "vRetIR")]
		public string VRetIR { get; set; }

		[XmlElement(ElementName = "vRetPISPASEP")]
		public string VRetPISPASEP { get; set; }

		[XmlElement(ElementName = "vRetCOFINS")]
		public string VRetCOFINS { get; set; }

		[XmlElement(ElementName = "vRetCSLL")]
		public string VRetCSLL { get; set; }

		[XmlElement(ElementName = "vRetINSS")]
		public string VRetINSS { get; set; }
	}

	[XmlRoot(ElementName = "ISS")]
	public class NfseDetailsISS
	{
		[XmlElement(ElementName = "vBCISS")]
		public string VBCISS { get; set; }

		[XmlElement(ElementName = "vISS")]
		public string VISS { get; set; }
	}

	[XmlRoot(ElementName = "total")]
	public class NfseDetailsTotal
	{
		[XmlElement(ElementName = "vServ")]
		public string VServ { get; set; }

		[XmlElement(ElementName = "vRedBCCivil")]
		public string VRedBCCivil { get; set; }

		[XmlElement(ElementName = "vDesc")]
		public string VDesc { get; set; }

		[XmlElement(ElementName = "vtDed")]
		public string VtDed { get; set; }

		[XmlElement(ElementName = "vtNF")]
		public string VtNF { get; set; }

		[XmlElement(ElementName = "vtLiq")]
		public string VtLiq { get; set; }

		[XmlElement(ElementName = "totalAproxTrib")]
		public string TotalAproxTrib { get; set; }

		[XmlElement(ElementName = "Ret")]
		public NfseDetailsRet Ret { get; set; }

		[XmlElement(ElementName = "vtLiqFaturas")]
		public string VtLiqFaturas { get; set; }

		[XmlElement(ElementName = "vtDespesas")]
		public string VtDespesas { get; set; }

		[XmlElement(ElementName = "ISS")]
		public NfseDetailsISS ISS { get; set; }

	}

	[XmlRoot(ElementName = "infNFSe")]
	public class NfseDetailsInfNFSe
	{
		[XmlElement(ElementName = "Id")]
		public NfseDetailsId Id { get; set; }

		[XmlElement(ElementName = "prest")]
		public NfseDetailsPrest Prest { get; set; }

		[XmlElement(ElementName = "TomS")]
		public NfseDetailsTomS TomS { get; set; }

		[XmlElement(ElementName = "det")]
		public NfseDetailsDet Det { get; set; }

		[XmlElement(ElementName = "total")]
		public NfseDetailsTotal Total { get; set; }

		[XmlElement(ElementName = "infAdicLT")]
		public string InfAdicLT { get; set; }

		[XmlElement(ElementName = "infAdicES")]
		public string InfAdicES { get; set; }

		[XmlElement(ElementName = "infAdicAT")]
		public string InfAdicAT { get; set; }

		[XmlAttribute(AttributeName = "versao")]
		public string Versao { get; set; }
	}
}