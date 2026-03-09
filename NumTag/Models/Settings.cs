using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using NumTag.Core.Models;

namespace NumTag.Models;

public record Settings(
    BehaviorSettings DefaultBehavior,
    ClientSettings Client,
    string? CurrentBehaviorSlot = null
)
{
    private static readonly string BaseDirectory;
    private static readonly string ConfigPath;
    private static readonly string BehaviorDirectory;

    static Settings()
    {
        const string rootDirectory = "NumTag";
        BaseDirectory = OperatingSystem.IsWindows()
            ? Path.Combine(Environment.ProcessPath ?? Environment.CurrentDirectory, rootDirectory)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), rootDirectory);
        ConfigPath = Path.Combine(BaseDirectory, "config.json");
        BehaviorDirectory = Path.Combine(BaseDirectory, "behaviors");
    }

    public BehaviorSettings MergedBehavior()
    {
        // TODO merge behavior
        return DefaultBehavior;
    }

    private static readonly Dictionary<string, BehaviorSettings> BehaviorSlots = [];

    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        AllowOutOfOrderMetadataProperties = true,
        AllowTrailingCommas = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        TypeInfoResolver = ModelJsonContext.Default
    };

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public void Write(string? behaviorSlot = null)
    {
        var configPath = behaviorSlot == null ? ConfigPath : Path.Combine(BehaviorDirectory, behaviorSlot + ".json");
        using var configFile = File.Open(configPath, FileMode.Create, FileAccess.Write, FileShare.Read);
        if (behaviorSlot == null) JsonSerializer.Serialize(configFile, this, DefaultOptions);
        else JsonSerializer.Serialize(configFile, BehaviorSlots[behaviorSlot], DefaultOptions);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public static Settings Read()
    {
        Directory.CreateDirectory(BaseDirectory);
        Directory.CreateDirectory(BehaviorDirectory);
        // try read behaviors
        foreach (var path in Directory.EnumerateFiles(BehaviorDirectory))
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (ext != "json") continue;
            var slot = Path.GetFileNameWithoutExtension(path);
            using var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var behavior = JsonSerializer.Deserialize<BehaviorSettings>(file, DefaultOptions);
            BehaviorSlots[slot] = behavior;
        }
        Settings? settings = null;
        // try read existed config
        if (File.Exists(ConfigPath))
        {
            var json = File.ReadAllText(ConfigPath);
            if (!string.IsNullOrWhiteSpace(json)) settings = JsonSerializer
                .Deserialize<Settings>(json, DefaultOptions) ?? throw new JsonException("Empty (null) value");
        }
        // create new file
        if (settings == null)
        {
            settings = new Settings(new BehaviorSettings(), new ClientSettings());
            settings.Write();
        }
        return settings;
    }
}
