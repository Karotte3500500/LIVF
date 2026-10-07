namespace Livf.Core.Nodes;

public abstract class LivfNode
{
    public string Id { get; }
    public string Name { get; }
    public bool Visible { get; }
    public double? Opacity { get; }
    public bool? Locked { get; }

    public IReadOnlyList<string>? Tags { get; }

    protected LivfNode(string id, string name, bool visible, double? opacity, bool? locked, IReadOnlyList<string>? tags)
    {
        Id = id;
        Name = name;
        Visible = visible;
        Opacity = opacity;
        Locked = locked;
        Tags = tags?.ToArray();
    }
}