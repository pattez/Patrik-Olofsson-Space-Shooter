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
    public class Enemy
    {
        public Texture2D EnemyTex, Bullet;
        public Vector2 EnemyPos;
        public Vector2 Origin;
        public float EnemyShipScale;
        public float RandX, RandY;
        public bool EnemySynlig;        
        public int EnemySpeed;
        public float BulletDelay;
        public Rectangle EnemyBoundingBox;
        public List<Bullet> bulletList;
        Random random = new Random();

        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            EnemyTex = newTexture;
            EnemyPos = newPosition;
            Bullet = newBulletTexture;
            EnemyShipScale = 0.4f;
            EnemySynlig = true;         
            EnemySpeed = 2;
            BulletDelay = 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (EnemySynlig)
            {
                spriteBatch.Draw(EnemyTex, EnemyPos, null, Color.White, 3.14f, Origin, EnemyShipScale, SpriteEffects.None, 0f);
                PixelDrawer.DrawPixels(EnemyBoundingBox, Color.BlanchedAlmond * 0.5f);
            }

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            EnemyBoundingBox = new Rectangle(
                (int)EnemyPos.X - (int)(EnemyTex.Width * EnemyShipScale) / 2,
                (int)EnemyPos.Y - (int)(EnemyTex.Height * EnemyShipScale) / 2,
                (int)(EnemyTex.Width * EnemyShipScale),
                (int)(EnemyTex.Height * EnemyShipScale));

            Origin.X = EnemyTex.Width / 2;
            Origin.Y = EnemyTex.Height / 2;

            EnemyPos.Y = EnemyPos.Y + EnemySpeed;
            if (EnemyPos.Y > 900)
            {
                EnemyPos = new Vector2(random.Next(0, 800), -50);
            }
                

            EnemyShoot();
            EnemyUpdateBullets();
        }

        public void EnemyShoot()
        {
            if (BulletDelay >= 0)
                BulletDelay--;

            if (BulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(Bullet);
                newBullet.Bulletposition = new Vector2(EnemyPos.X - 11, EnemyPos.Y + 6);

                newBullet.BulletSynlig = true;

                if (bulletList.Count() < 2)
                    bulletList.Add(newBullet);

                if (BulletDelay == 0)
                    BulletDelay = 40;

            }
        }

        public void EnemyUpdateBullets()
        {
            // För varje skott, uppdatera rörelsen om skottet träffar toppen av skärmen, och då tar den bort den.
            foreach (Bullet b in bulletList)
            {
                // Avgränsingslåda för varje bullet i vår bulletlista
                b.BulletboundingBox = new Rectangle((int)b.Bulletposition.X, (int)b.Bulletposition.Y, b.bullet.Width, b.bullet.Height);

                // Bullet rörelse
                b.Bulletposition.Y = b.Bulletposition.Y + b.Bulletspeed;

                // Om en Bullet kommer längst ner på skärmen gör Bulletsynlig falsk, alltså att Bullet försvinner
                if (b.Bulletposition.Y >= 950)
                    b.BulletSynlig = false;
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].BulletSynlig)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }

        }
    }
}