using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace XNALib.Menus
{
    public class MenuComponent : DrawableGameComponent
    {
        private int menuIndex;
        private List<MenuItem> menuItems;
        private SpriteBatch spritebatch;
        private Color colourNormal;
        private Color colourHighlighted;
        private SpriteFont fontNormal;
        private SpriteFont fontHighlighted;
        private Vector2 position;
        private Texture2D logo;

        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
        }

        public int MenuIndex
        {
            get { return menuIndex; }
            set { menuIndex = value; }
        }

        public MenuItem ActiveMenuItem
        {
            get { return menuItems[menuIndex]; }
        }

        public DrawableGameComponent this[int index]
        {
            get
            {
                return menuItems[index].Component;
            }
        }

        public void Add(string menuItemName, DrawableGameComponent menuItemComponent)
        {
            var newItem = new MenuItem();
            newItem.Name = menuItemName;
            newItem.Component = menuItemComponent;
            this.menuItems.Add(newItem);
        }

        public MenuComponent(Game game,
            SpriteBatch spritebatch, 
            Color colourNormal, 
            Color colourHighlighted, 
            SpriteFont fontNormal, 
            SpriteFont fontHighlighted,
            Vector2 position, Texture2D logo)
            : base(game)
        {
            this.spritebatch = spritebatch;
            this.colourNormal = colourNormal;
            this.colourHighlighted = colourHighlighted;
            this.fontNormal = fontNormal;
            this.fontHighlighted = fontHighlighted;
            this.position = position;
            this.menuItems = new List<MenuItem>();
            this.logo = logo;
        }

        public MenuComponent(Game game,
            SpriteBatch spritebatch, 
            Color colourNormal, 
            Color colourHighlighted, 
            SpriteFont fontNormal, 
            SpriteFont fontHighlighted,
            Vector2 position,
            List<MenuItem> menuItems, Texture2D logo)
            : this(game, spritebatch, colourNormal, colourHighlighted, fontNormal, fontHighlighted, position, logo)
        {
            this.menuItems = menuItems;
            this.logo = logo;
        }

        public override void Draw(GameTime gameTime)
        {
            
            SpriteFont font;
            Color colour;
            spritebatch.Begin();
            spritebatch.Draw(logo, new Vector2(500, 250), Color.White);
            for (int i = 0; i < menuItems.Count; i++)
            {

                if (i != menuIndex)
                {
                    colour = colourNormal;
                    font = fontNormal;
                }
                else
                {
                    colour = colourHighlighted;
                    font = fontHighlighted;
                }
                spritebatch.DrawString(font, menuItems[i].Name, position + new Vector2(100, i * font.LineSpacing), colour);
            }
            spritebatch.End();
            base.Draw(gameTime);
        }
    }

    public struct MenuItem
    {
        string name;
        DrawableGameComponent component;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DrawableGameComponent Component
        {
            get { return component; }
            set { component = value; }
        }
    }
}
