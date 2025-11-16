# Código Postal Mexico API

A RESTful API service built with .NET 9 that provides access to Mexican postal code information, including states (estados), municipalities (municipios), and neighborhoods (colonias).

## 📋 Table of Contents

-   [Features](#features)
-   [Architecture](#architecture)
-   [Prerequisites](#prerequisites)
-   [Getting Started](#getting-started)
    -   [Running with Docker](#running-with-docker)
    -   [Running Locally](#running-locally)
-   [API Endpoints](#api-endpoints)
-   [Configuration](#configuration)
-   [Project Structure](#project-structure)
-   [Technologies](#technologies)

## ✨ Features

-   **RESTful API** for Mexican postal code data
-   **Clean Architecture** with CQRS pattern using MediatR
-   **Snake case naming** for JSON properties
-   **API Versioning** support
-   **Swagger/OpenAPI** documentation
-   **Health checks** endpoint
-   **Docker** support with multi-stage builds
-   **Validation** using FluentValidation
-   **Exception handling** middleware
-   **CORS** enabled

## 🏗️ Architecture

The project follows Clean Architecture principles with the following layers:

-   **Api**: Web API layer with controllers, filters, and middleware
-   **Application**: Business logic with CQRS handlers
-   **Core**: Domain entities, DTOs, and interfaces
-   **Infrastructure**: External service implementations

## 📦 Prerequisites

### For Docker deployment:

-   [Docker](https://www.docker.com/get-started) (version 20.10 or higher)
-   [Docker Compose](https://docs.docker.com/compose/install/) (version 1.29 or higher)

### For local development:

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   PowerShell (for Playwright browser automation)

## 🚀 Getting Started

### Running with Docker

#### Development Environment

1. **Clone the repository**

    ```bash
    git clone https://github.com/SSR-Med/codigo-postal-mexico.git
    cd codigo-postal-mexico
    ```

2. **Run with Docker Compose (Development)**

    ```bash
    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
    ```

    This will:

    - Build the Docker image
    - Start the container on port **8180**
    - Mount the logs directory for persistence
    - Set up the development environment

3. **Access the API**

    - API Base URL: `http://localhost:8180/api`
    - Swagger Documentation: `http://localhost:8180/swagger`
    - Health Check UI: `http://localhost:8180/actuator`

4. **View logs**

    ```bash
    docker-compose logs -f api
    ```

5. **Stop the service**
    ```bash
    docker-compose down
    ```

#### Production Environment

1. **Create the external network** (if not already created)

    ```bash
    docker network create infra-network
    ```

2. **Run with Docker Compose (Production)**

    ```bash
    docker-compose up -d
    ```

    This will:

    - Build the Docker image
    - Start the container on port **8380**
    - Connect to the `infra-network` external network
    - Auto-restart on failures

3. **Access the API**
    - API Base URL: `http://localhost:8380/api`
    - Swagger Documentation: `http://localhost:8380/swagger`

#### Custom Port Configuration

To run on a different port, modify the `docker-compose.override.yml` or `docker-compose.dev.yml` file:

```yaml
services:
    api:
        ports:
            - "YOUR_PORT:8180" # Change YOUR_PORT to desired port
        environment:
            - ASPNETCORE_URLS=http://+:8180
```

#### Docker Commands Cheat Sheet

```bash
# Build the image
docker-compose build

# Start in detached mode
docker-compose up -d

# View running containers
docker ps

# Stop containers
docker-compose down

# View logs
docker-compose logs -f api

# Restart the service
docker-compose restart

# Remove containers and volumes
docker-compose down -v

# Rebuild and start
docker-compose up -d --build
```

### Running Locally

1. **Clone the repository**

    ```powershell
    git clone https://github.com/SSR-Med/codigo-postal-mexico.git
    cd codigo-postal-mexico
    ```

2. **Restore dependencies**

    ```powershell
    dotnet restore
    ```

3. **Run the application**

    ```powershell
    dotnet run --project Api/Api.csproj
    ```

4. **Access the API**
    - Default URL will be shown in the console (typically `http://localhost:5000` or `https://localhost:5001`)
    - Swagger UI: `http://localhost:5000/swagger`

## 🔌 API Endpoints

### States (Estados)

#### Get All States

```http
GET /api/estados
```

Returns a list of all Mexican states.

**Response:**

```json
[
  {
    "codigo": "01",
    "nombre": "Aguascalientes"
  },
  ...
]
```

### Municipalities (Municipios)

#### Get Municipalities by State

```http
GET /api/estados/{codigo_estado}/municipios
```

Returns all municipalities for a specific state.

**Parameters:**

-   `codigo_estado` (path) - State code (e.g., "09" for CDMX)

**Response:**

```json
{
  "codigo_estado": "09",
  "nombre_estado": "Ciudad de México",
  "municipios": [
    {
      "codigo": "002",
      "nombre": "Azcapotzalco"
    },
    ...
  ]
}
```

### Neighborhoods (Colonias)

#### Get Neighborhoods by Municipality

```http
GET /api/estados/{codigo_estado}/municipios/{codigo_municipio}/colonias
```

Returns all neighborhoods for a specific municipality.

**Parameters:**

-   `codigo_estado` (path) - State code
-   `codigo_municipio` (path) - Municipality code

**Response:**

```json
{
  "codigo_estado": "09",
  "nombre_estado": "Ciudad de México",
  "codigo_municipio": "002",
  "nombre_municipio": "Azcapotzalco",
  "colonias": [
    {
      "codigo": "00100",
      "nombre": "San Martín Xochinahuac"
    },
    ...
  ]
}
```

### Health Check

```http
GET /actuator
```

Health check UI for monitoring service status.

## ⚙️ Configuration

The application can be configured through `appsettings.json`:

### Swagger Configuration

```json
{
    "Swagger": {
        "DocumentTitle": "API CODIGO POSTAL MEXICO",
        "Version": "v1.0.0",
        "RoutePrefix": "swagger"
    }
}
```

### Web Scraping Configuration

```json
{
    "CodigoPostal": {
        "BaseUrl": "https://www.correosdemexico.gob.mx/sslservicios/consultacp/descarga.aspx",
        "TimeoutPage": 30000,
        "TimeoutElement": 5000,
        "RetryCount": 10,
        "RetryDelayMilliseconds": 3000
    }
}
```

### Environment Variables

-   `ASPNETCORE_ENVIRONMENT`: Environment name (Development/Staging/Production)
-   `ASPNETCORE_URLS`: URLs the server listens on
-   `APPDATA`: Application data directory

## 📁 Project Structure

```
codigo-postal-mexico/
├── Api/                          # Web API layer
│   ├── Controllers/              # API Controllers
│   ├── Filters/                  # Action filters and operation filters
│   ├── Middlewares/              # Custom middleware
│   ├── Configurations/           # Service configurations
│   ├── Binders/                  # Model binders
│   └── Policies/                 # Naming policies
├── Application/                  # Business logic layer
│   ├── Features/                 # CQRS handlers organized by feature
│   │   └── Geo/                  # Geographic data features
│   │       └── Queries/          # Query handlers
│   └── Behaviours/               # MediatR pipeline behaviors
├── Core/                         # Domain layer
│   ├── Dtos/                     # Data Transfer Objects
│   ├── Exceptions/               # Custom exceptions
│   ├── Interfaces/               # Contracts and interfaces
│   └── Extensions/               # Extension methods
├── Infrastructure/               # Infrastructure layer
│   └── Services/                 # External service implementations
├── docker-compose.yml            # Production Docker configuration
├── docker-compose.dev.yml        # Development Docker configuration
├── Dockerfile                    # Multi-stage Docker build
└── CodigoPostal.sln             # Solution file
```

## 🛠️ Technologies

-   **.NET 9.0** - Framework
-   **ASP.NET Core** - Web API framework
-   **MediatR** - CQRS implementation
-   **FluentValidation** - Input validation
-   **Swashbuckle** - Swagger/OpenAPI documentation
-   **Playwright** - Browser automation for web scraping
-   **Docker** - Containerization
-   **HealthChecks** - Service health monitoring

## 📝 Additional Notes

### Header Validation

The API includes middleware for validating custom headers:

-   `X-Channel-Id`: Channel identifier
-   `X-Correlation-Id`: Request correlation tracking
-   `X-Request-Id`: Unique request identifier

### Snake Case Naming Convention

All API responses use snake_case naming convention:

```json
{
    "codigo_estado": "09",
    "nombre_estado": "Ciudad de México"
}
```

### Logs

Application logs are stored in the `logs/` directory, which is mounted as a volume in Docker.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

## 👥 Author

**SSR-Med**

---

For issues or questions, please open an issue on the [GitHub repository](https://github.com/SSR-Med/codigo-postal-mexico/issues).
