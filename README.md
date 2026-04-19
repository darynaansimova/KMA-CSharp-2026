# Subjects Manager

A cross‑platform mobile and desktop application for managing academic subjects and their associated lessons. Built with .NET MAUI, following the MVVM pattern, and using SQLite for local data persistence.

## Overview

This application provides a complete interface for:

*   **Subjects (First‑Level Entities):** View, search, sort, create, edit, and delete subjects. Each subject is defined by a name, knowledge area, and ECTS credits.
*   **Lessons (Second‑Level Entities):** For any subject, manage a list of lessons. Each lesson includes date, start/end time, topic, and type (lecture, laboratory, etc.).

The UI is designed around a single main window with navigation handled via `.NET MAUI Shell`. All data operations are asynchronous to keep the interface responsive, and the application includes full validation logic.

## Architecture

The solution is strictly layered to ensure separation of concerns and maintainability:

*   **UI Layer (`SubjectsManager`)**: Contains XAML pages, code‑behind, and ViewModels. It interacts **only** with the Services layer.
*   **Services Layer (`SubjectsManager.Services`)**: Houses business logic, DTOs, and validation rules. It acts as a bridge between the UI and the data repositories.
*   **Repository Layer (`SubjectsManager.Repositories`)**: Defines contracts for data access and their concrete implementations, isolating the data source details.
*   **Storage Layer (`SubjectsManager.Storage`)**: Implements the actual data store using **SQLite**. It handles database initialization, migrations, and raw CRUD operations.
*   **Common Components (`SubjectsManager.CommonComponents`)**: Shared enumerations and utility classes used across all layers.
*   **DTO Models (`SubjectsManager.DTOModels`)**: Plain objects for transferring data between the service layer and the UI.
*   **DB Models (`SubjectsManager.DBModels`)**: Objects that map directly to the SQLite database schema.

This design respects the **Single Responsibility Principle** and utilizes **Dependency Injection** (IoC) to wire components together.

## Key Features & Implementation

*   **MVVM Pattern**: ViewModels inherit from `BaseViewModel` and use `CommunityToolkit.Mvvm` for `[ObservableProperty]` and `[RelayCommand]` attributes, keeping the code‑behind files minimal.
*   **Asynchronous Operations**: All data access and long‑running tasks are performed asynchronously (`async`/`await`). The UI is blocked with an `ActivityIndicator` during operations to prevent user interference.
*   **Data Persistence with SQLite**: The application uses a local SQLite database (`lesson_manager.db3`) stored in the device's app data directory. On the first launch, the database is seeded with realistic mock data.
*   **Cascading Deletes**: When a subject is deleted, all of its associated lessons are automatically removed from the database, ensuring referential integrity.
*   **Validation**: Input validation is implemented both in the ViewModels and in a dedicated `Validators` class, providing clear error messages to the user.
*   **Navigation**: The `.NET MAUI Shell` manages navigation between pages. The app uses a single main window, and all transitions are handled via routes (e.g., `//SubjectsPage`, `SubjectDetailsPage`).

## How to Run

### Prerequisites

*   .NET 9 SDK (or later)
*   Visual Studio 2022 (17.8 or later) with the **.NET Multi‑platform App UI development** workload installed
*   For Android deployment: Android SDK and an emulator or physical device

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/darynaansimova/KMA-CSharp-2026.git
    ```
2. Navigate to the MAUI project folder:
   ```bash
   cd KMA-CSharp-2026/SubjectsManager
   ```
3. Open the solution (.sln) in Visual Studio.

4. Select your target platform (Windows Machine or Android Emulator) from the debug toolbar.

5. Press F5 to build and run the application.

## Usage Guide

### 1. Viewing and Managing Subjects
   The main page (SubjectsPage) displays a list of all subjects.
   
   * Search: Use the search bar at the top to filter subjects by name.
   
   * Sort: Click the "Name" or "Lessons" headers to sort the list in ascending or descending order.
   
   * Add: Tap the "Create new Subject" button at the bottom.
   
   * Edit/Delete: Swipe left on any subject card to reveal Edit and Delete buttons. Alternatively, right‑click (or long‑press) to open a context menu.

### 2. Managing Lessons for a Subject
   Select any subject from the list to navigate to its details page (SubjectDetailsPage).
   
   * Overview: The top section shows the subject's full details.
   
   * Lesson List: All lessons for this subject are displayed below.
   
   * Search & Sort: Filter lessons by type or sort them by date or type.
   
   * Add Lesson: Tap the "Create new Lesson" button.
   
   * Edit/Delete Lessons: Use the same swipe or context‑menu actions available on the main subject list.
   
   * View Lesson Details: Tap any lesson card to see its full information, including the calculated duration.

### 3. Creating and Editing Data
   The "Create" and "Edit" pages provide input fields with built‑in validation.
   
   * Fields with errors are highlighted in red with descriptive messages.
   
   * All date and time selections are validated to ensure they are in the future and have a logical duration.
   
   * Use the "Back" button to cancel any changes.


### License
This project was created as part of a C# coursework assignment at KMA.
