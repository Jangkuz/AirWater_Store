using BuildingBlocks.CQRS;
using Ordering.Application.Data;

namespace Ordering.Application.Seed.Commands.ResetDb;

public record ResetDbCommand() : ICommand<ResetDbResult>;

public record ResetDbResult(bool IsSuccess);