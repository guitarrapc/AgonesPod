using System;
using System.Collections.Generic;
using System.Text;

namespace AgonesPod
{
    public interface IGameServerInfo
    {
        bool IsRunningOnKubernetes { get; }
        bool IsAllocated { get; }
        string Name { get; }
        string Address { get; }
        int Port { get; }
        string State { get; }
    }

    internal class GameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => true;
        public bool IsAllocated => State == "Allocated";
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string State { get; set; }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }

    internal class PseudoGameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => false;
        public bool IsAllocated => State == "Allocated";
        public string Name => "";
        public string Address => Environment.MachineName;
        public int Port => 0;
        public string State => "Allocated";

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}
