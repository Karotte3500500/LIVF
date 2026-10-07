namespace Livf.Core.States;

public sealed class LivfStateGroup
{
    public string Id { get; }
    public string Name { get; }
    public LivfSelectionMode SelectionMode { get; }
    public string? DefaultStateId { get; }
    public IReadOnlyList<string>? TargetIds { get; }
    public IReadOnlyList<string> StateIds { get; }

    public LivfStateGroup(string id, string name, LivfSelectionMode selectionMode, string? defaultStateId, IReadOnlyList<string>? targetIds, IReadOnlyList<string> stateIds)
    {
        Id = id;
        Name = name;
        SelectionMode = selectionMode;
        DefaultStateId = defaultStateId;
        TargetIds = targetIds is null ? null : Array.AsReadOnly(targetIds.ToArray());
        StateIds = Array.AsReadOnly(stateIds.ToArray());
    }
}