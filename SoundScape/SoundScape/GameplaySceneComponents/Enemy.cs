using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape.GameplaySceneComponents
{
    class Enemy : GameplaySceneComponent
    {
        private const int SCORE_REDUCTION = 3;
        private const int MAX_SCORE = 100;
        private const int MIN_SCORE = 50;
        private Vector2 _speed;
        
        public Enemy(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect)
            : base(scene, spriteBatch, position, texture, soundEffect)
        {
        }

        public Enemy(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, colour)
        {
        }

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public override int Score
        {
            // Score goes down by 3 every second. Minimum 50 points.
            get { return Math.Max(MAX_SCORE - Scene.RunningSeconds * SCORE_REDUCTION, MIN_SCORE); }
        }
    }
}
