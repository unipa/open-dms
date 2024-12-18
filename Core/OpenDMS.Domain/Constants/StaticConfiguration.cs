namespace OpenDMS.Domain.Constants;

public static class StaticConfiguration
{


    public const string CONST_DOCUMENTS_SOFTDELETE = "Documents:RecoverableDelete";
    public const string CONST_DOCUMENTS_FILESYSTEMTYPE = "Documents:FileSystemType";
    public const string CONST_DOCUMENTS_ROOLFOLDER = "Documents:RootFolder";
    public const string CONST_DOCUMENTS_FILEPREFIX = "Documents:FilePrefix";
    public const string CONST_DENY_GENERIC_DOCUMENT = "Documents:DenyGenericDocument";

    public const string CONST_AVATAR_FONT = "Avatar:Font";

    public const string CONST_PROTOCOL_LENGTH = "Documents:Protocol:Length";

//    public const string CONST_ENVIRONMENT_TYPE = "Tenant:EnvironmentType";

    public const string CONST_CAMUNDA_ENDPOINT = "Camunda:EndPoint";
    public const string CONST_CAMUNDA_CLIENTID = "Camunda:ClientId";
    public const string CONST_CAMUNDA_CLIENTSECRET = "Camunda:ClientSecret";
    public const string CONST_ELASTICSEARCH_ENDPOINT = "ElasticSearch:EndPoint";


    public const string CONST_ENDPOINT_ADMINSERVICE = "Endpoint:AdminService";
    public const string CONST_ENDPOINT_DOCUMENTSERVICE = "Endpoint:DocumentService";
    public const string CONST_ENDPOINT_USERSERVICE = "Endpoint:UserService";
    public const string CONST_ENDPOINT_PREVIEWSERVICE = "Endpoint:DocumentPreviewService";
    public const string CONST_ENDPOINT_UISETTINGSERVICE = "Endpoint:UIService";
    public const string CONST_ENDPOINT_SEARCHSERVICE = "Endpoint:SearchService";
    public const string CONST_ENDPOINT_TASKLISTSERVICE = "Endpoint:TaskList";
    public const string CONST_ENDPOINT_FRONTENDSERVICE = "Endpoint:FrontEnd";

    public const string CONST_MESSAGEBUS_URL = "MessageBus:URL";
    public const string CONST_MESSAGEBUS_TYPE = "MessageBus:Type";

    public const string CONST_TENANT_PROVIDER = "Provider";
    public const string CONST_TENANT_DATABASE = "ConnectionString";
    public const string CONST_TENANT_RESOLVER = "TenantResolver";


    public const string CONST_NOTIFICATIONSERVICE_SERVICETYPE = "MessageBus:Type";
    //public const string CONST_NOTIFICATIONSERVICE_URL = "Notification:URL";

    public const string CONST_NOTIFICATIONSERVICE_QUEUE = "Notification:Queue";

    public const string CONST_MAILSERVICE_SERVICETYPE = "MailService:Type";
    //public const string CONST_MAILSERVICE_URL = "MailService:URL";
    public const string CONST_MAILSERVICE_QUEUE = "MailService:Queue";
    public const string CONST_MAILSERVICE_FILESYSTEMTYPE = "MailService:FileSystemType";
    public const string CONST_MAILSERVICE_ROOTFOLDER = "MailService:RootFolder";

    public const string CONST_INDEXSERVICE_SERVICETYPE = "IndexingService:Type";
    //public const string CONST_INDEXSERVICE_URL = "IndexingService:URL";
    public const string CONST_INDEXSERVICE_QUEUE = "IndexingService:Queue";

    public const string CONST_PREVIEWSERVICE_SERVICETYPE = "PreviewService:Type";
    //public const string CONST_PREVIEWSERVICE_URL = "PreviewService:URL";
    public const string CONST_PREVIEWSERVICE_QUEUE = "PreviewService:Queue";


    public const string CONST_BPMSERVICE_SERVICETYPE = "BPMService:Type";
    //public const string CONST_BPMSERVICE_URL = "BPMService:URL";
    public const string CONST_BPMSERVICE_QUEUE = "BPMService:Queue";

    public const string CONST_IAM_REALM = "Keycloak:realm";
    public const string CONST_IAM_URL = "Keycloak:auth-server-url";

    public const string CONST_Services_Titulus = "Keycloak:auth-server-url";
    public const string CONST_Services_RemoteSizgn = "ExternalPages:RemoteSign:Infocert";
    public const string CONST_Services_OAUTH = "OAUTH:Redirect_Uri";

    //A3Synch
    public const string FILE_SETTINGS_FILE_PATH = "FileSettings:FilePath";
    public const string EMAIL_SETTINGS_SMTP_SERVER = "EmailSettings:SmtpServer";
    public const string EMAIL_SETTINGS_SMTP_PORT = "EmailSettings:SmtpPort";
    public const string EMAIL_SETTINGS_SMTP_USER = "EmailSettings:SmtpUser";
    public const string EMAIL_SETTINGS_FROM_EMAIL = "EmailSettings:FromEmail";
    public const string EMAIL_SETTINGS_TO_EMAIL = "EmailSettings:ToEmail";
    public const string EMAIL_SETTINGS_ENABLE_SSL = "EmailSettings:EnableSSL";

}
