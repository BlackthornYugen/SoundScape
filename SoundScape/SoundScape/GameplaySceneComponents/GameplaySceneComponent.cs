using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape.GameplaySceneComponents
{
    class GameplaySceneComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SoundEffect soundEffect;
        private Texture2D texture;
        private Color colour;
        private Vector2 position;
        private Rectangle hitbox;

        protected GameplaySceneComponent(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound, Rectangle hitbox) : base(game)
        {
            SpriteBatch = spriteBatch;
            Position = position;
            Texture = texture;
            SoundEffect = sound;
            Hitbox = hitbox;
            Colour = Color.White;
        }

        protected GameplaySceneComponent(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, SoundEffect sound, Rectangle hitbox,
            Color colour) : this(game, spriteBatch, position, texture, sound, hitbox)
        {
            Colour = colour;
        }

        protected SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        protected SoundEffect SoundEffect
        {
            get { return soundEffect; }
            set { soundEffect = value; }
        }

        protected Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected Color Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        protected Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(texture, Position, Colour);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
