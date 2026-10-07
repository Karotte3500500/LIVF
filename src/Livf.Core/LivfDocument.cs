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
        Nodes = nodes.ToArray();
        States = states.ToArray();
        StateGroups = stateGroups.ToArray();
    }

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
}
