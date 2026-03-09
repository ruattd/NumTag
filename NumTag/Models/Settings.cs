using System.Text.Json.Serialization;
using NumTag.Core.Models;

namespace NumTag.Models;

[JsonSerializable(typeof(Settings))]
public record Settings(
    BehaviorSettings DefaultBehavior,
    ClientSettings Client
)
{
    public BehaviorSettings MergedBehavior()
    {
        // TODO merge behavior
        return DefaultBehavior;
    }
}
