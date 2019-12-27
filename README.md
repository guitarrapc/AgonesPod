[![CircleCI](https://circleci.com/gh/guitarrapc/AgonesPod.svg?style=svg)](https://circleci.com/gh/guitarrapc/AgonesPod) [![NuGet](https://img.shields.io/nuget/v/agonespod.svg)](https://www.nuget.org/packages/agonespod) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) 

[![hub](https://img.shields.io/docker/pulls/guitarrapc/agonespod.svg)](https://hub.docker.com/r/guitarrapc/agonespod/) [![](https://images.microbadger.com/badges/image/guitarrapc/agonespod.svg)](https://microbadger.com/images/guitarrapc/agonespod "Get your own image badge on microbadger.com") [![](https://images.microbadger.com/badges/version/guitarrapc/agonespod.svg)](https://microbadger.com/images/guitarrapc/agonespod "Get your own version badge on microbadger.com")

## AgonesPod

Kubernetes Client to manipulate Agones.

## todo

- [x] GetGameServersAsync: `GameServers` (GET)
- [x] AllocateGameServersAsync: `Allocate` GameServer (POST)

> NOTE: Shutdown()

## how to run

Apply Agones to your kubernetes cluster.

> REF: https://agones.dev/site/docs/installation/#installing-agones

```
$ helm repo add agones https://agones.dev/chart/stable
$ helm upgrade --install --name agones --namespace agones-system agones/agones
$ helm delete agones
```

deploy Agones Fleet and agonespod to your cluster.
let's use https://github.com/guitarrapc/agones-udp-server-csharp to wake up fleet.

```
git clone https://github.com/guitarrapc/agones-udp-server-csharp.git
kubectl apply -f ./agones-udp-server-csharp/k8s/
kubectl delete -f ./agones-udp-server-csharp/k8s/
```

Run agonespod on pod.

```
kubectl exec -it agonespod -- dotnet AgonesPod.ConsoleSample.dll getgameserver -fleetName simple-udp
```

You may get Agones GameServer info.

```shell
# GetGameServer
  Host:Port = 192.168.65.3:7682
    IsRunningOnKubernetes : True
    IsAllocated : False
    Name : simple-udp-rcdvt-hhbz8
    Address : 192.168.65.3
    Port : 7682
    State : Ready
  Host:Port = 192.168.65.3:7969
    IsRunningOnKubernetes : True
    IsAllocated : False
    Name : simple-udp-rcdvt-xr6vn
    Address : 192.168.65.3
    Port : 7969
    State : Ready
```

## docker

Build and push Docker Image

```
docker build -t agonespod:0.4.3 -f samples/AgonesPod.ConsoleSample/Dockerfile .
docker tag agonespod:0.4.3 guitarrapc/agonespod:0.4.3
docker push guitarrapc/agonespod:0.4.3
```

## debug

publish on linux and cp to pod and run.

```
dotnet publish
kubectl cp ./samples/AgonesPod.ConsoleSample/bin/Debug/netcoreapp3.1/publish/AgonesPod.ConsoleSample.dll agonespod:/app/AgonesPod.ConsoleSample.dll
kubectl cp ./samples/AgonesPod.ConsoleSample/bin/Debug/netcoreapp3.1/publish/AgonesPod.dll agonespod:/app/AgonesPod.dll
kubectl exec -it agonespod dotnet AgonesPod.ConsoleSample.dll getgameserver
```
```
kubectl exec -it agonespod -- dotnet AgonesPod.ConsoleSample.dll getgameserver
kubectl exec -it agonespod -- dotnet AgonesPod.ConsoleSample.dll allocategameserver -f magiconion-chatserver
```

### cURL

AgonesPod checking kubernetes API && Agones Controller behaviour with cURL.

> [Access Agones via the Kubernetes API \| Agones](https://agones.dev/site/docs/guides/access-api/)

curl allocate

```
kubectl exec -it agonespod /bin/bash
TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -H "Authorization: Bearer $TOKEN" -d '{"apiVersion":"allocation.agones.dev/v1","kind":"GameServerAllocation","spec":{"required":{"matchLabels":{"agones.dev/fleet":"magiconion-chatserver"}}}}' -H "Content-Type: application/json" -X POST https://$KUBERNETES_SERVICE_HOST:443/apis/allocation.agones.dev/v1/namespaces/default/gameserverallocations -k
```

response

```json
{"kind":"GameServerAllocation","apiVersion":"allocation.agones.dev/v1","metadata":{"name":"simple-udp-btdzt-fn65w","namespace":"default","creationTimestamp":"2019-10-28T06:20:08Z"},"spec":{"multiClusterSetting":{"policySelector":{}},"required":{"matchLabels":{"agones.dev/fleet":"simple-udp"}},"scheduling":"Packed","metadata":{}},"status":{"state":"Allocated","gameServerName":"simple-udp-btdzt-fn65w","ports":[{"name":"default","port":7934}],"address":"192.168.65.3","nodeName":"docker-desktop"}}
```

## REF

> [Access Agones via the Kubernetes API \| Agones](https://agones.dev/site/docs/guides/access-api/)
> 
> [Agones Kubernetes API \| Agones](https://agones.dev/site/docs/reference/agones_crd_api_reference/)

## Thank you to

Overview is follow to Pripod structure and bollow JsonCodes.

> [mayuki/Pripod: Pripod enables you to easily access Pod information from the \.NET app inside a Pod\.](https://github.com/mayuki/Pripod)
