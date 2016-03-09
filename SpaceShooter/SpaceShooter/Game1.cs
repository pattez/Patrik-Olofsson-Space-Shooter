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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
      
        public Texture2D MenuImage, GameOverImage, ResumeGameImage, HeaderImage, GameInfoImage;
        public Rectangle playBoundingBox, gameoBoundingBox, qBoundingBox,
            ReBoundingBox, ReMenuBoundingBox;
        HudScore hud;

        List<Asteroid> AsteroidList = new List<Asteroid>();
        List<Enemy> EnemyList = new List<Enemy>();
        List<Powerups> ammoList = new List<Powerups>();
        List<HpPowerup> hpList = new List<HpPowerup>();

        // Spelare och Backgrund object
        Player p = new Player();
        Background b = new Background();
        Explosion e = new Explosion();


        public enum State
        {
            Playing,
            Menu,
            Gameover,
            Pause,
            GameInfo
        }

        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = " SpaceShooter ";
            Content.RootDirectory = "Content";
            playBoundingBox = new Rectangle(275, 185,250, 40);
            gameoBoundingBox = new Rectangle(263, 360, 275, 80);
            qBoundingBox = new Rectangle(275, 575, 250, 40);
            ReBoundingBox = new Rectangle(210, 185, 380, 40);
            ReMenuBoundingBox = new Rectangle(260, 790, 250, 45);         
            MenuImage = null;

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            b.LoadContent(Content);
            p.LoadContent(Content);
            e.LoadContent(Content);
            MenuImage = Content.Load<Texture2D>("Textures/GameState/Menu/menu");
            GameOverImage = Content.Load<Texture2D>("Textures/GameState/GameOver/go");
            ResumeGameImage = Content.Load<Texture2D>("Textures/GameState/Menu/resume");
            HeaderImage = Content.Load<Texture2D>("Textures/GameState/Menu/Header");
            GameInfoImage = Content.Load<Texture2D>("Textures/GameState/Menu/info");
            hud = new HudScore();
            hud.Font = Content.Load<SpriteFont>("Arial");
            Music.Setup(Content);
            PixelDrawer.Setup(Content, spriteBatch);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            switch (gameState)
            {
                case State.Playing:
                    {
                        if (ks.IsKeyDown(Keys.Escape))
                        {
                            gameState = State.Pause;
                            Music.PauseMusic();
                        }
                        // Varje Asteroid i våran Asteroidlista Uppdaterar och kolla efter kollision
                        foreach (Asteroid a in AsteroidList)
                        {
                            // Kolla om någon av Asteroid kolliderar med vårat Ship
                            if (a.Boundingbox.Intersects(p.boundingBox))
                            {
                                p.health -= 11;
                                Console.WriteLine(p.health);
                                a.AsteroidSynlig = false;
                                e.ExplosionSynlig = true;
                                e.Explosionposition = a.Rockposition;
                                e.TransparentExplosion = 1f;
                                Music.StartSoundEffect();
                            }

                            // Om skeppet når 0 hp så förstörs den
                            if (p.health < 0)
                            {
                                p.ShipDestroyed();
                                e.ExplosionSynlig = true;
                                e.Explosionposition = p.Shipposition;
                                e.TransparentExplosion = 1f;
                                gameState = State.Gameover;
                            }
                            // Om någon Asteroid kommer i kontakt med en bullet från våran Asteroidlista
                            // så uppdaterar den och kollar efter kollision, om det finns kollision = false;
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.Boundingbox.Intersects(p.bulletList[i].BulletboundingBox))
                                {
                                    a.AsteroidSynlig = false;
                                    // ElementAt, Kolla upp, Osäker
                                    p.bulletList.ElementAt(i).BulletSynlig = false;
                                    hud.Score += 3;
                                    Music.StartSoundEffect();
                                }
                                // Om någon asteroid kommer i kontakt med ett skott så skapas en explosion på den Asteroiden som sprängs
                                if (a.Boundingbox.Intersects(p.bulletList[i].BulletboundingBox))
                                {
                                    e.ExplosionSynlig = true;
                                    e.Explosionposition = a.Rockposition;
                                    e.TransparentExplosion = 1f;
                                    Music.StartSoundEffect();
                                }
                            }

                            a.Update(gameTime);
                        }

                        foreach (Enemy en in EnemyList)
                        {
                            // Om Fiende kolliderar med Player, gör följande.
                            if (en.EnemyBoundingBox.Intersects(p.boundingBox)) 
                            {
                                p.health -= 11;
                                en.EnemySynlig = false;
                                e.ExplosionSynlig = true;
                                e.Explosionposition = en.EnemyPos;
                                e.TransparentExplosion = 1f;
                                Music.StartSoundEffect();
                            }

                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (en.EnemyBoundingBox.Intersects(p.bulletList[i].BulletboundingBox))
                                {
                                    en.EnemySynlig = false;
                                    p.bulletList.ElementAt(i).BulletSynlig = false;
                                    e.ExplosionSynlig = true;
                                    e.Explosionposition = en.EnemyPos;
                                    e.TransparentExplosion = 1f;
                                    hud.Score += 10;
                                    Music.StartSoundEffect();
                                }
                            }
                            for (int i = 0; i < en.bulletList.Count; i++)
                            {
                                if (p.boundingBox.Intersects(en.bulletList[i].BulletboundingBox))
                                {
                                    p.health -= 11;
                                    en.bulletList.ElementAt(i).BulletSynlig = false;
                                    e.ExplosionSynlig = true;
                                    e.Explosionposition = p.Shipposition;
                                    e.TransparentExplosion = 1f;
                                    Music.StartSoundEffect();
                                }                             
                            }

                            en.Update(gameTime);
                        }

                        foreach (Powerups am in ammoList)
                        {                         
                                if (p.boundingBox.Intersects(am.ammoBoundingBox))
                                {
                                    Music.StartAmmoGet();
                                    am.ammoSynlig = false;
                                    p.ammo += 2f;
                                }
                            am.Update(gameTime);
                        }

                        foreach (HpPowerup hp in hpList)
                        {
                                if(p.boundingBox.Intersects(hp.hpBoundingBox))
                                {                                
                                    hp.hpSynlig = false;
                                    p.health += 6;
                                    Console.WriteLine(p.health);
                                    Music.StartLifePickUp();
                                }                                                          
                            hp.Update(gameTime);
                        }

                        if (ks.IsKeyDown(Keys.F1))
                        {
                            Music.PauseMusic();
                        }
    
                        b.Update(gameTime);
                        p.Update(gameTime);
                        e.Update(gameTime);                      
                        LoadAsteroid();
                        LoadEnemy();
                        LoadAmmo();
                        LoadHp();
                        
                        break;
                    }

                case State.Menu:
                    {
                        IsMouseVisible = true;
                       
                        if(ms.LeftButton == ButtonState.Pressed && playBoundingBox.Contains(ms.X,ms.Y))
                        {
                            Music.StartClickEffect();  
                            gameState = State.Playing;
                            Music.StartingPlayingMusic();
                        }

                        if (ms.LeftButton == ButtonState.Pressed && gameoBoundingBox.Contains(ms.X, ms.Y))
                        {
                            gameState = State.GameInfo;
                            Music.StartClickEffect();
                        }

                        if (ms.LeftButton == ButtonState.Pressed && qBoundingBox.Contains(ms.X, ms.Y))
                        {
                            Music.StartClickEffect();
                            this.Exit();
                        }

                        b.Update(gameTime);
                        b.Bgspeed = 1;
                        break;
                    }

                case State.Gameover:
                    {
                        if (p.health < 0)
                        {
                            gameState = State.Gameover;
                            Music.EndMusic();
                        }
                        b.Update(gameTime);
                        b.Bgspeed = 1;

                        if (ks.IsKeyDown(Keys.Escape))
                        {
                            this.Exit();
                        }
                        b.Update(gameTime);
                        b.Bgspeed = 1;
                        break;
                    }

                case State.Pause:
                    {
                        if (ms.LeftButton == ButtonState.Pressed && ReBoundingBox.Contains(ms.X, ms.Y))
                        {
                            Music.StartClickEffect();
                            gameState = State.Playing;
                            Music.ResumeMusic();
                        }

                        if (ms.LeftButton == ButtonState.Pressed && gameoBoundingBox.Contains(ms.X, ms.Y))
                        {
                            gameState = State.GameInfo;
                            Music.StartClickEffect();
                        }

                        if (ms.LeftButton == ButtonState.Pressed && qBoundingBox.Contains(ms.X, ms.Y))
                        {
                            this.Exit();
                        }

                        b.Update(gameTime);
                        b.Bgspeed = 1;
                        break;                      
                    }
                case State.GameInfo:
                    {
                        if (ms.LeftButton == ButtonState.Pressed && ReMenuBoundingBox.Contains(ms.X, ms.Y))
                        {
                            gameState = State.Pause;
                        }
                        b.Update(gameTime);
                        b.Bgspeed = 1;
                        break;
                    }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (gameState)
            {
                case State.Playing:
                    {
                        b.Draw(spriteBatch);
                        p.Draw(spriteBatch);
                        e.Draw(spriteBatch);
                        hud.Draw(spriteBatch);
                        
                        foreach (Asteroid a in AsteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy en in EnemyList)
                        {
                            en.Draw(spriteBatch);
                        }

                        foreach (Powerups am in ammoList)
                        {
                            am.Draw(spriteBatch);
                        }

                        foreach (HpPowerup hp in hpList)
                        {
                            hp.Draw(spriteBatch);
                        }
                        break;
                    }

                case State.Menu:
                    {
                        b.Draw(spriteBatch);
                        spriteBatch.Draw(MenuImage, new Vector2(0, 0), Color.White);
                        spriteBatch.Draw(HeaderImage, new Vector2(20, 40), Color.White);
                        PixelDrawer.DrawPixels(playBoundingBox, Color.Orange * 0.5f);
                        PixelDrawer.DrawPixels(gameoBoundingBox, Color.Yellow * 0.5f);
                        PixelDrawer.DrawPixels(qBoundingBox, Color.Blue * 0.5f);
                        break;
                    }

                case State.Gameover:
                    {
                        b.Draw(spriteBatch);
                        spriteBatch.Draw(GameOverImage, new Vector2(0, 0), Color.White);
                        break;
                    }
                case State.Pause:
                    {
                        b.Draw(spriteBatch);
                        spriteBatch.Draw(ResumeGameImage, new Vector2(0, 0), Color.White);
                        spriteBatch.Draw(HeaderImage, new Vector2(20, 40), Color.White);
                        PixelDrawer.DrawPixels(ReBoundingBox, Color.Blue * 0.5f);
                        break;
                    }
                case State.GameInfo:
                    {
                        b.Draw(spriteBatch);
                        spriteBatch.Draw(GameInfoImage, new Vector2(0, 0), Color.White);
                        PixelDrawer.DrawPixels(ReMenuBoundingBox, Color.Blue * 0.5f);
                        break;
                    }
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Load Asteroids
        public void LoadAsteroid()
        {
            // Random variabler för x och y axeln för Asteroid
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);

            // Antal Asteroid, gör 5 tills det är 5 på skärmen
            if (AsteroidList.Count() < 4)
            {
                AsteroidList.Add(new Asteroid(Content.Load<Texture2D>("Textures/Asteroid/asteroid"), new Vector2(randX, randY)));
            }

            // Lista, Tar bort en sten från listan om den är förstörd eller osynlig.
            for (int i = 0; i < AsteroidList.Count; i++)
            {
                if (!AsteroidList[i].AsteroidSynlig)
                {
                    AsteroidList.RemoveAt(i);
                    i--;
                }
            }
        }
        public void LoadEnemy()
        {
            int randY2 = random.Next(-600, -50);
            int randX2 = random.Next(50, 750);

            if (EnemyList.Count() < 3)
            {
                EnemyList.Add(new Enemy(Content.Load<Texture2D>("Textures/Ship/EnemyShip"), new Vector2(randX2, randY2), Content.Load<Texture2D>("Textures/Bullet/EnemyBullet")));
            }

            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (!EnemyList[i].EnemySynlig)
                {
                    EnemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void LoadAmmo()
        {
            // Random variabler för x och y axeln för Asteroid
            int randY3 = random.Next(-600, -50);
            int randX3 = random.Next(0, 750);

            // Antal Asteroid, gör 5 tills det är 5 på skärmen
            if (ammoList.Count() < 2)
            {
                ammoList.Add(new Powerups(Content.Load<Texture2D>("Textures/Powerups/ammo"), new Vector2(randX3, randY3)));
            }

            // Lista, Tar bort en sten från listan om den är förstörd eller osynlig.
            for (int i = 0; i < ammoList.Count; i++)
            {
                if (!ammoList[i].ammoSynlig)
                {
                    ammoList.RemoveAt(i);
                    i--;
                }
            }

        }

        public void LoadHp()
        {
            int randY4 = random.Next(-600,-50);
            int randX4 = random.Next(0,750);

            if(hpList.Count < 1)
            {
                hpList.Add(new HpPowerup(Content.Load<Texture2D>("Textures/Powerups/Hp"), new Vector2(randX4, randY4)));
            }

            for(int i = 0; i < hpList.Count; i++)
            {
                if(!hpList[i].hpSynlig)
                {
                    hpList.RemoveAt(i);
                    i--;
                }
            }

        }

    }
}
