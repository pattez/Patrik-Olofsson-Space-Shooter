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
    public static class Music
    {
        private static Song song;
        private static SoundEffect explosion, laser, click, ofa, wepload, lifepickup;
        private static SoundEffectInstance ofaEffect;

        public static void Setup(ContentManager Content)
        {
            song = Content.Load<Song>("Music/Song/timemachine");
            explosion = Content.Load<SoundEffect>("Music/SoundEffect/explosion");
            laser = Content.Load<SoundEffect>("Music/SoundEffect/laser_shooting_sfx");
            click = Content.Load<SoundEffect>("Music/SoundEffect/click");
            ofa = Content.Load<SoundEffect>("Music/SoundEffect/outofammo");
            wepload = Content.Load<SoundEffect>("Music/SoundEffect/weapload");
            lifepickup = Content.Load<SoundEffect>("Music/SoundEffect/life_pickup");
        }

        public static void StartingPlayingMusic()
        {
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(song);
        }

        public static void StartSoundEffect()
        {
            explosion.Play(0.05f,0.05f,0.05f);
        }

        public static void EndSoundEffect()
        {
            Music.EndSoundEffect();
        }

        public static void EndMusic()
        {
            MediaPlayer.Stop();
        }

        public static void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        public static void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public static void StartLaserEffect()
        {
            laser.Play(0.05f, 0.05f, 0.05f);
        }

        public static void StartClickEffect()
        {
            click.Play(0.7f,0f,0f);
        }

        public static void StartAmmoGet()
        {
            wepload.Play(1f, 0f, 0f);
        }
        /*
        public static void StartOutofAmmo()
        {
            ofaEffect = ofa.CreateInstance();

            if (ofaEffect.State == SoundState.Playing)
            {
                ofaEffect.Volume = 0.5f;
                ofaEffect.IsLooped = false;
                ofaEffect.Stop();
            }
            else
            {
                ofaEffect.Resume();
            }
        }
        */
        public static void StartLifePickUp()
        {
            lifepickup.Play(0.5f, 0f, 0f);
        }

    }
}
