using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    // Called by reference when button is clicked on
    public delegate void OnButtonClick();

    internal class Button
    {
        private Rectangle position;
        private Vector2 textPos;
        private Texture2D[] assets;
        private MouseState prevMState;
        private string text;
        private SpriteFont font;
        private Color fontColor;
        private bool active;

        public bool Active { get { return active; } }

        public event OnButtonClick OnLeftClick;

        /// <summary>
        /// Button Class
        /// </summary>
        /// <param name="assets">First asset is non-active texture, Second is when hovered over</param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fontColor"></param>
        public Button(Texture2D[] assets, Rectangle position, string text, SpriteFont font, Color fontColor)
        {
            this.assets = assets;
            this.text = text;
            this.font = font;
            this.fontColor = fontColor;
            this.position = position;

            Vector2 textLength = font.MeasureString(text);
            textPos = new Vector2(
                (position.X + position.Width / 2) - textLength.X / 2,
                (position.Y + position.Height / 2) - textLength.Y / 2
            );
        }

        public void Update(GameTime gameTime)
        {
            active = false;

            MouseState currentMState = Mouse.GetState();

            // If the cursor is over the button, it will trigger a different texture
            // and allow it to be clicked
            if (this.position.Contains(currentMState.Position))
            {
                active = true;
            }

            if (currentMState.LeftButton == ButtonState.Pressed &&
                prevMState.LeftButton == ButtonState.Released &&
                active) 
            {
                if (OnLeftClick != null)
                {
                    OnLeftClick();
                }
            }

            prevMState = currentMState;
        }

        public void Draw(SpriteBatch sb)
        {
            // Hovered over
            if (active)
            {
                int expand = 10;
                sb.Draw(assets[1], new Rectangle(position.X - expand, position.Y - expand, position.Width + expand * 2, position.Height + expand * 2), Color.White); ;
            }
            // Regular
            else
            {
                sb.Draw(assets[0], position, Color.White);
            }

            if (text != null)
            {
                // Draw button text over the button
                sb.DrawString(font, text, textPos, fontColor);
            }
        }
    }
}
