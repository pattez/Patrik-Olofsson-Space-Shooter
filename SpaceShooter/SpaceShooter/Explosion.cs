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
    public class Explosion
    {
        public Texture2D Explosiontex;
        public Vector2 Explosionposition;
        public float ExplosionDelay;
        public bool ExplosionSynlig;
        public float ExplosionScale = 0.6f;
        public Rectangle ExplosionBoundingbox;
        public float TransparentExplosion;


        public Explosion()
        {
           // Explosiontex = newExplosiontex;
            ExplosionDelay = 10;
            ExplosionSynlig = false;
            TransparentExplosion = 1f;
        }

        public void LoadContent(ContentManager Content)
        {
            Explosiontex = Content.Load<Texture2D>("Textures/Explosion/explosion0");
        }

        public void Update(GameTime gameTime)
        {
            ExplosionBoundingbox = new Rectangle((int)Explosionposition.X, (int)Explosionposition.Y, 15, 15);
            TransparentExplosion -= 0.03f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(ExplosionSynlig)
                spriteBatch.Draw(Explosiontex, Explosionposition,null,Color.White * TransparentExplosion,1f,Vector2.Zero,ExplosionScale, SpriteEffects.None,1f);
        }

    }
}
