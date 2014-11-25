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
            Position += Speed;
            int xSpeed = (int)Math.Abs(Speed.X * 3);
            int ySpeed = (int)Math.Abs(Speed.X * 3);

            var eastCollisionRect = new Rectangle(
                x: (int)(Hitbox.X + Hitbox.Width),
                y: (int)Hitbox.Y,
                width: xSpeed,
                height: Hitbox.Height);
            var westCollisionRect = new Rectangle(
                x: (int)(Hitbox.X - xSpeed),
                y: (int)Hitbox.Y,
                width: xSpeed,
                height: Hitbox.Height);
            var northCollisionRect = new Rectangle(
                x: (int)Hitbox.X,
                y: (int)(Hitbox.Y - ySpeed),
                width: Hitbox.Width,
                height: ySpeed);
            var southCollisionRect = new Rectangle(
                x: (int)Hitbox.X,
                y: (int)Hitbox.Y,
                width: Hitbox.Width,
                height: (int)(Hitbox.Height + ySpeed));

            foreach (var gameComponent in this.Scene.Components)
            {
                if (gameComponent is GameplaySceneComponent && gameComponent is Wall)
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
                        {   // On any collision event, play a bounce 
                            // sound then end the loop.
                            Scene.PlayBounceSound(SoundEffect, Position);
                            break;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
