
# Project Summary

This project is an Employee Management System developed using C# for the backend and JavaScript for the frontend. The system allows managing employee information, handling authentication, and performing various employee-related operations through a web interface.

---

## Features

### 1. Backend (C#)
   - **Data Access Layer**: Provides access to the database using various C# classes and methods.
     - `Database.cs`: Handles database connections and queries.
     - `Employee.cs`: Represents the employee model.
   - **Controllers**: Manages HTTP requests and routes them to the appropriate services.
     - `EmployeeController.cs`: Handles CRUD operations for employees.
   - **Models**: Defines the structure of data used within the application.
     - `EmployeesViewModels.cs`: Contains data models for employee views.

### 2. Frontend (JavaScript)
   - **User Interface**: Develops a user-friendly interface for interacting with the system.
     - `Login_form.cshtml`: Login form for user authentication.
     - `Registration_form.cshtml`: Registration form for new users.
     - Various `.cshtml` files for different employee operations such as `Blend.cshtml`, `CombineMachineOuts.cshtml`, etc.
   - **Scripts**: JavaScript files to enhance the functionality of the frontend.
     - `Custom.js`: Custom JavaScript functions for the application.
     - `Chart.js`: JavaScript library for creating charts and visualizations.

### 3. Database Models
   - **Database Schema**: Defines the structure of the database used in the application.
     - `ApplicationDBContext.cs`: Context class for the database.
     - `DatabaseInitializer.cs`: Initializes the database with seed data.

### 4. Configuration
   - **Web.config**: Configuration file for the application, defining settings for database connections, authentication, etc.
   - **App.config**: Configuration file for the class library.

### 5. Documentation and Resources
   - **Class Diagrams**: Visual representation of the classes and their relationships.
     - `ClassDiagram2.png`
   - **Project Readme**: Detailed project description and usage instructions.
     - `Project_Readme.html`
   - **Change Log**: Records of changes made to the project over time.
     - `Changes.txt`

---

## Detailed Explanation

### **Backend (C#)**

- **Data Access Layer**:
  - `Database.cs`: Manages database connections and executes queries.
  - `Employee.cs`: Represents the employee data model.
- **Controllers**:
  - `EmployeeController.cs`: Handles HTTP requests related to employee management.
- **Models**:
  - `EmployeesViewModels.cs`: Defines the data structure for employee views.

### **Frontend (JavaScript)**

- **User Interface**:
  - `Login_form.cshtml`: Provides a login form for user authentication.
  - `Registration_form.cshtml`: Registration form for new users.
  - Various other `.cshtml` files for different employee operations.
- **Scripts**:
  - `Custom.js`: Custom JavaScript functions for frontend operations.
  - `Chart.js`: Library for creating interactive charts.

### **Database Models**

- **Database Schema**:
  - `ApplicationDBContext.cs`: Defines the context for the database.
  - `DatabaseInitializer.cs`: Seeds the database with initial data.

### **Configuration**

- **Web.config**: Configuration settings for the web application.
- **App.config**: Configuration settings for the class library.

### **Documentation and Resources**

- **Class Diagrams**:
  - `ClassDiagram2.png`: Visual representation of the classes.
- **Project Readme**:
  - `Project_Readme.html`: Detailed project description and usage instructions.
- **Change Log**:
  - `Changes.txt`: Records changes made to the project.

---

## Usage Instructions

1. **Clone the Repository**: 
   ```bash
   git clone <repository-url>
   ```

2. **Install Dependencies**: 
   - Ensure you have the necessary dependencies installed for both the frontend and backend. This may include setting up a C# development environment and installing required JavaScript libraries.

3. **Configure the Application**:
   - Update the `Web.config` and `App.config` files with the correct settings for your environment.

4. **Run the Backend Server**:
   - Open the solution file `DCSLManufacturing.sln` in Visual Studio and run the project.

5. **Run the Frontend Application**:
   - Serve the frontend files using a web server and navigate to the application in your browser.

6. **Access the Application**:
   - Use the login and registration forms to authenticate and access employee management features.

---

## Conclusion

This Employee Management System provides a comprehensive solution for managing employee information and performing various employee-related operations. With a robust backend developed in C# and an interactive frontend built with JavaScript, it offers an efficient and user-friendly experience for managing employees.
