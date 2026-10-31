# PetPortal — Backend

**PetPortal** is a web platform where beginner developers find a team, join pet projects and get their first real experience of building software together, experience they can put in a portfolio or CV.

This repository contains the ASP.NET Core API. The web client lives in [Petportal](https://github.com/MechanicalDeathtrap/Petportal).

## Why PetPortal

Competition for entry-level IT jobs is high and keeps growing. Employers don't want to hire people who still need training. Junior candidates often can't even get an interview because they have no real development experience, and they can't get that experience without a job. For employers, finding the right candidate costs a lot of time and money.

Plenty of developers also have a project idea but no team, and find it hard to put into words what kind of teammates and skills the project needs.

PetPortal helps developers start a career by practicing on real projects. On PetPortal they can find a team for a startup and find or create a project worth working on. The project was started by students of the Institute of Information Technology and Intelligent Systems at Kazan Federal University, who ran into this problem themselves. The institute's students are the first audience. The plan is to expand to other universities after that.

## Goals

- **Build a team around an idea.** A beginner developer publishes a project idea and gathers a team to build it.
- **Gain first experience.** Developers join existing pet projects, work together on them and add the result to their portfolio.
- **Connect developers and companies.** Companies use the platform to find young developers, publish technical tasks, get MVPs built and bring people to hackathons.
- **Make requirements easy to write.** An AI assistant on the project creation page will help pick a tech stack, decide which roles the team needs and list the skills each role requires (planned).
- **Be sustainable.** The platform earns money from paid project placement, promotion packages, partner advertising (jobs, hiring, education) and an AI assistant subscription.

## Who it is for

| Audience | What PetPortal gives them |
|---|---|
| Students and beginner developers | Practice on real projects, portfolio cases, a team to work with |
| Experienced and independent developers | Co-authors or a team, a place to try new technologies, a way to hand frontend/backend tasks to less experienced developers |
| Startups | A team to build and test an idea |
| Companies | A hiring pipeline, outsourced MVPs and prototypes, hackathon participants |

## Features

Already implemented:

- Project catalogue with search, filters (role, commercial/non-commercial, budget, tags, archived) and sorting
- Commercial and non-commercial projects, each with its own card and flow
- Create and edit projects: description, plan, requirements, expected result, team roles, tags, budget and deadlines
- Paid project placement through YooKassa
- Projects are archived automatically once their application deadline has passed
- Applications to projects (responses) and reviewing them as the project owner
- Favourites
- Real-time chat (SignalR) with email notifications about unread messages
- Sign-up with email confirmation, sign-in (JWT in a cookie), password recovery
- Personal account: my projects, responses, favourites, settings, avatar upload

Planned:

- AI assistant for describing a project idea, choosing the stack and defining team roles and requirements
- Promotion packages (Lite, Standard, Pro) to raise and pin a project in search results
- Partner advertising

## Tech stack

**Backend (this repository):** C# / ASP.NET Core 8 Web API, Entity Framework Core + PostgreSQL 17, Redis (cache), MinIO (S3-compatible file storage), RabbitMQ, SignalR, JWT authentication, YooKassa payments, Swagger.

**Frontend:** React 18 + TypeScript, Vite, MobX, Axios, MUI, SASS + CSS Modules, Formik + Yup, SignalR client.

## Solution structure

| Project | Purpose |
|---|---|
| `PetPortalAPI` | Controllers, SignalR chat hub, app startup and DI (`Program.cs`) |
| `PetPortalApplication` | Services with the business logic, background jobs |
| `PetPortalCore` | Domain models, DTOs, contracts, service and repository interfaces |
| `PetPortalDAL` | EF Core `DbContext`, entities, repositories, migrations, seed data |

## Getting started

Requirements: .NET 8 SDK and Docker.

1. Start the infrastructure (PostgreSQL on port 5433, Redis, MinIO, RabbitMQ). In `PetPortalAPI/PetPortalAPI` run:

   ```bash
   docker compose up -d
   ```

2. Start the API from the same folder:

   ```bash
   dotnet run --launch-profile http
   ```

   Pending migrations are applied automatically at startup. In the Development environment the database is seeded with demo users, projects and tags. Swagger is available at http://localhost:5140/swagger.

| Service | Address |
|---|---|
| API / Swagger | http://localhost:5140/swagger |
| MinIO console | http://localhost:9001 |
| RabbitMQ management | http://localhost:15672 |

### Migrations

Run these commands from the repository root.

Apply migrations manually:

```bash
dotnet ef database update -s ./PetPortalAPI/PetPortalAPI/ -p ./PetPortalDAL/
```

Create a new migration:

```bash
dotnet ef migrations add MigrationName -s ./PetPortalAPI/PetPortalAPI/ -p ./PetPortalDAL/
```
