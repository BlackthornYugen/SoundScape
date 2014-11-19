using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundScape.GameplaySceneComponents
{
    class Wall : GameplaySceneComponent
    {
        public Wall(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox)
            : base(game, spriteBatch, position, texture, soundEffect, hitbox)
        {
        }

        public Wall(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(game, spriteBatch, position, texture, soundEffect, hitbox, colour)
        {
        }
    }
}
