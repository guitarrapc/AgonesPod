﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AgonesPod
{
    public interface IKubernetesServiceProvider
    {
        string AccessToken { get; }
        string HostName { get; }
        string NameSpace { get; }
        string KubernetesServiceEndPoint { get; }
        bool IsRunningOnKubernetes { get; }

        ValueTask<IGameServerInfo[]> GetGameServersAsync();
        ValueTask<IGameServerAllocationInfo> AllocateGameServerAsync(string fleetName);
    }
}
