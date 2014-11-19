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

        protected GameplaySceneComponent(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound, Rectangle hitbox)
            : base(scene.Game)
        {
            Scene = scene;
            _spriteBatch = spriteBatch;
            _position = position;
            _texture = texture;
            _soundEffect = sound;
            _hitbox = hitbox;
            _colour = Color.White;
        }

        protected GameplaySceneComponent(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound, Rectangle hitbox,
            Color colour) : this(scene, spriteBatch, position, texture, sound, hitbox)
        {
            _colour = colour;
        }

        protected GameplayScene Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        protected SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        protected Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        protected SoundEffect SoundEffect
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
            SpriteBatch.Draw(_texture, Position, Colour);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
