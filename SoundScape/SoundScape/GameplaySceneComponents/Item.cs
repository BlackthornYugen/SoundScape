﻿using System;
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
    class Item : GameplaySceneComponent
    {
        public Item(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox)
        {
        }

        public Item(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox, colour)
        {
        }
    }
}