# Trade Risk Evaluator API

A highly optimized, enterprise-grade REST API built with **.NET 8** for evaluating and categorizing financial trades based on risk criteria. 

This project was designed with a strong focus on **Clean Architecture**.

## 🚀 Technologies & Libraries
* **.NET 8** (C# 12)
* **MediatR** (CQRS Pattern & Pipeline Behaviors)
* **FluentValidation** (Fail-Fast Input Validation)
* **Swagger** (API Documentation)
* **PLINQ** (Parallel LINQ for high-performance processing)

## 🏗️ Architecture & Design Patterns

This application moves beyond traditional layered architectures by combining **Clean Architecture** with **Vertical Slice Architecture** in the Application layer.

### 1. Vertical Slices & CQRS
The application layer is organized by **Features** rather than technical concerns. Each feature (e.g., `CalculateRisk`, `CalculateRiskDistribution`) encapsulates its own Request, Command, Handler, and Validator. This ensures **High Cohesion** and strict adherence to the Single Responsibility Principle (SRP).

### 2. Strategy Pattern (Domain Layer)
The core business logic is completely isolated. Risk evaluation rules adheres to the **Open/Closed Principle (OCP)**: new risk categories can be added by simply creating a new rule class, without modifying the existing evaluation engine.

### 3. Fail-Fast Validation (Pipeline Behaviors)
Input validation is handled elegantly using **FluentValidation** coupled with a **MediatR Pipeline Behavior**. Bad requests are intercepted and rejected *before* they ever reach the domain logic, keeping the Handlers pristine and focused solely on orchestration.

### 4. Global Exception Handling
Leveraging the new .NET 8 `IExceptionHandler`, all unhandled exceptions and validation errors are intercepted globally and formatted according to the **RFC 7807 Problem Details** standard, preventing stack trace leaks and ensuring consistent API responses.

## ⚙️ How to Run

1. Ensure you have the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.
2. Clone the repository and navigate to the API project folder:
    ```bash
     cd TradeRiskEvaluator.API
3. Run the application:
    ```bash
    dotnet run

4. Open your browser and navigate to the Swagger UI to test the endpoints:
http://localhost:5000/swagger
