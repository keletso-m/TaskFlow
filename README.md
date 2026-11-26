# TaskFlow

**A modern project management platform built with C# and .NET.**

TaskFlow is a Jira-inspired project and task management system designed to help software development teams organise work, manage projects, collaborate efficiently and track development progress.

The platform provides a centralised workspace where teams can create projects, assign tasks, manage workflows and monitor progress through an intuitive board-based interface.

Built with **ASP.NET Core, C#, Blazor and PostgreSQL**, TaskFlow focuses on maintainable architecture, secure application design and practical software engineering principles.

<p align="center">

  ![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
  ![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=csharp&logoColor=white)
  ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
  ![Blazor](https://img.shields.io/badge/Blazor-Framework-512BD4?style=for-the-badge&logo=blazor&logoColor=white)

</p>

---

## Overview

Managing software development projects requires more than simply creating tasks and assigning deadlines. Teams need visibility into their workloads, clear ownership of responsibilities and a reliable way to monitor progress.

TaskFlow aims to address these challenges by combining project organisation, task management and workflow tracking into a single application.

The platform is designed around three core concepts:

| Component | Description |
|---|---|
| Projects | Organise work into individual development projects. |
| Tasks | Represent individual work items, bugs, features and improvements. |
| Boards | Visualise project workflows and monitor task progress. |

## Features

The following functionality is planned for TaskFlow.

### Project Management
- Create and manage multiple projects.
- Organise team members within projects.
- Track project progress and completion.
- Manage project descriptions and deadlines.

### Task & Issue Management
- Create, update and delete tasks.
- Assign tasks to team members.
- Define task priorities and deadlines.
- Support different task types, including features, bugs and improvements.
- Create subtasks and track dependencies.
- Add comments and descriptions.

### Workflow Management
- Kanban-style task boards.
- Customisable task statuses.
- Move tasks between workflow stages.
- Filter and search tasks.
- Track task history and status changes.

### Authentication & Authorisation
- User registration and authentication.
- Role-based access control.
- Project-specific permissions.
- Secure API endpoints.
- Protected application resources.

### Reporting & Activity Tracking
- Project progress dashboards.
- Task distribution by status and priority.
- User activity history.
- Project completion statistics.

---

## Technology Stack

TaskFlow is built entirely within the Microsoft .NET ecosystem.

| Technology | Responsibility |
|---|---|
| C# | Primary programming language |
| .NET 10 | Application framework |
| ASP.NET Core Web API | Backend services and REST endpoints |
| Blazor | Interactive frontend |
| Entity Framework Core 10 | Database access and ORM |
| PostgreSQL | Relational database |
| ASP.NET Core Identity | Identity and user management |
| SignalR | Real-time communication |
| xUnit | Unit testing |
| Docker | Containerisation |
| GitHub Actions | CI/CD automation |

---

## Architecture

TaskFlow follows Clean Architecture principles, separating business logic, application functionality, infrastructure and presentation concerns.

This approach aims to keep the application maintainable, testable and independent of specific infrastructure implementations.

### Solution Structure

```text
TaskFlow/
│
├── src/
│   │
│   ├── TaskFlow.Web/
│   │   └── Blazor application
│   │
│   ├── TaskFlow.API/
│   │   └── REST API and endpoints
│   │
│   ├── TaskFlow.Core/
│   │   └── Domain entities and interfaces
│   │
│   ├── TaskFlow.Application/
│   │   └── Use cases, services and validation
│   │
│   └── TaskFlow.Infrastructure/
│       └── Database and external services
│
├── tests/
│   │
│   ├── TaskFlow.UnitTests/
│   │
│   └── TaskFlow.IntegrationTests/
│
├── docker-compose.yml
├── .gitignore
└── README.md
```

### Architectural Responsibilities

- **Core:** Contains domain entities, business rules and abstractions.
- **Application:** Implements application use cases and coordinates business operations.
- **Infrastructure:** Handles persistence, Entity Framework Core and external integrations.
- **API:** Exposes application functionality through RESTful endpoints.
- **Web:** Provides the user interface through Blazor components.

---

## Database Design

The application will use PostgreSQL as its primary relational database, with Entity Framework Core handling persistence and database migrations.

The initial domain model is expected to include:

| Entity | Purpose |
|---|---|
| User | Application users and identity information |
| Project | Individual projects |
| ProjectMember | User membership and project roles |
| Task | Project tasks and work items |
| TaskComment | Task discussions |
| TaskLabel | Task categorisation |
| TaskActivity | Historical task changes |
| TaskDependency | Relationships between tasks |

Database relationships and constraints will be designed to maintain data integrity and enforce appropriate access boundaries.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)
- An IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

**1. Clone the repository**

```bash
git clone https://github.com/YOUR_USERNAME/TaskFlow.git
```

**2. Navigate to the project directory**

```bash
cd TaskFlow
```

**3. Restore dependencies**

```bash
dotnet restore
```

**4. Configure the database**

Configure your PostgreSQL connection string using .NET user secrets or environment variables.

**5. Apply database migrations**

```bash
dotnet ef database update
```

**6. Start the application**

```bash
dotnet run --project src/TaskFlow.API
```

> Installation instructions will be updated as the application structure and deployment configuration are finalised.

---

## Development Roadmap

TaskFlow will be developed incrementally, prioritising core functionality before introducing more advanced features.

- [ ] **Phase 1 – Foundation**
  - [x] Initialise solution and project structure.
  - [x] Configure PostgreSQL and EF Core.
  - [x] Implement domain entities.
  - [x] Establish application architecture.

- [ ] **Phase 2 – Authentication & Access**
  - [x] Implement user registration and login.
  - [x] Configure authentication.
  - [x] Implement role-based authorisation.
  - [x] Establish project membership and permissions.

- [ ] **Phase 3 – Project & Task Management**
  - [x] Implement project management.
  - [ ] Implement task creation and assignment.
  - [ ] Implement priorities and statuses.
  - [ ] Add task comments and labels.

- [ ] **Phase 4 – Workflow & Collaboration**
  - [ ] Develop Kanban boards.
  - [ ] Implement task filtering and searching.
  - [ ] Add activity tracking.
  - [ ] Implement real-time updates using SignalR.

- [ ] **Phase 5 – Testing & Deployment**
  - [ ] Write unit tests.
  - [ ] Implement integration tests.
  - [ ] Add structured logging and exception handling.
  - [ ] Containerise the application with Docker.
  - [ ] Configure GitHub Actions CI/CD.
  - [ ] Deploy the application.

---

## Engineering Focus

Beyond implementing application features, TaskFlow is intended to demonstrate practical software engineering concepts:

- Clean Architecture and SOLID principles.
- Object-oriented programming and domain modelling.
- Dependency Injection and service lifetimes.
- Asynchronous programming with `async` and `await`.
- RESTful API design and HTTP fundamentals.
- Entity Framework Core and relational database design.
- Authentication, authorisation and application security.
- Data validation and exception handling.
- Unit testing and integration testing.
- Logging, monitoring and deployment practices.

---

## Project Goals

TaskFlow is a software engineering portfolio project focused on building a realistic, maintainable application using modern C# and .NET technologies.

The project aims to demonstrate the ability to:

- Design and implement a multi-layered .NET application.
- Build and consume RESTful APIs.
- Model and manage relational data.
- Implement secure authentication and authorisation.
- Develop interactive web applications with Blazor.
- Apply software engineering principles to a practical problem.
- Test, containerise and deploy a software application.

---

## License

This project is currently under development. Licensing information will be added when the repository is ready for public release.

---

**TaskFlow** — Built with C#, ASP.NET Core and .NET 10.
