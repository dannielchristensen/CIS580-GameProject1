using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FootballBlast
{
   


    public class InputManager
    {

        /// <summary>
        /// denotes if the game has started
        /// </summary>
        public bool Start { get; private set; } = false;
        KeyboardState currentState;
        KeyboardState priorState;
        /// <summary>
        /// The current direction of willie
        /// </summary>


        public Vector2 NPC_Direction { get; private set; }
        /// <summary>
        /// float to determine the acceleration of the player.
        /// = 1 is normal speed; < 1 means slower; > 1 means faster;
        /// </summary>
        public float Acceleration { get; private set; } = 1;

        public bool Exit{get; private set;} = false;
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);

            priorState = currentState;
            currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.Escape))
            {
                Exit = true;
                return;
            }
            if ((currentState.IsKeyDown(Keys.Enter) && priorState.IsKeyUp(Keys.Enter)) ||
                (currentState.IsKeyDown(Keys.P) && priorState.IsKeyUp(Keys.P)))
            {
                Start = !Start;
                return;
            } 
        }

        public void NPC_Update_Ball(GameTime gameTime, Vector2 ballPosition, Vector2 NPCPosition)
        {
            NPC_Direction = new Vector2(0, 0);


            if (ballPosition.X < NPCPosition.X)
            {
                NPC_Direction += new Vector2(-65 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            } else if (ballPosition.X > NPCPosition.X)
            {
                NPC_Direction += new Vector2(65 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }

            if (ballPosition.Y < NPCPosition.Y)
            {
                NPC_Direction += new Vector2(0, -65 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (ballPosition.Y > NPCPosition.Y)
            {
                NPC_Direction += new Vector2(0, 65 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
          

        }

        public void EndGame()
        {
            Start = false;
        }
    }
}
