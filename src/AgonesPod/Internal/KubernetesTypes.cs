using System;
using System.Collections.Generic;
using System.Text;
using AgonesPod.Internal.Utf8Json;

namespace AgonesPod.Internal.Kubernetes
{
    internal interface ITypeMeta
    {
        string Kind { get; }
        string ApiVersion { get; }
    }

    internal interface IMetaV1Object
    {
        MetaV1Metadata Metadata { get; }
    }

    internal class MetaV1Metadata
    {
        public string Name { get; }
        public string GenerateName { get; }
        public string Namespace { get; }
        public string SelfLink { get; }
        public string Uid { get; }
        public string ResourceVersion { get; }
        public string CreationTimestamp { get; }
        public IReadOnlyDictionary<string, string> Labels { get; } = JsonReaderHelper.EmptyStringDictionary;
        public IReadOnlyDictionary<string, string> Annotations { get; } = JsonReaderHelper.EmptyStringDictionary;
        public IReadOnlyList<OwnerReference> OwnerReferences { get; } = Array.Empty<OwnerReference>();

        public MetaV1Metadata(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "name":
                                    Name = reader.ReadString();
                                    break;
                                case "generateName":
                                    GenerateName = reader.ReadString();
                                    break;
                                case "namespace":
                                    Namespace = reader.ReadString();
                                    break;
                                case "selfLink":
                                    SelfLink = reader.ReadString();
                                    break;
                                case "uid":
                                    Uid = reader.ReadString();
                                    break;
                                case "resourceVersion":
                                    ResourceVersion = reader.ReadString();
                                    break;
                                case "creationTimestamp":
                                    CreationTimestamp = reader.ReadString();
                                    break;
                                case "labels":
                                    Labels = JsonReaderHelper.ReadAsStringDictionary(ref reader);
                                    break;
                                case "annotations":
                                    Annotations = JsonReaderHelper.ReadAsStringDictionary(ref reader);
                                    break;
                                case "ownerReferences":
                                    OwnerReferences = JsonReaderHelper.ReadAsArray(ref reader, OwnerReference.Create);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class OwnerReference
    {
        public static OwnerReference Create(ref JsonReader reader) => new OwnerReference(ref reader);

        public string ApiVersion { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }

        public OwnerReference(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "name":
                                    Name = reader.ReadString();
                                    break;
                                case "uid":
                                    Uid = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class GameServer : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; }
        public string ApiVersion { get; }
        public MetaV1Metadata Metadata { get; }
        public GameServerSpec Spec { get; set; }
        public GameServerStatus Status { get; set; }

        public GameServer(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var prop = reader.ReadPropertyName();
                            switch (prop)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                case "spec":
                                    Spec = new GameServerSpec(ref reader);
                                    break;
                                case "status":
                                    Status = new GameServerStatus(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        break;
                }
            }
        }
    }

    internal class GameServerSpec
    {
        public GameServerSpecPort Ports { get; set; }

        public GameServerSpec(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while(true)
            {
                var token = reader.GetCurrentJsonToken();
                switch(token)
                {
                    case JsonToken.String:
                        {
                            var prop = reader.ReadPropertyName();
                            switch(prop)
                            {
                                case "ports":
                                    new GameServerSpecPort(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        break;
                }
            }
        }
    }
    internal class GameServerSpecPort
    {
        public string HostPort { get; }

        public GameServerSpecPort(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var prop = reader.ReadPropertyName();
                            switch (prop)
                            {
                                case "hostPort":
                                    HostPort = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        break;
                }
            }
        }
    }

    internal class GameServerStatus
    {
        public string Address { get; }
        public string NodeName { get; }
        public string State { get; }

        public GameServerStatus(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "address":
                                    Address = reader.ReadString();
                                    break;
                                case "nodeName":
                                    NodeName = reader.ReadString();
                                    break;
                                case "state":
                                    State = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }
}
