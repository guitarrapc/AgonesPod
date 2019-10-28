using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgonesPod
{
    public class WindowsKubernetesServiceProvider : KubernetesServiceProviderBase
    {
        private string _accessToken;
        private string _hostName;
        private string _namespace;
        private string _kubernetesServiceEndPoint;
        private bool? _isRunningOnKubernetes;

        public override string AccessToken => _accessToken ?? string.Empty;
        public override string HostName => _hostName ?? (_hostName = Environment.GetEnvironmentVariable("COMPUTERNAME"));
        public override string NameSpace => _namespace ?? "default";
        public override string KubernetesServiceEndPoint => _kubernetesServiceEndPoint ?? (_kubernetesServiceEndPoint = $"https://{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT")}");
        public override bool IsRunningOnKubernetes => _isRunningOnKubernetes ?? (bool)(_isRunningOnKubernetes = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")));
    }
}
