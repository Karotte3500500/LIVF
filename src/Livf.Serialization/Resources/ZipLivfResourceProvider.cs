using System.IO.Compression;

namespace Livf.Serialization.Resources;

internal sealed class ZipLivfResourceProvider : ILivfResourceProvider
{
    private readonly ZipArchive _archive;

    public ZipLivfResourceProvider(ZipArchive archive)
    {
        _archive = archive;
    }

    public bool Exists(string path)
    {
        return FindEntry(path) is not null;
    }

    public Stream Open(string path)
    {
        var entry = FindEntry(path);

        if (entry is null)
        {
            throw new FileNotFoundException
            (
                $"The resource '{path}' was not found in the LIVF container."
            );
        }

        return entry.Open();
    }

    private ZipArchiveEntry? FindEntry(string path)
    {
        return _archive.Entries.FirstOrDefault
        (
            entry => string.Equals
            (
                entry.FullName,
                path,
                StringComparison.Ordinal
            )
        );
    }

    public void Dispose()
    {
        _archive.Dispose();
    }
}
