FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443

ENV DEBIAN_FRONTEND noninteractive

RUN apt-get update && apt-get install -y --allow-unauthenticated \
    fonts-dejavu-core \
    fonts-liberation \
    fonts-noto-cjk \
    fonts-noto-color-emoji \
    fonts-noto-core \
    fonts-noto-extra \
    fonts-opensymbol \
    fonts-symbola \
	ghostscript \
	libgdiplus libc6-dev \
	libx11-dev \
	ffmpeg \
	wkhtmltopdf 
	
RUN apt-get --no-install-recommends install libreoffice -y
RUN apt-get install -y libreoffice-java-common
RUN apt-get install unoconv
RUN apt-get install -y pdftohtml

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FrontEnd/Web/Web.csproj", "Web/"]
COPY ["Core/OpenDMS.Infrastructure.Database/OpenDMS.Infrastructure.Database.csproj", "lib/OpenDMS.Infrastructure.Database/"]
COPY ["Core/OpenDMS.Infrastructure.VirtualFileSystem/OpenDMS.Infrastructure.VirtualFileSystem.csproj", "lib/OpenDMS.Infrastructure.VirtualFileSystem/"]
COPY ["Core/OpenDMS.Domain/OpenDMS.Domain.csproj", "lib/OpenDMS.Domain/"]
COPY ["Core/OpenDMS.MultiTenancy/OpenDMS.MultiTenancy.csproj", "lib/OpenDMS.MultiTenancy/"]

COPY . .
RUN dotnet build "FrontEnd/Web/Web.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish "FrontEnd/Web/Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY certs/dms_cert.pfx .
COPY Setup/OpenDMS/OpenDMSClient.exe ./wwwroot/download/

ENV ASPNETCORE_URLS=https://+;http://+
ENV ASPNETCORE_HTTPS_PORT=443
ENTRYPOINT ["dotnet", "Web.dll"]
