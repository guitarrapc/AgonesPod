using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using AgonesPod.Internal.Utf8Json;
using AgonesPod.KubernetesService.Requests;
using AgonesPod.KubernetesService.Respones;

namespace AgonesPod
{
    public abstract class KubernetesServiceProviderBase : IKubernetesServiceProvider
    {
        private static readonly MediaTypeHeaderValue applicationJsonContentType = new MediaTypeHeaderValue("application/json");
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string NameSpace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificateValidation { get; set; } = true;

        public async ValueTask<IGameServerInfo[]> GetGameServersAsync()
        {
            using var httpClient = CreateHttpClient();
            // /apis/agones.dev/v1/gameservers/

            var @namespace = NameSpace;
            var hostName = HostName;
            var gameServers = await GetGameServers(httpClient, $"/apis/agones.dev/v1/gameservers");
            var response = JsonSerializer.Deserialize<GameServersResponse>(gameServers);
            var gameserverInfos = response.items.Select(x => new GameServerInfo()
            {
                NameSpace = x.metadata.@namespace,
                Name = x.metadata.name,
                Address = x.status.address,
                Port = x?.status.ports?.FirstOrDefault().port ?? 0,
                State = x.status.state,
                Fleet = x.metadata.labels?.agonesdevfleet,
                SdkVersion = x.metadata.annotations.agonesdevsdkversion,
            })
            .ToArray();

            // TODO: read with Internal Utf8Json.
            //var reader = await GetGameServerJSon(httpClient, $"/apis/agones.dev/v1/gameservers");
            //var gameServer = new Internal.Kubernetes.GameServer(ref reader);
            //var gameserverInfo = new GameServerInfo(gameServer);
            //return new[] { gameserverInfo };
            return gameserverInfos;
        }

        public async ValueTask<IGameServerAllocationInfo> AllocateGameServerAsync(string fleetName)
        {
            using var httpClient = CreateHttpClient();
            // /apis/allocation.agones.dev/v1/namespaces/{namespace}/gameserverallocations

            var @namespace = NameSpace;
            var hostName = HostName;
            var allocation = await AllocateGameServer(httpClient, $"/apis/allocation.agones.dev/v1/namespaces/{@namespace}/gameserverallocations", fleetName);
            var response = JsonSerializer.Deserialize<GameServerAllocationResponse>(allocation);
            var allocationInfo = new GameServerAllocationInfo()
            {
                NameSpace = response.metadata.@namespace,
                Name = response.metadata.name,
                Address = response?.status?.address,
                NodeName = response?.status?.nodeName,
                Scheduling = response?.spec?.scheduling,
                Port = response?.status.ports?.FirstOrDefault().port ?? 0,
                State = response?.status?.state,
                Fleet = response.spec.required?.matchLabels?.agonesdevfleet,
            };
            return allocationInfo;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            static bool AlwaysTrue(HttpRequestMessage r, X509Certificate2 c2, X509Chain c, System.Net.Security.SslPolicyErrors e) => true;
            if (SkipCertificateValidation)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = AlwaysTrue;
            }

            return httpClient;
        }

        private async ValueTask<byte[]> GetGameServers(HttpClient httpClient, string apiPath)
        {
            var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
            return bytes;
        }

        private async ValueTask<byte[]> AllocateGameServer(HttpClient httpClient, string apiPath, string fleetName)
        {
            var body = GameServerAllocationRequest.CreateRequest(fleetName);
            var json = JsonSerializer.Serialize(body);
            
            using var request = new HttpRequestMessage(HttpMethod.Post, KubernetesServiceEndPoint + apiPath);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AccessToken}");

            // agones can not accept content-type with media-type. 
            // see: https://github.com/googleforgames/agones/blob/0e244fddf938e88dc5156ac2c7339adbb230daee/vendor/k8s.io/apimachinery/pkg/runtime/codec.go#L218-L220
            var requestContent = new StringContent(json);
            // re-apply request content-type to remove `media-type: utf8` from contet-type.
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

        //private async Task<JsonReader> GetGameServerJSon(HttpClient httpClient, string apiPath)
        //{
        //    var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
        //    return new JsonReader(bytes);
        //}
    }
}
