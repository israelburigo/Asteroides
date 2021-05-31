using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaBatalhaDoCangaco.Engine.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 Rotate(this Vector2 v, float rad)
        {
            return v.Rotate(rad, Vector2.Zero);
        }

        public static Vector2 Rotate(this Vector2 v, float rad, Vector2 origin)
        {
            var sin = Math.Sin(rad);
            var cos = Math.Cos(rad);
            var tx = v.X - origin.X;
            var ty = v.Y - origin.Y;
            v.X = (float)(cos * tx - sin * ty) + origin.X;
            v.Y = (float)(sin * tx + cos * ty) + origin.Y;
            return v;
        }
        
        public static Tuple<int, Vector2, Vector2> IntersectCircle(this Vector2 p2, Vector2 p1, Vector2 c, float r)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;

            var A = dx * dx + dy * dy;
            var B = 2 * (dx * (p1.X - c.X) + dy * (p1.Y - c.Y));
            var C = (p1.X - c.X) * (p1.X - c.X) + (p1.Y - c.Y) * (p1.Y - c.Y) - r * r;

            var det = B * B - 4 * A * C;

            if ((A <= 0.0000001) || (det < 0))
                return Tuple.Create(0, Vector2.Zero, Vector2.Zero);

            if(det == 0)
            {
                var t = -B / (2 * A);
                var v = new Vector2(p1.X + t * dx, p1.Y + t * dy);
                return Tuple.Create(1, v, Vector2.Zero);
            }
            else
            {
                var t1 = (-B + MathF.Sqrt(det)) / (2 * A);
                var t2 = (-B - MathF.Sqrt(det)) / (2 * A);
                var v1 = new Vector2(p1.X + t1 * dx, p1.Y + t1 * dy);
                var v2 = new Vector2(p1.X + t2 * dx, p1.Y + t2 * dy);
                return Tuple.Create(2, v1, v2);
            }
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
