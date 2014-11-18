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


namespace XNALib.Scenes
{
    public abstract class GameScene : DrawableGameComponent
    {
        private GameComponentCollection compontents;
        protected SpriteBatch spritebatch;

        protected GameComponentCollection Compontents
        {
            get { return compontents; }
            set { compontents = value; }
        }

        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public GameScene(Game game, SpriteBatch spritebatch)
            : base(game)
        {
            this.compontents = new GameComponentCollection();
            Hide();
            this.spritebatch = spritebatch;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (GameComponent item in compontents)
            {
                if (item.Enabled)
                    item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawableGC;
            foreach (GameComponent item in compontents)
            {
                if (item is DrawableGameComponent)
                {
                    drawableGC = item as DrawableGameComponent;
                    if (drawableGC.Visible)
                        drawableGC.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
    }
}
