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
    public class Powerups
    {
        public int ammospeed;
        public Texture2D ammo;
        public Vector2 ammoPos;
        public Vector2 Origin;
        public Rectangle ammoBoundingBox;
        public bool ammoSynlig;
        public float ammoScale;
        Random random = new Random();

        public Powerups(Texture2D newTexture, Vector2 newPos)
        {
            ammo = newTexture;
            ammoPos = newPos;
            ammospeed = 3;
            ammoSynlig = true;        
            ammoScale = .7f;           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ammoSynlig)
            {
                spriteBatch.Draw(ammo, ammoPos, null, Color.White, 0f, Origin, ammoScale, SpriteEffects.None, 1f);
                PixelDrawer.DrawPixels(ammoBoundingBox, Color.Orange * 0.5f);
            }
        }

        public void Update(GameTime gameTime)
        {
            ammoBoundingBox = new Rectangle((int)ammoPos.X - (int)(ammo.Width * ammoScale) / 2, (int)ammoPos.Y - (int)(ammo.Height * ammoScale) / 2,(int)(ammo.Width * ammoScale), (int)(ammo.Height * ammoScale));
            Origin.X = ammo.Width / 2;
            Origin.Y = ammo.Height / 2;

            ammoPos.Y = ammoPos.Y + ammospeed;
            if (ammoPos.Y > 950)
            {
                ammoPos = new Vector2(random.Next(0,800),-50);
            }
        }
    }
}
