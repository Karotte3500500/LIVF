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
        Children = Array.AsReadOnly(children.ToArray());
    }

    /// <summary>
    /// 子孫をすべて列挙する。子要素とその子孫を再帰的に含む。
    /// Enumerates all descendants of this folder, including children and their descendants recursively.
    /// </summary>
    public IEnumerable<LivfNode> EnumerateDescendants()
    {
        foreach (var child in Children)
        {
            yield return child;

            if (child is not LivfFolder folder)
            {
                continue;
            }
            
            foreach (var descendant in folder.EnumerateDescendants())
            {
                yield return descendant;
            }
        }
    }
}