The API is built with **.NET 8** and supports full **CRUD operations** using **Entity Framework Core** with an **in-memory database**. It follows **REST API standards** and includes automated unit tests with code coverage.

## Architecture Overview

The solution is organized into two main projects:

- **FlightInformationAPI** – The main Web API project.
- **UnitTests.Tests** – The unit test project using xUnit and Moq.

## Folder Structure
![image](https://github.com/user-attachments/assets/8e972f2a-18d5-44d1-a791-54fa800205d0)

The structure promotes **clean architecture**, **separation of concerns**, and **testability** through dependency injection and modular design.


## **How to Run the API**
1. Clone the repository using the following command: 
   git clone https://github.com/sabirshah/FlightInformationAPI.git
2. Navigate to the main project directory and then to the src folder: 
   cd FlightInformationAPI/src
3. Restore the dependencies using: 
   dotnet restore
4. Run the application using: 
   dotnet run
5. You will see output similar to: Now listening on: 
   https://localhost:53521
6. Open your browser and go to: 
   https://localhost:53521/swagger 
   This will open the Swagger documentation.

## **How to Run Unit Tests**
1. Navigate to the tests directory: 
   cd ../tests
2. Run the unit tests using: 
   dotnet test
   
## **How to Check Code Coverage**
1. Open PowerShell.
2. Bypass the execution policy (only needed once per session) using:
   Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
3. Run the coverage script:
   .\generate-coverage.ps1
4. After running the script, a browser window will open showing the HTML report with detailed coverage metrics.
![image](https://github.com/user-attachments/assets/17042fcf-19c9-42de-8258-9b58cd27bdd9)

## **How to Generate a Postman Collection**
1. Start the API and access the Swagger documentation at:
   https://localhost:53521/swagger/v1/swagger.json
2. Copy the Swagger JSON URL.
3. Open Postman, go to the Import menu, choose Link, and paste the Swagger URL.
4. Postman will generate a collection of all available API endpoints based on the Swagger definition.



