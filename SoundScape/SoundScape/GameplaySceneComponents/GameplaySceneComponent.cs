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
        private Color _originalColour;
        private Color _colour;
        private int _scoreAtDeath;
        private Color _deathColour;
        private float _highlightScore;


        public virtual int Score
        {
            get { return 0; }
        }

        public virtual void Kill(Color? killedByColour = null)
        {
            _scoreAtDeath = Score;
            _deathColour = killedByColour ?? Color.Black;
            string itemName = ToString().Split('.').Reverse().First();
            var p = this as Player;
            if (p != null)
            {
                itemName += " " + p.Controller.PlayerIndex;
            }
            else if (this is Enemy)
            {
                itemName += " Enemy";
            }

            Game.Speak(String.Format("{0} has been destroyed.", itemName));
            // Dead things stay visible but are turned gray
            Colour = Color.Gray; 
            Enabled = false;
            Scene.Score += _scoreAtDeath;
            Console.WriteLine("\n--- {0} Died! ---\nScene Score = {1} ({2}{3})",
                itemName,
                Scene.Score,
                _scoreAtDeath > 0 ? "+" : "",
                _scoreAtDeath);
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
            _originalColour = _colour = Color.White;
        }

        protected GameplaySceneComponent(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound,
            Color colour) : this(scene, spriteBatch, position, texture, sound)
        {
            _originalColour = _colour = colour;
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

        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public void ResetColour()
        {
            _colour = _originalColour;
        }

        public override void Draw(GameTime gameTime)
        {
            var font = Game.BigFont;
            Vector2 fontAdjustment = font.MeasureString(_scoreAtDeath.ToString()) / 2;
            string message = null;
            if (_scoreAtDeath > 0) message = _scoreAtDeath.ToString();
            if (_scoreAtDeath < 0) message = String.Format("-{0}", Math.Abs((int)_scoreAtDeath));

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            SpriteBatch.Draw(_texture, Position - new Vector2(Texture.Width / 2f, Texture.Height / 2f), Colour);

            if(!Enabled && _highlightScore < 7)
                _highlightScore = (_highlightScore + 0.04f) % 10;

            if (message != null && _highlightScore < 7)
            {
                float f = 0.01f;
                for (; f < 1; f += 0.02f)
                {
                    Color drawColour;
                    if (Math.Abs(f - _highlightScore) < 0.3)
                        drawColour = _deathColour;
                    else 
                        drawColour = Colour;
                    SpriteBatch.DrawString(font, message, Position, drawColour, 0, fontAdjustment, f, SpriteEffects.None, 0);
                    if (f > _highlightScore) break;
                }
                SpriteBatch.DrawString(font, message, Position, _deathColour, 0, fontAdjustment, f, SpriteEffects.None, 0);
            } 
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
