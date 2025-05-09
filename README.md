# 📚 Library Management System

## 📋 Overview
The **Library Management System** is a comprehensive web application designed to facilitate the management of library resources, including books, authors, publishers, and categories. This system allows users to efficiently manage library operations, track inventory, and streamline the borrowing process. The application is built using ASP.NET Core and follows modern web development practices, ensuring a robust and user-friendly experience.

## ✨ Features
- 📚 **Book Management**: Add, update, and delete books from the library database.
- 👨‍🏫 **Author Management**: Manage author details and their associated books.
- 🏢 **Publisher Management**: Keep track of publishers and their publications.
- 📂 **Category Management**: Organize books into categories for easier access.
- 🔍 **Search Functionality**: Quickly search for books, authors, or publishers.
- 📊 **User Dashboard**: A user-friendly dashboard for library staff to monitor activities.
- 🔒 **User Authentication**: Secure login and registration features for library staff.
- 📅 **Borrowing System**: Keep track of borrowed books and due dates.

## 🚀 Installation
To set up the Library Management System, follow these steps:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/ElvinIsmayil/LibraryManagementSystem.git
   ```

2. **Navigate to the project directory**:
   ```bash
   cd LibraryManagementSystem
   ```

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Run migrations** (if using Entity Framework):
   ```bash
   dotnet ef database update
   ```

5. **Start the application**:
   ```bash
   dotnet run
   ```

6. **Access the application**:
   Open your browser and navigate to `http://localhost:5000`.

## 🔧 Configuration
Configuration settings can be found in the `appsettings.json` files. Below are some key configurations:

### Database Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-CIU9UU4\\SQLEXPRESS;Database=LibraryManagementSystemDB;Trusted_Connection=True;"
  }
}
```

### Logging Configuration
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 📊 Usage Examples
Here are some examples of how to use the Library Management System:

### Adding a New Book
```csharp
public async Task<IActionResult> Create(BookCreateVM model)
{
    if (ModelState.IsValid)
    {
        var book = new Book
        {
            Title = model.Title,
            AuthorId = model.AuthorId,
            CategoryId = model.CategoryId,
            PublisherId = model.PublisherId,
            PublishedDate = model.PublishedDate
        };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    return View(model);
}
```

### Searching for an Author
```csharp
public async Task<IActionResult> Search(string searchTerm)
{
    var authors = await _context.Authors
        .Where(a => a.Name.Contains(searchTerm))
        .ToListAsync();
    return View(authors);
}
```

## 📘 API Reference
### Book Controller
- **GET /api/books**
  - **Description**: Retrieves a list of all books.
  - **Response**: `200 OK` with a list of books.

- **POST /api/books**
  - **Description**: Adds a new book to the library.
  - **Request Body**:
    ```json
    {
      "title": "Book Title",
      "authorId": 1,
      "categoryId": 2,
      "publisherId": 3,
      "publishedDate": "2023-01-01"
    }
    ```
  - **Response**: `201 Created` with the created book object.

## 🧩 Architecture
The Library Management System is structured using a layered architecture, which separates concerns and promotes maintainability.

```
+-------------------------+
|      Presentation       |
|   (MVC Controllers)     |
+-------------------------+
            |
+-------------------------+
|         Services        |
|    (Business Logic)     |
+-------------------------+
            |
+-------------------------+
|        Data Access      |
|  (Entity Framework DB)  |
+-------------------------+
```

## 🔒 Security Considerations
- Use HTTPS for secure data transmission.
- Implement user authentication and authorization.
- Regularly update dependencies to patch vulnerabilities.

## 🧪 Testing
To run tests for the Library Management System, use the following command:

```bash
dotnet test
```

Ensure that you have the necessary testing framework set up in your project.

## 🤝 Contributing
We welcome contributions to the Library Management System! To contribute, please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and commit them with descriptive messages.
4. Push your branch and submit a pull request.

## 📝 License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Thank you for checking out the Library Management System! We hope you find it useful for managing your library's resources efficiently. If you have any questions or suggestions, feel free to reach out!
