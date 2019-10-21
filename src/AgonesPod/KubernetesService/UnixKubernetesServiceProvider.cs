using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgonesPod
{
    public class UnixKubernetesServiceProvider : KubernetesServiceProviderBase
    {
        private string _accessToken;
        private string _hostName;
        private string _namespace;
        private string _kubernetesServiceEndPoint;
        private bool? _isRunningOnKubernetes;

        public override string AccessToken => _accessToken ?? (_accessToken = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token"));
        public override string HostName => _hostName ?? (_hostName = Environment.GetEnvironmentVariable("HOSTNAME"));
        public override string NameSpace => _namespace ?? (_namespace = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/namespace"));
        public override string KubernetesServiceEndPoint => _kubernetesServiceEndPoint ?? (_kubernetesServiceEndPoint = $"https://{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT")}");
        public override bool IsRunningOnKubernetes => _isRunningOnKubernetes ?? (bool)(_isRunningOnKubernetes = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")));
    }
}
