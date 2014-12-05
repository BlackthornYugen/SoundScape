using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace XNALib.Menus
{
    public class MenuComponent : DrawableGameComponent
    {
        private int _menuIndex;
        private List<MenuItem> _menuItems;
        private SpriteBatch _spritebatch;
        private Color _colourNormal;
        private Color _colourHighlighted;
        private SpriteFont _fontNormal;
        private SpriteFont _fontHighlighted;
        private Vector2 _position;
        private Texture2D _logo;
        private Vector2 _logoPosition;

        public List<MenuItem> MenuItems
        {
            get { return _menuItems; }
        }

        public int MenuIndex
        {
            get { return _menuIndex; }
            set { _menuIndex = value; }
        }

        public MenuItem ActiveMenuItem
        {
            get { return _menuItems[_menuIndex]; }
        }

        public Texture2D Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public Vector2 LogoPosition
        {
            get { return _logoPosition; }
            set { _logoPosition = value; }
        }

        public DrawableGameComponent this[int index]
        {
            get
            {
                return _menuItems[index].Component;
            }
        }

        public void Add(string menuItemName, DrawableGameComponent menuItemComponent)
        {
            var newItem = new MenuItem();
            newItem.Name = menuItemName;
            newItem.Component = menuItemComponent;
            this._menuItems.Add(newItem);
        }

        public MenuComponent(Game game,
            SpriteBatch spritebatch, 
            Color colourNormal, 
            Color colourHighlighted, 
            SpriteFont fontNormal, 
            SpriteFont fontHighlighted,
            Vector2 position)
            : base(game)
        {
            this._spritebatch = spritebatch;
            this._colourNormal = colourNormal;
            this._colourHighlighted = colourHighlighted;
            this._fontNormal = fontNormal;
            this._fontHighlighted = fontHighlighted;
            this._position = position;
            this._menuItems = new List<MenuItem>();
        }

        public MenuComponent(Game game,
            SpriteBatch spritebatch, 
            Color colourNormal, 
            Color colourHighlighted, 
            SpriteFont fontNormal, 
            SpriteFont fontHighlighted,
            Vector2 position,
            List<MenuItem> menuItems)
            : this(game, spritebatch, colourNormal, colourHighlighted, fontNormal, fontHighlighted, position)
        {
            this._menuItems = menuItems;
        }

        public override void Draw(GameTime gameTime)
        {
            _spritebatch.Begin();
            if(Logo != null && LogoPosition != Vector2.Zero)
                _spritebatch.Draw(Logo, LogoPosition, Color.White);
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Color colour;
                SpriteFont font;
                if (i != _menuIndex)
                {
                    colour = _colourNormal;
                    font = _fontNormal;
                }
                else
                {
                    colour = _colourHighlighted;
                    font = _fontHighlighted;
                }
                _spritebatch.DrawString(font, _menuItems[i].Name, _position + (Vector2.UnitY * i * font.LineSpacing), colour);
            }
            _spritebatch.End();
            base.Draw(gameTime);
        }
    }

    public struct MenuItem
    {
        public string Name { get; set; }
        public DrawableGameComponent Component { get; set; }
    }
}
