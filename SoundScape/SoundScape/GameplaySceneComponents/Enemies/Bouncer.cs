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
    class Bouncer : Enemy
    {
        public Bouncer(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect)
            : base(scene, spriteBatch, position, texture, soundEffect)
        {
        }

        public Bouncer(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, colour)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var eastCollisionRect = new Rectangle(
                x: (int)(Position.X + Hitbox.Width),
                y: (int)Position.Y,
                width: (int)Speed.X,
                height: Hitbox.Height);
            var westCollisionRect = new Rectangle(
                x: (int)(Position.X - Speed.X),
                y: (int)Position.Y,
                width: (int)Speed.X,
                height: Hitbox.Height);
            var northCollisionRect = new Rectangle(
                x: (int)Position.X,
                y: (int)(Position.Y - Speed.Y),
                width: (int)Speed.X,
                height: Hitbox.Height);
            var southCollisionRect = new Rectangle(
                x: (int)Position.X,
                y: (int)Position.Y,
                width: Hitbox.X,
                height: (int)Speed.Y);

            foreach (var gameComponent in this.Scene.Components)
            {
                if (gameComponent is GameplaySceneComponent && gameComponent is Player) // Bouncer set only to collide with player for debugging
                { 
                    var component = (GameplaySceneComponent)gameComponent;
                    if (component != this)
                    {
                        bool collissionEastWest =
                            component.Hitbox.Intersects(eastCollisionRect) ||
                            component.Hitbox.Intersects(westCollisionRect);
                        bool collissionNorthSouth =
                            component.Hitbox.Intersects(northCollisionRect) ||
                            component.Hitbox.Intersects(southCollisionRect);

                        if (collissionEastWest && collissionNorthSouth)
                            Speed *= -Vector2.One;
                        else if (collissionEastWest)
                            Speed *= new Vector2(-1, 1);
                        else if (collissionNorthSouth)
                            Speed *= new Vector2(1, -1);

                        if (collissionEastWest || collissionNorthSouth)
                            break;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
