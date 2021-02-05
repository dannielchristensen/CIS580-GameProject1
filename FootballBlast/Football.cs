using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FootballBlast.Collisions;

namespace FootballBlast
{
    public class Football
    {
        private FootballGame game;
        public Texture2D texture;
        public bool IsCollected = false;
        private BoundingCircle bounds;
        private Random r;
        /// <summary>
        /// The bounding volume of the ball.
        /// </summary>
        public BoundingCircle Bounds => bounds;
        public Vector2 Position { get; private set; }

        public Football(FootballGame game)
        {
            this.game = game;
            r = new Random();
            Position = new Vector2(100, 100);
            this.Punt();
            this.bounds = new BoundingCircle(Position, 16);

        }

        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("Football");


        }
        public void Update(Vector2 pos)
        {
            Position = new Vector2(pos.X, pos.Y);
            bounds.Center = Position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture is null) throw new InvalidOperationException("Texture must be loaded to render");
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }

        public void Punt()
        {

            Position = new Vector2(
                // https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
                // for the * (float) (max-min) + min to keep the ball on the screen
                (float)r.NextDouble() * (game.GraphicsDevice.Viewport.Width-300)+100,
                (float)r.NextDouble() * (game.GraphicsDevice.Viewport.Height-300)+200
                );
            bounds.Center = Position;

        }
    }
}
