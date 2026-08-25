# ⚡ RMS Backend · Enterprise Clean Architecture Solution

> **High-Performance .NET 8 Web API Core** engineered for high-concurrency table reservations, optimistic locking, distributed Redis caching, MassTransit outbox event messaging, and real-time SignalR WebSocket dispatch.

---

## 🏛️ Clean Architecture Design

```text
┌─────────────────────────────────────────────────────────┐
│                    RMS.WebApi (Presentation)            │
│         Controllers · Middleware · SignalR Hubs         │
└────────────────────────────┬────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────┐
│                  RMS.Application (Core CQRS)            │
│   DTOs · Interfaces · Services · FluentValidation · AutoMapper  │
└────────────────────────────┬────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────┐
│                    RMS.Domain (Enterprise)              │
│       Entities · Enums · Domain Events · Domain Exceptions      │
└────────────────────────────▲────────────────────────────┘
                             │
┌────────────────────────────┴────────────────────────────┐
│                RMS.Infrastructure (Persistence)         │
│  EF Core 8 · SQL Server 2022 · Redis · MassTransit · Stripe │
└─────────────────────────────────────────────────────────┘
```

---

## 🚀 Key Architectural Highlights

* **Table Reservation & Live Hold Engine (Spec 0001):** Distributed table hold locks with Redis TTL and optimistic concurrency control (`RowVersion`).
* **Pre-Payment & Deposit Gateway (Spec 0002):** Stripe payment intent generation, automatic webhook handlers, tiered cancellation penalty calculation, and deposit deduction upon POS settlement.
* **MassTransit Outbox Pattern:** Zero-event-loss transactional outbox for publishing domain events (`ReservationConfirmedEvent`, `OrderCreatedEvent`, `OrderUpdatedEvent`, `ReservationCancelledEvent`).
* **Keyset Pagination:** High-volume keyset pagination with cursor navigation (`lastSeenId`) for zero-offset degradation on millions of order records.
* **SignalR Real-Time Hub:** `/rmshub` broadcasts real-time kitchen order state transitions and table occupancy changes.
* **Multi-Tenant Architecture:** Request-scoped tenant context resolving tenant isolation headers (`X-Branch-ID`).

---

## 🛠️ Technology Stack

* **Language / Framework:** C# (.NET 8.0 SDK)
* **ORM:** Entity Framework Core 8.0 (SQL Server 2022)
* **Messaging & Outbox:** MassTransit + RabbitMQ
* **Caching & Distributed Locks:** StackExchange.Redis
* **Real-Time WebSockets:** ASP.NET Core SignalR
* **Payment Processing:** Stripe .NET SDK
* **Logging & Observability:** Serilog, OpenTelemetry
* **Validation & Mapping:** FluentValidation, AutoMapper 13.0

---

## 🧪 Testing & Verification

The solution features automated unit and integration tests across table hold lifecycles, POS check-in settlements, and cancellation refund tiers.

```bash
# Run all backend test suites
dotnet test Backend/RMS/RMS.sln
```

---

## 🚀 Running the Backend

```bash
# 1. Navigate to WebApi directory
cd Backend/RMS/RMS.WebApi

# 2. Restore and Build
dotnet restore
dotnet build

# 3. Launch the API Server
dotnet run --launch-profile https
```

Swagger API Documentation will be accessible at: `https://localhost:7083/swagger` (or `http://localhost:5083/swagger`)
