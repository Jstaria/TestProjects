using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        WinUI,
        Inventory
    }

    internal class SlotMachine
    {
        private Dictionary<string, Texture2D> assets;
        private Texture2D[] buttonAssets;

        private Vector2 position;

        private float timeOfWin;
        private float sceneSwitchTime;
        private float currentSceneTime;

        private Item[] wonItems;
        private Item prevWonItem;

        private SlotUI SlotUI;
        private Inventory inventory;

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
            this.inventory = new Inventory(SlotUI.Items);

            this.wonItems = null;

            buttons = new Dictionary<string, List<Button>>();

            CreateButtons();
        }

        public void Update(GameTime gameTime)
        {
            currentSceneTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float timeSinceWin = (float)gameTime.TotalGameTime.TotalSeconds - timeOfWin;

            switch (currentState)
            {
                case SlotState.SlotsUI:

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["SlotUI"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    wonItems = SlotUI.Update(gameTime);

                    if (wonItems[0] != null)
                    {
                        //currentState = SlotState.WinUI;
                        SlotUI.Idle = true;
                    }

                    if (wonItems[0] != prevWonItem && wonItems[0] != null)
                    {
                        Item wonItem = new Item(wonItems[0].Weight, wonItems[0].Color, wonItems[0].Name, wonItems[0].UniqueID);

                        inventory.AddItem(wonItem, true);
                        timeOfWin = (float)gameTime.TotalGameTime.TotalSeconds;
                    }

                    prevWonItem = wonItems[0];

                    break;

                case SlotState.WinUI:

                    break;

                case SlotState.Inventory:

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["Inventory"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    inventory.Update(gameTime);

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

                    PointManager.DrawPoints(sb, new Vector2(620, 685) - Game1.ReadOut.MeasureString(PointManager.TotalPoints.ToString()) / 2);

                    if (SlotUI.spinParticle != null)
                    {
                        SlotUI.spinParticle.Draw(sb);
                    }

                    break;

                case SlotState.WinUI:

                    break;

                case SlotState.Inventory:

                    sb.Draw(assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);

                    
                    foreach (Button button in buttons["Inventory"])
                    {
                        button.Draw(sb);
                    }
                    
                    inventory.Draw(sb);

                    break;
            }
        }

        private bool CanUseButtons(GameTime gameTime)
        {
            bool canUse = false;

            if (gameTime.TotalGameTime.TotalSeconds - sceneSwitchTime > .1f) { canUse = true; }

            return canUse;
        }

        private void OpenInventory()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = SlotState.Inventory;
        }

        public void CloseInventory()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = SlotState.SlotsUI;
        }

        private void CreateButtons()
        {
            buttons.Add("SlotUI", new List<Button>());

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(520, 550, 200, 100), "Spin", Game1.ReadOut, Color.Black, Color.White, 0));
            buttons["SlotUI"][0].OnLeftClick += SlotUI.Spin;

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(950, 565, 120, 75), "Inventory", Game1.regular, Color.Black, Color.White, .5f));
            buttons["SlotUI"][1].OnLeftClick += OpenInventory;

            buttons.Add("Inventory", new List<Button>());

            buttons["Inventory"].Add(new Button(buttonAssets, new Rectangle(950, 565, 120, 75), "Back", Game1.regular, Color.Black, Color.White, .5f));
            buttons["Inventory"][0].OnLeftClick += CloseInventory;
        }
    }
}
