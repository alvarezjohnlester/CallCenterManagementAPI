# Call Center Management API

Welcome to the Call Center Management API project! This API is built using ASP.NET Core (.NET 6) and is designed to help manage call center operations efficiently. Below you'll find everything you need to get started.

## Table of Contents
- Setup Instructions
- Assumptions and Design Decisions
- Approach

## Setup Instructions

### Prerequisites
Before you begin, make sure you have the following installed:
- .NET 6 SDK
- SQL Server
- Bruno API Client

### Steps to Run the Project

1. **Clone the repository**:
   ```bash
   git clone https://github.com/alvarezjohnlester/CallCenterManagementAPI.git
   
2. **Set up the database**:
- Update the connection string in appsettings.json
  ```
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=CallCenterDB;User Id=your_user;Password=your_password;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  ```
- Apply migrations:
  
  - this will create the tables from db context
    
  .Net CLI
  ```
  dotnet ef database update
  ```
  Visual Studio
  ```
  Update-Database
  ```
3. **Run the application**:
   
   - This will spawn Swagger client and console app
     
5. **Setup Bruno API Client**:
   - Open Bruno and import the provided .bru files.
     
     - Open Bruno then Select "Collection"
     - Choose "Open Collection" then Navigate to Folder Location of the App
     - Choose this folder APICollection/CallCenterManagementAPI
       
   - Use the predefined requests to test the API endpoints.
     
   - To Test the Endpoints, use must register first then use the credentials to generate token then use that token as Bearer Token to the API endpoints.

### Project Assumptions and Key Design Choices

Here are a few things we assumed while building this project. These assumptions helped us move forward, but they should be checked as the project evolves.
- Team members are familiar with ASP.NET Core and related technologies.
- Since we are using JWT for authentication, we need credentials. Therefore, a User model was created to manage these credentials.
- The system will be able to handle up to 10,000 concurrent users without performance issues.

### Approach
1. **Coding**
   - We use Code Fist approach since we use EF Core and we already know the model details.
   - Created a Interface for the same endpoint method on every data models
     
2. **Project Setup**:
   - Created a new ASP.NET Core Web API project and added necessary NuGet packages for Entity Framework Core, JWT authentication, and testing.
  
   
