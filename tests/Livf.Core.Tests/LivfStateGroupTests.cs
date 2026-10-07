using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfStateGroupTests
{
    [Fact]
    public void Constructor_CopiesTargetsAndStateIds()
    {
        var targetIds = new List<string> { "layer" };
        var stateIds = new List<string> { "state" };

        var group = new LivfStateGroup
        (
            "group",
            "Group",
            LivfSelectionMode.Multiple,
            null,
            targetIds,
            stateIds
        );

        targetIds.Clear();
        stateIds.Clear();

        Assert.Equal("layer", Assert.Single(group.TargetIds!));
        Assert.Equal("state", Assert.Single(group.StateIds));
    }

    [Fact]
    public void Constructor_DistinguishesOmittedAndEmptyTargets()
    {
        var withoutTargets = new LivfStateGroup
        (
            "without",
            "Without",
            LivfSelectionMode.Multiple,
            null,
            null,
            []
        );

        var withEmptyTargets = new LivfStateGroup
        (
            "empty",
            "Empty",
            LivfSelectionMode.Multiple,
            null,
            [],
            []
        );

        Assert.Null(withoutTargets.TargetIds);
        Assert.NotNull(withEmptyTargets.TargetIds);
        Assert.Empty(withEmptyTargets.TargetIds);
    }
}
