# **Saga**

Saga is a web-based information system designed to manage the registration and information of students enrolled in the MSc program in Computer Science at CEFET/RJ. The system is intended to replace the current spreadsheet-based approach, which has become increasingly cumbersome to manage as the volume of data grows.

### Architecture

The project follows a typical N-tier architecture, with the following layers:

- Models (Enities, mappers and DTOs)
- Presentation (controllers)
- Service (business logic)
- Data (repositories and database context)

- #### Model Layer
The models layer represent the data structures of the entities in the system, such as Student, Teacher, ResearchLine, and ResearchProject. These models are used to map the database tables and to perform CRUD (Create-Read-Update-Delete) operations on the data.
The models represent entities in the system, such as Student, Teacher, ResearchLine, and ResearchProject, and are used for mapping database tables and performing CRUD operations.

- #### Presentation Layer
The presentation layer includes controllers, which define API endpoints for data transfer between the API and service layer. 
Controllers handle HTTP requests and responses, executing the logic of the application. Each entity has its own controller, such as StudentsController, TeachersController, ResearchLinesController, and ResearchProjectsController, with endpoints for creating, reading, updating, and deleting data.

- #### Service Layer 
The service layer contains the business logic of the application. It communicates with the data layer (repositories and database context) to perform CRUD operations on the database. The services are used by the controllers to perform complex operations that require multiple database queries or complex calculations. In Saga, each entity has its own service, such as StudentService, TeacherService, ResearchLineService, and ResearchProjectService.

- #### Data Layer

The data layer consists of repositories and a database context. The repositories encapsulate the data access logic, while the database context provides a connection to the database and manages the entities.

The repository is responsible for communicating with the database. It uses Entity Framework to perform the CRUD operations on the database tables. In Saga, each entity has its own repository, such as StudentRepository, TeacherRepository, ResearchLineRepository, and ResearchProjectRepository. The repositories are used by the services to access the data.

### Technologies Used

- ASP.NET Core
- Entity Framework Core
- Npgsql (PostgreSQL database provider)
- Other technologies used in the project.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
Prerequisites

- .NET Core SDK
- PostgreSQL

### Installing

1. Clone the repository to your local machine.
2. Configure the database connection string in appsettings.json.
3. Run the database migrations to create the required tables in the database.
4. Run the project.
~~~
git clone git@github.com:radhanama/saga.git
cd saga
dotnet ef database update
dotnet run
~~~
### Usage

The application exposes the following endpoints:

    GET /api/students: Returns a list of all students.
    GET /api/students/{id}: Returns the student with the specified ID.
    POST /api/students: Creates a new student.
    PUT /api/students/{id}: Updates the student with the specified ID.
    DELETE /api/students/{id}: Deletes the student with the specified ID.


### Development Container

The Saga development container is a Docker container that includes all the required dependencies to run the Saga application, including PostgreSQL. Using a development container allows you to quickly set up your development environment without installing anything other than Docker and Visual Studio Code (VS Code). Additionally, it provides a consistent and reproducible development environment that is isolated from your local machine.

To use the development container, follow these steps:

1. Install Docker on your machine.
2. Clone the project repository to your local machine.
3. If you have the Remote - Containers extension installed, VS Code will prompt you to reopen the project in a container. Click "Reopen in Container" to continue.
4. If you don't see this prompt, open the Command Palette in VS Code (Ctrl/Cmd + Shift + P) and select "Remote-Containers: Reopen in Container" from the list of options.
5. In the file ./.devcontainer/devcontainer.json, you may need to modify the path to the SSH key if you are not running Linux.
6. Wait for the container to build and start up. This may take a few minutes the first time you run it.
7. Once the container is ready, open a terminal in VS Code and run the following command to create the database tables:
    ~~~
    dotnet ef database update
    ~~~

8. You can now run the project by pressing F5 or running the "Run" task in VS Code.
    ~~~
    dotnet run
    ~~~
9. The application will be available at http://localhost:5000.

#### Using the Dev Container

With the dev container running, you can interact with the application and the database just like you would on a local machine. You can use the VS Code editor to modify the code, and any changes you make will be automatically synced to the container.

To access the database, you can use a PostgreSQL client such as pgAdmin or DBeaver. The dev container exposes PostgreSQL on port 5432, so you can connect to it using the IP address 127.0.0.1 and port 5432. The default username and password are postgres and password, respectively.