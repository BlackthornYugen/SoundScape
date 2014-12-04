using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib.Scenes;
using XNALib.Menus;


namespace SoundScape
{
    public class StartScene : GameScene
    {
        private SpriteFont _regularFont, _highlightFont;
        private MenuComponent _menu;

        public int SelectedIndex
        {
            get { return _menu.MenuIndex; }
            set { _menu.MenuIndex = value; }
        }

        public int Count
        {
            get
            {
                return _menuItems.Count;                
            }
        }

        public MenuItem SelectedItem
        {
            get { return _menu.ActiveMenuItem; }
        }
        
        private Color _regularColour = Color.CornflowerBlue;
        private Color _highlightColor = Color.Green;
        private List<string> _menuItems;

        public StartScene(Game game, SpriteBatch sb, string[] menuItems)
            : base(game, sb)
        {
            _menuItems = menuItems.ToList();

            base.Initialize();
        }

        protected override void LoadContent()
        {


            _regularFont = Game.Content.Load<SpriteFont>("fonts/regularFont");
            _highlightFont = Game.Content.Load<SpriteFont>("fonts/highFont");

            _menu = new MenuComponent(Game, _spritebatch, _regularColour, _highlightColor, _regularFont, _highlightFont, Vector2.Zero);
            foreach (string item in _menuItems)
            {
                _menu.Add(item, null);
            }
            Components.Add(_menu);
            base.LoadContent();
        }
    }
}
