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
    public class Asteroid
    {
        public Texture2D Rock;
        public Vector2 Rockposition;
        public Vector2 Origin;
        public float rotation;
        public int Rockspeed;
        public Rectangle Boundingbox;
        public bool AsteroidSynlig;
        Random random = new Random();
        //Konstruktor
        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            Rockposition = newPosition;
            Rock = newTexture;
            Rockspeed = 3;
            AsteroidSynlig = true;
        }

        public void Update(GameTime gameTime)
        {
            // Sätter avgränsning för kollision
            Boundingbox = new Rectangle((int)Rockposition.X - Rock.Width / 2, (int)Rockposition.Y - Rock.Height / 2, 45, 45);

            //Uppdaterar Origin för rotation
            Origin.X = Rock.Width / 2;
            Origin.Y = Rock.Height / 2;

            // Uppdaterar rörelser
            Rockposition.Y = Rockposition.Y + Rockspeed;

            if (Rockposition.Y > 950)
            {
                Rockposition = new Vector2(random.Next(0, 800), -50);
            }
            
            // Roterar Stenen eller Asteroiden
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotation += elapsed;
            float circle = MathHelper.Pi * 2;
            rotation = rotation % circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (AsteroidSynlig)
            {
                spriteBatch.Draw(Rock, Rockposition, null, Color.White, rotation, Origin, 1f, SpriteEffects.None, 0f);
                PixelDrawer.DrawPixels(Boundingbox, Color.BurlyWood * 0.5f);
            } 
            
        }


    }
}
