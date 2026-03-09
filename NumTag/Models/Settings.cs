using System.Text.Json.Serialization;
using NumTag.Core.Models;

namespace NumTag.Models;

[JsonSerializable(typeof(Settings))]
public record Settings(
    BehaviorSettings DefaultBehavior,
    ClientSettings Client
);
