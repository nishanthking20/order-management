# Order Management System

A web-based order management application built with ASP.NET Core, allowing users to browse items, manage carts, make purchases, and view transaction history. Administrators can manage items and customer accounts.

## Features

- **User Authentication**: Register and login with email verification
- **Item Browsing**: View items categorized by type
- **Shopping Cart**: Add items to cart, update quantities, and checkout
- **Purchase History**: View past transactions and details
- **Admin Dashboard**: Add new items, manage inventory, and view customer information
- **Email Notifications**: SMTP-based email service for user verification

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core with SQLite
- Razor Views
- Bootstrap for UI
- SMTP for email services

## Prerequisites

- .NET 8.0 SDK
- SQLite (included with .NET)

## Installation

1. Clone the repository:
   ```
   git clone https://github.com/nishanthking20/order-management.git
   cd order-management
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Build the application:
   ```
   dotnet build
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. Open your browser and navigate to `https://localhost:5001` (or the port specified in launchSettings.json)

## Configuration

The application uses the following configurations in `appsettings.json`:

- **Database**: SQLite database (`Order.db`)
- **Email Settings**: Configured for Gmail SMTP (update credentials as needed)

For production, update the email settings and database connection string accordingly.

## Usage

1. **Register**: Create a new account with email verification
2. **Login**: Access your dashboard
3. **Browse Items**: View available products
4. **Add to Cart**: Select items and quantities
5. **Checkout**: Complete purchases
6. **View History**: Check past transactions
7. **Admin Access**: Use admin credentials to manage items and users

## Project Structure

- `Controllers/`: MVC controllers for handling requests
- `Models/`: Entity models (User, Item, History)
- `Views/`: Razor views for UI
- `Data/`: Database context and configurations
- `Services/`: Email service implementation
- `wwwroot/`: Static files (CSS, JS, images)

## Contributing

This is an assignment project. For contributions, please fork the repository and submit a pull request.

## License

This project is for educational purposes.