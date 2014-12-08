using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    class Level2 : GameplayScene
    {
        public Level2(GameLoop game, SpriteBatch sb, bool spectatorMode)
            : base(game, sb, spectatorMode)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var r = new Random();
            var cb = Game.Window.ClientBounds;

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

            Components.Add(new Player(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions.Dequeue(),
                texture: Textures[Entity.PlayerTwo],
                soundEffect: SFX[Entity.PlayerTwo],
                pan: 1f,
                weaponSoundEffect: SFX[Entity.Sonar],
                colour: Color.Blue)
            {
                Controller = Game.PlayerTwo,
                SonarTexture = Textures[Entity.Sonar],
            });

            // North Wall
            Components.Add(new Wall(
                scene: this,
                spriteBatch: _spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(0, -WallThickness, cb.Width, 100)));

            // West Wall
            Components.Add(new Wall(
                scene: this,
                spriteBatch: _spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(-WallThickness, 0, WallThickness, cb.Height)));

            // East Wall
            Components.Add(new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width, 0, WallThickness, cb.Height)));

            // South Wall
            Components.Add(new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(0, cb.Height, cb.Width, WallThickness)));

            // Middle Wall
            Components.Add(new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width / 2 - WallThickness * 2, cb.Height / 2 - WallThickness * 2 , WallThickness * 4, WallThickness * 4)));

            Components.Add(new Bouncer(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions.Dequeue(),
                texture: Textures[Entity.EnemyBouncer],
                soundEffect: SFX[Entity.EnemyBouncer],
                colour: Color.Green)
            {
                Speed = new Vector2(
                    r.Next(2) == 0 ? -1 : 1,
                    r.Next(2) == 0 ? -1 : 1
                    )
            });

            Components.Add(new Bouncer(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions.Dequeue(),
                texture: Textures[Entity.EnemyBouncer],
                soundEffect: SFX[Entity.EnemyBouncer],
                colour: Color.Green)
            {
                Speed = new Vector2(
                    r.Next(2) == 0 ? -1 : 1,
                    r.Next(2) == 0 ? -1 : 1
                    )
            });
        }
    }
}
