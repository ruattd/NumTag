namespace NumTag.Models;

public record struct ClientSettings(
    int ClientId = 0,
    string ServerAddress = ""
);
