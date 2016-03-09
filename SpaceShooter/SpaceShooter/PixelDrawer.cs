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
    public static class PixelDrawer
    {
        private static Texture2D pixel;
        private static SpriteBatch spritebatch;
        private static bool enabled = false;

        public static void Setup(ContentManager Content, SpriteBatch spriteBatch)
        {
            pixel = Content.Load<Texture2D>("pixel");
            spritebatch = spriteBatch;
        }

        public static void DrawPixels(Rectangle rect, Color color)
        {
            if(enabled)
                spritebatch.Draw(pixel, rect, color);
        }
    }
}
