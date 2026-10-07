namespace Livf.Core;

public sealed class LivfCanvas
{
    public int Width { get; }
    public int Height { get; }

    public LivfCanvas(int width, int height)
    {
        Width = width;
        Height = height;
    }
}