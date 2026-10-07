namespace Livf.Core.States;

public sealed class LivfChange
{
    public string TargetId { get; }
    public bool? Visible { get; }
    public double? Opacity { get; }

    public LivfChange(string targetId, bool? visible, double? opacity)
    {
        TargetId = targetId;
        Visible = visible;
        Opacity = opacity;
    }
}