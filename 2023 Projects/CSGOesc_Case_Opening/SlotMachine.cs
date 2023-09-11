using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        private float expand;
        private double time;
        private Vector2 position;

        private float timeOfWin;
        private float sceneSwitchTime;
        private float currentSceneTime;

        private Item[] wonItems;
        private Item prevWonItem;

        private bool menuActive;
        private bool autoSpin;

        private SlotUI SlotUI;
        private Inventory inventory;

        private SlotState currentState;

        private static string[] oneOfEach;
        private List<Particle> particles;
        private List<ParticleSystem> pSystems;
        private ParticleSystem ps;

        private Dictionary<string, List<Button>> buttons;

        public static Item WonItem { get; private set; }
        public static string[] OneOfEach { get { return oneOfEach; } }

        public SlotMachine(Dictionary<string, Texture2D> assets, Vector2 position, bool menuActive, float idleSpeed)
        {
            this.expand = 0;
            this.menuActive = menuActive;
            this.particles = new List<Particle>();

            this.assets = assets;
            this.buttonAssets = new Texture2D[]
            {
                assets["button_active"],
                assets["button_inactive"]
            };

            this.position = position;

            this.currentState = SlotState.SlotsUI;

            this.SlotUI = new SlotUI(assets, position, menuActive, idleSpeed);
            this.inventory = new Inventory(SlotUI.Items);

            this.wonItems = null;
            oneOfEach = new string[7];

            this.autoSpin = false;

            buttons = new Dictionary<string, List<Button>>();

            CreateButtons();
            this.menuActive = menuActive;

            pSystems = new List<ParticleSystem>();

            for (int i = 0; i < SlotUI.Items.Count; i++)
            {
                Texture2D asset =
                    i <= 1 ? assets["square"] :
                    i > 1 && i <= 3 ? assets["cloud"] :
                    i == 4 ? assets["light_effect"] :
                    i == 5 ? assets["orb"] :
                    assets["sparkle"];

                int speed =
                    i == 0 ? 1 :
                    i > 0 && i < 3 ? 2 :
                    i == 3 ? 3 :
                    i == 4 ? 4 :
                    i == 5 ? 5 :
                    10;

                pSystems.Add(new ParticleSystem((i + 1) * (i + 1) * 200, SlotUI.Items[i].Color, Color.White, .96f, asset, new Rectangle(540,230,160,160), speed, true, 360));
            }
        }

        public void Update(GameTime gameTime)
        {
            currentSceneTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float timeSinceWin = (float)gameTime.TotalGameTime.TotalSeconds - timeOfWin;

            switch (currentState)
            {
                case SlotState.SlotsUI:

                    expand = 0;

                    buttons["SlotUI"][2].SwitchBool = autoSpin;

                    if (autoSpin)
                    {
                        SlotUI.AutoSpin();

                        autoSpin = PointManager.CurrentPoints < 15 ? false : true; 
                    }

                    if (!menuActive)
                    {
                        if (CanUseButtons(gameTime))
                        {
                            foreach (Button button in buttons["SlotUI"])
                            {
                                button.Update(gameTime);
                            }
                        }
                    }

                    wonItems = SlotUI.Update(gameTime);

                    if (wonItems[0] != prevWonItem && wonItems[0] != null)
                    {
                        WonItem = wonItems[0];

                        for (int i = 0; i < oneOfEach.Length; i++)
                        {
                            if (oneOfEach[i] != null) { continue; }

                            oneOfEach[i] = wonItems[0].Name == SlotUI.Items[i].Name ? wonItems[0].Name : null;
                        }

                        //currentState = SlotState.WinUI;
                        SlotUI.Idle = true;

                        Item wonItem = new Item(wonItems[0].Weight, wonItems[0].Color, wonItems[0].Name, wonItems[0].UniqueID, Item.sound.Volume);

                        inventory.AddItem(wonItem, true);
                        timeOfWin = (float)gameTime.TotalGameTime.TotalSeconds;

                        string name = wonItems[0].Name;

                        ps =
                            name == "Common" ? pSystems[0] :
                            name == "Uncommon" ? pSystems[1] :
                            name == "Rare" ? pSystems[2] :
                            name == "Epic" ? pSystems[3] :
                            name == "Mystic" ? pSystems[4] :
                            name == "Legendary" ? pSystems[5] :
                            name == "Royal" ? pSystems[6] : null;

                        if (!autoSpin)
                        {
                            currentState = SlotState.WinUI;
                            ps.Reset();
                        }
                    }

                    prevWonItem = wonItems[0];

                    break;

                case SlotState.WinUI:

                    time = gameTime.TotalGameTime.TotalSeconds - timeOfWin;

                    if (time > .25 && time < 9.5)
                    {
                        ps.Update();
                    }

                    if (time < 2.5)
                    {
                        expand = (float)Math.Clamp(expand + .05, 0, 1);
                    }
                    else if (time > 9.5)
                    {
                        expand = (float)Math.Clamp(expand - .1, 0, 1);
                    }

                    if (time > 10)
                    {
                        currentState = SlotState.SlotsUI;
                    }

                    foreach (Button button in buttons["WinUI"])
                    {
                        button.Update(gameTime);
                    }

                    break;

                case SlotState.Inventory:

                    if (CanUseButtons(gameTime))
                    {
                        foreach (Button button in buttons["Inventory"])
                        {
                            button.Update(gameTime);
                        }
                    }

                    foreach (Particle particle in particles)
                    {
                        particle.Update();
                    }

                    inventory.Update(gameTime);

                    break;
            }

            buttons["Inventory"][1].HoverText = String.Format("+" + inventory.InventoryValue.ToString());
        }

        public void Draw(SpriteBatch sb)
        {
            switch (currentState)
            {
                case SlotState.SlotsUI:

                    SlotUIDraw(sb);

                    break;

                case SlotState.WinUI:

                    SlotUIDraw(sb);

                    sb.Draw(Game1.assets["square"], new Rectangle((int)(100 + (520*(1-expand))), 100, (int)(1040 * expand), 520), Color.Black*.9f);

                    if (time > .05 && time < 9.5)
                    {
                        int yOffset = -50;

                        sb.Draw(Game1.assets["square"], new Rectangle(400 + (int)(Math.Clamp(100 * (1 - expand * 2), 0, 200)), 280 + yOffset, (int)(100 * Math.Clamp((expand * 2), 0, 1)), 160), wonItems[0].Color * .75f);
                        sb.Draw(Game1.assets["square"], new Rectangle(740 + (int)(Math.Clamp(100 * (1 - expand * 2), 0, 200)), 280 + yOffset, (int)(100 * Math.Clamp((expand * 2), 0, 1)), 160), wonItems[0].Color * .75f);

                        sb.Draw(Game1.assets["square"], new Rectangle(330 + (int)(Math.Clamp(100 * (1 - expand * 2), 0, 200)), 300 + yOffset, (int)(50 * Math.Clamp((expand * 2), 0, 1)), 120), wonItems[0].Color * .25f);
                        sb.Draw(Game1.assets["square"], new Rectangle(860 + (int)(Math.Clamp(100 * (1 - expand * 2), 0, 200)), 300 + yOffset, (int)(50 * Math.Clamp((expand * 2), 0, 1)), 120), wonItems[0].Color * .25f);

                        ps.Draw(sb);
                        
                        sb.Draw(Game1.assets["square"], new Rectangle(520 + (int)(Math.Clamp(100 * (1 - expand * 2), 0, 200)), 260 + yOffset, (int)(200 * Math.Clamp((expand * 2), 0, 1)), 200), wonItems[0].Color);

                        foreach (Button button in buttons["WinUI"])
                        {
                            button.Draw(sb);
                        }
                    }

                    break;

                case SlotState.Inventory:

                    sb.Draw(assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);
                    
                    foreach (Button button in buttons["Inventory"])
                    {
                        button.Draw(sb);
                    }
                    
                    foreach (Particle particle in particles)
                    {
                        particle.DrawString(sb);
                    }

                    inventory.Draw(sb);

                    break;
            }
        }

        private void SlotUIDraw(SpriteBatch sb)
        {
            if (!menuActive)
            {
                sb.Draw(assets["square"], new Rectangle(0, 0, 1240, 720), Color.Black * .2f);
            }

            SlotUI.Draw(sb);

            if (!menuActive)
            {
                sb.Draw(assets["square"], new Rectangle(0, 0, 1240, 120), Color.Gray);
                sb.Draw(assets["square"], new Rectangle(0, 500, 1240, 720), Color.Gray);

                if (wonItems != null)
                {
                    string name = wonItems[1].Name;

                    sb.DrawString(Game1.ReadOut, wonItems[1].Name.ToUpper(), new Vector2(620 - ((Game1.ReadOut.MeasureString(name).X) / 1.75f), 60), Color.Black);

                }

                foreach (Button button in buttons["SlotUI"])
                {
                    button.Draw(sb);
                }

                PointManager.DrawPoints(sb, new Vector2(620, 685) - Game1.ReadOut.MeasureString(PointManager.CurrentPoints.ToString()) / 2);

                if (SlotUI.spinParticle != null)
                {
                    SlotUI.spinParticle.DrawString(sb);
                }
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

        private void SpawnParticle(string text)
        {
            particles.Add(new Particle(Mouse.GetState().Position.ToVector2(), .985f, text, Game1.regular));
        }

        private void AutoSpin()
        {
            autoSpin = !autoSpin;
        }

        public void Slots()
        {
            sceneSwitchTime = currentSceneTime;
            currentState = SlotState.SlotsUI;
        }

        public void VolumeUp()
        {
            SlotUI.VolumeUp();
        }

        public void VolumeDown()
        {
            SlotUI.VolumeDown();
        }

        public void Clear()
        {
            inventory.Clear();

            oneOfEach = new string[7];
        }

        private void CreateButtons()
        {
            buttons.Add("SlotUI", new List<Button>());

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(520, 550, 200, 100), "Spin", Game1.ReadOut, Color.Black, Color.White, 0));
            buttons["SlotUI"][0].OnLeftClick += SlotUI.Spin;

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(950, 565, 120, 75), "Inventory", Game1.regular, Color.Black, Color.White, .5f));
            buttons["SlotUI"][1].OnLeftClick += OpenInventory;

            buttons["SlotUI"].Add(new Button(buttonAssets, new Rectangle(175, 565, 120, 75), "Auto Spin", Game1.regular, Color.Black, Color.LightCoral, Color.LightGreen, .5f, true));
            buttons["SlotUI"][2].OnLeftClick += AutoSpin;

            buttons.Add("Inventory", new List<Button>());

            buttons["Inventory"].Add(new Button(buttonAssets, new Rectangle(950, 565, 120, 75), "Back", Game1.regular, Color.Black, Color.White, .5f));
            buttons["Inventory"][0].OnLeftClick += Slots;

            buttons["Inventory"].Add(new Button(buttonAssets, new Rectangle(175, 565, 90, 75), "Sell All", Game1.regular, Color.Black, Color.White, .5f));
            buttons["Inventory"][1].OnLeftClick += inventory.SellAll;
            buttons["Inventory"][1].OnLeftClickString += SpawnParticle;

            buttons.Add("WinUI", new List<Button>());

            buttons["WinUI"].Add(new Button(buttonAssets, new Rectangle(450, 500, 150, 75), "Sell", Game1.ReadOut, Color.Black, Color.White, .5f));
            buttons["WinUI"][0].OnLeftClick += inventory.SellWonItem;
            buttons["WinUI"][0].OnLeftClick += Slots;

            buttons["WinUI"].Add(new Button(buttonAssets, new Rectangle(640, 500, 150, 75), "Continue", Game1.ReadOut, Color.Black, Color.White, .5f));
            buttons["WinUI"][1].OnLeftClick += Slots;
        }
    }
}
