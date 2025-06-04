The API is built with **.NET 8** and supports full **CRUD operations** using **Entity Framework Core** with an **in-memory database**. It follows **REST API standards** and includes automated unit tests with code coverage.

## Architecture Overview
This project follows an **n-layered architecture** that separates concerns across multiple layers such as controllers, services, data access, and models. This promotes modularity, maintainability, and testability.

The solution is organized into two main projects:

- **FlightInformationAPI** – The main Web API project.
- **UnitTests.Tests** – The unit test project using xUnit and Moq.

## Folder Structure
```
FlightInformationAPI/
│
├── Controllers/               # Handles HTTP requests (FlightController)
├── Data/                      # CSV data files (e.g., FlightInformation.csv)
├── DBContext/                 # EF Core DB context configuration
├── DTOs/                      # Data Transfer Objects (Create, Update, Read)
├── Enumerations/              # Enum declarations (FlightStatus)
├── Interfaces/                # Abstractions for services and data loaders
├── Mapper/                    # AutoMapper profiles for DTO<->Model mapping
├── Middlewares/               # Centralized exception handling middleware
├── Models/                    # Domain entities (e.g., Flight)
├── Services/                  # Business logic (service and CSV loader)
├── Validators/                # FluentValidation validators for DTOs
├── appsettings.json           # Application configuration file
└── Program.cs                 # Application startup and middleware registration
```
The structure follows an **n-layered approach** with a clear **separation of concerns**, enabling easier testing, scaling, and maintenance.

## **How to Run the API**
1. Clone the repository using the following command: 
   <pre>git clone https://github.com/sabirshah/FlightAPI-DotNet8-Coverage.git</pre>
2. Navigate to the main project directory and then to the src folder: 
   <pre>cd FlightInformationAPI/src</pre>
3. Restore the dependencies using: 
   <pre>dotnet restore</pre>
4. Run the application using: 
   <pre>dotnet run</pre>
5. You will see output similar to: Now listening on: 
   <pre>https://localhost:53521</pre>
6. Open your browser and go to: 
   <pre>https://localhost:53521/swagger</pre>
   This will open the Swagger documentation.

## **How to Run Unit Tests**
1. Navigate to the tests directory: 
   <pre>cd ../tests</pre>
2. Run the unit tests using: 
   <pre>dotnet test</pre>
   
## **How to Check Code Coverage**
1. Open PowerShell.
2. Bypass the execution policy (only needed once per session) using:
   <pre>Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process</pre>
3. Run the coverage script:
   <pre>.\generate-coverage.ps1</pre>
4. After running the script, a browser window will open showing the HTML report with detailed coverage metrics.
![image](https://github.com/user-attachments/assets/17042fcf-19c9-42de-8258-9b58cd27bdd9)

## **How to Generate a Postman Collection**
1. Start the API and access the Swagger documentation at:
   <pre>https://localhost:53521/swagger/v1/swagger.json</pre>
2. Copy the Swagger JSON URL.
3. Open Postman, go to the Import menu, choose Link, and paste the Swagger URL.
4. Postman will generate a collection of all available API endpoints based on the Swagger definition.



