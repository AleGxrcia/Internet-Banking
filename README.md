# Internet Banking App

Internet Banking App is a web application developed using ASP.NET Core MVC 6, designed for managing banking operations with two user roles: Administrator and Client.

## Key Functionalities

- **Login:** Secure authentication with redirection based on user roles. Includes handling inactive accounts and displaying appropriate messages for incorrect credentials.
- **Security:** Authorization control prevents unauthorized access to functionalities based on user roles.
- **Administrator Dashboard:** Provides a comprehensive overview of system metrics such as total transactions, payments, active clients, and product assignments.
- **User Management:** Enables administrators to create, edit, activate/deactivate users, and assign products. Includes validations for required fields and user type differentiations.
- **Client Dashboard:** Displays product listing with details like type (savings account, credit card, loan), balances, and options for payments, beneficiaries, and transfers.
- **Beneficiary Management:** Clients can manage beneficiaries for easy payment processing.
- **Payments:** Facilitates various payment types such as express payments, credit card payments, loan payments, and payments to beneficiaries. Includes validations for available balances and debt calculations.
- **Cash Advance:** Clients can request cash advances against their credit cards with validations for credit limits.
- **Transfer Between Accounts:** Allows fund transfers between client accounts with balance validations.

## Technologies Used

### Frontend:
- HTML
- CSS
- Bootstrap
- ASP.NET Razor

### Backend:
- C# ASP.NET Core (8.0)
- Microsoft Entity Framework Core (Code First approach)
- Microsoft Identity for user management
- AutoMapper for mapping ViewModels, Entities, and DTOs

### Database:
- SQL Server

## Installation and Prerequisites

### Prerequisites

To run the Internet Banking App, you'll need:

- Visual Studio 2022 or newer
- ASP.NET Core (8.0 or newer)
- SQL Server

### Installation Steps

1. Download or clone the project.
2. Open the project in Visual Studio 2022.
3. Update the server name in `appsettings.Development.json` to match your environment.
4. Run the `Update-Database` command in the Package Management Console to update the database schema.
5. Run the project, and it will open in your default browser.

## Developer
- [Federico A. Garcia](https://github.com/AleGxrcia)
