# FinPay Domain Entity Relationships

This document describes the current relationships among `User`, `Wallet`, and
`Account`, including their identifiers, foreign keys, navigation properties,
and the enums they use.

```mermaid
classDiagram
    direction LR

    class Entity {
        <<abstract>>
        +Guid Id
    }

    class AuditableEntity {
        +DateTime CreatedAt
        +DateTime? UpdatedAt
        +MarkUpdated()
    }

    Entity <|-- AuditableEntity
    AuditableEntity <|-- User
    AuditableEntity <|-- Wallet
    AuditableEntity <|-- Account

    class User {
        +Guid Id
        +string FirstName
        +string LastName
        +string Email
        +bool IsActive
        +IReadOnlyCollection~Wallet~ Wallets
        +Activate()
        +Deactivate()
    }

    class Wallet {
        +Guid Id
        +Guid UserId
        +Currency Currency
        +WalletStatus Status
        +User User
        +IReadOnlyCollection~Account~ Accounts
        +Activate()
        +Suspend()
        +Freeze()
        +Close()
    }

    class Account {
        +Guid Id
        +Guid WalletId
        +Currency Currency
        +AccountType Type
        +decimal Balance
        +Wallet Wallet
        +ApplyCredit(decimal amount)
        +ApplyDebit(decimal amount)
    }

    User "1" --> "0..*" Wallet : Wallets / User
    Wallet "1" --> "0..*" Account : Accounts / Wallet

    class Currency {
        <<enumeration>>
        USD
        EUR
        GBP
        EGP
    }

    class WalletStatus {
        <<enumeration>>
        Active
        Suspended
        Frozen
        Closed
    }

    class AccountType {
        <<enumeration>>
        Customer
        System
        Merchant
    }

    Wallet --> Currency
    Wallet --> WalletStatus
    Account --> Currency
    Account --> AccountType
```

## Identifier and key mapping

`User`, `Wallet`, and `Account` inherit their primary key from `Entity` through
`AuditableEntity`:

```csharp
public Guid Id { get; protected set; } = Guid.NewGuid();
```

| Entity | Primary key | Foreign key | References |
| --- | --- | --- | --- |
| `User` | `Id` | — | — |
| `Wallet` | `Id` | `UserId` | `User.Id` |
| `Account` | `Id` | `WalletId` | `Wallet.Id` |

Therefore:

```text
Wallet.UserId      -> User.Id
Account.WalletId   -> Wallet.Id
```

`Wallet` does not currently have a property named `UserGuid`; the `UserId`
property is the GUID foreign-key property by naming convention. The `User`
property is its navigation property. Likewise, `Account.WalletId` is the
foreign-key property and `Account.Wallet` is its navigation property.

In Entity Framework Core, these names conventionally map to database foreign
keys. A database-level foreign-key constraint is created when the relationship
is included in the EF Core model and a migration is applied.

## Relationship cardinality

- One `User` can own zero or more `Wallet` instances.
- One `Wallet` belongs to exactly one `User` through `UserId`.
- One `Wallet` can contain zero or more `Account` instances.
- One `Account` belongs to exactly one `Wallet` through `WalletId`.
