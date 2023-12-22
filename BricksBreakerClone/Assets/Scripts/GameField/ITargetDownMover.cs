namespace BrickBreaker
{
    public interface ITargetDownMover
    {
        bool IsMoving { get; }
        void PrepareNextPosition();
        void MakeStep();
    }
}