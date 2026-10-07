namespace Livf.Core.Nodes;

public sealed class LivfFolder : LivfNode
{
    public LivfSelectionMode? SelectionMode { get; }
    public string? DefaultChildId { get; }
    public IReadOnlyList<LivfNode> Children { get; }

    public LivfFolder(string id, string name, bool visible, double? opacity, bool? locked, IReadOnlyList<string>? tags, LivfSelectionMode? selectionMode, string? defaultChildId, IReadOnlyList<LivfNode> children)
        : base(id, name, visible, opacity, locked, tags)
    {
        SelectionMode = selectionMode;
        DefaultChildId = defaultChildId;
        Children = children.ToArray();
    }
}