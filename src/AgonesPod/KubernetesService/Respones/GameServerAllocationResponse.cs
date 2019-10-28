using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AgonesPod.KubernetesService.Respones
{
    public class GameServerAllocationResponse
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public Metadata metadata { get; set; }
        public Spec spec { get; set; }
        public Status status { get; set; }

        public class Metadata
        {
            public string name { get; set; }
            public string _namespace { get; set; }
            public DateTime creationTimestamp { get; set; }
        }

        public class Spec
        {
            public Multiclustersetting multiClusterSetting { get; set; }
            public Required required { get; set; }
            public string scheduling { get; set; }
            public Metadata1 metadata { get; set; }
        }

        public class Multiclustersetting
        {
            public Policyselector policySelector { get; set; }
        }

        public class Policyselector
        {
        }

        public class Required
        {
            public Matchlabels matchLabels { get; set; }
        }

        public class Matchlabels
        {
            [DataMember(Name = "agones.dev/fleet")]
            public string agonesdevfleet { get; set; }
        }

        public class Metadata1
        {
        }

        public class Status
        {
            public string state { get; set; }
            public string gameServerName { get; set; }
            public Port[] ports { get; set; }
            public string address { get; set; }
            public string nodeName { get; set; }
        }

        public class Port
        {
            public string name { get; set; }
            public int port { get; set; }
        }
    }
}
