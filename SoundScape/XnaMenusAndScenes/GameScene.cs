using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Scenes
{
    public abstract class GameScene : DrawableGameComponent
    {
        protected Texture2D _background;
        protected SpriteBatch _spritebatch;
        public bool AllowExit { get; protected set; }

        public GameComponentCollection Components { get; set; }

        public Texture2D Background
        {
            set { _background = value; }
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
            Components = new GameComponentCollection();
            _spritebatch = spritebatch;
            AllowExit = true;
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
            if (_background != null)
            {
                _spritebatch.Begin();
                _spritebatch.Draw(_background, Vector2.Zero, Color.White);
                _spritebatch.End();
            }

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
