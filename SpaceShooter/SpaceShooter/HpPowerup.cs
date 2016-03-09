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
    public class HpPowerup
    {
        public Texture2D hp;
        public Vector2 hpPos, HpOrigin;
        public bool hpSynlig;
        public int hpspeed;
        public float hpScale;
        public Rectangle hpBoundingBox;
        Random random = new Random();

        public HpPowerup(Texture2D newHpTexture, Vector2 newHpPos)
        {
            hp = newHpTexture;
            hpPos = newHpPos;
            hpSynlig = true;
            hpspeed = 3;
            hpScale = 0.7f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hpSynlig)
            {
                spriteBatch.Draw(hp, hpPos, null, Color.White, 0f, HpOrigin,hpScale, SpriteEffects.None, 1f);
                PixelDrawer.DrawPixels(hpBoundingBox, Color.Yellow * 0.5f);
            }
        }

        public void Update(GameTime gameTime)
        {
            hpBoundingBox = new Rectangle((int)hpPos.X - (int)(hp.Width * hpScale) / 2, (int)hpPos.Y - (int)(hp.Height * hpScale) / 2, (int)(hp.Width * hpScale), (int)(hp.Height * hpScale));

            HpOrigin.X = hp.Width / 2;
            HpOrigin.Y = hp.Height / 2;

            hpPos.Y = hpPos.Y + hpspeed;
            if (hpPos.Y > 950)
            {
                hpPos = new Vector2(random.Next(0, 800), -50);
            }
        }

    }
}
