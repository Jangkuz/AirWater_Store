namespace Catalog.API.Seed.ResetDb;

public record ResetDbCommand() : ICommand<ResetDbResult>;

public record ResetDbResult(bool IsSuccess);

public class ResetDbHandler
    (IDocumentStore store)
    : ICommandHandler<ResetDbCommand, ResetDbResult>
{
    public async Task<ResetDbResult> Handle(ResetDbCommand request, CancellationToken cancellationToken)
    {
        await store.Advanced.Clean.DeleteAllDocumentsAsync(cancellationToken);

        return new ResetDbResult(true);
    }
}
