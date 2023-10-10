namespace TGProVN.Extension.Browser.Events;

public class ChangedEventArgs
{
    public string? Key { get; set; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
}