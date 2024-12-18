FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443

RUN apt-get update && apt-get install -y \
	ghostscript
	
RUN apt-get --no-install-recommends install libreoffice -y
RUN apt-get install -y libreoffice-java-common
RUN apt-get install unoconv
RUN apt-get install -y pdftohtml

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API/OpenDMS.Preview.API/OpenDMS.Preview.API/OpenDMS.Preview.API.csproj", "OpenDMS.Preview.API/"]
COPY ["Core/OpenDMS.Core/OpenDMS.Core.csproj", "OpenDMS.Core/"]
COPY ["Core/OpenDMS.Infrastructure.Database/OpenDMS.Infrastructure.Database.csproj", "OpenDMS.Infrastructure.Database/"]
COPY ["Core/OpenDMS.Infrastructure.Services/OpenDMS.Infrastructure.Services.csproj", "OpenDMS.Infrastructure.Services/"]
COPY ["Core/OpenDMS.Infrastructure.VirtualFileSystem/OpenDMS.Infrastructure.VirtualFileSystem.csproj", "OpenDMS.Infrastructure.VirtualFileSystem/"]
COPY ["Core/OpenDMS.Domain/OpenDMS.Domain.csproj", "OpenDMS.Domain/"]
COPY ["Core/OpenDMS.Startup/OpenDMS.Startup.csproj", "OpenDMS.Startup/"]
COPY . .
RUN dotnet build "API/OpenDMS.Preview.API/OpenDMS.Preview.API/OpenDMS.Preview.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/OpenDMS.Preview.API/OpenDMS.Preview.API/OpenDMS.Preview.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY certs/dms_cert.pfx .

ENV ASPNETCORE_URLS=https://+;http://+
ENV ASPNETCORE_HTTPS_PORT=443
ENTRYPOINT ["dotnet", "OpenDMS.Preview.API.dll"]
