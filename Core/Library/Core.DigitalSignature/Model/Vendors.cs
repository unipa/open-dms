using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature.Token
{
    public static class Vendors
    {

        public static Dictionary<string, string> List = new Dictionary<string, string>()
        {
            { "bit4xpki.dll",  "Infocert, Aruba (bit4xpki)"},
            { "bit4ipki.dll",  "Infocert 1401,1402, 1501, card OS API (bit4ipki)"},
            { "cvP11_M4.dll",  "Infocert 1601, 1602, 1603"},
            { "inpkisc.dll",  "Infocert 7420, 1202, 1203, 1204, 1205, 6090"},
            { "bit4opki.dll",  "ST INCARD 1203 (bit4opki)"},
            { "SI_PKCS11.dll",  "Postacert 7420"},
            { "incryptoki2.dll",  "Aruba Oberon"},
            { "ipmpki32.dll",  "Actalis"},
            { "CardOS_PKCS11.dll",  "Card OS"},
            { "cnsPKCS11.dll",  "CRS / CNS Siemens"},
            { "cmp11.dll",  "Token EUTRON"},
            { "siecap11.dll",  "Siemens Card"},
            { "asepkcs.dll",  "Athena"}
        };

    }

}
