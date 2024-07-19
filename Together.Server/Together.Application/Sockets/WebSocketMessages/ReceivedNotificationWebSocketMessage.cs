﻿using Together.Application.Features.FeatureNotification.Responses;

namespace Together.Application.Sockets.WebSocketMessages;

[WebSocketMessageTarget(WebSocketClientTarget.ReceivedNotification)]
public class ReceivedNotificationWebSocketMessage : NotificationViewModel;