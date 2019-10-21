using System;
using System.Collections.Generic;
using System.Text;
using GameServerObject = AgonesPod.Internal.Kubernetes.GameServer;

namespace AgonesPod
{
    public interface IGameServerInfo
    {
        bool IsRunningOnKubernetes { get; }
        string Host { get; }
        string Port { get; }
    }

    //internal class GameServerInfo : IGameServerInfo
    //{
    //    private readonly GameServerObject _object;

    //    public bool IsRunningOnKubernetes => true;
    //    public string Host => _object.Status.Address;
    //    public string Port => _object.Spec.Ports.HostPort;

    //    internal GameServerInfo(GameServerObject obj)
    //    {
    //        _object = obj;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{Host}:{Port}";
    //    }
    //}

    internal class GameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => true;
        public string Host { get; set; }
        public string Port { get; set; }

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

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}
