namespace Ramendo.Infrastructure.Persistence;

public sealed class SystemSetting
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = "{}";
    public DateTime UpdatedAt { get; set; }
}
