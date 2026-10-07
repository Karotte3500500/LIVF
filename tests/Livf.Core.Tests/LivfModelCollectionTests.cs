using Livf.Core.Nodes;
using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfModelCollectionTests
{
    // 公開コレクションを配列やリストとして扱っても、宣言内容を差し替えられない。
    [Fact]
    public void PublicCollections_CannotBeModifiedThroughExposedLists()
    {
        var metadata = new LivfMetadata
        (
            "Test",
            null,
            null,
            null,
            null,
            null,
            null,
            new[] { "metadata" }
        );

        var layer = new LivfLayer
        (
            "layer",
            "Layer",
            true,
            null,
            null,
            new[] { "node" },
            "images/layer.png",
            0,
            0
        );

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
            new LivfNode[] { layer }
        );

        var change = new LivfChange
        (
            "layer",
            true,
            null
        );

        var state = new LivfState
        (
            "state",
            "State",
            new[] { change },
            new[] { "state" }
        );

        var group = new LivfStateGroup
        (
            "group",
            "Group",
            LivfSelectionMode.Multiple,
            null,
            new[] { "layer" },
            new[] { "state" }
        );

        var document = new LivfDocument
        (
            "0.1.0",
            metadata,
            new LivfCanvas(512, 768),
            null,
            new LivfNode[] { folder },
            new[] { state },
            new[] { group }
        );

        AssertCannotReplaceFirstElement(document.Nodes);
        AssertCannotReplaceFirstElement(document.States);
        AssertCannotReplaceFirstElement(document.StateGroups);
        AssertCannotReplaceFirstElement(metadata.Tags!);
        AssertCannotReplaceFirstElement(layer.Tags!);
        AssertCannotReplaceFirstElement(folder.Children);
        AssertCannotReplaceFirstElement(state.Changes);
        AssertCannotReplaceFirstElement(state.Tags!);
        AssertCannotReplaceFirstElement(group.TargetIds!);
        AssertCannotReplaceFirstElement(group.StateIds);
    }

    // MetadataとNodeのtagsは、省略と空リストの明示を区別して保持する。
    [Fact]
    public void OptionalTags_DistinguishOmittedAndEmptyLists()
    {
        var metadataWithoutTags = new LivfMetadata
        (
            "Without",
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        var metadataWithEmptyTags = new LivfMetadata
        (
            "Empty",
            null,
            null,
            null,
            null,
            null,
            null,
            []
        );

        var layerWithoutTags = new LivfLayer
        (
            "without",
            "Without",
            true,
            null,
            null,
            null,
            "images/without.png",
            0,
            0
        );

        var layerWithEmptyTags = new LivfLayer
        (
            "empty",
            "Empty",
            true,
            null,
            null,
            [],
            "images/empty.png",
            0,
            0
        );

        Assert.Null(metadataWithoutTags.Tags);
        Assert.NotNull(metadataWithEmptyTags.Tags);
        Assert.Empty(metadataWithEmptyTags.Tags);
        Assert.Null(layerWithoutTags.Tags);
        Assert.NotNull(layerWithEmptyTags.Tags);
        Assert.Empty(layerWithEmptyTags.Tags);
    }

    private static void AssertCannotReplaceFirstElement<T>(IReadOnlyList<T> values)
    {
        Assert.False(values is T[]);

        if (values is IList<T> list)
        {
            Assert.Throws<NotSupportedException>(() => list[0] = values[0]);
        }
    }
}
