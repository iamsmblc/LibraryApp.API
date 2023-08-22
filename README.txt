# LibraryApp.API

Welcome to LibraryApp.API, a web application for managing a library's resources.

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Usage](#usage)


## Overview

LibraryApp.API is a web application designed to help you manage and organize your library's resources, including books, authors, and more. This README provides a brief overview of the project's structure and how to get started.

## Project Structure

The project is structured as follows:

- `LibraryApp.API`: Main project folder.
  - `Controllers`: Contains API controllers for handling requests.
    - `LibraryController.cs`: Controller for managing library resources.
  - `Data`: Contains the data context and models.
    - `DataContext.cs`: Entity Framework data context for interacting with the database.
  - `Entities`: Contains entity models representing various data tables.
    - `ImageTable.cs`: Model representing image data.
    - `LibraryLog.cs`: Model representing library log entries.
    - `LibraryResponseTable.cs`: Model representing library responses.
    - `LibraryTable.cs`: Model representing library resources.
  - `Services`: Contains services for business logic.
    - `LibraryService.cs`: Service for managing library-related operations.

## Installation

To run the project locally, follow these steps:

1. Clone the repository: `git clone https://github.com/iamsmblc/LibraryApp.API.git`
2. Navigate to the project directory: `cd LibraryApp.API`
3. Install dependencies: `dotnet restore`
4. Set up your database connection in `appsettings.json`.
5. Apply migrations: `dotnet ef database update`
6. Run the application: `dotnet run`

The application should be accessible at `http://localhost:8080`.

## Usage

Upon running the application, you can use API endpoints to interact with the library resources. Refer to the API documentation for detailed information on available endpoints and their usage.

