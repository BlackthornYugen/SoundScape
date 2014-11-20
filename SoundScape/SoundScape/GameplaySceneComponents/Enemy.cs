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
    class Enemy : GameplaySceneComponent
    {
        private Vector2 speed;
        
        public Enemy(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox)
        {
        }

        public Enemy(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox, colour)
        {
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public override void Update(GameTime gameTime)
        {
            Position += Speed;
            base.Update(gameTime);
        }
    }
}
