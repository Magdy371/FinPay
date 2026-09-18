# FinPay — Digital Wallet & Banking Platform

**FinPay** is an enterprise-grade, multi-user digital wallet and banking system built on modern C# and .NET 10 principles. Designed around Clean Architecture and a CQRS application layer, FinPay implements a real-world **Double-Entry Ledger Accounting Engine** to ensure transaction integrity, auditability, and financial precision.

## 📋 Table of Contents

* [Overview](#-overview)

* [Key Features & Modules](#-key-features--modules)

* [Core Architecture: Double-Entry Ledger](#-core-architecture-double-entry-ledger)

* [Technology Stack](#-technology-stack)

* [System Architecture](#-system-architecture)

* [Project Layout](#-project-layout)

* [Transaction Flow Sequence](#-transaction-flow-sequence)

* [Key Architectural Concepts](#-key-architectural-concepts)

* [Getting Started](#-getting-started)

* [License](#-license)

## 🌐 Overview

Unlike simple CRUD projects that directly update account balances, **FinPay** mimics real fintech core banking software. It tracks money movement through immutable debit and credit ledger entries, provides robust risk management (KYC, rate limiting, suspicious activity detection), and features interactive real-time user and administrator dashboards.

## ✨ Key Features & Modules

```
FinPay
│
├── 🔐 Authentication      ─ Register, Login, Email Verification, 2FA/OTP, Password Reset
├── 👤 Users               ─ Profile Management, KYC Status Verification, Security Settings
├── 👛 Wallets             ─ Multi-Currency Wallets, Balance Tracking, Deposits, Withdrawals
├── 💸 Transactions        ─ Transfers, Peer-to-Peer Payments, Direct Bill Pay, Immutable Audit History
├── 👥 Beneficiaries       ─ Add, Remove, and Verify Saved Beneficiaries
├── 💳 Cards               ─ Virtual Card Generation, Freeze/Unfreeze Status, Spending Limits
├── 🧾 Payments            ─ Merchant Checkout, Utility Bill Payments, Payment Receipts
├── 🛡️ Admin Panel         ─ User Oversight, Transaction Auditing, KYC Approvals, Suspicious Activity Flags
└── 🔔 Notifications       ─ Email Alerts, SMS Gateway Integration, Real-Time In-App Alerts

```

## ⚖️ Core Architecture: Double-Entry Ledger

In enterprise financial engineering, direct balance manipulation (`Wallet.Balance -= X`) is prone to race conditions, auditing failure, and data corruption. FinPay introduces an immutable **Double-Entry Ledger Engine**.

### Balance Logic Example

When **Alice** transfers **\$100** to **Bob**:

* **Alice's Wallet Account:** `Debit` \$100

* **Bob's Wallet Account:** `Credit` \$100

### Data Model Schema

#### `Transactions`

| 

| **Column** | **Type** | **Description** | 
| `Id` | `Guid` | Unique primary key | 
| `Reference` | `string` | Unique business transaction reference code | 
| `Type` | `enum` | Transfer, Deposit, Withdrawal, Payment | 
| `Status` | `enum` | Pending, Completed, Failed, Reversed | 
| `Amount` | `decimal` | Transaction total amount | 
| `Currency` | `string` | ISO Currency code (e.g., USD, EUR) | 
| `CreatedAt` | `DateTimeOffset` | UTC timestamp | 

#### `LedgerEntries`

| **Column** | **Type** | **Description** | 
| `Id` | `Guid` | Primary key | 
| `TransactionId` | `Guid` | Foreign key referencing `Transactions.Id` | 
| `AccountId` | `Guid` | Account/Wallet identifier | 
| `EntryType` | `enum` | `Debit` or `Credit` | 
| `Amount` | `decimal` | Amount applied | 
| `Currency` | `string` | ISO Currency code | 
| `CreatedAt` | `DateTimeOffset` | UTC timestamp | 

## 🛠️ Technology Stack

* **Framework:** C# 12 / .NET 10

* **Web & UI:** ASP.NET Core MVC (RESTful APIs & Controllers), Blazor (Interactive UI & Dashboards)

* **Application Pattern:** CQRS via [MediatR](https://github.com/jbogard/MediatR?utm_source=gemini), Validation via [FluentValidation](https://fluentvalidation.net/?utm_source=gemini)

* **Data Access:** Entity Framework Core 10 (ORM) with PostgreSQL / SQL Server

* **Caching & Rate Limiting:** Redis

* **Authentication & Security:** ASP.NET Core Identity, JWT Bearer Tokens, 2FA / OTP

* **Logging & Observability:** Serilog (Structured Logging)

* **DevOps & Containerization:** Docker, Docker Compose

* **Testing:** xUnit, Moq, FluentAssertions

## 🏗️ System Architecture

FinPay adheres to strict **Clean Architecture** principles, segregating domain logic from infrastructure details and external delivery layers.

```
                    ┌──────────────────┐
                    │     Blazor UI    │
                    └────────┬─────────┘
                             │
                             ▼
                  ┌─────────────────────┐
                  │ ASP.NET Core MVC/API │
                  └──────────┬──────────┘
                             │
              ┌──────────────┼──────────────┐
              ▼              ▼              ▼
        Application       Domain       Infrastructure
              │              │              │
              │              │         ┌────┴─────┐
              │              │         │ EF Core  │
              │              │         │ Redis    │
              │              │         │ Email    │
              │              │         └──────────┘
              │              │
              └──────────────┘
                     │
                     ▼
                 PostgreSQL

```

## 📁 Project Layout

```
FinPay.sln
│
├── src/
│   ├── FinPay.Web/                 # Presentation Layer (MVC Controllers, Blazor Pages/Components)
│   │   ├── Controllers/
│   │   ├── Components/
│   │   ├── Pages/
│   │   └── Program.cs
│   │
│   ├── FinPay.Application/         # Business Logic, Commands/Queries (MediatR), Handlers, DTOs
│   │   ├── Users/
│   │   ├── Wallets/
│   │   ├── Transactions/
│   │   └── Payments/
│   │
│   ├── FinPay.Domain/              # Core Domain (Entities, Value Objects, Enums, Interfaces)
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── ValueObjects/
│   │   └── Interfaces/
│   │
│   └── FinPay.Infrastructure/      # External Services, Persistence, EF Core, Redis, Identity
│       ├── Persistence/
│       ├── Repositories/
│       ├── Identity/
│       ├── Redis/
│       └── Services/
│
└── tests/
    ├── FinPay.UnitTests/           # Domain & Application layer unit tests
    └── FinPay.IntegrationTests/    # API & Database integration tests

```

## 🔄 Transaction Flow Sequence

When a user initiates a transfer (\$500 from Alice to Bob):

```
POST /api/transfers
        │
        ▼
[TransferController] ──► Decodes Request
        │
        ▼
[TransferService / CommandHandler]
        │
        ├──► 1. Validate (Alice & Bob exist, beneficiary active, sufficient balance, limits)
        ├──► 2. Create Transaction Record
        ├──► 3. Generate Ledger Entries (Debit Alice $500, Credit Bob $500)
        ├──► 4. Commit Database Transaction (Atomic Unit of Work)
        ├──► 5. Publish `TransactionCompletedEvent`
        │
        ▼
[Notification Service] ──► Send Email / SMS / Push Notifications
        │
        ▼
[Blazor UI Dashboard] ──► Real-time signal update via SignalR / Interactive State

```

## 🎯 Key Architectural Concepts Covered

Building and studying FinPay provides hands-on practice with enterprise C# / .NET concepts:

* **Domain-Driven Design (DDD) & Clean Architecture**

* **SOLID Principles & Object-Oriented Design**

* **Command Query Responsibility Segregation (CQRS)**

* **Double-Entry Accounting & Financial Ledger Engineering**

* **Optimistic/Pessimistic Concurrency Handling**

* **Background Jobs & Event-Driven Domain Events**

* **Distributed Caching & Rate Limiting using Redis**

* **Containerized Deployment with Docker & Docker Compose**

## 🚀 Getting Started

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/?utm_source=gemini)

* [Docker Desktop](https://www.docker.com/?utm_source=gemini) (or local PostgreSQL and Redis instances)

### Setup & Installation

1. **Clone the repository:**

   ```
   git clone https://github.com/your-username/FinPay.git
   cd FinPay
   
   ```

2. **Spin up Infrastructure Containers (PostgreSQL & Redis):**

   ```
   docker-compose up -d
   
   ```

3. **Apply Database Migrations:**

   ```
   dotnet ef database update --project src/FinPay.Infrastructure --startup-project src/FinPay.Web
   
   ```

4. **Run the Application:**

   ```
   dotnet run --project src/FinPay.Web
   
   ```

5. **Access the Application:**

   * Web / Blazor UI: `https://localhost:7001`

   * Swagger API Docs: `https://localhost:7001/swagger`

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
