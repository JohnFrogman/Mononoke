using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mononoke
{
    internal static class SoundAssetManager
    {
        static Dictionary<string, Song> Songs = new();
        static Dictionary<string, SoundEffect> SoundEffects = new();
        static TimeSpan Timer;
        static List<Song> songQueue = new();
        static int currentSong = 0;
        static bool Paused = false;
        static float MusicVolume = 0.01f;
        public static void SetVolume(float f)
        { 
            if (f > 1.0f)
                f = 1.0f;
            if (f < 0.0f)
                f = 0.0f;
            MediaPlayer.Volume = f * MusicVolume;
        }
        public static void SetPaused(bool b)
        {
            Paused = b;
            if (Paused)
            {
                MediaPlayer.Pause();
            }
            else
                MediaPlayer.Resume();
        }
        public static void Update(GameTime gameTime)
        {
            if (Paused) return;

            Timer += gameTime.ElapsedGameTime;
            if (Timer > songQueue[0].Duration)
            {
                currentSong++;
                if (currentSong >= songQueue.Count)
                {
                    currentSong = 0;
                }
                MediaPlayer.Play(songQueue[currentSong]);
            }
        }
        public static void PlaySFXByName(string name)
        {
            if (SoundEffects.ContainsKey(name.ToLower()))
            {
                //MediaPlayer.Play(SoundEffects[name.ToLower()]);
            }
        }
        public static void PlaySongsByName(List<string> names)
        {
            Paused = false;
            songQueue.Clear();
            Timer = TimeSpan.Zero;
            foreach (string name in names)
            { 
                if (Songs.ContainsKey(name.ToLower()))
                {
                    songQueue.Add(Songs[name.ToLower()]);
                }
            }
            if (songQueue.Count > 0)
            {
                currentSong = 0;
                MediaPlayer.Play(songQueue[currentSong]);
            }
            else 
                Paused = true;
        }
        public static void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }
        public static void Initialise()
        {
            MediaPlayer.Volume = 0.01f;
            string musicDir = "data/music/";
            string sfxDir = "data/sfx/";
            if (!Directory.Exists(musicDir))
            {
                throw new Exception("Music directory does not exist! " + musicDir);
            }
            //if (!Directory.Exists(sfxDir))
            //{
            //    throw new Exception("SFX directory does not exist! " + sfxDir);
            //}   
            string[] files = Directory.GetFiles(musicDir);
            foreach (string f in files)
            {
                if (".wav" == Path.GetExtension(f) || ".ogg" == Path.GetExtension(f))
                {
                    string name = f.Substring(musicDir.Length, f.Length - musicDir.Length - 4).ToLower();
                    Songs.Add(name, Song.FromUri(name, new Uri(Path.GetFullPath(f))));
                }
            }
            //files = Directory.GetFiles(sfxDir);
            //foreach (string f in files)
            //{
            //    if (".wav" == Path.GetExtension(f) || ".ogg" == Path.GetExtension(f))
            //    {
            //        SoundEffects.Add(f.Substring(sfxDir.Length, f.Length - sfxDir.Length - 4).ToLower(), SoundEffect.FromFile(f));
            //    }
            //}
        }
    }
}
