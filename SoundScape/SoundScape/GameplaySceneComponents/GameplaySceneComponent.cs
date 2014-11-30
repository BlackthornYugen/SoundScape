using System;
using System.Linq;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape.GameplaySceneComponents
{
    class GameplaySceneComponent : DrawableGameComponent
    {
        private GameplayScene _scene;
        private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private Texture2D _texture;
        private SoundEffect _soundEffect;
        private Rectangle _hitbox;
        private Color _colour;


        public virtual int Score
        {
            get { return 0; }
        }

        public virtual void Kill()
        {
            int scoreChange = Score;
            string itemName = ToString().Split('.').Reverse().First();
            var p = this as Player;
            if (p != null)
            {
                itemName += " " + p.ControllerIndex;
            }
            else if (this is Enemy)
            {
                itemName += " Enemy";
            }

            Game.Speak(String.Format("{0} has been destroyed.", itemName));

            Visible = false;
            Enabled = false;

            Scene.Score += scoreChange;
            Console.WriteLine("\n--- {0} Died! ---\nScene Score = {1} ({2}{3})",
                itemName,
                Scene.Score,
                scoreChange > 0 ? "+" : "",
                scoreChange);
        }

        protected GameplaySceneComponent(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound)
            : base(scene.Game)
        {
            Scene = scene;
            _spriteBatch = spriteBatch;
            _position = position;
            _texture = texture;
            _soundEffect = sound;
            _hitbox = new Rectangle((int)_position.X - texture.Width/2, (int)_position.Y - texture.Width/2, texture.Width, texture.Height);
            _colour = Color.White;
        }

        protected GameplaySceneComponent(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound,
            Color colour) : this(scene, spriteBatch, position, texture, sound)
        {
            _colour = colour;
        }

        protected GameplayScene Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        protected SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _hitbox.X = (int)value.X - (_texture.Width / 2);
                _hitbox.Y = (int)value.Y - (_texture.Height / 2);
                _position = value;
            }
        }

        protected Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public SoundEffect SoundEffect
        {
            get { return _soundEffect; }
            set { _soundEffect = value; }
        }

        public Rectangle Hitbox
        {
            get { return _hitbox; }
            set { _hitbox = value; }
        }

        protected Color Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_texture, Position - new Vector2(Texture.Width / 2f, Texture.Height / 2f), Colour);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
