using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNALib.Scenes
{
    public abstract class GameScene : DrawableGameComponent
    {
        private GameComponentCollection _components;
        protected SpriteBatch spritebatch;

        public GameComponentCollection Components
        {
            get { return _components; }
            set { _components = value; }
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
            this.Components = new GameComponentCollection();
            Hide();
            this.spritebatch = spritebatch;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var components = Components.ToArray();
            foreach (GameComponent item in components)
            {
                if (item.Enabled)
                    item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var components = Components.ToArray();
            DrawableGameComponent drawableGC;
            foreach (GameComponent item in components)
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
