## AgonesPod

Kubernetes Client to manipulate Agones.

## TODO

- [x] Get GameServers
- [ ] Post Allocate GameServer
- [ ] Post L

## How to run

Build Docker Image

```
cd samples
docker build -t agonespod:0.0.1 -f AgonesPod.ConsoleSample/Dockerfile .
docker tag agonespod:0.0.1 guitarrapc/agonespod:0.0.1
docker push guitarrapc/agonespod:0.0.1
```

Apply to your kubernetes

```
kubectl kustomize ./k8s | kubectl apply -f -
kubectl kustomize ./k8s | kubectl delete -f -
```

Run agonespod on pod.

```
kubectl exec -it agonespod dotnet AgonesPod.ConsoleSample.dll
```

## debug

publish and cp to pod.

```
dotnet publish
kubectl cp ./samples/AgonesPod.ConsoleSample/bin/Debug/netcoreapp3.0/publish/AgonesPod.ConsoleSample.dll gameserverpod:/app/AgonesPod.ConsoleSample.dll
```

## REF

> [Access Agones via the Kubernetes API \| Agones](https://agones.dev/site/docs/guides/access-api/)
> 
> [Agones Kubernetes API \| Agones](https://agones.dev/site/docs/reference/agones_crd_api_reference/)

## Thank you to

Overview is follow to Pripod structure and bollow JsonCodes.

> [mayuki/Pripod: Pripod enables you to easily access Pod information from the \.NET app inside a Pod\.](https://github.com/mayuki/Pripod)
