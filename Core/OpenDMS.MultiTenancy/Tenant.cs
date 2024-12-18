using System.ComponentModel.DataAnnotations;

namespace OpenDMS.MultiTenancy.Interfaces;
public class Tenant : ITenant
{
    [Key]
    [StringLength(64)]
    public string Id { get; set; }

    [StringLength(250)]
    public string ConnectionString { get; set; } = "";

    [StringLength(50)]
    public string Provider { get; set; } = "";

    [StringLength(128)]
    public string Description { get; set; } = "";
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public bool Offline { get; set; } = false;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;


    [StringLength(255)]
    public string OpenIdConnectAuthority { get; set; } = "";

    [StringLength(255)]
    public string URL { get; set; } = "";


    [StringLength(128)]
    public string OpenIdConnectClientId { get; set; } = "";

    [StringLength(255)]
    public string OpenIdConnectClientSecret { get; set; } = "";


    [StringLength(255)]
    public string ChallengeScheme { get; set; } = "OpenIdConnect";


    [StringLength(255)]
    public string RootFolder { get; set; } = "";


    public Tenant(string description, string Provider, string connectionString, bool offline)
    {
        Description = description;
        ConnectionString = connectionString;
        this.Provider = Provider;
        RootFolder = "/localhost";
        Offline = offline;
    }
    public Tenant() { }

    public Tenant(string provider, string connectionString, bool offline) : this(connectionString, provider, connectionString, offline) { }

    public Tenant(string provider,string connectionString) : this(connectionString,  provider, connectionString, false) { }


}
