using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using FootballBlast.Collisions;


namespace FootballBlast
{
    public class Footballer
    {
        private FootballGame game;

        /// <summary>
        /// Current position of the football player
        /// </summary>
        public Vector2 Position { get; private set; }

        private bool isWillie;
        public Texture2D texture;
        private BoundingRectangle bounds;
        /// <summary>
        /// The bounding volume of the Footballer
        /// </summary>
        public BoundingRectangle Bounds => bounds;
        /// <summary>
        /// Boolean representing if the footballer has the ball
        /// </summary>
        public bool HasBall = false;

        public Footballer(FootballGame game, bool isWillie, Vector2 pos)
        {
            this.game = game;
            this.isWillie = isWillie;
            Position = pos;
            bounds = new BoundingRectangle(pos, 16, 55);
        }

        public void LoadContent()
        {
            if (this.isWillie)
            {
                texture = game.Content.Load<Texture2D>("WillieWildcat");
            }
            else
            {
                texture = game.Content.Load<Texture2D>("KUEvilFootball");
            }
        }

        public void Update(Vector2 position)
        {
            this.Position += position;
            
            
            if(this.Position.X >= game.GraphicsDevice.Viewport.Width-64)
            {
                this.Position -= new Vector2(position.X, 0);
            }else if(this.Position.X <= 0)
            {
                this.Position -= new Vector2(position.X, 0);
            }

            if (this.Position.Y >= game.GraphicsDevice.Viewport.Height-90)
            {
                this.Position -= new Vector2(0, position.Y);
            }
            else if (this.Position.Y <= 30)
            {
                this.Position -= new Vector2(0, position.Y);
            }
            bounds.X = this.Position.X;
            bounds.Y = this.Position.Y;
        }
        public void Reset(Vector2 pos)
        {
            Position = pos;
            bounds.X = pos.X;
            bounds.Y = pos.Y;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }
    }
}
