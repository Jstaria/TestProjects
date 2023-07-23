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
    /// <summary>
    /// SlotsUI is the spin interface, 
    /// WinUI is after spinning when object is pulled into a bigger window
    /// </summary>
    public enum SlotState
    {
        SlotsUI,
        WinUI
    }

    internal class SlotMachine
    {
        private Dictionary<string, Texture2D> assets;
        private Texture2D[] buttonAssets;

        private Vector2 position;

        private float timeOfWin;

        private Item[] wonItems;
        private Item prevWonItem;

        private SlotUI SlotUI;

        private SlotState currentState;

        private Dictionary<string, List<Button>> buttons;

        public SlotMachine(Dictionary<string, Texture2D> assets, Vector2 position)
        {
            this.assets = assets;
            this.buttonAssets = new Texture2D[]
            {
                assets["button_active"],
                assets["button_inactive"]
            };

            this.position = position;

            this.currentState = SlotState.SlotsUI;

            this.SlotUI = new SlotUI(assets, position);

            this.wonItems = null;

            buttons = new Dictionary<string, List<Button>>();

            CreateButtons();
        }

        public void Update(GameTime gameTime)
        {
            float timeSinceWin = (float)gameTime.TotalGameTime.TotalSeconds - timeOfWin;
            System.Diagnostics.Debug.WriteLine(timeSinceWin);

            switch (currentState)
            {
                case SlotState.SlotsUI:

                    wonItems = SlotUI.Update(gameTime);

                    if (wonItems[0] != null)
                    {
                        currentState = SlotState.WinUI;
                    }

                    if (wonItems[0] != prevWonItem)
                    {
                        timeOfWin = (float)gameTime.TotalGameTime.TotalSeconds;
                    }

                    foreach (Button button in buttons["SlotUI"])
                    {
                        button.Update(gameTime);
                    }

                    prevWonItem = wonItems[0];

                    break;

                case SlotState.WinUI:
                    System.Diagnostics.Debug.WriteLine(wonItems[0].Name);
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            switch (currentState)
            {
                case SlotState.SlotsUI:

                    SlotUI.Draw(sb);

                    sb.Draw(assets["square"], new Rectangle(0, 0, 1240, 120), Color.Gray);
                    sb.Draw(assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);

                    string name = wonItems[1].Name;

                    sb.DrawString(Game1.ReadOut, wonItems[1].Name.ToUpper(), new Vector2(620 - ((Game1.ReadOut.MeasureString(name).X) / 1.75f), 60), Color.Black);

                    foreach (Button button in buttons["SlotUI"])
                    {
                        button.Draw(sb);
                    }

                    break;

                case SlotState.WinUI:

                    break;
            }
        }

        private void CreateButtons()
        {
            buttons.Add("SlotUI", new List<Button>());

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(520, 550, 200, 100), "Spin", Game1.ReadOut, Color.Black));
            buttons["SlotUI"][0].OnLeftClick += SlotUI.Spin;

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(60, 565, 90, 75), "Next\n Song", Game1.ReadOut, Color.Black));
            buttons["SlotUI"][1].OnLeftClick += Game1.PlayNext;
        }
    }
}
