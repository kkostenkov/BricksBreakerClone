using UnityEngine;

namespace BrickBreaker
{
    public static class VectorExtentions
    {
        public static bool IsCloseEnoughTo(this Vector3 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2) < 0.0001f;
        }
    }
}