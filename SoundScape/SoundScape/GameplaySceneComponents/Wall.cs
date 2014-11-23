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
    class Wall : GameplaySceneComponent
    {
        public Wall(GameplayScene scene, SpriteBatch spriteBatch, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox)
            : this(scene, spriteBatch, texture, soundEffect, hitbox, Color.White)
        {
        }

        public Wall(GameplayScene scene, SpriteBatch spriteBatch, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(scene, spriteBatch, new Vector2(hitbox.X, hitbox.Y), texture, soundEffect, colour)
        {
            Hitbox = hitbox;
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, Hitbox, null, Color.White);
            SpriteBatch.End();
        }
    }
}
