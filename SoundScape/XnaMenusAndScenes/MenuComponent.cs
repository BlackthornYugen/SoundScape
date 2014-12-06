using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Menus
{
    public class MenuComponent<T> : DrawableGameComponent
    {
        private Color _colourHighlighted;
        private Color _colourNormal;
        private readonly SpriteFont _fontHighlighted;
        private readonly SpriteFont _fontNormal;
        private readonly List<MenuItem<T>> _menuItems;
        private Vector2 _position;
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
            _menuItems = new List<MenuItem<T>>();
        }

        public MenuComponent(Game game,
            SpriteBatch spritebatch,
            Color colourNormal,
            Color colourHighlighted,
            SpriteFont fontNormal,
            SpriteFont fontHighlighted,
            Vector2 position,
            List<MenuItem<T>> menuItems)
            : this(game, spritebatch, colourNormal, colourHighlighted, fontNormal, fontHighlighted, position)
        {
            _menuItems = menuItems;
        }

        public List<MenuItem<T>> MenuItems
        {
            get { return _menuItems; }
        }

        public int MenuIndex
        {
            get { return _menuIndex; }
            set { _menuIndex = value; }
        }

        public MenuItem<T> ActiveMenuItem
        {
            get { return _menuItems[_menuIndex]; }
        }

        public Texture2D Logo { get; set; }

        public Vector2 LogoPosition
        {
            get { return _logoPosition; }
            set { _logoPosition = value; }
        }

        public Color ColourHighlighted
        {
            get { return _colourHighlighted; }
            set { _colourHighlighted = value; }
        }

        public Color ColourNormal
        {
            get { return _colourNormal; }
            set { _colourNormal = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public T this[int index]
        {
            get { return _menuItems[index].Component; }
        }

        public void Add(string menuItemName, T menuItemComponent)
        {
            var newItem = new MenuItem<T>();
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
                _spritebatch.DrawString(font, _menuItems[i].Name, Position + (Vector2.UnitY*i*font.LineSpacing), colour);
            }
            _spritebatch.End();
            base.Draw(gameTime);
        }
    }

    public struct MenuItem<T>
    {
        public string Name { get; set; }
        public T Component { get; set; }
    }
}