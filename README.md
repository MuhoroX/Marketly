
# Marketly

Marketly is an open-source e-shop project built using ASP.NET. The platform is designed to resemble popular e-commerce websites like Amazon and Trendyol, offering users a seamless shopping experience with a range of products, categories, and features.

## Features
- Product listing with detailed descriptions
- User accounts and authentication
- Shopping cart and checkout system
- Admin panel for managing products, orders, and users
- Responsive design, optimized for both desktop and mobile
- Search functionality for easy product discovery

## Technologies
- **Backend**: ASP.NET Core
- **Database**: SQL Server
- **Frontend**: HTML, CSS, JavaScript (with Bootstrap)
- **Authentication**: ASP.NET Identity
- **ORM**: Entity Framework Core
- **Hosting**: IIS (or other web servers)

## Installation

To get started with the Marketly project, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/YourUsername/marketly.git
   cd marketly
   ```

2. Open the project in Visual Studio.

3. Open the **Package Manager Console** in Visual Studio (`Tools` -> `NuGet Package Manager` -> `Package Manager Console`).

4. Run the following command to update the database:
   ```bash
   Update-Database
   ```

   This command will apply the necessary migrations and set up your database schema.

5. Once the database is set up, you can run the project by pressing `F5` or by using the **Run** button in Visual Studio.

Your e-shop should now be up and running locally. You can start developing and customizing it as per your needs.

## Contributing

Contributions are welcome! If you want to contribute to the project, please fork the repository, make your changes, and submit a pull request.