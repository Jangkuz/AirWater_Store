/**
 * ChatRoomWebSocket - Centralized WebSocket manager for chat room communications
 * 
 * Usage:
 * const ws = new ChatRoomWebSocket({
 *     url: 'ws://localhost:6000/chatroom-service/chatrooms/ws/1',
 *     currentUserId: 123,
 *     currentUsername: 'John Doe',
 *     onMessageReceived: (data) => { ... },
 *     onTypingReceived: (data) => { ... },
 *     onConnected: () => { ... },
 *     onDisconnected: () => { ... },
 *     reconnectionDelayMs: 3000,
 *     typingTimeoutMs: 1000
 * });
 * 
 * ws.sendMessage('Hello world', 'message');
 * ws.sendTypingStatus(true);
 */
class ChatRoomWebSocket {
    constructor(options) {
        this.url = options.url;
        this.currentUserId = options.currentUserId;
        this.currentUsername = options.currentUsername;
        this.onMessageReceived = options.onMessageReceived || (() => {});
        this.onTypingReceived = options.onTypingReceived || (() => {});
        this.onConnected = options.onConnected || (() => {});
        this.onDisconnected = options.onDisconnected || (() => {});
        this.reconnectionDelayMs = options.reconnectionDelayMs || 3000;
        this.typingTimeoutMs = options.typingTimeoutMs || 1000;

        this.socket = null;
        this.typingTimer = null;
    }

    /**
     * Connects to the WebSocket server
     */
    connect() {
        this.socket = new WebSocket(this.url);

        this.socket.onopen = (event) => {
            console.log('WebSocket connected.');
            this.onConnected();
        };

        this.socket.onmessage = (event) => {
            const data = JSON.parse(event.data);

            if (data.type === 'message') {
                this.onMessageReceived(data);
            } else if (data.type === 'typing') {
                this.onTypingReceived(data);
            }
        };

        this.socket.onclose = (event) => {
            console.log('WebSocket disconnected.');
            this.onDisconnected();
            setTimeout(() => this.connect(), this.reconnectionDelayMs);
        };

        this.socket.onerror = (error) => {
            console.error('WebSocket error:', error);
        };
    }

    /**
     * Disconnects from the WebSocket server
     */
    disconnect() {
        if (this.socket) {
            this.socket.close();
            this.socket = null;
        }
        if (this.typingTimer) {
            clearTimeout(this.typingTimer);
            this.typingTimer = null;
        }
    }

    /**
     * Sends a message through the WebSocket
     * @param {string} content - The message content
     * @param {string} type - The message type (default: 'message')
     */
    send(content, type = 'message') {
        if (this.socket && this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(JSON.stringify({
                type: type,
                userId: this.currentUserId,
                username: this.currentUsername,
                content: content
            }));
            return true;
        }
        return false;
    }

    /**
     * Sends a typing status indicator
     * @param {boolean} isTyping - Whether the user is typing
     */
    sendTypingStatus(isTyping) {
        if (this.socket && this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(JSON.stringify({
                type: 'typing',
                userId: this.currentUserId,
                username: this.currentUsername,
                isTyping: isTyping
            }));
            return true;
        }
        return false;
    }

    /**
     * Gets the current connection state
     * @returns {number} WebSocket readyState
     */
    getReadyState() {
        return this.socket ? this.socket.readyState : WebSocket.CLOSED;
    }

    /**
     * Checks if connected
     * @returns {boolean}
     */
    isConnected() {
        return this.socket && this.socket.readyState === WebSocket.OPEN;
    }
}
