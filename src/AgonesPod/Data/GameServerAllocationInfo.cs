using System;
using System.Collections.Generic;
using System.Text;
using GameServerObject = AgonesPod.Internal.Kubernetes.GameServer;

namespace AgonesPod
{
    public interface IGameServerAllocationInfo : IGameServerCore
    {
        string Scheduling { get; }
        string NodeName { get; }
    }

    public class GameServerAllocationInfo : IGameServerAllocationInfo
    {
        public bool IsRunningOnKubernetes => true;
        public bool IsAllocated => State == "Allocated";
        public string NameSpace { get; set; }
        public string Name { get; set; }
        public string Scheduling { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string NodeName { get; set; }
        public int Port { get; set; }
        public string Fleet { get; set; }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }

    public class PseudoGameServerAllocationInfo : IGameServerAllocationInfo
    {
        public bool IsRunningOnKubernetes => false;
        public bool IsAllocated => State == "Allocated";
        public string NameSpace => throw new NotImplementedException();
        public string Name { get; set; }
        public string Scheduling => "Packed";
        public string State => "Allocated";
        public string Address => throw new NotImplementedException();
        public string NodeName => throw new NotImplementedException();
        public int Port => throw new NotImplementedException();
        public string Fleet => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}
