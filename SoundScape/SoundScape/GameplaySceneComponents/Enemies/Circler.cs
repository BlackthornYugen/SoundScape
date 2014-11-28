using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape.GameplaySceneComponents.Enemies
{
    class Circler : Enemy
    {
        private const int SCORE_ADJUSTMENT = 50;
        private const int SPEED_FACTOR = 100;
        private const int FULL_CIRCLE_FRAMES = 500;
        private float _angle = 0;
        private float _offsetX;
        private float _offsetY;

        public Circler(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect)
            : this(scene, spriteBatch, position, texture, soundEffect, Color.White)
        {
        }

        public Circler(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, colour)
        {
            this._offsetX = position.X;
            this._offsetY = position.Y;
        }

        public override void Update(GameTime gameTime)
        {
            float increase = (float)Math.PI * 2 / FULL_CIRCLE_FRAMES;
            float x = (float)(Speed.X * SPEED_FACTOR * Math.Cos(_angle) + _offsetX);
            float y = (float)(Speed.Y * SPEED_FACTOR * Math.Sin(_angle) + _offsetY);
            
            _angle += increase;

            base.Update(gameTime);
            Position = new Vector2(x, y);
        }

        public override int Score
        {
            // SCORE_ADJUSTMENT more points than the base
            get { return base.Score + SCORE_ADJUSTMENT; }
        }
    }
}
