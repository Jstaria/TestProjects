using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal class Soundtrack
    {
        private Song[] songs;

        private Random rng;

        private float[] volumes;

        private int prevCount;
        private int count;

        public Soundtrack(Song[] songs)
        {
            this.songs = songs;
            this.volumes = new float[songs.Length];

            List<string> stringVolumes = FileIO.ReadFrom("Volume");

            for (int i = 0; i < songs.Length; i++)
            {
                volumes[i] = float.Parse(stringVolumes[i + 1]);
            }

            MediaPlayer.IsMuted = bool.Parse(stringVolumes[0]);

            rng = new Random();

            count = rng.Next(songs.Length);
        }

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

        public void VolumeUp()
        {
            MediaPlayer.Pause();

            List<string> newVolumes = FileIO.ReadFrom("Volume");

            for (int i = 0; i < songs.Length; i++)
            {
                volumes[i] = Math.Clamp(volumes[i] + .02f, 0, 1);
                newVolumes[i + 1] = volumes[i].ToString();
            }

            FileIO.WriteTo("Volume", newVolumes);

            MediaPlayer.Volume = volumes[count];

            MediaPlayer.Resume();
        }

        public void VolumeDown()
        {
            MediaPlayer.Pause();

            List<string> newVolumes = FileIO.ReadFrom("Volume");

            for (int i = 0; i < songs.Length; i++)
            {
                volumes[i] = Math.Clamp(volumes[i] - .01f, 0, 1);
                newVolumes[i + 1] = volumes[i].ToString();
            }

            FileIO.WriteTo("Volume", newVolumes);

            MediaPlayer.Volume = volumes[count];

            MediaPlayer.Resume();
        }

        public void Previous()
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = volumes[prevCount];
            MediaPlayer.Play(songs[prevCount]);
        }

        public void Mute()
        {
            List<string> newVolumes = FileIO.ReadFrom("Volume");

            MediaPlayer.IsMuted = !bool.Parse(newVolumes[0]);

            newVolumes[0] = MediaPlayer.IsMuted.ToString();

            FileIO.WriteTo("Volume", newVolumes);
        }
    }
}
