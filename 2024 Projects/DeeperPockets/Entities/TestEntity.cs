using DeeperPockets.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeperPockets.Entities
{
    internal class TestEntity : Entity
    {
        public TestEntity(Vector2 pos, Texture2D tex) : base(pos, tex)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)drawPos.X - tex.Width / 2, (int)drawPos.Y - tex.Height / 2, tex.Width, tex.Height), Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            drawPos = pos - Global.Instance.WorldOffset - Global.Instance.CameraOffset;
        }
    }
}
