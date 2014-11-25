using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib.Scenes;

namespace SoundScape.GameplaySceneComponents
{
    class Circler : Enemy
    {
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
            float speed = 222; // TODO: Generate speed using Vector2 Speed
            float increase = (float) (Math.PI*2/speed);
            float x = (float)(200 * Math.Cos(_angle) + _offsetX); // TODO: Replace 200 with a formula that uses speed.x
            float y = (float)(200 * Math.Sin(_angle) + _offsetY); // TODO: Replace 200 with a formula that uses speed.y
            
            _angle += increase;

            base.Update(gameTime);
            Position = new Vector2(x, y);
        }
    }
}
