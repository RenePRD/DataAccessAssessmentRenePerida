Data Access Project
This project is a C# console application that connects to a Microsoft SQL Server database to manage employees and departments in a company. The project demonstrates basic CRUD (Create, Read, Update, Delete) operations on two entities: Employee and Department.

Key Features
Uses ADO.NET to interact with a Microsoft SQL Server database.
Implements repositories for Employee and Department entities.
Demonstrates:
Adding new employees and departments.
Fetching data by ID and by name.
Updating existing records.
Deleting records from the database.
Retrieving all records for display.
Technologies Used
C#
ADO.NET for database connectivity.
Microsoft SQL Server for database management.
Database
The project is connected to a SQL Server database using the following connection string:
bash
Copy code
Data Source=.\SQLEXPRESS; Initial Catalog=CompanyFdm; Integrated Security=sspi;
Make sure you have SQL Server running and the database CompanyFdm set up.
Setup and Usage
Clone the repository.
Restore NuGet packages if required.
Modify the connection string in Program.cs if needed to match your local SQL Server setup.
Run the project to see the CRUD operations in action.
