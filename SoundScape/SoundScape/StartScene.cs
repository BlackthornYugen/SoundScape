using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNALib.Scenes;
using XNALib.Menus;


namespace SoundScape
{
    public class StartScene : GameScene
    {
        ParticleEmiter partEmitterTopRight1, partEmitterTopRight2;

        private SpriteFont regularFont, highlightFont;
        private MenuComponent menu;

        public int SelectedIndex
        {
            get { return menu.MenuIndex; }
        }

        public MenuItem SelectedItem
        {
            get { return menu.ActiveMenuItem; }
        }
        
        private Color regularColour = Color.CornflowerBlue;
        private Color highlightColor = Color.Green;
        private List<string> menuItems;

        public StartScene(Game game, SpriteBatch sb, string[] menuItems)
            : base(game, sb)
        {
            this.menuItems = menuItems.ToList();
            Initialize();
        }

        protected override void LoadContent()
        {
            partEmitterTopRight1 = new ParticleEmiter(this, spritebatch, new Vector2(-150, 300), 25, 0.1f);
            this.Components.Add(partEmitterTopRight1);

            partEmitterTopRight2 = new ParticleEmiter(this, spritebatch, new Vector2(-200, 300), 30, 0.3f);
            this.Components.Add(partEmitterTopRight2);


            regularFont = Game.Content.Load<SpriteFont>("fonts/regularFont");
            highlightFont = Game.Content.Load<SpriteFont>("fonts/highFont");

            this.menu = new MenuComponent(Game, spritebatch, regularColour, highlightColor, regularFont, highlightFont, Vector2.Zero);
            foreach (string item in menuItems)
            {
                menu.Add(item, null);
            }
            this.Components.Add(menu);
            base.LoadContent();
        }
    }
}
