using UnityEngine;

namespace BrickBreaker
{
    public class TargetController : MonoBehaviour
    {
        private void Awake()
        {
            var initialTargets = GetComponentsInChildren<Target>();
            foreach (var target in initialTargets) {
                target.Destroyed += OnTargetDestroyed;
            }
        }
    }
}