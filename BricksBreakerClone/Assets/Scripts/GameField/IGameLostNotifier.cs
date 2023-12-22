using System;

namespace BrickBreaker
{
    public interface IGameLostNotifier
    {
        event Action TargetReachedGameLostTrigger;
    }
}