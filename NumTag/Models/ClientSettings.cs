namespace NumTag.Models;

public record struct ClientSettings(
    int ClientId = 0,
    int CurrentBehaviorSlot = 0,
    string ServerAddress = ""
);
