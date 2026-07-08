namespace task04;

public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 50;
    
    public bool IsMoving { get; private set; }
    public int CurrentAngle { get; private set; }
    public bool HasFired { get; private set; }

    public void MoveForward() => IsMoving = true;
    public void Rotate(int angle) => CurrentAngle += angle;
    public void Fire() => HasFired = true;
}
