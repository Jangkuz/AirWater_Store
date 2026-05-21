namespace AirWaterStore.Web.Helper;

/// <summary>
/// Centralized configuration for WebSocket connections.
/// </summary>
public class WebSocketConfig
{
    /// <summary>
    /// Gets the WebSocket URL for a chat room.
    /// </summary>
    /// <param name="gatewayAddress">The base gateway address (e.g., http://localhost:6000)</param>
    /// <param name="chatRoomId">The chat room ID</param>
    /// <returns>The full WebSocket URL</returns>
    public static string GetChatRoomWebSocketUrl(IConfiguration configuration, string chatRoomId)
    {
        string gatewayAddress = configuration["ApiSettings:GatewayAddress"] ?? "http://localhost:6000";
        var wsGatewayAddress = gatewayAddress
            .Replace("http://", "ws://")
            .Replace("https://", "wss://");

        return $"{wsGatewayAddress}/chatroom-service/chatrooms/ws/{chatRoomId}";
    }

    /// <summary>
    /// Gets the WebSocket reconnection delay in milliseconds.
    /// </summary>
    public static int ReconnectionDelayMs => 3000;

    /// <summary>
    /// Gets the typing status timeout in milliseconds.
    /// </summary>
    public static int TypingStatusTimeoutMs => 1000;
}
