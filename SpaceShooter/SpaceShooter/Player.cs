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
    public class Player
    {
        // Skepp, position och hastighets variabler
        public Texture2D Ship, Bullet;
        public Vector2 Shipposition;
        public int Shipspeed;
        public float ammo, outofammo;
        public float scale = 0.3f;
        // avgränsningslåda, dvs en osynlig låda för spriten, så om den spriten kolliderar med ena spriten så kommer det finnas en explosion.
        public Rectangle boundingBox; 
        public bool Collision;
        public float BulletDelay;
        public bool IsActive;
        // Hp(Hit-points) variabler
        public Texture2D Hpbar;
        public Vector2 HpPosition;
        public float Hpscale;
        public int health;
        public Rectangle HpRectangle;
        // Lista Bullet
        public List<Bullet> bulletList;
        public List<Powerups> ammoList;

        private Vector2 HudammoPos = new Vector2(200, 10);
        public SpriteFont Font { get; set; }

        public Player()
        {
            bulletList = new List<Bullet>();
            Shipposition = new Vector2(300, 300);
            BulletDelay = 5;
            Shipspeed = 10;
            Collision = false;
            health = 200;
            HpPosition = new Vector2(250, 929);
            Hpscale = 0.6f;
            IsActive = true;
            outofammo = 0f;
        }

        public void LoadContent(ContentManager Content)
        {
            Ship = Content.Load<Texture2D>("Textures/Ship/ship2");
            Bullet = Content.Load<Texture2D>("Textures/Bullet/Bullet3");
            Hpbar = Content.Load<Texture2D>("Textures/HealthBar/healthbar");
            Font = Content.Load<SpriteFont>("Arial");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;

            Vector2 Origin = new Vector2(Ship.Width / 2, Ship.Height / 2);

            if (IsActive)
            {
                spriteBatch.Draw(Ship, Shipposition, null, Color.White, 0f, Origin, scale, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(Hpbar, HpPosition, HpRectangle, Color.White);

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }

            spriteBatch.DrawString(Font, "Ammo: " + ammo.ToString(), HudammoPos, Color.Red);

           PixelDrawer.DrawPixels(boundingBox, Color.Green * 0.5f);
             
        }

        public void Update(GameTime gameTime)
        {
            // Tangentbords statement
            KeyboardState ks = Keyboard.GetState();

            if (!IsActive)
                return;

            //Skepp Kontroller
            if (ks.IsKeyDown(Keys.W))
                Shipposition.Y = Shipposition.Y - Shipspeed;

            if (ks.IsKeyDown(Keys.S))
                Shipposition.Y = Shipposition.Y + Shipspeed;

            if (ks.IsKeyDown(Keys.A))
                Shipposition.X = Shipposition.X - Shipspeed;

            if (ks.IsKeyDown(Keys.D))
                Shipposition.X = Shipposition.X + Shipspeed;

            // Skepp inom spelskärmen
            if (Shipposition.X < (Ship.Width / 2) * scale)
                Shipposition.X = (Ship.Width / 2) * scale;

            if (Shipposition.X > 800 - (Ship.Width / 2) * scale)
                Shipposition.X = 800 - (Ship.Width / 2) * scale;

            if (Shipposition.Y < (Ship.Height / 2) * scale)
                Shipposition.Y = (Ship.Height / 2) * scale;

            if (Shipposition.Y > 950 - (Ship.Height / 2) * scale)
                Shipposition.Y = 950 - (Ship.Height / 2) * scale;

            // Skjuter bara om bulletdelay startar om
            if (BulletDelay >= 0)
                BulletDelay--;
            // Om bulletdelay är på 0, skapa ny bullet och gör den synlig, sen lägger den till i listan.

            // Skjuter skott med space
            if (ks.IsKeyDown(Keys.Space) && ammo >= 1f && BulletDelay <= 0)
            {
                Shoot();
                Music.StartLaserEffect();                             
            }
            
            UpdateBullets();
            //Console.WriteLine(ammo);
            // startar om bulletdelay
            if (BulletDelay <= 0)
                BulletDelay = 5;

            // Boundingbox/avgränsningslåda för Ship
            boundingBox = new Rectangle((int)Shipposition.X - (int)(Ship.Width * scale) / 2, (int)Shipposition.Y - (int)(Ship.Height * scale) / 2, (int)(Ship.Width * scale), (int)(Ship.Height * scale));

            HpRectangle = new Rectangle((int)HpPosition.X, (int)HpPosition.Y, health, 20);   


        }

        //Shoot metod
        public void Shoot()
        {         
            Bullet newBullet = new Bullet(Bullet);
            newBullet.Bulletposition = new Vector2(Shipposition.X + 1 - newBullet.bullet.Width / 2, Shipposition.Y + 1); // Skjuter skott från skeppets position

            //Gör skottet/kulan synlig
            newBullet.BulletSynlig = true;

            if (bulletList.Count() < 100)
                bulletList.Add(newBullet);
            ammo -= 1f;
          //  hud.amountofammo -= 1;
            
        }

        // Uppdaterar skotten
        public void UpdateBullets()
        {
            // För varje skott, uppdatera rörelsen om skottet träffar toppen av skärmen, och då tar den bort den.
            foreach (Bullet b in bulletList)
            {
                // Avgränsingslåda för varje bullet i vår bulletlista
                b.BulletboundingBox = new Rectangle((int)b.Bulletposition.X, (int)b.Bulletposition.Y, b.bullet.Width, b.bullet.Height);

                // Bullet rörelse
                b.Bulletposition.Y = b.Bulletposition.Y - b.Bulletspeed;

                // Om en Bullet kommer högst upp på skärmen gör Bulletsynlig falsk, alltså att Bullet försvinner
                if (b.Bulletposition.Y <= 0)
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

        public void ShipDestroyed()
        {
                IsActive = false;
        }
    }
}
