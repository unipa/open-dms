using System.Text.Json.Serialization;

namespace OpenDMS.MaximoSR.API.Models
{
    public class QueryMXASSET_DMSResponse
    {
        [JsonPropertyName("QueryMXASSET_DMSResponse")]
        public MXASSET_DMSSetWrapper MXASSET_DMSSetWrapper { get; set; }
    }

    public class MXASSET_DMSSetWrapper
    {
        [JsonPropertyName("MXASSET_DMSSet")]
        public MXASSET_DMSSet MXASSET_DMSSet { get; set; }
    }

    public class MXASSET_DMSSet
    {
        [JsonPropertyName("ASSET")]
        public List<ASSET> ASSET { get; set; }
    }

    public class ASSET
    {
        [JsonPropertyName("rowstamp")]
        public string Rowstamp { get; set; }

        [JsonPropertyName("ASSETNUM")]
        public string AssetNumber { get; set; }

        [JsonPropertyName("ASSETTYPE")]
        public string AssetType { get; set; }

        [JsonPropertyName("DESCRIPTION")]
        public string Description { get; set; }

        [JsonPropertyName("SITEID")]
        public string SiteId { get; set; }

        [JsonPropertyName("LOCATION")]
        public string? Location { get; set; }
    }
}
