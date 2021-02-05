using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace FootballBlast.Collisions
{
    public struct BoundingCircle
    {
        /// <summary>
        /// The center of the BoundingCircle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// The radius of the BoundingCircle.
        /// </summary>
        public float Radius;

        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;

        }
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

    }
}
