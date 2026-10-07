using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfChangeTests
{
    [Fact]
    public void Constructor_PreservesOmittedVisibleAndExplicitZeroOpacity()
    {
        var change = new LivfChange
        (
            "layer",
            null,
            0.0
        );

        Assert.Null(change.Visible);
        Assert.Equal(0.0, change.Opacity);
    }

    [Fact]
    public void Constructor_PreservesExplicitFalseAndOmittedOpacity()
    {
        var change = new LivfChange
        (
            "layer",
            false,
            null
        );

        Assert.False(change.Visible);
        Assert.Null(change.Opacity);
    }
}