namespace Livf.Serialization.Resources;

public interface ILivfResourceProvider
{
    bool Exists(string path);
    Stream Open(string path);
}