Library Management Platform

Overview

This platform helps manage library operations (book reservations, returns...), customer accounts, employee management, data reports and etc. The project is designed with a multi-layer architecture to separate concerns and ensure maintainability.

Features

Onion Architecture: Ensures separation of concerns and maintainability.
Entity Framework: ORM with code-first migrations and custom configurations for each entity.
Identity Framework: Helps with user authorization and authentication.
FluentValidation: Ensures robust validation for user inputs, protecting the application from invalid data.
Unit of Work & Generic Repository: Simplifies CRUD operations, reducing redundancy in the data access layer and ensures a clean, organized approach to handling data.
Service Layer: Acts as a middleman between controllers and data access, ensuring business logic is separated and validation is consistently applied.
Result Pattern: Provides a clear, exception-free code flow, offering predictable results instead of handling unexpected exceptions.
Logging: Tracks all operations and errors, providing valuable insights for troubleshooting and future analysis.
Mailjet: Sending emails to confirm registration, reset password, and more.
TinyMCE: Editor to create custom-styled email templates for easier handling using parameters.
Dynamic Navigation Menu: Is stored in database and generated recursively based on user's role.
Admin panel: For admins/moderators to assign roles to employees, add email templates, books, make reservations and etc.
ClosedXML: Generating monthly/annual reports of books, employees, customers with information about their activity within Library.
SQL Stored Procedures: Custom-built procedures with date parameters that power the report generation process.
AutoMapper & Custom Mapper: Used to ensure seamless communication between data transfer objects of layers.
Generic implementation of sorting, searching, filtering: Can be applied to every entity, reducing redundancy and making code cleaner.
Unit Tests: Test application's correctness and reliability.

Technologies used

ASP.NET Core MVC
Entity Framework Core
Identity Framework
FluentValidation
AutoMapper
SQL Server
jQuery
Mailjet
NLog
ClosedXML
TinyMCE

Getting Started

1. Clone the repository:
git clone https://github.com/UcanCallMeBlobFish/LibraryManagement.git

2. Navigate to the project directory:
cd LibraryManagement

3. Set up the database.
   Update the connection string in appsettings.json
   Run migrations to create the database
dotnet ef database update

4. Run the application:
dotnet run

Admin:
Email: giorgiotinashvili12@gmail.com
Password: Pass123!

Employees:
Email: giorgiminishvili12@gmail.com
Password: Pass123!

Email: asdfddfhsdf@gmail.com
Password: Pass123!
