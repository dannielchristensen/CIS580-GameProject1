using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FootballBlast
{
    public class FootballGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SoundEffect tackle;
        private SoundEffect playBand;
        private SoundEffect pickupBall;
        private SpriteBatch spriteBatch;
        private InputManager inputManager;
        private Footballer KU;
        private Footballer KState;
        private Football ball;
        private SpriteFont bangers;
        private NoteSprite noteSprite;
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
            noteSprite = new NoteSprite(this);
            tackle = Content.Load<SoundEffect>("Randomize11");
            playBand = Content.Load<SoundEffect>("Powerup24");
            pickupBall = Content.Load<SoundEffect>("Pickup_Coin62");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            KU.LoadContent();
            KState.LoadContent();
            ball.LoadContent();
            noteSprite.LoadContent(Content);
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
                ball.Reset();
                KState.HasSpeedup = false;
                noteSprite.IsCollected = false;
                noteSprite.Spawn();
            }else if (lose)
            {
                lose = false;
                KState.HasBall = false;
                ball.IsCollected = false;
                totalPunts = 0;
                totalWins = 0;
                KU.Reset(new Vector2(100, 200));
                KState.Reset(new Vector2(600, 200));
                ball.Reset();
                KState.HasSpeedup = false;
                noteSprite.IsCollected = false;
                noteSprite.Spawn();

            }

            KState.Update(gameTime);
            // TODO: Add your update logic here
            inputManager.NPC_Update_Ball(gameTime, ball.Position, KU.Position);
            KU.UpdateNPC(inputManager.NPC_Direction);
            if (!ball.IsCollected && ball.Bounds.CollidesWith(KState.Bounds))
            {
                pickupBall.Play();
                ball.IsCollected = true;
                KState.HasBall = true;
                winTimer = 0;
                ball.Update(new Vector2(KState.Position.X, KState.Position.Y-20));
            } else if (ball.IsCollected && KState.HasBall)
            {
                ball.Update(new Vector2(KState.Position.X, KState.Position.Y + 20));
            }
            if(!noteSprite.IsCollected && KState.Bounds.CollidesWith(noteSprite.Bounds))
            {
                noteSprite.IsCollected = true;
                KState.HasSpeedup = true;
                playBand.Play();
                totalPunts--;
            }
            if (!noteSprite.IsCollected && KU.Bounds.CollidesWith(noteSprite.Bounds))
            {
                noteSprite.IsCollected = true;
                KState.HasSpeedup = false;

            }
            if (!ball.IsCollected && ball.Bounds.CollidesWith(KU.Bounds))
            {
                ball.Punt();
                totalPunts++;


            }


            if (ball.IsCollected && (KState.Bounds.CollidesWith(KU.Bounds) || KU.Bounds.CollidesWith(KState.Bounds)))
            {
                tackle.Play();
                ball.IsCollected = false;
                KState.HasBall = false;
                ball.Punt();
                totalPunts++;
                

            }
            if(KState.Bounds.CollidesWith(KU.Bounds) || KU.Bounds.CollidesWith(KState.Bounds))
            {
                tackle.Play();
                noteSprite.IsCollected = false;
                noteSprite.Spawn();
                KState.HasSpeedup = false;
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
                if(!noteSprite.IsCollected) noteSprite.Draw(gameTime, spriteBatch);
            }
            spriteBatch.DrawString(bangers, $"Total Wins: {totalWins}", new Vector2(2* this.GraphicsDevice.Viewport.Width / 4, 10), Color.Purple);
            spriteBatch.DrawString(bangers, $"Total Punts: {totalPunts}", new Vector2(3 * this.GraphicsDevice.Viewport.Width / 4, 10), Color.Blue);

            //spriteBatch.Draw(ball.texture, ball.Position, null, Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
