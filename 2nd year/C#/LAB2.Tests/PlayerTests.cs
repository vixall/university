using Xunit;

public class PlayerTests
{
    [Theory]
    [InlineData(1, 16, 1)]
    [InlineData(16, 16, 16)]
    [InlineData(17, 16, 1)]
    [InlineData(0, 16, 16)]
    [InlineData(-7, 16, 9)]
    [InlineData(32, 16, 16)]
    [InlineData(33, 16, 1)]
    public void Normalize_WrapsAroundBoard(int pos, int size, int expected)
    {
        Assert.Equal(expected, Player.Normalize(pos, size));
    }

    [Fact]
    public void NewPlayer_IsNotInGame()
    {
        var p = new Player("Test");
        Assert.Equal(State.NotInGame, p.state);
        Assert.False(p.IsInitialized);
        Assert.Equal(0, p.distanceTraveled);
        Assert.Equal(-1, p.location);
    }

    [Fact]
    public void SetInitialPosition_DoesNotAddDistance()
    {
        var p = new Player("Mouse");
        p.SetInitialPosition(5, 16);
        Assert.Equal(5, p.location);
        Assert.Equal(0, p.distanceTraveled);
        Assert.Equal(State.Playing, p.state);
        Assert.True(p.IsInitialized);
    }

    [Fact]
    public void SetInitialPosition_NormalizesPosition()
    {
        var p = new Player("Mouse");
        p.SetInitialPosition(20, 16);
        Assert.Equal(4, p.location);
    }

    [Fact]
    public void Move_AddsAbsoluteValueToDistance()
    {
        var p = new Player("Mouse");
        p.SetInitialPosition(5, 16);
        p.Move(-7, 16);
        Assert.Equal(14, p.location);
        Assert.Equal(7, p.distanceTraveled);
    }

    [Fact]
    public void Move_AccumulatesDistance()
    {
        var p = new Player("Cat");
        p.SetInitialPosition(1, 16);
        p.Move(3, 16);
        p.Move(-5, 16);
        p.Move(10, 16);
        Assert.Equal(18, p.distanceTraveled);
    }

    [Fact]
    public void Move_WrapsAroundBoard()
    {
        var p = new Player("Mouse");
        p.SetInitialPosition(15, 16);
        p.Move(5, 16);
        Assert.Equal(4, p.location);
    }

    [Fact]
    public void Move_WhenNotPlaying_DoesNothing()
    {
        var p = new Player("Mouse");
        p.Move(5, 16);
        Assert.Equal(-1, p.location);
        Assert.Equal(0, p.distanceTraveled);
    }
}