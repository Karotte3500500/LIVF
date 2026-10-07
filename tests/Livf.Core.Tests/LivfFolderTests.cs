using Livf.Core.Nodes;

namespace Livf.Core.Tests;

public class LivfFolderTests
{
    [Fact]
    public void EnumerateDescendants_EnumeratesNestedNodesInDeclarationOrder()
    {
        var layerA = new LivfLayer
        (
            "a",
            "A",
            true,
            null,
            null,
            null,
            "images/a.png",
            0,
            0
        );
        
        var layerB = new LivfLayer
        (
            "b",
            "B",
            true,
            null,
            null,
            null,
            "images/b.png",
            0,
            0
        );

        var layerC = new LivfLayer
        (
            "c",
            "C",
            true,
            null,
            null,
            null,
            "images/c.png",
            0,
            0
        );

        var innerFolder = new LivfFolder
        (
            "inner",
            "Inner",
            true,
            null,
            null,
            null,
            null,
            null,
            new LivfNode[]
            {
                layerB,
                layerC
            }
        );

        var rootFolder = new LivfFolder
        (
            "root",
            "Root",
            true,
            null,
            null,
            null,
            null,
            null,
            new LivfNode[]
            {
                layerA,
                innerFolder
            }
        );

        var result = rootFolder.EnumerateDescendants().Select(node => node.Id).ToArray();

        Assert.Equal(new[] { "a", "inner", "b", "c" }, result);
    }

    [Fact]
    public void Constructor_CopiesChildren()
    {
        var layerA = new LivfLayer
        (
            "a",
            "A",
            true,
            null,
            null,
            null,
            "images/a.png",
            0,
            0
        );

        var layerB = new LivfLayer
        (
            "b",
            "B",
            true,
            null,
            null,
            null,
            "images/b.png",
            0,
            0
        );

        var children = new List<LivfNode>
        {
            layerA
        };

        var folder = new LivfFolder
        (
            "folder",
            "Folder",
            true,
            null,
            null,
            null,
            null,
            null,
            children
        );

        children.Add(layerB);

        Assert.Single(folder.Children);
        Assert.Same(layerA, folder.Children[0]);
    }
}