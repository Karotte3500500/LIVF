using Livf.Core.States;

namespace Livf.Core.Tests;

public class LivfChangeTests
{
    // visibleの省略と、opacityに明示した0を区別して保持する。
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

    // visibleに明示したfalseと、opacityの省略を区別して保持する。
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