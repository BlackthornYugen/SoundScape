﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    /// <summary>
    /// This level has two bouncing enemies that 
    /// start in a random corner and a circling 
    /// enemy that orbits the small center wall
    /// </summary>
    class Level3 : GameplayScene
    {
        public Level3(GameLoop game, SpriteBatch sb, GameOptions options)
            : base(game, sb, options)
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

            if (Options.HasFlag(GameOptions.Multiplayer))
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
                 hitbox: new Rectangle(cb.Width / 2 - WallThickness, cb.Height / 2 - WallThickness, WallThickness * 2, WallThickness * 2)));

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

            Components.Add(new Circler(
                scene: this,
                spriteBatch: _spritebatch,
                position: new Vector2(cb.Width / 2f, cb.Height / 2f), 
                texture: Textures[Entity.EnemyBouncer],
                soundEffect: SFX[Entity.EnemyBouncer],
                colour: Color.Green)
                {
                    Speed = new Vector2(
                        r.Next(2) == 0 ? -3 : 3,
                        r.Next(2) == 0 ? -3 : 3
                        )
                });
        }
    }
}
