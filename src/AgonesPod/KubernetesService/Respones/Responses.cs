using System;
using System.Collections.Generic;
using System.Text;

namespace AgonesPod.KubernetesService.Respones
{
    public class GameServersResponse
    {
        public string apiVersion { get; set; }
        public Item[] items { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }

        public class Metadata
        {
            public string _continue { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
        }

        public class Item
        {
            public string apiVersion { get; set; }
            public string kind { get; set; }
            public Metadata1 metadata { get; set; }
            public Spec spec { get; set; }
            public Status status { get; set; }
        }

        public class Metadata1
        {
            public Annotations annotations { get; set; }
            public DateTime creationTimestamp { get; set; }
            public string[] finalizers { get; set; }
            public string generateName { get; set; }
            public int generation { get; set; }
            public Labels labels { get; set; }
            public string name { get; set; }
            public string _namespace { get; set; }
            public Ownerreference[] ownerReferences { get; set; }
            public string resourceVersion { get; set; }
            public string selfLink { get; set; }
            public string uid { get; set; }
        }

        public class Annotations
        {
            public string agonesdevsdkversion { get; set; }
        }

        public class Labels
        {
            public string agonesdevfleet { get; set; }
            public string agonesdevgameserverset { get; set; }
            public string app { get; set; }
        }

        public class Ownerreference
        {
            public string apiVersion { get; set; }
            public bool blockOwnerDeletion { get; set; }
            public bool controller { get; set; }
            public string kind { get; set; }
            public string name { get; set; }
            public string uid { get; set; }
        }

        public class Spec
        {
            public string container { get; set; }
            public Health health { get; set; }
            public Port[] ports { get; set; }
            public string scheduling { get; set; }
            public Template template { get; set; }
        }

        public class Health
        {
            public int failureThreshold { get; set; }
            public int initialDelaySeconds { get; set; }
            public int periodSeconds { get; set; }
        }

        public class Template
        {
            public Metadata2 metadata { get; set; }
            public Spec1 spec { get; set; }
        }

        public class Metadata2
        {
            public object creationTimestamp { get; set; }
        }

        public class Spec1
        {
            public Container[] containers { get; set; }
        }

        public class Container
        {
            public string image { get; set; }
            public string imagePullPolicy { get; set; }
            public string name { get; set; }
            public Resources resources { get; set; }
        }

        public class Resources
        {
            public Limits limits { get; set; }
            public Requests requests { get; set; }
        }

        public class Limits
        {
            public string cpu { get; set; }
            public string memory { get; set; }
        }

        public class Requests
        {
            public string cpu { get; set; }
            public string memory { get; set; }
        }

        public class Port
        {
            public int containerPort { get; set; }
            public int hostPort { get; set; }
            public string name { get; set; }
            public string portPolicy { get; set; }
            public string protocol { get; set; }
        }

        public class Status
        {
            public string address { get; set; }
            public string nodeName { get; set; }
            public Port1[] ports { get; set; }
            public object reservedUntil { get; set; }
            public string state { get; set; }
        }

        public class Port1
        {
            public string name { get; set; }
            public int port { get; set; }
        }
    }
}
