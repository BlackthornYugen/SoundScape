using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;

namespace SoundScape.Levels
{
    class LevelDebug : GameplayScene
    {
        public LevelDebug(GameLoop game, SpriteBatch sb)
            : base(game, sb)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var cb = Game.Window.ClientBounds;
            var r = new Random();

            var pWidth = Textures[Entity.PlayerOne].Width;
            var pHeight = Textures[Entity.PlayerOne].Height;

            var startingPositions = new Queue<Vector2>((new[]
            {
                new Vector2(
                    x: pWidth,
                    y: pHeight),
                new Vector2(
                    x: cb.Width - pWidth,
                    y: pHeight),
                new Vector2(
                    x: cb.Width - pWidth,
                    y: cb.Height - pHeight),
                new Vector2(
                    x: pWidth,
                    y: cb.Height - pHeight),
            }).OrderBy((a) => r.Next()));


            Components.Add(new Player(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions.Dequeue(),
                texture: Textures[Entity.PlayerOne],
                soundEffect: SFX[Entity.PlayerOne],
                pan: -1f,
                weaponSoundEffect: SFX[Entity.Sonar],
                colour: Color.Red)
            {
                Controller = Game.PlayerOne,
                SonarTexture = Textures[Entity.Sonar],
            });
        }
    }
}
