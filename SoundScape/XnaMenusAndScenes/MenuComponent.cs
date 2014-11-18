using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNALib.Menus
{
    public class MenuComponent : DrawableGameComponent
    {
        private int menuIndex = 0;
        private List<MenuItem> menuItems;
        private KeyboardState oldKS;
        private SpriteBatch spritebatch;
        private Color colourNormal;
        private Color colourHighlighted;
        private SpriteFont fontNormal;
        private SpriteFont fontHighlighted;
        private Vector2 position;

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
            Vector2 position)
            : base(game)
        {
            this.spritebatch = spritebatch;
            this.colourNormal = colourNormal;
            this.colourHighlighted = colourHighlighted;
            this.fontNormal = fontNormal;
            this.fontHighlighted = fontHighlighted;
            this.position = position;
            this.menuItems = new List<MenuItem>();
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
            this.menuItems = menuItems;
        }

        public override void Update(GameTime gameTime)
        {    
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyUp(Keys.Down) && oldKS.IsKeyDown(Keys.Down))
                menuIndex = Math.Min(menuIndex + 1, menuItems.Count - 1);
            else if (ks.IsKeyUp(Keys.Up) && oldKS.IsKeyDown(Keys.Up))
                menuIndex = Math.Max(menuIndex - 1, 0);

            this.oldKS = ks;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteFont font;
            Color colour;
            spritebatch.Begin();
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
