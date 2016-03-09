using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    public class HudScore
    {
        private Vector2 scorePos = new Vector2(20, 10);
        public SpriteFont Font { get; set; }
        public int Score { get; set; }
   
        public HudScore()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, "Score: " + Score.ToString(), scorePos, Color.Red);                     
        }
    }
}
