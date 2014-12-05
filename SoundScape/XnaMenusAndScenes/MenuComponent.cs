using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Menus
{
    public class MenuComponent : DrawableGameComponent
    {
        private readonly Color _colourHighlighted;
        private readonly Color _colourNormal;
        private readonly SpriteFont _fontHighlighted;
        private readonly SpriteFont _fontNormal;
        private readonly List<MenuItem> _menuItems;
        private readonly Vector2 _position;
        private readonly SpriteBatch _spritebatch;
        private Vector2 _logoPosition;
        private int _menuIndex;

        public MenuComponent(Game game,
            SpriteBatch spritebatch,
            Color colourNormal,
            Color colourHighlighted,
            SpriteFont fontNormal,
            SpriteFont fontHighlighted,
            Vector2 position)
            : base(game)
        {
            _spritebatch = spritebatch;
            _colourNormal = colourNormal;
            _colourHighlighted = colourHighlighted;
            _fontNormal = fontNormal;
            _fontHighlighted = fontHighlighted;
            _position = position;
            _menuItems = new List<MenuItem>();
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
            _menuItems = menuItems;
        }

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

        public Texture2D Logo { get; set; }

        public Vector2 LogoPosition
        {
            get { return _logoPosition; }
            set { _logoPosition = value; }
        }

        public DrawableGameComponent this[int index]
        {
            get { return _menuItems[index].Component; }
        }

        public void Add(string menuItemName, DrawableGameComponent menuItemComponent)
        {
            var newItem = new MenuItem();
            newItem.Name = menuItemName;
            newItem.Component = menuItemComponent;
            _menuItems.Add(newItem);
        }

        public override void Draw(GameTime gameTime)
        {
            _spritebatch.Begin();
            if (Logo != null && LogoPosition != Vector2.Zero)
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
                _spritebatch.DrawString(font, _menuItems[i].Name, _position + (Vector2.UnitY*i*font.LineSpacing), colour);
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