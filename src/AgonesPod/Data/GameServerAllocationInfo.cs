using System;
using System.Collections.Generic;
using System.Text;
using GameServerObject = AgonesPod.Internal.Kubernetes.GameServer;

namespace AgonesPod
{
    public interface IGameServerAllocationInfo : IGameServerInfo
    {
        string Scheduling { get; }
        string Address { get; }
        string NodeName { get; }
    }

    public class GameServerAllocationInfo : IGameServerAllocationInfo
    {
        public bool IsRunningOnKubernetes => true;
        public bool IsAllocated => State == "Allocated";
        public string Name { get; set; }
        public string Scheduling { get; set; }
        public string State { get; set; }
        public string Host { get; set; }
        public string Address { get; set; }
        public string NodeName { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }

    public class PseudoGameServerAllocationInfo : IGameServerAllocationInfo
    {
        public bool IsRunningOnKubernetes => false;
        public bool IsAllocated => State == "Allocated";
        public string Name { get; set; }
        public string Scheduling => "Packed";
        public string State => "Allocated";
        public string Host => throw new NotImplementedException();
        public string Address => throw new NotImplementedException();
        public string NodeName => throw new NotImplementedException();
        public int Port => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}
