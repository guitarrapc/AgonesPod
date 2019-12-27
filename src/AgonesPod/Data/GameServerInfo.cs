using System;
using System.Collections.Generic;
using System.Text;

namespace AgonesPod
{
    public interface IGameServerCore
    {
        bool IsRunningOnKubernetes { get; }
        bool IsAllocated { get; }
        string NameSpace { get; }
        string Name { get; }
        string Address { get; }
        int Port { get; }
        string State { get; }
        string Fleet { get; }
    }

    public interface IGameServerInfo : IGameServerCore
    {
        string SdkVersion { get; }
    }

    internal class GameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => true;
        public bool IsAllocated => State == "Allocated";
        public string NameSpace { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string State { get; set; }
        public string Fleet { get; set; }
        public string SdkVersion { get; set; }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }

    internal class PseudoGameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => false;
        public bool IsAllocated => State == "Allocated";
        public string NameSpace => "default";
        public string Name => "localhost";
        public string Address => Environment.MachineName;
        public int Port => 0;
        public string State => "Allocated";
        public string Fleet => "fleet";
        public string SdkVersion => "1.2.0";

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}
