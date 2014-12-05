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
        private GameScene _controllerStats;

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

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }
        
        private Color _regularColour = Color.LightYellow;
        private Color _highlightColor = Color.Yellow;
        private List<string> _menuItems;

        public StartScene(Game game, SpriteBatch sb, string[] menuItems)
            : base(game, sb)
        {
            _menuItems = menuItems.ToList();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _controllerStats = new ControllerStatus(Game, _spritebatch);
            _regularFont = Game.DefaultGameFont;
            _highlightFont = Game.Content.Load<SpriteFont>("fonts/highFont");

            _menu = new MenuComponent(Game, _spritebatch, _regularColour, _highlightColor, _regularFont, _highlightFont,
                Vector2.One * 100) { Logo = Game.Content.Load<Texture2D>("logo"), LogoPosition = new Vector2(500, 250) };
            foreach (string item in _menuItems)
            {
                _menu.Add(item, null);
            }
            Components.Add(_menu);
            Components.Add(_controllerStats);
            _controllerStats.Show();
            _controllerStats.Initialize();
            base.LoadContent();
        }
    }
}
