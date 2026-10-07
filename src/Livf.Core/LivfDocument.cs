using Livf.Core.States;
using Livf.Core.Nodes;

namespace Livf.Core;

public sealed class LivfDocument
{
    public string LivfVersion { get; }
    public LivfMetadata Metadata { get; }
    public LivfCanvas Canvas { get; }

    public string? DefaultStateId { get; }

    public IReadOnlyList<LivfNode> Nodes { get; }

    public IReadOnlyList<LivfState> States { get; }
    public IReadOnlyList<LivfStateGroup> StateGroups { get; }


    public LivfDocument(string livfVersion, LivfMetadata metadata, LivfCanvas canvas, string? defaultStateId, IReadOnlyList<LivfNode> nodes, IReadOnlyList<LivfState> states, IReadOnlyList<LivfStateGroup> stateGroups)
    {
        LivfVersion = livfVersion;
        Metadata = metadata;
        Canvas = canvas;
        DefaultStateId = defaultStateId;
        Nodes = Array.AsReadOnly(nodes.ToArray());
        States = Array.AsReadOnly(states.ToArray());
        StateGroups = Array.AsReadOnly(stateGroups.ToArray());
    }

    /// <summary>
    /// ノードを列挙する。フォルダの子孫も含む。
    /// This method enumerates all nodes, including descendants of folders.
    /// </summary>
    public IEnumerable<LivfNode> EnumerateNodes()
    {
        foreach (var node in Nodes)
        {
            yield return node;

            if (node is not LivfFolder folder)
            {
                continue;
            }
            
            foreach (var descendant in folder.EnumerateDescendants())
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// 指定されたIDを持つノードを列挙する。このメソッドではIDの一意性を検証しない。
    /// Enumerates nodes with the specified ID. This method does not validate ID uniqueness.
    /// </summary>
    public IEnumerable<LivfNode> FindNodes(string id)
    {
        return EnumerateNodes().Where(n => n.Id == id);
    }

    /// <summary>
    /// 指定されたIDを持つステートを列挙する。このメソッドではIDの一意性を検証しない。
    /// Enumerates states with the specified ID. This method does not validate ID uniqueness.
    /// </summary>
    public IEnumerable<LivfState> FindStates(string id)
    {
        return States.Where(s => s.Id == id);
    }

    /// <summary>
    /// 指定されたIDを持つステートグループを列挙する。このメソッドではIDの一意性を検証しない。
    /// Enumerates state groups with the specified ID. This method does not validate ID uniqueness.
    /// </summary>
    public IEnumerable<LivfStateGroup> FindStateGroups(string id)
    {
        return StateGroups.Where(sg => sg.Id == id);
    }
}
