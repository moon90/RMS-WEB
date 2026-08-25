# RMS Core Backend (.NET 8)

## Overview

The core backend solution housing the Domain entities, Application CQRS commands/queries, Infrastructure persistence, and ASP.NET Core WebApi controllers.

## Key files

| File | Owns |
|---|---|
| `RMS.WebApi/Program.cs` | Application startup, OpenTelemetry, Serilog, SignalR hub mapping (`/rmshub`), Auth, & Rate Limiting |
| `RMS.Infrastructure/Persistences/RestaurantDbContext.cs` | EF Core DbContext with Optimistic Concurrency (`RowVersion`), TableReservations, and PaymentTransactions |
| `RMS.Infrastructure/Persistences/Migrations/` | Entity Framework database migrations (`Phase9_TableReservation`, `Phase10_PaymentTransaction`) |
| `RMS.Application/Implementations/TableReservationService.cs` | Reservation hold engine with Redis TTL locks (`reservation_lock:{tableId}:{slot}`) |
| `RMS.Application/Implementations/StripePaymentService.cs` | Stripe PaymentIntent checkout sessions, webhook verification, and financial idempotency |
| `RMS.Application/Handlers/ReservationConfirmedConsumer.cs` | MassTransit Outbox consumer for `ReservationConfirmedEvent`, SignalR broadcasts, and email receipts |
| `RMS.Application/Handlers/ReservationCancelledConsumer.cs` | MassTransit Outbox consumer for `ReservationCancelledEvent`, automated Stripe refunds, SignalR table releases, and cancellation receipts |
| `RMS.WebApi/Hubs/RMSHub.cs` | SignalR WebSocket hub endpoint (`/rmshub`) broadcasting real-time table status updates |
| `RMS.Tests/` | xUnit unit test suite (`TableReservationServiceTests`, `StripePaymentServiceTests`, `ReservationConfirmedConsumerTests`, `PosCheckInAndSettlementTests`, `CancellationAndRefundPolicyTests`) |

## Commands

```bash
dotnet build RMS-WEB/Backend/RMS/RMS.sln
dotnet test RMS-WEB/Backend/RMS/RMS.Tests/RMS.Tests.csproj
dotnet ef database update --project RMS.Infrastructure --startup-project RMS.WebApi --context RestaurantDbContext
```

## Conventions

- CQRS logic and service implementations are structured under `RMS.Application/Implementations/`.
- Repository patterns are defined in `RMS.Domain/Interfaces` and implemented in `RMS.Infrastructure/Repositories/`.
- MassTransit outbox consumers (`ReservationConfirmedConsumer.cs`, `ReservationCancelledConsumer.cs`) handle domain events asynchronously without blocking HTTP request threads.
- Financial transactions (`PaymentTransaction.cs`) require unique database index constraints on `TransactionReference` for idempotent webhook replays.
- Cancellation policy engine calculates tiered refund windows (>24h 100%, 12-24h 50%, <12h 0%) and releases table holds in real time.
- Background workers handle DB archiving (`DatabaseMaintenanceWorker.cs`) and hold expirations (`ReservationHoldExpiryWorker.cs`).

## Gotchas

- DB migrations must reference `[RMS_DB]` (not `[RMSDB]`).
- Seq authentication requires `SEQ_FIRSTRUN_NOAUTHENTICATION=True`.
- SignalR WebSockets connect via `http://localhost:5000/rmshub` and require JWT Bearer authorization.
- MassTransit RabbitMQ connection settings read host, username, and password from `IConfiguration` via `RabbitMQ__Host` in Docker.

_Updated by /audit for RMS Core Backend (.NET 8)._
