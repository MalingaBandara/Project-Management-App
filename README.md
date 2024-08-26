
# Project Management App

## Project Overview

This repository contains a distributed project management application developed as a final year group project using UWP, C#, ASP.NET, and WCF. The application is designed to facilitate team collaboration with features like task management, role-based access control, real-time communication, and project reporting. It provides an intuitive interface for managing projects, assigning tasks, and monitoring progress in real-time.

## Key Features

- Task Management: Create, assign, and track project tasks.
- Role-Based Access Control: Manage user roles and permissions.
- Real-Time Communication: Integrated chat functionality for team collaboration.
- Project Reporting: Generate reports based on project progress.

## Technologies Used

- **C# (UWP)**
- **ASP.NET**
- **WCF Services**
- **SQL Server**

## Project Structure and Code Explanation

1. **API and Backend Logic**:
   - The backend is powered by ASP.NET and WCF services to handle API requests and communication between the client and server.

2. **Task Management Module**:
   - Includes CRUD operations for tasks, allowing users to create, update, and delete tasks.

3. **Role-Based Access Control**:
   - The application includes role-based permissions, ensuring that users only have access to the features relevant to their roles.

4. **Real-Time Communication**:
   - Integrated SignalR for real-time chat, allowing team members to communicate effectively.

5. **User Interface (UWP)**:
   - The frontend is built using UWP, providing a responsive and intuitive interface for managing projects.

## How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/MalingaBandara/Project-Management-App.git
   ```
2. Open the solution in Visual Studio.
3. Build the project and ensure all dependencies are restored.
4. Run the WCF service and UWP client.

## Purpose and Future Enhancements

This project demonstrates the implementation of a distributed project management tool with advanced collaboration features. Future enhancements may include integration with third-party tools, more customizable reporting options, and enhanced security features.

## License

This project is licensed under the MIT License.
