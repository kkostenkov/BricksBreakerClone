using System;
using UnityEngine;

namespace BrickBreaker
{
    public class GameLostTrigger : MonoBehaviour, IGameLostNotifier
    {
        public event Action TargetReachedGameLostTrigger;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag(Constants.Tags.Target)) {
                TargetReachedGameLostTrigger?.Invoke();
            }
        }
    }
}