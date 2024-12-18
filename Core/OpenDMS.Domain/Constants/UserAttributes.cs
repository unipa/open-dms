using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Constants
{
    public static class UserAttributes
    {

        
        public const string CONST_IDENTITY_DOCUMENT_TYPE = "DocumentType";
        public const string CONST_IDENTITY_DOCUMENT_ID = "DocumentId";
        public const string CONST_COUNTRY = "Country";
        public const string CONST_BIRTHDATE = "BirthDate";

        public const string CONST_NOTIFICATION_MAIL = "Notifications.Mail";
        public const string CONST_NOTIFICATION_TYPE = "Notifications.Type";
        public const string CONST_NOTIFICATION_PREFIX = "Notifications.";

        public const string CONST_REMOTESIGNATURE_SERVICE = "DigitalSignature.RemoteService.URL";
        public const string CONST_OTPSIGNATURE_SERVICE = "DigitalSignature.OTPService.URL";
        public const string CONST_DIGITALSIGNATURE_VENDOR = "DigitalSignature.TokenService.Vendor";
        public const string CONST_DIGITALSIGNATURE_PORT = "DigitalSignature.TokenService.Port";
        public const string CONST_FEASIGNATURE_VENDOR = "DigitalSignature.FEA.Vendor";
    }
}
