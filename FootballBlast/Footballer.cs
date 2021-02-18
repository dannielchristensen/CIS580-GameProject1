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
        const float LINEAR_ACCELERATION = 75;

        private FootballGame game;
        KeyboardState currentState;
        KeyboardState priorState;
        public Vector2 Direction { get; private set; }

        public Vector2 velocity;
        /// <summary>
        /// boolean to determine if current player has hit a note sprite
        /// and has a speed boost
        /// </summary>
        public bool HasSpeedup = false;
        /// <summary>
        /// Current position of the football player
        /// </summary>
        public Vector2 Position;
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

        public void Update(GameTime gameTime)
        {
            if (isWillie) 
            {
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 acceleration = new Vector2(0, 0);

                priorState = currentState;
                currentState = Keyboard.GetState();

                if (currentState.IsKeyDown(Keys.Left) ||
                   currentState.IsKeyDown(Keys.A))
                {
                    if (HasSpeedup) velocity.X = -100;  
                    else acceleration += new Vector2(-LINEAR_ACCELERATION, 0);

                }

                if (currentState.IsKeyDown(Keys.Right) ||
                    currentState.IsKeyDown(Keys.D))
                {
                    if (HasSpeedup) velocity.X = 100;
                    else acceleration += new Vector2(LINEAR_ACCELERATION, 0);

                }

                if (currentState.IsKeyDown(Keys.Up) ||
                    currentState.IsKeyDown(Keys.W))
                {
                    if (HasSpeedup) velocity.Y = -100;
                    else acceleration += new Vector2(0, -LINEAR_ACCELERATION);
                }

                if (currentState.IsKeyDown(Keys.Down) ||
                    currentState.IsKeyDown(Keys.S))
                {
                    if (HasSpeedup) velocity.Y = 100;
                    else acceleration += new Vector2(0, LINEAR_ACCELERATION);
                }
                if(!HasSpeedup) velocity += acceleration * t;
                if (!HasSpeedup && velocity.X > 100) velocity.X = 100;
                if (!HasSpeedup && velocity.X < -100) velocity.X = -100;
                if (!HasSpeedup && velocity.Y > 100) velocity.Y = 100;
                if (!HasSpeedup && velocity.Y < -100) velocity.Y = -100;
           
                this.Position += velocity * t;

            }

            // from professor bean's work on PhysicsExampleB -- I was trying to do this in the last project
            // so I deprecated that version and implemented this
            var viewport = game.GraphicsDevice.Viewport;
            if (Position.Y < 35) Position.Y = viewport.Height;
            if (Position.Y > viewport.Height) Position.Y = 35;
            if (Position.X < 0) Position.X = viewport.Width;
            if (Position.X > viewport.Width) Position.X = 0;
            bounds.X = this.Position.X;
            bounds.Y = this.Position.Y;
        }

        public void UpdateNPC(Vector2 Position)
        {
            this.Position += Position;
            if (this.Position.X >= game.GraphicsDevice.Viewport.Width - 64)
            {
                this.Position -= new Vector2(Position.X, 0);
            }
            else if (this.Position.X <= 0)
            {
                this.Position -= new Vector2(Position.X, 0);
            }

            if (this.Position.Y >= game.GraphicsDevice.Viewport.Height - 90)
            {
                this.Position -= new Vector2(0, Position.Y);
            }
            else if (this.Position.Y <= 30)
            {
                this.Position -= new Vector2(0, Position.Y);
            }
            bounds.X = this.Position.X;
            bounds.Y = this.Position.Y;
        }
        public void Reset(Vector2 pos)
        {
            velocity = Vector2.Zero;
            HasSpeedup = false;
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
