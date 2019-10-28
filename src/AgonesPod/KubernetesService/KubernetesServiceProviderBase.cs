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
using AgonesPod.KubernetesService.Requests;
using AgonesPod.KubernetesService.Respones;
using Utf8Json;

namespace AgonesPod
{
    public abstract class KubernetesServiceProviderBase : IKubernetesServiceProvider
    {
        private static readonly Encoding encoding = new UTF8Encoding(false);
        private static readonly MediaTypeHeaderValue applicationJsonContentType = new MediaTypeHeaderValue("application/json");
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string NameSpace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificateValidation { get; set; } = true;

        public async ValueTask<IGameServerInfo[]> GetGameServersAsync()
        {
            using (var httpClient = CreateHttpClient())
            {
                // /apis/agones.dev/v1/gameservers/

                var @namespace = NameSpace;
                var hostName = HostName;
                var gameServers = await GetGameServer(httpClient, $"/apis/agones.dev/v1/gameservers");
                var response = Utf8Json.JsonSerializer.Deserialize<GameServersResponse>(gameServers);
                var gameserverInfos = response.items.Select(x => new GameServerInfo()
                {
                    Name = x.metadata.name,
                    Host = x.status.address,
                    Port = x.status.ports.Select(y => y.port).FirstOrDefault().ToString(),
                    State = x.status.state,
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

        public async ValueTask<IGameServerAllocationInfo> AllocateGameServersAsync(string fleetName)
        {
            using (var httpClient = CreateHttpClient())
            {
                // /apis/allocation.agones.dev/v1/namespaces/{namespace}/gameserverallocations

                var @namespace = NameSpace;
                var hostName = HostName;
                var allocation = await AllocateGameServer(httpClient, $"/apis/allocation.agones.dev/v1/namespaces/{@namespace}/gameserverallocations", fleetName);
                var response = Utf8Json.JsonSerializer.Deserialize<GameServerAllocationResponse>(allocation);
                var allocationInfo = new GameServerAllocationInfo()
                {
                    Address = response?.status?.address,
                    GameServerName = response?.status?.gameServerName,
                    NodeName = response?.status?.nodeName,
                    Scheduling = response?.spec?.scheduling,
                    Port = response?.status.ports?.First().port ?? 0,
                    Status = response?.status?.state,
                };
                return allocationInfo;
            }
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

        private async ValueTask<byte[]> GetGameServer(HttpClient httpClient, string apiPath)
        {
            var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
            return bytes;
        }

        private async ValueTask<byte[]> AllocateGameServer(HttpClient httpClient, string apiPath, string fleetName)
        {
            var body = GameServerAllocationRequest.CreateRequest(fleetName);
            var json = JsonSerializer.ToJsonString(body);
            using (var request = new HttpRequestMessage(HttpMethod.Post, KubernetesServiceEndPoint + apiPath))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AccessToken}");

                // agones can not accept content-type with media-type. 
                // see: https://github.com/googleforgames/agones/blob/0e244fddf938e88dc5156ac2c7339adbb230daee/vendor/k8s.io/apimachinery/pkg/runtime/codec.go#L218-L220
                var requestContent = new StringContent(json, null, "application/json");
                requestContent.Headers.ContentType = applicationJsonContentType;
                request.Content = requestContent;

                var response = await httpClient.SendAsync(request);

                //todo: error handling
                if (!response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    return Array.Empty<byte>();
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                return bytes;
            }
        }

        //private async Task<JsonReader> GetGameServerJSon(HttpClient httpClient, string apiPath)
        //{
        //    var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
        //    return new JsonReader(bytes);
        //}
    }
}
