using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FootballBlast
{
    public class FootballGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private InputManager inputManager;
        private Footballer KU;
        private Footballer KState;
        private Football ball;
        private SpriteFont bangers;
        private double winTimer;
        private bool win = false;
        private bool lose = false;
        private int totalWins = 0;
        private int totalPunts = 0;


        public FootballGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            KU = new Footballer(this, false, new Vector2(100, 200));
            KState = new Footballer(this, true, new Vector2(600, 200));
            ball = new Football(this);
            inputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            KU.LoadContent();
            KState.LoadContent();
            ball.LoadContent();
            bangers = Content.Load<SpriteFont>("bangers");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            inputManager.Update(gameTime);
            if (inputManager.Exit)
            {
                Exit();
            }
            if (!inputManager.Start)
            {
                return;
            }else if (win){
                win = false;
                totalPunts = 0;
                KState.HasBall = false;
                ball.IsCollected = false;
                KU.Reset(new Vector2(100, 200));
                KState.Reset(new Vector2(600, 200));
                ball.Punt();
            }else if (lose)
            {
                lose = false;
                KState.HasBall = false;
                ball.IsCollected = false;
                totalPunts = 0;
                totalWins = 0;
                KU.Reset(new Vector2(100, 200));
                KState.Reset(new Vector2(600, 200));
                ball.Punt();
            }
            
            KState.Update(inputManager.Direction);
            // TODO: Add your update logic here
            inputManager.NPC_Update_Ball(gameTime, ball.Position, KU.Position);
            KU.Update(inputManager.NPC_Direction);
            if (!ball.IsCollected && ball.Bounds.CollidesWith(KState.Bounds))
            {
                ball.IsCollected = true;
                KState.HasBall = true;
                winTimer = 0;
                ball.Update(new Vector2(KState.Position.X, KState.Position.Y-20));
            } else if (ball.IsCollected && KState.HasBall)
            {
                ball.Update(new Vector2(KState.Position.X, KState.Position.Y + 20));
            }

            if (!ball.IsCollected && ball.Bounds.CollidesWith(KU.Bounds))
            {
                ball.Punt();
                totalPunts++;


            }


            if (ball.IsCollected && (KState.Bounds.CollidesWith(KU.Bounds) || KU.Bounds.CollidesWith(KState.Bounds)))
            {
                ball.IsCollected = false;
                KState.HasBall = false;
                ball.Punt();
                totalPunts++;

            }
            if (KState.HasBall)
            {
                winTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(winTimer > 30)
                {
                    win = true;
                    totalWins++;
                    inputManager.EndGame();
                }
            }

            if(totalPunts >= 10)
            {
                lose = true;
                inputManager.EndGame();

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black) ;

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            if (KState.HasBall && !win && !lose)
            {
                
                spriteBatch.DrawString(bangers, $"{winTimer:##.##}", new Vector2(this.GraphicsDevice.Viewport.Width / 4, 10), Color.Purple);

            }

            else if (KState.HasBall && win)
            {
                spriteBatch.DrawString(bangers, $"YOU WIN.", new Vector2(10, 10), Color.Purple);
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO PLAY AGAIN.", new Vector2(10, 60), Color.Purple);
                spriteBatch.DrawString(bangers, $"PRESS ESC TO QUIT.", new Vector2(10, 110), Color.Purple);


            }else if (lose)
            {
                spriteBatch.DrawString(bangers, $"YOU LOSE.", new Vector2(10, 10), Color.Red);
                spriteBatch.DrawString(bangers, $"KU FINALLY RAN A TOUCHDOWN.", new Vector2(10, 60), Color.Blue);
                
                spriteBatch.DrawString(bangers, $"PRESS ENTER TO RESET YOUR RUN.", new Vector2(10, 110), Color.Purple);
                spriteBatch.DrawString(bangers, $"PRESS ESC TO QUIT.", new Vector2(10, 160), Color.Purple);
            }
            if (!lose && !win)
            {
                KU.Draw(gameTime, spriteBatch);
                KState.Draw(gameTime, spriteBatch);
                ball.Draw(spriteBatch);
            }
            spriteBatch.DrawString(bangers, $"Total Wins: {totalWins}", new Vector2(2* this.GraphicsDevice.Viewport.Width / 4, 10), Color.Purple);
            spriteBatch.DrawString(bangers, $"Total Punts: {totalPunts}", new Vector2(3 * this.GraphicsDevice.Viewport.Width / 4, 10), Color.Blue);

            //spriteBatch.Draw(ball.texture, ball.Position, null, Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
