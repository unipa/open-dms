using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
        public class Topology
        {
            public List<BrokerInfo> Brokers { get; set; } = new();

        public string GatewayVersion { get; set; }
        }

    public class BrokerInfo
    {
        public int NodeId { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Address { get; set; }

        public List<PartitionInfo> Partitions { get; set; } = new();
    }

    public class PartitionInfo
    {
        public int PartitionId { get; set; } = 0;

        public bool IsLeader { get; set; } = true;
    }


}


