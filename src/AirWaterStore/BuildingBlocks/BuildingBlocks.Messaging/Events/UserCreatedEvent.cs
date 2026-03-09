namespace BuildingBlocks.Messaging.Events;
public record UserCreatedEvent : IntegrationEvent
{
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    //TODO: add role
    public IList<String> Roles { get; set; } = default!;
}
