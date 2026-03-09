using System.Text.Json.Serialization;

namespace NumTag.Models;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    // AllowOutOfOrderMetadataProperties = true,
    // AllowTrailingCommas = true,
    // WriteIndented = true,
    UseStringEnumConverter = true
)]

[JsonSerializable(typeof(NumTag.Core.Models.BehaviorSettings))]
[JsonSerializable(typeof(Settings))]

public sealed partial class ModelJsonContext : JsonSerializerContext;
