# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos de la solución y proyectos
COPY *.sln .
COPY Api/*.csproj Api/
COPY Application/*.csproj Application/
COPY Core/*.csproj Core/
COPY Infrastructure/*.csproj Infrastructure/

# Restaurar dependencias de toda la solución
RUN dotnet restore

# Copiar el resto de los archivos
COPY . .

# Construir la aplicación
RUN dotnet publish Api/Api.csproj -c Release -o /out --no-restore

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
RUN mkdir -p /app/tmp

# Instalar dependencias necesarias para Playwright
RUN apt-get update && apt-get install -y \
    libnss3 \
    libnspr4 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libasound2 \
    libpango-1.0-0 \
    libcairo2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out ./

# Instalar PowerShell y Playwright browsers en runtime
RUN apt-get update && \
    apt-get install -y wget apt-transport-https software-properties-common && \
    wget -q "https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb" && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y powershell && \
    rm packages-microsoft-prod.deb && \
    pwsh -Command "Set-PSRepository -Name PSGallery -InstallationPolicy Trusted" && \
    pwsh /app/playwright.ps1 install --with-deps chromium && \
    rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=dev
ENV ASPNETCORE_URLS=http://+:8380
EXPOSE 8380

ENTRYPOINT ["dotnet", "Api.dll"]