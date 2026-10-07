namespace Livf.Core.States;

public sealed class LivfState
{
    public string Id { get; }
    public string Name { get; }
    public IReadOnlyList<LivfChange> Changes { get; }
    public IReadOnlyList<string>? Tags { get; }


    public LivfState(string id, string name, IReadOnlyList<LivfChange> changes, IReadOnlyList<string>? tags)
    {
        Id = id;
        Name = name;
        Changes = Array.AsReadOnly(changes.ToArray());
        Tags = tags is null ? null : Array.AsReadOnly(tags.ToArray());
    }
}