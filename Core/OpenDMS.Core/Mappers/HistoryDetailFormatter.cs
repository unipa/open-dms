using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Events.Types;

namespace OpenDMS.core.Mappers
{
    public class HistoryDetailFormatter : IHistoryDetailFormatter
    {
        public List<HistoryDetail> Format(HistoryEntry entry)
        {
            List<HistoryDetail> details = new List<HistoryDetail>();
            //switch (entry.EventType)
            //{
            //    //case EventType.Share:
            //        //HistoryDetail d = new HistoryDetail();
            //        //d.Label = "A";
            //        //d.Values = entry.Recipients.Where(r=>!r.CC).Select(r=>r.ProfileId)
            //        //details.Add(d);
            //        //break;
            //    case EventType.Update:

            Dictionary<string, string> changed = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Details);


            if (entry.EventType == EventType.Update && changed.Count > 0)
            {
                HistoryDetail d = new HistoryDetail();
                d.Label = "<strong>Proprietà Modificata</strong>";
                    d.Values = new[] { "<strong>Valore Precedente</strong>" };
                details.Add(d);
            }
            foreach (var r in changed)
                if (r.Value != null)
                    {
                        HistoryDetail d = new HistoryDetail();
                        d.Label = r.Key;
                    if (r.Value.StartsWith("["))
                    {
                        try
                        {
                            d.Values = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject[]>(r.Value).Select(o => o.ToString()).ToArray();

                        }
                        catch (Exception)
                        {
                            d.Values = new[] { r.Value };
                        }

                    }
                    else
                            d.Values = new[] { r.Value };
                        details.Add(d);
                    }
            //        break;
            //    default: break; 
            //}
            return details;
        }
    }
}
