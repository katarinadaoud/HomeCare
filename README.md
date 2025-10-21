# HomeCare
ITPE3200 ASP.NET Core 8MVC project

LIFELINK - Homecare appointment management tool

This is a webapplication for the homecare system where patients 
and employees can administrate appointments and services. Per now the application lets patients register appointments and employees register/update patients info. We will later on make modifications to the web application so it has more functions.

The application has two dashboards, one for patient and one for employee.
In the patientportal you can create, read, update and delete an appointment by clicking on "Book new appointment".
In the employeeportal you can create, read, update and delete a patient by clicking on the "Patients" button in the navbar.

Backend: ASP.NET Core 7.0 MVC.
Database: Entity Framework Core with SQLite
Frontend: HTML%, CSS#, Bootstrap 5, Javascript
Node.js: v18.17.0
Authentication: ASP.NET Core Identity
Logging: ILogger
Patterns: Repository pattern, Viewmodels

In the arcitechture we are using:
- MVC Controllers
- domain models
- view-specific models
- razor views
- data access layer
- static files.

How to run:

git clone <https://github.com/katarinadaoud/HomeCare.git>
cd HomeCareApp
dotnet restore
node --version #Should be v18.17.0
dotnet ef database update
dotnet run

#Then open in localhost

Inspirations and sources:
We have used AI-assistants such as ChatGPT and Copilot in VScode to troubleshoot and improve our project. The framework is mainly ispired by the modules in ITPE3200.
