using Xunit;
using task04;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Spaceships_ShouldMoveForward()
    {
        var fighter = new Fighter();
        
        fighter.MoveForward();
        
        Assert.True(fighter.IsMoving);
    }

    [Fact]
    public void Spaceships_ShouldRotateCorrectly()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(90);
        
        Assert.Equal(90, cruiser.CurrentAngle);
    }

    [Fact]
    public void Spaceships_ShouldFireWeapon()
    {
        var fighter = new Fighter();
        
        fighter.Fire();
        
        Assert.True(fighter.HasFired);
    }
}