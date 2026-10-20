using Livf.Core;
using Livf.Serialization.Resources;

namespace Livf.Serialization;

public sealed class LivfPackage : IDisposable
{
    public LivfDocument Document { get; }
    public ILivfResourceProvider Resource { get; }

    public LivfPackage(LivfDocument document, ILivfResourceProvider resource)
    {
        Document = document;
        Resource = resource;
    }

    public void Dispose()
    {
        Resource.Dispose();
    }
}