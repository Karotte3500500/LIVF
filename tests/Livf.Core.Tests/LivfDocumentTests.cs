using Livf.Core.Nodes;
using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfDocumentTests
{
    // IDが重複しても、該当するNodeを宣言順にすべて返す。
    [Fact]
    public void FindNodes_ReturnsAllNodesWithDuplicateIds()
    {
        var layerA = new LivfLayer
        (
            "duplicate",
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
            "duplicate",
            "B",
            true,
            null,
            null,
            null,
            "images/b.png",
            0,
            0
        );

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata
            (
                "Test",
                null,
                null,
                null,
                null,
                null,
                null,
                null
            ),
            new LivfCanvas(512, 768),
            null,
            new LivfNode[]
            {
                layerA,
                layerB
            },
            [],
            []
        );

        var result = document.FindNodes("duplicate").ToArray();

        Assert.Equal(2, result.Length);
        Assert.Same(layerA, result[0]);
        Assert.Same(layerB, result[1]);
    }

    // ルートNodeとFolderの子孫を宣言順に列挙する。
    [Fact]
    public void EnumerateNodes_EnumeratesRootAndNestedNodesInDeclarationOrder()
    {
        var nestedLayer = new LivfLayer
        (
            "nested",
            "Nested",
            true,
            null,
            null,
            null,
            "images/nested.png",
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
            new LivfNode[] { nestedLayer }
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
            new LivfNode[] { innerFolder }
        );

        var rootLayer = new LivfLayer
        (
            "sibling",
            "Sibling",
            true,
            null,
            null,
            null,
            "images/sibling.png",
            0,
            0
        );

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            new LivfNode[] { rootFolder, rootLayer },
            [],
            []
        );

        var result = document.EnumerateNodes().Select(node => node.Id).ToArray();

        Assert.Equal(new[] { "root", "inner", "nested", "sibling" }, result);
    }

    // Folder内にあるNodeもIDから参照できる。
    [Fact]
    public void FindNodes_FindsNodeInsideFolder()
    {
        var layer = new LivfLayer
        (
            "nested",
            "Nested",
            true,
            null,
            null,
            null,
            "images/nested.png",
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

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            new LivfNode[] { folder },
            [],
            []
        );

        Assert.Same(layer, Assert.Single(document.FindNodes("nested")));
    }

    // Node、State、StateGroupのIDは大文字と小文字を区別し、該当しない場合は空を返す。
    [Fact]
    public void FindMethods_DistinguishCaseAndReturnEmptyWhenMissing()
    {
        var layer = new LivfLayer
        (
            "Layer",
            "Layer",
            true,
            null,
            null,
            null,
            "images/layer.png",
            0,
            0
        );

        var state = new LivfState("State", "State", [], null);
        var group = new LivfStateGroup("Group", "Group", LivfSelectionMode.Multiple, null, null, []);

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            new LivfNode[] { layer },
            new LivfState[] { state },
            new LivfStateGroup[] { group }
        );

        Assert.Same(layer, Assert.Single(document.FindNodes("Layer")));
        Assert.Empty(document.FindNodes("layer"));
        Assert.Empty(document.FindNodes("missing"));
        Assert.Same(state, Assert.Single(document.FindStates("State")));
        Assert.Empty(document.FindStates("state"));
        Assert.Empty(document.FindStates("missing"));
        Assert.Same(group, Assert.Single(document.FindStateGroups("Group")));
        Assert.Empty(document.FindStateGroups("group"));
        Assert.Empty(document.FindStateGroups("missing"));
    }

    // IDが重複しても、該当するStateを宣言順にすべて返す。
    [Fact]
    public void FindStates_ReturnsAllStatesWithDuplicateIdsInDeclarationOrder()
    {
        var stateA = new LivfState("duplicate", "A", [], null);
        var otherState = new LivfState("other", "Other", [], null);
        var stateB = new LivfState("duplicate", "B", [], null);

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            [],
            new LivfState[] { stateA, otherState, stateB },
            []
        );

        var result = document.FindStates("duplicate").ToArray();

        Assert.Equal(2, result.Length);
        Assert.Same(stateA, result[0]);
        Assert.Same(stateB, result[1]);
    }

    // IDが重複しても、該当するStateGroupを宣言順にすべて返す。
    [Fact]
    public void FindStateGroups_ReturnsAllGroupsWithDuplicateIdsInDeclarationOrder()
    {
        var groupA = new LivfStateGroup("duplicate", "A", LivfSelectionMode.Multiple, null, null, []);
        var otherGroup = new LivfStateGroup("other", "Other", LivfSelectionMode.Multiple, null, null, []);
        var groupB = new LivfStateGroup("duplicate", "B", LivfSelectionMode.Multiple, null, null, []);

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            [],
            [],
            new LivfStateGroup[] { groupA, otherGroup, groupB }
        );

        var result = document.FindStateGroups("duplicate").ToArray();

        Assert.Equal(2, result.Length);
        Assert.Same(groupA, result[0]);
        Assert.Same(groupB, result[1]);
    }

    // Document構築後に入力元のリストを変更しても、宣言内容は変わらない。
    [Fact]
    public void Constructor_CopiesNodesStatesAndStateGroups()
    {
        var layer = new LivfLayer
        (
            "layer",
            "Layer",
            true,
            null,
            null,
            null,
            "images/layer.png",
            0,
            0
        );
        var state = new LivfState("state", "State", [], null);
        var group = new LivfStateGroup("group", "Group", LivfSelectionMode.Multiple, null, null, []);

        var nodes = new List<LivfNode> { layer };
        var states = new List<LivfState> { state };
        var stateGroups = new List<LivfStateGroup> { group };

        var document = new LivfDocument
        (
            "0.1.0",
            new LivfMetadata("Test", null, null, null, null, null, null, null),
            new LivfCanvas(512, 768),
            null,
            nodes,
            states,
            stateGroups
        );

        nodes.Clear();
        states.Clear();
        stateGroups.Clear();

        Assert.Same(layer, Assert.Single(document.Nodes));
        Assert.Same(state, Assert.Single(document.States));
        Assert.Same(group, Assert.Single(document.StateGroups));
    }
}
