# LekPing Server

A lightweight, modular **.NET (C#) Web API** for medication reminders: medicines, schedules, and intake logs. The goal is a clean, testable backend you can extend into a full reminder SaaS (email/push/SMS).

> Note on code comments: parts of the code may still have Polish comments. The app logic is language‑agnostic; you can gradually translate comments without changing behavior. See [FAQ](#faq) for tips.

---

## Table of Contents

* [Why](#why)
* [Features](#features)
* [Architecture](#architecture)
* [Project Structure](#project-structure)
* [Tech Stack](#tech-stack)
* [Getting Started](#getting-started)
* [Configuration](#configuration)
* [Run with Docker](#run-with-docker)
* [Testing](#testing)
* [API Outline (MVP)](#api-outline-mvp)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
* [License](#license)
* [FAQ](#faq)

---

## Why

Medication adherence is hard. LekPing Server provides a small, focused API to:

* register medicines and their dose information,
* build flexible taking schedules (daily, interval‑based, or custom rules),
* generate a day view of planned intakes,
* confirm, skip, or edit intake logs.

This codebase favors simplicity, explicit domain types, and a vertical‑slice feature layout.

## Features

* CRUD for **medicines** (name, strength, unit, form, notes)
* CRUD for **schedules** per medicine (frequency, times of day, start/end)
* **Intake logs** (taken / skipped, timestamp, dose)
* Health endpoint for liveness
* Unit & integration tests scaffolding

> Authentication, notifications, and persistence in a production DB are on the roadmap.

## Architecture

A pragmatic "clean-ish" approach:

* **Domain** – entities, value objects, and domain services. No web/ORM dependencies.
* **Features/** – vertical slices (e.g. `Meds`) that own their controllers, DTOs, handlers, and registration.
* **Tests/** – unit tests for domain logic and integration tests for the API using `WebApplicationFactory`.

This keeps domain rules testable and features composable.

## Project Structure

```
lekping-server/
├─ Domain/                      # domain entities & services (pure C#)
├─ Features/
│  └─ Meds/                     # vertical slice for medicines (controllers, DTOs, etc.)
├─ tests/
│  └─ Lekping.Server.Tests/     # unit/integration tests
├─ lekping-server.sln           # solution file
└─ README.md
```

## Tech Stack

* **Language**: C# (.NET 8+)
* **Web**: ASP.NET Core Web API
* **Validation**: FluentValidation or data annotations (pick one and standardize)
* **Mapping**: Mapster/AutoMapper or manual mapping (your call)
* **Logging**: `Microsoft.Extensions.Logging` (console by default)
* **DB**: In‑memory/dev provider now; **PostgreSQL + EF Core** planned
* **Tests**: xUnit + FluentAssertions (recommended)

## Getting Started

**Prereqs:**

* .NET SDK 8 (or newer)

**Build & run:**

```bash
# clone
git clone https://github.com/SculptTechProject/lekping-server.git
cd lekping-server

# build
dotnet build

# run (adjust path if your startup project differs)
dotnet run --project ./lekping-server/Lekping.Server
```

The API will listen on the port configured in your launch profile; expose a simple `/health` endpoint.

### Useful commands

```bash
# format (if you add dotnet‑format)
dotnet format

# upgrade packages (NuGet)
dotnet list package --outdated
```

## Configuration

Use typical ASP.NET Core config layering:

* `appsettings.json` – defaults
* `appsettings.Development.json` – local overrides
* environment variables for secrets/connection strings

Suggested variables (when DB/auth land):

```
ASPNETCORE_ENVIRONMENT=Development
DB__CONNECTION=Host=localhost;Database=lekping;Username=postgres;Password=postgres
AUTH__JWT__ISSUER=...
AUTH__JWT__AUDIENCE=...
AUTH__JWT__KEY=...
```

## Run with Docker

> Optional scaffold — adapt as your project evolves.

**Dockerfile** (example):

```dockerfile
# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","Lekping.Server.dll"]
```

**docker-compose.yml** (example with Postgres):

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      DB__CONNECTION: Host=db;Database=lekping;Username=postgres;Password=postgres
    depends_on: [db]

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: lekping
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

## Testing

Run the full suite:

```bash
dotnet test
```

Guidelines:

* Domain tests: no I/O, no ASP.NET dependencies
* API tests: `WebApplicationFactory` + minimal seeding
* Use builders/fixtures to keep tests expressive

## API Outline (MVP)

> Precise contracts evolve with implementation. Below is a sketch to keep interfaces consistent.

**Medicines**

```
GET    /api/meds
POST   /api/meds
GET    /api/meds/{id}
PATCH  /api/meds/{id}
DELETE /api/meds/{id}
```

**Schedules**

```
GET    /api/meds/{id}/schedules
POST   /api/meds/{id}/schedules
PATCH  /api/schedules/{id}
DELETE /api/schedules/{id}
```

**Intakes**

```
GET    /api/intakes?day=YYYY-MM-DD
POST   /api/intakes/{id}/confirm
POST   /api/intakes/{id}/skip
```

**Health**

```
GET /health
```

## Roadmap

* PostgreSQL + EF Core migrations
* Authentication (JWT) & multi‑user support
* HostedServices for schedule expansion + notifications (email/push/SMS)
* OpenAPI/Swagger with example payloads
* CI (GitHub Actions): build, test, image publish
* Observability: OpenTelemetry + exporter, Sentry for errors
* Role‑based access / multi‑tenant (if needed)

## Contributing

1. Create a branch: `feature/...` or `fix/...`
2. Keep vertical slices self‑contained
3. Write tests (unit + integration)
4. Submit a PR with a short summary and, if helpful, a small demo (gif/screenshot)

## License

MIT

## FAQ

**Q: Code comments are in Polish. Will it block contributors?**
A: Not at all. Logic and naming are consistent. Translate comments gradually (PRs welcome). Use English for public APIs, DTOs, and exceptions.

**Q: Why vertical slices instead of a classic layered MVC?**
A: Features stay cohesive and scale better. Domain rules remain testable, and you avoid a god‑service anti‑pattern.

**Q: Is the DB required?**
A: Not for a quick demo. In‑memory works for local testing; PostgreSQL lands with EF Core in the roadmap.
