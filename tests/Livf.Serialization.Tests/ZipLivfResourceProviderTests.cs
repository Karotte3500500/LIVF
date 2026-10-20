using System.IO.Compression;
using Livf.Serialization.Resources;

namespace Livf.Serialization.Tests.Resources;

public sealed class ZipLivfResourceProviderTests
{
    // パスが存在する場合、Existsメソッドはtrueを返すことを確認するテスト
    [Fact]
    public void Exists_ReturnsTrue_WhenEntryExists()
    {
        using var stream = CreateZip();
        using var provider = CreateProvider(stream);

        var exists = provider.Exists("images/test.bin");

        Assert.True(exists);
    }

    // パスが存在しない場合、Existsメソッドはfalseを返すことを確認するテスト
    [Fact]
    public void Exists_ReturnsFalse_WhenEntryDoesNotExist()
    {
        using var stream = CreateZip();
        using var provider = CreateProvider(stream);

        var exists = provider.Exists("images/missing.bin");

        Assert.False(exists);
    }

    // 文字列の大文字小文字を区別することを確認するテスト
    [Fact]
    public void Exists_IsCaseSensitive()
    {
        using var stream = CreateZip();
        using var provider = CreateProvider(stream);

        Assert.True(provider.Exists("images/test.bin"));
        Assert.False(provider.Exists("images/Test.bin"));
    }

    // Openメソッドが存在するエントリの内容を返すことを確認するテスト
    [Fact]
    public void Open_ReturnsEntryContent()
    {
        using var stream = CreateZip();
        using var provider = CreateProvider(stream);
        using var resource = provider.Open("images/test.bin");

        using var buffer = new MemoryStream();
        resource.CopyTo(buffer);

        Assert.Equal(new byte[] { 1, 2, 3, 4 }, buffer.ToArray());
    }

    /*ヘルパー*/
    private static MemoryStream CreateZip()
    {
        var stream = new MemoryStream();

        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            var manifest = archive.CreateEntry("manifest.json");

            using (var writer = new StreamWriter(manifest.Open()))
            {
                writer.Write("{}");
            }

            var image = archive.CreateEntry("images/test.bin");

            using (var resourceStream = image.Open())
            {
                resourceStream.Write([1, 2, 3, 4]);
            }
        }

        stream.Position = 0;

        return stream;
    }

    [Fact]
    public void Open_ThrowsFileNotFoundException_WhenEntryDoesNotExist()
    {
        using var stream = CreateZip();
        using var provider = CreateProvider(stream);

        Assert.Throws<FileNotFoundException>(() => provider.Open("images/missing.bin"));
    }

    private static ZipLivfResourceProvider CreateProvider(MemoryStream stream)
    {
        var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        return new ZipLivfResourceProvider(archive);
    }
}