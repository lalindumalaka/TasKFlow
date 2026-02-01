# 🚀 TaskFlow Analytics

A sleek **Blazor WebAssembly + EF Core** task performance tracker. This project is a dedicated sandbox for mastering **CI/CD pipelines**, containerization, and database migration strategies.

---

### 🛠️ Tech Stack
* **UI:** Blazor WebAssembly (.NET 9)
* **API:** ASP.NET Core Web API
* **Data:** EF Core (PostgreSQL / SQL Server)
* **DevOps:** GitHub Actions & Multi-stage Docker

---

### 🏗️ Architecture
The solution uses a **Clean Architecture Lite** approach to ensure a seamless build-and-deploy pipeline:
* `TaskFlow.UI` — Client-side logic & UI.
* `TaskFlow.API` — Backend controllers & JWT auth.
* `TaskFlow.Data` — `DbContext`, Migrations, and Data Seeding.
* `TaskFlow.Shared` — Universal DTOs and entities.

---

### 🔄 CI/CD Practice Hooks
Built specifically to test your automation skills:
- [x] **Automated Builds:** Validating code on every push.
- [x] **Migration Bundling:** Generating idempotent SQL scripts for DB safety.
- [x] **Health Checks:** Built-in `/health` endpoint for deployment verification.
- [x] **Secret Management:** Practicing secure connection string handling.
- [x] **Containerization:** Optimized multi-stage Docker builds.

---

### 🚦 Quick Start
1.  **Clone the repo:**
    ```bash
    git clone [https://github.com/lalindumalaka/TasKFlow.git](https://github.com/lalindumalaka/TasKFlow.git)
    ```
2.  **Update Database:**
    ```bash
    dotnet ef database update --project TaskFlow.Data --startup-project TaskFlow.API
    ```
3.  **Run:**
    ```bash
    dotnet run --project TaskFlow.API
    ```

---

### 🧪 Pipeline Exercises
* **The Breaking Migration:** Add a `Required` field without a default to test migration failure.
* **Automated Rollback:** Configure GitHub Actions to stop if the health check fails.
* **Secret Rotation:** Practice moving DB keys from `appsettings` to GitHub Secrets.
