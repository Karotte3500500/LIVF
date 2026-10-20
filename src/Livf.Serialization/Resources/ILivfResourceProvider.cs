namespace Livf.Serialization.Resources;

public interface ILivfResourceProvider : IDisposable
{
    bool Exists(string path);
    Stream Open(string path);
}