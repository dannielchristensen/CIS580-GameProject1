using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FootballBlast.Collisions;

namespace FootballBlast
{
    public class NoteSprite
    {
        private FootballGame game;
        private Texture2D texture;
        private bool decrementAnimation = false;
        private double animationTimer;
        private short animationFrame = 1;
        private BoundingCircle bounds;
        private Random r;

        /// <summary>
        /// The bounding volume of the Footballer
        /// </summary>
        public BoundingCircle Bounds => bounds;
        /// <summary>
        /// area around sprite that increases player velocity
        /// </summary>
        public bool IsCollected = false;
        public Vector2 Position { get; private set; }
        public NoteSprite(FootballGame game)
        {
            this.game = game;
            this.Spawn();
            bounds = new BoundingCircle(this.Position, 32);
            r = new Random();

        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ksu_note");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            var source = new Rectangle();

            // update animation frame
            if (animationTimer > .5)
            {
                animationTimer -= .5;
                if (decrementAnimation)
                    animationFrame--;
                else
                    animationFrame++;
                if (animationFrame > 4)
                {
                    decrementAnimation = true;
                    animationFrame = 3;
                }else if(animationFrame < 1)
                {
                    animationFrame = 2;
                    decrementAnimation = false;
                }
            }
            switch (animationFrame)
            {
                case 1:
                    source = new Rectangle(0, 0, 32, 32);
                    break;
                case 2:
                    source = new Rectangle(32, 0, 32, 32);
                    break;
                case 3:
                    source = new Rectangle(0, 32, 32, 32);
                    break;
                case 4:
                    source = new Rectangle(32, 32, 32, 32);
                    break;
                default:
                    source = new Rectangle(0, 0, 32, 32);
                    break;
            }
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);


        }
        public void Spawn()
        {
            var viewport = game.GraphicsDevice.Viewport;

            // https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
            // for the * (float) (max-min) + min to keep the ball on the screen
            Position = new Vector2(
                viewport.Width/2,
                100
                );
            bounds.Center = Position;
            decrementAnimation = false;
            animationFrame = 1;
        }
    }
    
}
