[![CircleCI](https://circleci.com/gh/guitarrapc/AgonesPod.svg?style=svg)](https://circleci.com/gh/guitarrapc/AgonesPod) [![NuGet](https://img.shields.io/nuget/v/agonespod.svg)](https://www.nuget.org/packages/agonespod) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) 

[![hub](https://img.shields.io/docker/pulls/guitarrapc/agonespod.svg)](https://hub.docker.com/r/guitarrapc/agonespod/) [![](https://images.microbadger.com/badges/image/guitarrapc/agonespod.svg)](https://microbadger.com/images/guitarrapc/agonespod "Get your own image badge on microbadger.com") [![](https://images.microbadger.com/badges/version/guitarrapc/agonespod.svg)](https://microbadger.com/images/guitarrapc/agonespod "Get your own version badge on microbadger.com")

## AgonesPod

Kubernetes Client to manipulate Agones.

## todo

- [x] GetGameServersAsync: `GameServers` (GET)
- [x] AllocateGameServersAsync: `Allocate` GameServer (POST)

> Spec: [Agones Kubernetes API \| Agones](https://agones.dev/site/docs/reference/agones_crd_api_reference/)

## install

**nuget**

```shell
dotnet add package AgonesPod
```

**kubernetes**

> [deploy.yaml](k8s/deploy.yaml)

## docker

Build and push Docker Image

```
docker_version=1.2.0
docker build -t agonespod:${docker_version} -f samples/AgonesPod.ConsoleSample/Dockerfile .
docker tag agonespod:${docker_version} guitarrapc/agonespod:${docker_version}
docker push guitarrapc/agonespod:${docker_version}
```

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
kubectl apply -f ./k8s/
pod=$(kubectl get pods -o json | jq -r .items[].metadata.name | grep agonespod | head -n 1)
kubectl exec -it $pod -- dotnet AgonesPod.ConsoleSample.dll getgameserver
```

You may get Agones GameServer info.

```shell
# GetGameServer
  Host:Port = 192.168.65.3:7969
    IsRunningOnKubernetes : True
    IsAllocated : False
    NameSpace : default
    Name : simple-udp-rcdvt-xr6vn
    Address : 192.168.65.3
    Port : 7969
    State : Ready
    Fleet : simple-udp
    SdkVersion : 1.2.0
  Host:Port = 192.168.65.3:7598
    IsRunningOnKubernetes : True
    IsAllocated : True
    NameSpace : default
    Name : simple-udp-rcdvt-zxpwl
    Address : 192.168.65.3
    Port : 7598
    State : Allocated
    Fleet : simple-udp
    SdkVersion : 1.2.0
```


## Debug

publish on linux and cp to pod and run.

```
dotnet publish
pod=$(kubectl get pods -o json | jq -r .items[].metadata.name | grep agonespod | head -n 1)
kubectl cp ./samples/AgonesPod.ConsoleSample/bin/Debug/netcoreapp3.1/publish/AgonesPod.ConsoleSample.dll ${pod}:/app/AgonesPod.ConsoleSample.dll
kubectl cp ./samples/AgonesPod.ConsoleSample/bin/Debug/netcoreapp3.1/publish/AgonesPod.dll ${pod}:/app/AgonesPod.dll
kubectl exec -it ${pod} dotnet AgonesPod.ConsoleSample.dll getgameserver
```

sample usage

```
pod=$(kubectl get pods -o json | jq -r .items[].metadata.name | grep agonespod | head -n 1)
kubectl exec -it ${pod} -- dotnet AgonesPod.ConsoleSample.dll getgameserver
kubectl exec -it ${pod} -- dotnet AgonesPod.ConsoleSample.dll allocategameserver -f simple-udp
```

### cURL

AgonesPod checking kubernetes API && Agones Controller behaviour with cURL.

> [Access Agones via the Kubernetes API \| Agones](https://agones.dev/site/docs/guides/access-api/)

#### list agones resources

request

```shell
pod=$(kubectl get pods -o json | jq -r .items[].metadata.name | grep agonespod | head -n 1)
kubectl exec -it ${pod} /bin/bash
TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -s -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" https://$KUBERNETES_SERVICE_HOST:443/apis -k | grep agones -A 3 -B 1
```

response

```json
    {
      "name": "agones.dev",
      "versions": [
        {
          "groupVersion": "agones.dev/v1",
          "version": "v1"
        }
      ],
      "preferredVersion": {
        "groupVersion": "agones.dev/v1",
        "version": "v1"
      }
    },
    {
      "name": "allocation.agones.dev",
      "versions": [
        {
          "groupVersion": "allocation.agones.dev/v1",
          "version": "v1"
        }
      ],
      "preferredVersion": {
        "groupVersion": "allocation.agones.dev/v1",
        "version": "v1"
      }
    },
    {
      "name": "autoscaling.agones.dev",
      "versions": [
        {
          "groupVersion": "autoscaling.agones.dev/v1",
          "version": "v1"
        }
      ],
      "preferredVersion": {
        "groupVersion": "autoscaling.agones.dev/v1",
        "version": "v1"
      }
    },
    {
      "name": "multicluster.agones.dev",
      "versions": [
        {
          "groupVersion": "multicluster.agones.dev/v1alpha1",
          "version": "v1alpha1"
        }
      ],
      "preferredVersion": {
        "groupVersion": "multicluster.agones.dev/v1alpha1",
        "version": "v1alpha1"
      }
    },
```

#### getgameserver

request

```shell
pod=$(kubectl get pods -o json | jq -r .items[].metadata.name | grep agonespod | head -n 1)
kubectl exec -it ${pod} /bin/bash
TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -s -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" https://$KUBERNETES_SERVICE_HOST:443/apis/agones.dev/v1/namespaces/default/gameservers -k
```

response

```json
{"apiVersion":"agones.dev/v1","items":[{"apiVersion":"agones.dev/v1","kind":"GameServer","metadata":{"annotations":{"agones.dev/ready-container-id":"docker://f6603880bfb885906780709d9d5ae8ec8b2a02ba591d12e76dc27ebc101e36e6","agones.dev/sdk-version":"1.2.0"},"creationTimestamp":"2019-12-27T08:43:36Z","finalizers":["agones.dev"],"generateName":"simple-udp-rcdvt-","generation":6,"labels":{"agones.dev/fleet":"simple-udp","agones.dev/gameserverset":"simple-udp-rcdvt"},"name":"simple-udp-rcdvt-xr6vn","namespace":"default","ownerReferences":[{"apiVersion":"agones.dev/v1","blockOwnerDeletion":true,"controller":true,"kind":"GameServerSet","name":"simple-udp-rcdvt","uid":"f89d85f7-2884-11ea-850e-00155d644a2b"}],"resourceVersion":"1184988","selfLink":"/apis/agones.dev/v1/namespaces/default/gameservers/simple-udp-rcdvt-xr6vn","uid":"f8bcf887-2884-11ea-850e-00155d644a2b"},"spec":{"container":"simple-udp","health":{"failureThreshold":3,"initialDelaySeconds":15,"periodSeconds":5},"ports":[{"containerPort":7654,"hostPort":7969,"name":"default","portPolicy":"Dynamic","protocol":"UDP"}],"scheduling":"Packed","sdkServer":{"grpcPort":9357,"httpPort":9358,"logLevel":"Info"},"template":{"metadata":{"creationTimestamp":null},"spec":{"containers":[{"args":["run"],"image":"guitarrapc/agones-udp-server-csharp:1.2.1","imagePullPolicy":"Always","name":"simple-udp","resources":{"limits":{"cpu":"20m","memory":"64Mi"},"requests":{"cpu":"20m","memory":"64Mi"}}}]}}},"status":{"address":"192.168.65.3","nodeName":"docker-desktop","ports":[{"name":"default","port":7969}],"reservedUntil":null,"state":"Ready"}},{"apiVersion":"agones.dev/v1","kind":"GameServer","metadata":{"annotations":{"agones.dev/ready-container-id":"docker://30127e97b7248eacfd82d64f1556dbe73c2291d81007a23d73a2298091519534","agones.dev/sdk-version":"1.2.0"},"creationTimestamp":"2019-12-27T08:47:57Z","finalizers":["agones.dev"],"generateName":"simple-udp-rcdvt-","generation":7,"labels":{"agones.dev/fleet":"simple-udp","agones.dev/gameserverset":"simple-udp-rcdvt"},"name":"simple-udp-rcdvt-zxpwl","namespace":"default","ownerReferences":[{"apiVersion":"agones.dev/v1","blockOwnerDeletion":true,"controller":true,"kind":"GameServerSet","name":"simple-udp-rcdvt","uid":"f89d85f7-2884-11ea-850e-00155d644a2b"}],"resourceVersion":"1185633","selfLink":"/apis/agones.dev/v1/namespaces/default/gameservers/simple-udp-rcdvt-zxpwl","uid":"94ad2ab3-2885-11ea-850e-00155d644a2b"},"spec":{"container":"simple-udp","health":{"failureThreshold":3,"initialDelaySeconds":15,"periodSeconds":5},"ports":[{"containerPort":7654,"hostPort":7598,"name":"default","portPolicy":"Dynamic","protocol":"UDP"}],"scheduling":"Packed","sdkServer":{"grpcPort":9357,"httpPort":9358,"logLevel":"Info"},"template":{"metadata":{"creationTimestamp":null},"spec":{"containers":[{"args":["run"],"image":"guitarrapc/agones-udp-server-csharp:1.2.1","imagePullPolicy":"Always","name":"simple-udp","resources":{"limits":{"cpu":"20m","memory":"64Mi"},"requests":{"cpu":"20m","memory":"64Mi"}}}]}}},"status":{"address":"192.168.65.3","nodeName":"docker-desktop","ports":[{"name":"default","port":7598}],"reservedUntil":null,"state":"Allocated"}}],"kind":"GameServerList","metadata":{"continue":"","resourceVersion":"1191374","selfLink":"/apis/agones.dev/v1/namespaces/default/gameservers"}}
```

#### allocate

request

```shell
kubectl exec -it agonespod /bin/bash
TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
curl -s -H "Authorization: Bearer $TOKEN" -d '{"apiVersion":"allocation.agones.dev/v1","kind":"GameServerAllocation","spec":{"required":{"matchLabels":{"agones.dev/fleet":"simple-udp"}}}}' -H "Content-Type: application/json" -X POST https://$KUBERNETES_SERVICE_HOST:443/apis/allocation.agones.dev/v1/namespaces/default/gameserverallocations -k
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
