using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfStateTests
{
    // State構築後に入力元のChangeとタグを変更しても、宣言内容は変わらない。
    [Fact]
    public void Constructor_CopiesChangesAndTags()
    {
        var change = new LivfChange
        (
            "layer",
            true,
            null
        );

        var changes = new List<LivfChange> { change };
        var tags = new List<string> { "expression" };

        var state = new LivfState
        (
            "state",
            "State",
            changes,
            tags
        );

        changes.Clear();
        tags.Clear();

        Assert.Same(change, Assert.Single(state.Changes));
        Assert.Equal("expression", Assert.Single(state.Tags!));
    }

    // tagsの省略と空リストの明示を区別して保持する。
    [Fact]
    public void Constructor_DistinguishesOmittedAndEmptyTags()
    {
        var withoutTags = new LivfState
        (
            "without",
            "Without",
            [],
            null
        );

        var withEmptyTags = new LivfState
        (
            "empty",
            "Empty",
            [],
            []
        );

        Assert.Null(withoutTags.Tags);
        Assert.NotNull(withEmptyTags.Tags);
        Assert.Empty(withEmptyTags.Tags);
    }
}
