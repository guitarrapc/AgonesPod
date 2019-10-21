using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AgonesPod.Internal.Utf8Json;
using AgonesPod.KubernetesService.Respones;

namespace AgonesPod
{
    public abstract class KubernetesServiceProviderBase : IKubernetesServiceProvider
    {
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string NameSpace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificateValidation { get; set; } = true;

        public async Task<IGameServerInfo[]> GetGameServersAsync()
        {
            using (var httpClient = CreateHttpClient())
            {
                // Endpoints:
                // /apis/{APIGroup}/{VERSION}/{RESOURCE}/
                // /apis/agones.dev/v1/gameservers/

                var @namespace = NameSpace;
                var hostName = HostName;
                var gameServers = await GetGameServer(httpClient, $"/apis/agones.dev/v1/gameservers");
                var response = Utf8Json.JsonSerializer.Deserialize<GameServersResponse>(gameServers);
                var gameserverInfos = response.items.Select(x => new GameServerInfo()
                {
                    Host = x.status.address,
                    Port = x.spec.ports.Select(y => y.hostPort).FirstOrDefault().ToString(),
                })
                .ToArray();

                // TODO: read with Internal Utf8Json.
                //var reader = await GetGameServerJSon(httpClient, $"/apis/agones.dev/v1/gameservers");
                //var gameServer = new Internal.Kubernetes.GameServer(ref reader);
                //var gameserverInfo = new GameServerInfo(gameServer);
                //return new[] { gameserverInfo };
                return gameserverInfos;
            }
        }

        public Task AllocateGameServersAsync()
        {
            throw new NotImplementedException();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            bool AlwaysTrue(HttpRequestMessage r, X509Certificate2 c2, X509Chain c, System.Net.Security.SslPolicyErrors e) => true;
            if (SkipCertificateValidation)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = AlwaysTrue;
            }

            return httpClient;
        }

        private async Task<byte[]> GetGameServer(HttpClient httpClient, string apiPath)
        {
            var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
            return bytes;
        }

        private async Task<JsonReader> GetGameServerJSon(HttpClient httpClient, string apiPath)
        {
            var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
            return new JsonReader(bytes);
        }
    }
}
