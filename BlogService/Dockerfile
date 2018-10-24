FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY BlogService/BlogService.csproj BlogService/
COPY BlogService.Domain/BlogService.Domain.csproj BlogService.Domain/
COPY BlogService.DataModels/BlogService.DataModels.csproj BlogService.DataModels/
COPY BlogService.Abstractions/BlogService.Abstractions.csproj BlogService.Abstractions/
RUN dotnet restore BlogService/BlogService.csproj
COPY . .
WORKDIR /src/BlogService
RUN dotnet build BlogService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BlogService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BlogService.dll"]
