using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AgonesPod.KubernetesService.Requests
{
    // '{"apiVersion":"allocation.agones.dev/v1","kind":"GameServerAllocation","spec":{"required":{"matchLabels":{"agones.dev/fleet":"FLEETNAME"}}}}'
    public class GameServerAllocationRequest
    {
        public static GameServerAllocationRequest CreateRequest(string fleetName)
        {
            var request = new GameServerAllocationRequest()
            {
                spec = new Spec
                {
                    required = new Required
                    {
                        matchLabels = new Matchlabels
                        {
                            agonesdevfleet = fleetName,
                        }
                    }
                }
            };
            return request;
        }
        public string apiVersion { get; set; } = "allocation.agones.dev/v1";
        public string kind { get; set; } = "GameServerAllocation";
        public Spec spec { get; set; }

        public class Spec
        {
            public Required required { get; set; }
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
    }
}
