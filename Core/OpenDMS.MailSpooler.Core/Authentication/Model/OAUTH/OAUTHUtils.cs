using OpenDMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core
{
    public static class OAUTHUtils
    {
        #region OAUTH Getter

        public static String Get_OAUTH_TOKEN(String BD, String Username, String MailAddress, MailType Tipo)
        {
            return "";
           
        }

        public static String Get_OAUTH_REFRESHTOKEN(String BD, String Username, String MailAddress, MailType Tipo)
        {
            return "";
        }

        public static AuthenticationType Get_OAUTH_TYPE(String BD, String Username, String MailAddress, MailType Tipo)
        {
            return AuthenticationType.Google_OAuth;
           
        }

        public static String GetServerURL()
        {
            return "";
        }

        #endregion

        #region OAUTH Setter

        public static void Set_OAUTH_TOKEN(String Username, String MailAddress, MailType Tipo, String Token)
        {
            
        }

        public static void Set_OAUTH_REFRESHTOKEN( String Username, String MailAddress, MailType Tipo, String RefreshToken)
        {
           
        }

        public static void Set_OAUTH_TYPE(String Username, String MailAddress, MailType Tipo, AuthenticationType tipo)
        {
           
        }

        #endregion

        private static string GetEnumDescription(Enum value)
        {
            return "";
        }
    }
}
