namespace Livf.Core.Nodes;

public sealed class LivfLayer : LivfNode
{
    public string Source { get; }
    public int X { get; }
    public int Y { get; }
    public LivfLayer(string id, string name, bool visible, double? opacity, bool? locked, IReadOnlyList<string>? tags, string source, int x, int y)
        : base(id, name, visible, opacity, locked, tags)
    {
        Source = source;
        X = x;
        Y = y;
    }
}