﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace FootballBlast.Collisions
{
    public static class CollisionHelper
    {
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            bool yo = !(a.Right < b.Left || a.Left > b.Right ||
                        a.Top > b.Bottom || a.Bottom < b.Top);
            return !(a.Right < b.Left || a.Left > b.Right ||
                        a.Top > b.Bottom || a.Bottom < b.Top);
        }

        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            bool stuff = Math.Pow(c.Radius, 2) >= (Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2));
            return Math.Pow(c.Radius, 2) >= (Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2));
        }
    }
}
