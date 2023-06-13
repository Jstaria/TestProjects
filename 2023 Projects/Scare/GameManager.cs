using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scare
{
    internal class GameManager
    {
        private List<Enemy> enemies;
        private Player player;

        private Dictionary<string, Texture2D> assets;

        private Camera camera;

        private float timeSinceLastFrame;

        /// <summary>
        /// Manages player, enemies, and objectives
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="enemies"></param>
        /// <param name="player"></param>
        public GameManager(List<Enemy> enemies, Player player, Camera camera, Dictionary<string, Texture2D> assets)
        {
            this.enemies = enemies;
            this.player = player;
            this.camera = camera;
            this.assets = assets;
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastFrame;

            System.Diagnostics.Debug.WriteLine(time);

            player.Update(gameTime);
            camera.Follow(player);

            timeSinceLastFrame = (float)gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch, camera.Transform);

            int width = player.RunTime / 10;

            int barWidthHelf = 500;

            System.Diagnostics.Debug.WriteLine(player.Position);

            spriteBatch.Draw(
                assets["underbar"], 
                new Vector2(
                    MathHelper.Clamp(player.Position.X + player.Asset.Width / 2 - barWidthHelf, -camera.Clamp - barWidthHelf, camera.Clamp - barWidthHelf), 
                    MathHelper.Clamp(player.Position.Y - Game1.height / 2 + 50, -camera.Clamp - 15 - barWidthHelf, camera.Clamp - barWidthHelf - 15)), 
                new Rectangle(
                    (int)player.Position.X + player.Asset.Width / 2 - barWidthHelf, 
                    (int)player.Position.Y - Game1.height / 2 + 50, barWidthHelf * 2, 10), 
                Color.Gray * player.Opacity);

            spriteBatch.Draw(
                assets["bar"], 
                new Vector2(
                    MathHelper.Clamp(player.Position.X + player.Asset.Width / 2 - barWidthHelf, -camera.Clamp - barWidthHelf, camera.Clamp - barWidthHelf), 
                    MathHelper.Clamp(player.Position.Y - Game1.height / 2 + 50, -camera.Clamp - 15 - barWidthHelf, camera.Clamp - barWidthHelf - 15)), 
                new Rectangle(
                    (int)player.Position.X + player.Asset.Width / 2 - barWidthHelf, 
                    (int)player.Position.Y - Game1.height / 2 + 50, width, 10), 
                Color.White * player.Opacity);
        }
    }
}
