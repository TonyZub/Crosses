using UnityEngine;


namespace Extensions
{
    public static partial class Vector2Extensions
    {
        public static float GetRandom(this Vector2 v)
        {
            return Random.Range(v.x, v.y);
        }

        public static Vector2 GetRoundPosition(this Vector2 value)
        {
            value.x = Mathf.Round(value.x * 100f) / 100f;
            value.y = Mathf.Round(value.y * 100f) / 100f;
            return value;
        }
    }
}
