# Issue Tracker

A full-stack issue & project management app (think a lightweight Jira) with a drag-and-drop Kanban board, role-based permissions, and a Clean Architecture .NET backend.

[![CI](https://github.com/dotNetGaijin87/IssueTracker/actions/workflows/ci.yml/badge.svg)](https://github.com/dotNetGaijin87/IssueTracker/actions/workflows/ci.yml)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-18-61DAFB?logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE.md)

<img src="./assets/issue_1.jpg" >

<img src="./assets/kanban_1.jpg" >

## Overview

Issue Tracker lets teams organise work into **projects**, break projects down into **issues**, collaborate through **comments**, and manage who can do what via **per-issue permissions**. Issues move through their workflow on a drag-and-drop **Kanban board**, and authentication/authorization is handled by **Auth0**.

The project's main purpose is to demonstrate a production-shaped codebase: a layered **Clean Architecture** backend using the **CQRS** pattern, a typed **React + TypeScript** frontend, and a test suite covering unit and integration levels.

## Features

- 📋 **Projects, issues & comments** — full CRUD with server-side validation
- 🔀 **Kanban board** — drag issues between workflow columns; positions persist
- 🔐 **Auth0 authentication** + role/permission-based authorization
- 🧮 **Filtering, sorting & pagination** on list views
- 🌍 **i18n-ready** UI (react-i18next)
- 📝 **Markdown** issue descriptions and comments
- 🩺 **Health-check** endpoint for container orchestration

## Architecture

The backend follows **Clean Architecture**, splitting the solution into four projects with dependencies pointing inward toward the domain:

```
┌───────────────────────────────────────────────────────────┐
│  IssueTracker (Web API)   controllers, DI wiring, auth      │
│  ┌─────────────────────────────────────────────────────┐  │
│  │  Application   CQRS commands/queries, validation,     │  │
│  │                MediatR pipeline behaviours, mapping   │  │
│  │  ┌───────────────────────────────────────────────┐  │  │
│  │  │  Domain   entities & enums (no dependencies)    │  │  │
│  │  └───────────────────────────────────────────────┘  │  │
│  └─────────────────────────────────────────────────────┘  │
│  Infrastructure   EF Core DbContext, persistence, services  │
└───────────────────────────────────────────────────────────┘
```

Every request flows through a **MediatR** pipeline of cross-cutting behaviours before it reaches its handler:

```
HTTP request → Controller → MediatR → [ Authorization → Validation → Performance ] → Handler → EF Core
```

- **Authorization behaviour** — checks the caller's role/permissions against attributes on the request
- **Validation behaviour** — runs FluentValidation validators and short-circuits on failure
- **Performance behaviour** — logs slow requests (>1s)

## Tech Stack

|   |  |   |   |   |
|---|---|---|---|---|
|  Frontend | <div align="center"><img src="./assets/react_logo.png" width="100" height="100"></br>React 18</div> | <div align="center"><img src="./assets/ts_logo.png" width="100" height="100"></br>TypeScript</div>  |  <div align="center"><img src="./assets/mui_logo.png" width="100" height="100"></br>Material UI</div> | <div align="center"><img src="./assets/auth0_logo2.png" width="100" height="100"></br>Auth0</div>  |
| Backend  | <div align="center"><img src="./assets/dotnet_logo.png" width="100" height="100"></br>.NET 8</div>| <div align="center"><img src="./assets/csharp_logo.png" width="100" height="100"></br>C#</div> | <div align="center"><img src="./assets/ef_logo.png" width="100" height="100"></br>EF Core</div>  | <div align="center"><img src="./assets/x_unit.png" width="100" height="100"></br>xUnit</div>  |

**Also using:** MediatR (CQRS), FluentValidation, AutoMapper, Vite, react-hook-form, Docker, SQL Server.

## Engineering decisions

- **CQRS with MediatR** keeps each use case in its own command/query handler — small, independently testable units rather than fat services. Each feature folder co-locates its command, handler, validator and exception.
- **Clean Architecture** keeps business logic free of framework and database concerns; the Domain project has no external dependencies, which makes the core logic trivial to unit-test.
- **Pipeline behaviours** centralise cross-cutting concerns (auth, validation, logging) so handlers stay focused on a single responsibility.
- **RFC 7807 Problem Details** are returned for errors, giving the SPA a consistent error contract.
- **Typed API adapter** on the frontend wraps Axios so components never touch raw HTTP.

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- SQL Server 2019+ (or SQL Server Express / LocalDB)

### Backend

Create your local settings from the template and point the connection string at your
SQL Server (environment-specific settings are git-ignored, never committed):

```bash
cd server/src/IssueTracker
cp appsettings.Development.json.example appsettings.Development.json
# edit the ConnectionStrings, then:
dotnet run
```

The connection string can also be supplied via an environment variable
(`ConnectionStrings__DefaultConnection`) or [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets),
which is preferred for anything sensitive.

### Frontend

```bash
cd client
cp .env.example .env   # API URL + Auth0 settings (see .env.example)
npm install
npm run dev
```

The app starts on http://localhost:3000 and talks to the backend API.

### Demo login

A read-only demo account is available via Auth0:

> Email: www.admin@gmail.com </br>
> Password: Admin12#$

> _Note:_ this is a throwaway demo account on a development Auth0 tenant, kept here so
> reviewers can try the app. No application secrets are committed to the repo — the
> backend validates Auth0 JWTs using only the public authority/audience, and the
> frontend uses public SPA values.

### Docker

Both the API and the client ship with Dockerfiles:

```bash
docker build -t issuetracker-api -f Dockerfile .
docker build -t issuetracker-web ./client
```

## Tests

Unit tests run without any external dependencies:

```bash
dotnet test server/tests/UnitTests/IssueTracker.ApplicationTests
dotnet test server/tests/UnitTests/IssueTracker.InfrastructureTests
```

Integration tests spin up the API in-memory via `WebApplicationFactory` and require a SQL Server instance:

```bash
dotnet test server/tests/IntegrationTests/IssueTracker.IntegrationTests
```

The backend build + unit tests and the frontend build run automatically on every push via [GitHub Actions](./.github/workflows/ci.yml).

## License

[MIT](./LICENSE.md)
