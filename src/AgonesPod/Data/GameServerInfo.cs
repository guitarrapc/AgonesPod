using System;
using System.Collections.Generic;
using System.Text;

namespace AgonesPod
{
    public interface IGameServerInfo
    {
        bool IsRunningOnKubernetes { get; }
        string Host { get; }
        string Port { get; }
        string State { get; }
    }

    internal class GameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => true;
        public string Host { get; set; }
        public string Port { get; set; }
        public string State { get; set; }

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }

    internal class PseudoGameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => false;
        public string Host => Environment.MachineName;
        public string Port => "0";
        public string State => "Allocated";

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}
