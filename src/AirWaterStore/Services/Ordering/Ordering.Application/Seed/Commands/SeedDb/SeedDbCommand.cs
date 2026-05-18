using BuildingBlocks.CQRS;
using Ordering.Application.Data;

namespace Ordering.Application.Seed.Commands.SeedDb;

public record SeedDbCommand() : ICommand<SeedDbResult>;

public record SeedDbResult(bool IsSuccess);