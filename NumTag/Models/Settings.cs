using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using NumTag.Core.Models;

namespace NumTag.Models;

public record Settings
{
    public required BehaviorSettings DefaultBehavior { get; set; }
    public required ClientSettings Client { get; set; }
    public string? CurrentBehaviorSlot { get; set; } = null;

    private static readonly string BaseDirectory;
    private static readonly string ConfigPath;
    private static readonly string BehaviorDirectory;

    static Settings()
    {
        const string rootDirectory = "NumTag";
        BaseDirectory = OperatingSystem.IsWindows()
            ? Path.Combine(Path.GetDirectoryName(Environment.ProcessPath) ?? Environment.CurrentDirectory,
                rootDirectory)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), rootDirectory);
        ConfigPath = Path.Combine(BaseDirectory, "config.json");
        BehaviorDirectory = Path.Combine(BaseDirectory, "behaviors");
    }

    private static readonly Dictionary<string, BehaviorSettings> BehaviorSlotMap = [];

    public static IReadOnlyCollection<string> BehaviorSlots() => BehaviorSlotMap.Keys;

    public BehaviorSettings MergedBehavior()
    {
        // TODO merge behavior
        return CurrentBehaviorSlot == null
            ? DefaultBehavior
            : BehaviorSlotMap.GetValueOrDefault(CurrentBehaviorSlot, DefaultBehavior);
    }

    public void SaveAsCurrentBehavior(BehaviorSettings settings)
    {
        if (CurrentBehaviorSlot == null) DefaultBehavior = settings;
        else BehaviorSlotMap[CurrentBehaviorSlot] = settings;
    }

    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        AllowOutOfOrderMetadataProperties = true,
        AllowTrailingCommas = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        TypeInfoResolver = ModelJsonContext.Default
    };

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    [UnconditionalSuppressMessage("AOT", "IL3050")]
    private static string SerializeSettings<TSettings>(TSettings obj)
        where TSettings : class
    {
        return JsonSerializer.Serialize(obj, DefaultOptions);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    [UnconditionalSuppressMessage("AOT", "IL3050")]
    private static TSettings? DeserializeSettings<TSettings>(string json)
        where TSettings : class
    {
        try
        {
            return JsonSerializer.Deserialize<TSettings>(json, DefaultOptions)
                   ?? throw new JsonException("Empty (null) value");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Exception thrown while deserializing config");
            Console.Error.WriteLine(ex);
            return null;
        }
    }

    private static string GetBehaviorSlotPath(string slot)
    {
        return Path.Combine(BehaviorDirectory, slot + ".json");
    }

    public void Write(string? behaviorSlot = null)
    {
        if (behaviorSlot == null && CurrentBehaviorSlot != null) Write(CurrentBehaviorSlot);
        var configPath = behaviorSlot == null ? ConfigPath : GetBehaviorSlotPath(behaviorSlot);
        var json = behaviorSlot == null ? SerializeSettings(this) : SerializeSettings(BehaviorSlotMap[behaviorSlot]);
        File.WriteAllText(configPath, json);
    }

    public static void DeleteBehaviorSlot(string behaviorSlot)
    {
        BehaviorSlotMap.Remove(behaviorSlot);
        File.Delete(GetBehaviorSlotPath(behaviorSlot));
    }

    public static Settings Read()
    {
        Directory.CreateDirectory(BaseDirectory);
        Directory.CreateDirectory(BehaviorDirectory);
        // read behaviors
        foreach (var path in Directory.EnumerateFiles(BehaviorDirectory))
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (ext != "json") continue;
            var slot = Path.GetFileNameWithoutExtension(path);
            var behavior = DeserializeSettings<BehaviorSettings>(File.ReadAllText(path));
            BehaviorSlotMap[slot] = behavior ?? new BehaviorSettings();
        }
        Settings? settings = null;
        // try read existed config
        if (File.Exists(ConfigPath))
        {
            var json = File.ReadAllText(ConfigPath);
            if (!string.IsNullOrWhiteSpace(json)) settings = DeserializeSettings<Settings>(json);
        }
        // create new file
        if (settings == null)
        {
            settings = new Settings { DefaultBehavior = new BehaviorSettings(), Client = new ClientSettings() };
            settings.Write();
        }
        return settings;
    }
}
