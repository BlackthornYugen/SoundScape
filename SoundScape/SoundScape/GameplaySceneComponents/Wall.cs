using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape.GameplaySceneComponents
{
    class Wall : GameplaySceneComponent
    {
        private const int SCORE = -25;
        public Wall(GameplayScene scene, SpriteBatch spriteBatch, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox)
            : this(scene, spriteBatch, texture, soundEffect, hitbox, Color.DarkSalmon)
        {
        }

        public Wall(GameplayScene scene, SpriteBatch spriteBatch, Texture2D texture,
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(scene, spriteBatch, new Vector2(hitbox.X, hitbox.Y), texture, soundEffect, colour)
        {
            Hitbox = hitbox;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Texture, destinationRectangle: Hitbox, sourceRectangle: null, color: Colour);
            SpriteBatch.End();
        }

        public override void Kill(Color? killedByColour = null, bool alterScore = true)
        {
            int scoreChange = alterScore ? Score : 0;
            Scene.Score += scoreChange;
            Console.WriteLine("\n--- Walls don't die! ---\nScene Score = {0} ({1}{2})",
                Scene.Score,
                scoreChange > 0 ? "+" : "",
                scoreChange);
        }

        public override int Score
        {
            get { return SCORE; }
        }
    }
}
