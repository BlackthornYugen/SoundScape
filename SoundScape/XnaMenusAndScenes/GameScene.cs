using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNALib.Scenes
{
    public abstract class GameScene : DrawableGameComponent
    {
        private GameComponentCollection _components;
        protected SpriteBatch _spritebatch;

        public GameComponentCollection Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }

        public GameScene(Game game, SpriteBatch spritebatch)
            : base(game)
        {
            _components = new GameComponentCollection();
            _spritebatch = spritebatch;
            Hide();
        }

        public override void Update(GameTime gameTime)
        {
            IGameComponent[] components = Components.ToArray();
            foreach (IGameComponent gameComponent in components)
            {
                var component = gameComponent as GameComponent;
                if (component != null && component.Enabled)
                    component.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            IGameComponent[] components = Components.ToArray();
            foreach (IGameComponent gameComponent in components)
            {
                var drawableGameComponent = gameComponent as DrawableGameComponent;
                if (drawableGameComponent != null && drawableGameComponent.Visible)
                    drawableGameComponent.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
