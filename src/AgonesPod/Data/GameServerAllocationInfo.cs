using System;
using System.Collections.Generic;
using System.Text;
using GameServerObject = AgonesPod.Internal.Kubernetes.GameServer;

namespace AgonesPod
{
    public interface IGameServerAllocationInfo
    {
        bool IsAllocated { get; }
        string Scheduling { get; }
        string Status { get; }
        string GameServerName { get; }
        string Address { get; }
        string NodeName { get; }
        int Port { get; }
    }

    public class GameServerAllocationInfo : IGameServerAllocationInfo
    {
        public bool IsAllocated => Status == "Allocated";
        public string Scheduling { get; set; }
        public string Status { get; set; }
        public string GameServerName { get; set; }
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
        public bool IsAllocated => Status == "Allocated";
        public string Scheduling => "Packed";
        public string Status => "Allocated";
        public string GameServerName => throw new NotImplementedException();
        public string Address => throw new NotImplementedException();
        public string NodeName => throw new NotImplementedException();
        public int Port => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}
