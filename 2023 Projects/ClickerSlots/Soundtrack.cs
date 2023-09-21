using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerSlots
{
    internal class Soundtrack
    {
        private Song[] songs;

        private Random rng;

        private float[] volumes;

        private int prevCount;
        private int count;

        /// <summary>
        /// Manages songs
        /// </summary>
        /// <param name="songs">List of songs</param>
        public Soundtrack(Song[] songs)
        {
            this.songs = songs;
            this.volumes = new float[songs.Length];

            List<string> stringVolumes = FileIO.ReadFrom("Volume");

            for (int i = 0; i < songs.Length; i++)
            {
                volumes[i] = float.Parse(stringVolumes[i + 2]);
            }

            MediaPlayer.IsMuted = bool.Parse(stringVolumes[0]);

            rng = new Random();

            count = rng.Next(songs.Length);
        }

        /// <summary>
        /// Plays the next song in queue
        /// </summary>
        public void Play()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                PlayNext();
            }
        }

        /// <summary>
        /// Stops the player, which then makes it play another song
        /// </summary>
        public void PlayNext()
        {
            prevCount = count;

            while ((count = rng.Next(songs.Length)) == prevCount)
            {
                count = rng.Next(songs.Length);
            }

            MediaPlayer.Volume = volumes[count];
            MediaPlayer.Play(songs[count]);
        }

        /// <summary>
        /// Turns only the current songs volume up 
        /// </summary>
        public void VolumeUp()
        {
            MediaPlayer.Pause();

            List<string> newVolumes = FileIO.ReadFrom("Volume");

            volumes[count] = Math.Clamp(volumes[count] + .02f, 0, .2f);
            newVolumes[count + 2] = volumes[count].ToString();

            FileIO.WriteTo("Volume", newVolumes);

            MediaPlayer.Volume = volumes[count];

            MediaPlayer.Resume();
        }

        /// <summary>
        /// Turns only the current songs volume down 
        /// </summary>
        public void VolumeDown()
        {
            MediaPlayer.Pause();

            List<string> newVolumes = FileIO.ReadFrom("Volume");

            volumes[count] = Math.Clamp(volumes[count] - .02f, 0, .2f);
            newVolumes[count + 2] = volumes[count].ToString();

            FileIO.WriteTo("Volume", newVolumes);

            MediaPlayer.Volume = volumes[count];

            MediaPlayer.Resume();
        }

        /// <summary>
        /// Changes to the previously played song, Prev song is updated only after another is played
        /// </summary>
        public void Previous()
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = volumes[prevCount];
            MediaPlayer.Play(songs[prevCount]);
        }

        /// <summary>
        /// Mutes media player
        /// </summary>
        public void Mute()
        {
            List<string> newVolumes = FileIO.ReadFrom("Volume");

            MediaPlayer.IsMuted = !bool.Parse(newVolumes[0]);

            newVolumes[0] = MediaPlayer.IsMuted.ToString();

            FileIO.WriteTo("Volume", newVolumes);
        }
    }
}
