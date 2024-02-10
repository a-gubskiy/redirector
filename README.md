# //devdigest

Project supported by [Ukrainian .NET Developer Community](https://dotnet.city/community/17), [Microsoft Azure Ukraine User Group](https://dotnet.city/community/18), and [Xamarin Ukraine User Group](https://dotnet.city/community/19).

## How to build docker image

```
docker build ./ --file ./Dockerfile --tag ghcr.io/dncuug/devdigest.today/devdigest.today:latest

docker push ghcr.io/dncuug/devdigest.today/devdigest.today:latest

docker run --rm -it -e WebSiteUrl=http://localhost:8000/ -e ConnectionStrings__DefaultConnection="xxx" -p 8000:80 ghcr.io/dncuug/devdigest.today/devdigest.today:latest 
```

### How to login into GitHub container registry

```
docker login ghcr.io
```
