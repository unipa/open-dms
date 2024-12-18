FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API/OpenDMS.RemoteSign.API/RemoteSignInfocert/RemoteSignInfocert.csproj", "OpenDMS.RemoteSign.API/"]
COPY ["Core/OpenDMS.Infrastructure.Database/OpenDMS.Infrastructure.Database.csproj", "lib/OpenDMS.Infrastructure.Database/"]
COPY ["Core/OpenDMS.Infrastructure.VirtualFileSystem/OpenDMS.Infrastructure.VirtualFileSystem.csproj", "lib/OpenDMS.Infrastructure.VirtualFileSystem/"]
COPY ["Core/OpenDMS.Domain/OpenDMS.Domain.csproj", "lib/OpenDMS.Domain/"]
COPY ["Core/OpenDMS.MultiTenancy/OpenDMS.MultiTenancy.csproj", "lib/OpenDMS.MultiTenancy/"]

COPY . .
RUN dotnet build "API/OpenDMS.RemoteSign.API/RemoteSignInfocert/RemoteSignInfocert.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish "API/OpenDMS.RemoteSign.API/RemoteSignInfocert/RemoteSignInfocert.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY certs/dms_cert.pfx .
COPY API/OpenDMS.RemoteSign.API/RemoteSignInfocert/DB/mydb.db .

ENV ASPNETCORE_URLS=https://+;http://+
ENV ASPNETCORE_HTTPS_PORT=443
ENTRYPOINT ["dotnet", "RemoteSignInfocert.dll"]
