using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaBatalhaDoCangaco.Engine.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 Rotate(this Vector2 v, float rad)
        {
            var sin = Math.Sin(rad);
            var cos = Math.Cos(rad);
            var tx = v.X;
            var ty = v.Y;
            v.X = (float)(cos * tx - sin * ty);
            v.Y = (float)(sin * tx + cos * ty);
            return v;
        }

        public static float Angle(this Vector2 v)
        {
            var a = MathHelper.TwoPi - (Math.Atan2(v.X, v.Y) * -1);
            if (v.X < 0)
                return (float)a;
            return (float)Math.Atan2(v.X, v.Y);
        }
    }
}
