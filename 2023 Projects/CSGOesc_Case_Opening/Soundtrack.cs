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
            this.volumes = new float[]
            {
                0.05f,
                0.1f,
                0.075f,
                0.075f,
                0.075f,
                0.1f,
                0.075f,
            };

            rng = new Random();

            count = rng.Next(songs.Length);

            MediaPlayer.IsRepeating = true;
        }

        public void Play()
        {
            if (!(MediaPlayer.State == MediaState.Playing))
            {
                MediaPlayer.Volume = volumes[count];
                MediaPlayer.Play(songs[count]);

                while ((count = rng.Next(songs.Length)) == prevCount)
                {
                    count = rng.Next(songs.Length);
                }
            }

            prevCount = count;
        }

        /// <summary>
        /// Stops the player, which then makes it play another song
        /// </summary>
        public void PlayNext()
        {
            MediaPlayer.Stop();
        }
    }
}
