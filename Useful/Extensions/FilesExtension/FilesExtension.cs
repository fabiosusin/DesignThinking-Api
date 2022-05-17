﻿using System.IO;

namespace Useful.Extensions.FilesExtension
{
    public static class FilesExtension
    {
        public static byte[] GetByteFromFile(string path)
        {
            byte[] bytes;
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }
            catch
            {
                bytes = File.ReadAllBytes(path);
            }

            try
            {
                File.Delete(path);
            }
            catch { }

            return bytes;
        }

        public static string GetContentType(string fileExtension) => !fileExtension.StartsWith(".") ? "." + fileExtension : fileExtension switch
        {
            ".323" => "text/h323",
            ".3g2" => "video/3gpp2",
            ".3gp" => "video/3gpp",
            ".3gp2" => "video/3gpp2",
            ".3gpp" => "video/3gpp",
            ".aac" => "audio/aac",
            ".ac3" => "audio/ac3",
            ".accda" => "application/msaccess",
            ".accdb" => "application/msaccess",
            ".accdc" => "application/msaccess",
            ".accde" => "application/msaccess",
            ".accdr" => "application/msaccess",
            ".accdt" => "application/msaccess",
            ".acrobatsecuritysettings" => "application/vnd.adobe.acrobat-security-settings",
            ".AddIn" => "text/xml",
            ".ade" => "application/msaccess",
            ".adp" => "application/msaccess",
            ".adts" => "audio/aac",
            ".ai" => "application/postscript",
            ".aif" => "audio/aiff",
            ".aifc" => "audio/aiff",
            ".aiff" => "audio/aiff",
            ".air" => "application/vnd.adobe.air-application-installer-package+zip",
            ".amc" => "application/x-mpeg",
            ".application" => "application/x-ms-application",
            ".asa" => "application/xml",
            ".asax" => "application/xml",
            ".ascx" => "application/xml",
            ".asf" => "video/x-ms-asf",
            ".ashx" => "application/xml",
            ".asm" => "text/plain",
            ".asmx" => "application/xml",
            ".aspx" => "application/xml",
            ".asx" => "video/x-ms-asf",
            ".au" => "audio/basic",
            ".avi" => "video/avi",
            ".bcf" => "application/vnd.belarc-cf",
            ".bci" => "application/vnd.belarc-bci",
            ".blogthis" => "application/x-blogthis",
            ".bmp" => "image/bmp",
            ".c" => "text/plain",
            ".caf" => "audio/x-caf",
            ".cat" => "application/vnd.ms-pki.seccat",
            ".cc" => "text/plain",
            ".cd" => "text/plain",
            ".cdda" => "audio/aiff",
            ".cer" => "application/x-x509-ca-cert",
            ".cod" => "text/plain",
            ".config" => "application/xml",
            ".coverage" => "application/xml",
            ".cpp" => "text/plain",
            ".crl" => "application/pkix-crl",
            ".crt" => "application/x-x509-ca-cert",
            ".cs" => "text/plain",
            ".csdproj" => "text/plain",
            ".csproj" => "text/plain",
            ".csv" => "application/vnd.ms-excel",
            ".cur" => "text/plain",
            ".cxx" => "text/plain",
            ".datasource" => "application/xml",
            ".dcr" => "application/x-director",
            ".def" => "text/plain",
            ".der" => "application/x-x509-ca-cert",
            ".dib" => "image/bmp",
            ".dif" => "video/x-dv",
            ".dir" => "application/x-director",
            ".divx" => "ICM.DIV6",
            ".dll" => "application/x-msdownload",
            ".dlm" => "text/dlm",
            ".doc" => "application/msword",
            ".docm" => "application/vnd.ms-word.document.macroEnabled.12",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".dot" => "application/msword",
            ".dotm" => "application/vnd.ms-word.template.macroEnabled.12",
            ".dotx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.template",
            ".dsp" => "text/plain",
            ".dsw" => "text/plain",
            ".dtd" => "application/xml-dtd",
            ".dtsConfig" => "text/xml",
            ".dv" => "video/x-dv",
            ".dwfx" => "model/vnd.dwfx+xps",
            ".dxr" => "application/x-director",
            ".eml" => "message/rfc822",
            ".eps" => "application/postscript",
            ".EVR" => "audio/evrc-qcp",
            ".EVRC" => "audio/evrc-qcp",
            ".exe" => "application/x-msdownload",
            ".fdf" => "application/vnd.fdf",
            ".fif" => "application/fractals",
            ".flv" => "video/x-flv",
            ".gif" => "image/gif",
            ".gsm" => "audio/x-gsm",
            ".gz" => "application/x-gzip",
            ".h" => "text/plain",
            ".hpp" => "text/plain",
            ".hqx" => "application/mac-binhex40",
            ".hta" => "application/hta",
            ".htm" => "text/html",
            ".html" => "text/html",
            ".hxa" => "application/xml",
            ".hxc" => "application/xml",
            ".hxd" => "application/octet-stream",
            ".hxe" => "application/xml",
            ".hxf" => "application/xml",
            ".hxh" => "application/octet-stream",
            ".hxi" => "application/octet-stream",
            ".hxk" => "application/xml",
            ".hxq" => "application/octet-stream",
            ".hxr" => "application/octet-stream",
            ".hxs" => "application/octet-stream",
            ".hxt" => "application/xml",
            ".hxv" => "application/xml",
            ".hxw" => "application/octet-stream",
            ".hxx" => "text/plain",
            ".i" => "text/plain",
            ".ico" => "image/x-icon",
            ".idl" => "text/plain",
            ".iii" => "application/x-iphone",
            ".inc" => "text/plain",
            ".infopathxml" => "application/ms-infopath.xml",
            ".inl" => "text/plain",
            ".ins" => "application/x-internet-signup",
            ".ipproj" => "text/plain",
            ".iqy" => "text/x-ms-iqy",
            ".ismv" => "video/ismv",
            ".isp" => "application/x-internet-signup",
            ".jfif" => "image/jpeg",
            ".jnlp" => "application/x-java-jnlp-file",
            ".jpe" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".jpg" => "image/jpeg",
            ".jtx" => "application/x-jtx+xps",
            ".khub" => "application/x-khuboffline",
            ".latex" => "application/x-latex",
            ".lst" => "text/plain",
            ".m1v" => "video/mpeg",
            ".m2v" => "video/mpeg",
            ".m3u" => "audio/x-mpegurl",
            ".m4a" => "audio/x-m4a",
            ".m4b" => "audio/x-m4b",
            ".m4p" => "audio/x-m4p",
            ".m4v" => "video/x-m4v",
            ".mac" => "image/x-macpaint",
            ".mak" => "text/plain",
            ".man" => "application/x-troff-man",
            ".map" => "text/plain",
            ".master" => "application/xml",
            ".mda" => "application/msaccess",
            ".mdb" => "application/msaccess",
            ".mde" => "application/msaccess",
            ".mdp" => "text/plain",
            ".mfp" => "application/x-shockwave-flash",
            ".mht" => "message/rfc822",
            ".mhtml" => "message/rfc822",
            ".mid" => "audio/mid",
            ".midi" => "audio/mid",
            ".mk" => "text/plain",
            ".mod" => "video/mpeg",
            ".mov" => "video/quicktime",
            ".mp2" => "video/mpeg",
            ".mp2v" => "video/mpeg",
            ".mp3" => "audio/mpeg",
            ".mp4" => "video/mp4",
            ".mpa" => "video/mpeg",
            ".mpd" => "application/vnd.ms-project",
            ".mpe" => "video/mpeg",
            ".mpeg" => "video/mpeg",
            ".mpf" => "application/vnd.ms-mediapackage",
            ".mpg" => "video/mpeg",
            ".mpp" => "application/vnd.ms-project",
            ".mpt" => "application/vnd.ms-project",
            ".mpv2" => "video/mpeg",
            ".mpw" => "application/vnd.ms-project",
            ".mpx" => "application/vnd.ms-project",
            ".mqv" => "video/quicktime",
            ".NMW" => "application/nmwb",
            ".nws" => "message/rfc822",
            ".odc" => "text/x-ms-odc",
            ".odh" => "text/plain",
            ".odl" => "text/plain",
            ".odp" => "application/vnd.oasis.opendocument.presentation",
            ".ods" => "application/vnd.oasis.opendocument.spreadsheet",
            ".odt" => "application/vnd.oasis.opendocument.text",
            ".ols" => "application/vnd.ms-publisher",
            ".orderedtest" => "application/xml",
            ".p10" => "application/pkcs10",
            ".p12" => "application/x-pkcs12",
            ".p7b" => "application/x-pkcs7-certificates",
            ".p7c" => "application/pkcs7-mime",
            ".p7m" => "application/pkcs7-mime",
            ".p7r" => "application/x-pkcs7-certreqresp",
            ".p7s" => "application/pkcs7-signature",
            ".pct" => "image/pict",
            ".pdf" => "application/pdf",
            ".pdfxml" => "application/vnd.adobe.pdfxml",
            ".pdx" => "application/vnd.adobe.pdx",
            ".pfx" => "application/x-pkcs12",
            ".pic" => "image/pict",
            ".pict" => "image/pict",
            ".pko" => "application/vnd.ms-pki.pko",
            ".png" => "image/png",
            ".pnt" => "image/x-macpaint",
            ".pntg" => "image/x-macpaint",
            ".pot" => "application/vnd.ms-powerpoint",
            ".potm" => "application/vnd.ms-powerpoint.template.macroEnabled.12",
            ".potx" => "application/vnd.openxmlformats-officedocument.presentationml.template",
            ".ppa" => "application/vnd.ms-powerpoint",
            ".ppam" => "application/vnd.ms-powerpoint.addin.macroEnabled.12",
            ".pps" => "application/vnd.ms-powerpoint",
            ".ppsm" => "application/vnd.ms-powerpoint.slideshow.macroEnabled.12",
            ".ppsx" => "application/vnd.openxmlformats-officedocument.presentationml.slideshow",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptm" => "application/vnd.ms-powerpoint.presentation.macroEnabled.12",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".prf" => "application/pics-rules",
            ".ps" => "application/postscript",
            ".psc1" => "application/PowerShell",
            ".pub" => "application/vnd.ms-publisher",
            ".pwz" => "application/vnd.ms-powerpoint",
            ".qcp" => "audio/qcelp",
            ".qht" => "text/x-html-insertion",
            ".qhtm" => "text/x-html-insertion",
            ".qt" => "video/quicktime",
            ".qti" => "image/x-quicktime",
            ".qtif" => "image/x-quicktime",
            ".qtl" => "application/x-quicktimeplayer",
            ".ra" => "audio/vnd.rn-realaudio",
            ".ram" => "audio/x-pn-realaudio",
            ".rat" => "application/rat-file",
            ".rc" => "text/plain",
            ".rc2" => "text/plain",
            ".rct" => "text/plain",
            ".rdlc" => "application/xml",
            ".resx" => "application/xml",
            ".rgs" => "text/plain",
            ".rjt" => "application/vnd.rn-realsystem-rjt",
            ".rm" => "application/vnd.rn-realmedia",
            ".rmi" => "audio/mid",
            ".rmj" => "application/vnd.rn-realsystem-rmj",
            ".rmm" => "audio/x-pn-realaudio",
            ".rmp" => "application/vnd.rn-rn_music_package",
            ".rms" => "application/vnd.rn-realmedia-secure",
            ".rmvb" => "application/vnd.rn-realmedia-vbr",
            ".rmx" => "application/vnd.rn-realsystem-rmx",
            ".rnx" => "application/vnd.rn-realplayer",
            ".rp" => "image/vnd.rn-realpix",
            ".rpm" => "audio/x-pn-realaudio-plugin",
            ".rpt" => "application/x-rpt",
            ".rqy" => "text/x-ms-rqy",
            ".rsml" => "application/vnd.rn-rsml",
            ".rt" => "text/vnd.rn-realtext",
            ".rtf" => "application/msword",
            ".rv" => "video/vnd.rn-realvideo",
            ".s" => "text/plain",
            ".sct" => "text/scriptlet",
            ".sd2" => "audio/x-sd2",
            ".sdp" => "application/sdp",
            ".settings" => "application/xml",
            ".shtml" => "text/html",
            ".sit" => "application/x-stuffit",
            ".sitemap" => "application/xml",
            ".skin" => "application/xml",
            ".sldm" => "application/vnd.ms-powerpoint.slide.macroEnabled.12",
            ".sldx" => "application/vnd.openxmlformats-officedocument.presentationml.slide",
            ".slk" => "application/vnd.ms-excel",
            ".sln" => "text/plain",
            ".smi" => "application/smil",
            ".smil" => "application/smil",
            ".snd" => "audio/basic",
            ".snippet" => "application/xml",
            ".sol" => "text/plain",
            ".sor" => "text/plain",
            ".spc" => "application/x-pkcs7-certificates",
            ".spl" => "application/futuresplash",
            ".srf" => "text/plain",
            ".SSISDeploymentManifest" => "text/xml",
            ".sst" => "application/vnd.ms-pki.certstore",
            ".stl" => "application/vnd.ms-pki.stl",
            ".svc" => "application/xml",
            ".swf" => "application/x-shockwave-flash",
            ".tar" => "application/x-tar",
            ".testrunconfig" => "application/xml",
            ".tga" => "image/targa",
            ".tgz" => "application/x-compressed",
            ".thmx" => "application/vnd.ms-officetheme",
            ".tlh" => "text/plain",
            ".tli" => "text/plain",
            ".trx" => "application/xml",
            ".txt" => "text/plain",
            ".uls" => "text/iuls",
            ".user" => "text/plain",
            ".vb" => "text/plain",
            ".vbdproj" => "text/plain",
            ".vbproj" => "text/plain",
            ".vcproj" => "Application/xml",
            ".vddproj" => "text/plain",
            ".vdp" => "text/plain",
            ".vdproj" => "text/plain",
            ".vdx" => "application/vnd.visio",
            ".vscontent" => "application/xml",
            ".vsd" => "application/vnd.visio",
            ".vsi" => "application/ms-vsi",
            ".vsl" => "application/vnd.visio",
            ".vsmdi" => "application/xml",
            ".vspscc" => "text/plain",
            ".vss" => "application/vnd.visio",
            ".vsscc" => "text/plain",
            ".vssettings" => "text/xml",
            ".vssscc" => "text/plain",
            ".vst" => "application/vnd.visio",
            ".vstemplate" => "text/xml",
            ".vsu" => "application/vnd.visio",
            ".vsw" => "application/vnd.visio",
            ".vsx" => "application/vnd.visio",
            ".vtx" => "application/vnd.visio",
            ".wav" => "audio/wav",
            ".wax" => "audio/x-ms-wax",
            ".wbk" => "application/msword",
            ".wdp" => "image/vnd.ms-photo",
            ".webp" => "image/webp",
            ".wiz" => "application/msword",
            ".wm" => "video/x-ms-wm",
            ".wma" => "audio/x-ms-wma",
            ".wmd" => "application/x-ms-wmd",
            ".wmv" => "video/x-ms-wmv",
            ".wmx" => "video/x-ms-wmx",
            ".wmz" => "application/x-ms-wmz",
            ".wpl" => "application/vnd.ms-wpl",
            ".wpost" => "application/x-wpost",
            ".wsc" => "text/scriptlet",
            ".wsdl" => "application/xml",
            ".wvx" => "video/x-ms-wvx",
            ".xaml" => "application/xaml+xml",
            ".xbap" => "application/x-ms-xbap",
            ".xdp" => "application/vnd.adobe.xdp+xml",
            ".xdr" => "application/xml",
            ".xfdf" => "application/vnd.adobe.xfdf",
            ".xht" => "application/xhtml+xml",
            ".xhtml" => "application/xhtml+xml",
            ".xla" => "application/vnd.ms-excel",
            ".xlam" => "application/vnd.ms-excel.addin.macroEnabled.12",
            ".xlk" => "application/vnd.ms-excel",
            ".xll" => "application/vnd.ms-excel",
            ".xlm" => "application/vnd.ms-excel",
            ".xls" => "application/vnd.ms-excel",
            ".xlsb" => "application/vnd.ms-excel.sheet.binary.macroEnabled.12",
            ".xlsm" => "application/vnd.ms-excel.sheet.macroEnabled.12",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".xlt" => "application/vnd.ms-excel",
            ".xltm" => "application/vnd.ms-excel.template.macroEnabled.12",
            ".xltx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.template",
            ".xlw" => "application/vnd.ms-excel",
            ".xml" => "text/xml",
            ".xmta" => "application/xml",
            ".XOML" => "text/plain",
            ".xps" => "application/vnd.ms-xpsdocument",
            ".xsc" => "application/xml",
            ".xsd" => "application/xml",
            ".xsl" => "text/xml",
            ".xslt" => "application/xml",
            ".xss" => "application/xml",
            ".z" => "application/x-compress",
            ".zip" => "application/x-zip-compressed",
            _ => "application/octet-stream",
        };

    }
}
