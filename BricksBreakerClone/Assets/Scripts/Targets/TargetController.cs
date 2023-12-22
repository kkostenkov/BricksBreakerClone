using System;
using UnityEngine;

namespace BrickBreaker
{
    public class TargetController : MonoBehaviour, ITargetsDestroyedNotifier
    {
        private int targetsCount;
        private Target[] initialTargets;
        
        public event Action AllTargetsDestroyed;

        private void Awake()
        {
            this.initialTargets = GetComponentsInChildren<Target>();
            this.targetsCount = initialTargets.Length;
            foreach (var target in this.initialTargets) {
                target.Destroying += OnTargetDestroying;
            }
        }

        private void OnDestroy()
        {
            foreach (var target in this.initialTargets) {
                if (target) {
                    target.Destroying -= OnTargetDestroying;
                }
            }
        }

        private void OnTargetDestroying(Target target)
        {
            target.Destroying -= OnTargetDestroying;
            targetsCount--;
            if (this.targetsCount <= 0) {
                AllTargetsDestroyed?.Invoke();
            }
        }
    }
}