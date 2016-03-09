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
    public class Bullet
    {
        public Texture2D bullet;
        public Vector2 Bulletposition;
        public Vector2 Origin;
        public Rectangle BulletboundingBox;
        public bool BulletSynlig;
        public float Bulletspeed;

        public Bullet(Texture2D newTexture)
        {
            Bulletspeed = 7;
            bullet = newTexture;          
            BulletSynlig = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bullet, Bulletposition, Color.White);
            PixelDrawer.DrawPixels(BulletboundingBox, Color.Red * 0.5f);
        }

    }
}
