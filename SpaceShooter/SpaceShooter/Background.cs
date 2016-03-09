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
    public class Background
    {
       public Texture2D background;
       // Två styck bakgrunder, för att ena ska starta över och den andra under
       public Vector2 backgroundposition, backgroundposition2; 
       public int Bgspeed;

       public Background()
       {
           backgroundposition = new Vector2(0, 0);
           backgroundposition2 = new Vector2(0, -950);
           Bgspeed = 2;
       }

       public void LoadContent(ContentManager Content)
       {
           background = Content.Load<Texture2D>("Textures/Background/space");
       }

       public void Draw(SpriteBatch spriteBatch)
       {
           spriteBatch.Draw(background, backgroundposition, Color.White);
           spriteBatch.Draw(background, backgroundposition2, Color.White);
       }

       public void Update(GameTime gameTime)
       {
           //Bakgrundsbildens scroll
           backgroundposition.Y = backgroundposition.Y + Bgspeed;
           backgroundposition2.Y = backgroundposition2.Y + Bgspeed;
          // Console.WriteLine(backgroundposition);
           //Console.WriteLine(backgroundposition2);
           // Bakgrundsbildens repeat
           if (backgroundposition.Y > 950)
           {
               backgroundposition.Y = 0;
               backgroundposition2.Y = -950;
           }
       }


    }
}
