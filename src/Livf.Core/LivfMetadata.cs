namespace Livf.Core;

public sealed class LivfMetadata
{
    public string Title { get; }
    public string? Author { get; }
    public string? Description { get; }
    
    public string? CreatedAt { get; }
    public string? ModifiedAt { get; }

    public string? Application { get; }

    public string? License { get; }

    public IReadOnlyList<string>? Tags { get; }

    public LivfMetadata(string title, string? author, string? description, string? createdAt, string? modifiedAt, string? application, string? license, IReadOnlyList<string>? tags)
    {
        Title = title;
        Author = author;
        Description = description;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        Application = application;
        License = license;
        Tags = tags is null ? null : Array.AsReadOnly(tags.ToArray());
    }

}