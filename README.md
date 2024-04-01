# eclipseworks
**EclipseWorks - Task Management System API**

**Overview**

This Task Management System API is a .NET Core application designed to help users organize and monitor their daily tasks, as well as collaborate with teammates on various projects. It supports managing projects and tasks, including creation, viewing, updating, and deletion functionalities.

**Getting Started**

**Prerequisites**

- .NET 6.0 or later
- Docker

**Installation**

1. **Clone the repository:**

```git clone “https://github.com/utrabo/eclipseworks.git”```

1. **Navigate to the project directory:**

```cd eclipseworks\EclipseWorks.TasksManagementSystem```

1. **Run the docker.sh script to build and run the Docker container:**

```./docker.sh ```

This script automates the Docker build and run process.

**Running the Application**

After installation, the application can be accessed at **http://localhost:8000/swagger**.

**Usage**

Two users are automatically created in the initialization of the application:

```
- UserID: 1 – Common User
- UserID: 2 – Manager
```

Those users IDs can be used as parameters for the API endpoints.
This API provides the following endpoints:

- **Projects:**
  - **GET /api/users/{userId}/projects** - Retrieve all projects for a user.
  - **POST /api/projects** - Create a new project.
  - **PUT /api/projects/{projectId}** - Update a project.
  - **DELETE /api/projects/{projectId}** - Delete a project.
- **Tasks:**
  - **GET /api/projects/{projectId}/tasks** - Retrieve all tasks for a project.
  - **POST /api/projects/{projectId}/tasks** - Add a new task to a project.
  - **PUT /api/projects/{projectId}/tasks/{taskId}** - Update a task.
  - **DELETE /api/projects/{projectId}/tasks/{taskId}** - Remove a task from a project.
- **Comments:**
  - **POST /api/tasks/{taskId}/comments** - Add a comment to a task.
- **Performance Reports:**
  - **GET /api/dashboard/{managerUserId}/performance/{userId}** - Get completed tasks per user for the last 30 days.

**Future Enhancements**

**Questions for the Product Owner (PO)**

- **Refinement Questions:**
  - How do we envision integrating this API with other services, particularly for authentication?
  - Are there any specific performance metrics we aim to achieve in terms of API response times?
  - How will the roles and permissions be managed, and what impact does this have on our current API design?

**Proposed Improvements**

- **Logging:** Implement logging using Serilog for better traceability and debugging.
- **Database:** Setup a properly managed database service, considering scalability and security. Removing the database initialization logic that was created only to facilitate local testing.
- **Instrumentation:** Utilize Application Insights or similar for telemetry, to monitor and analyze API usage patterns.
- **Security:** While authentication is not within this project's scope, outlining potential security implementations or service integrations would be beneficial.

