using System;

namespace BrickBreaker
{
    public interface ITargetsDestroyedNotifier
    {
        event Action AllTargetsDestroyed;
    }
}