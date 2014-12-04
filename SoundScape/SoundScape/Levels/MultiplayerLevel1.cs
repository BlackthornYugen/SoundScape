using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    class MultiplayerLevel1 : GameplayScene
    {
        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        public MultiplayerLevel1(GameLoop game, SpriteBatch sb)
            : base(game, sb)
        {
            var dt = new LevelDataTransfer(){
                Entities = new List<LevelDataTransfer.LevelEntity>()
                {
                    new LevelDataTransfer.LevelEntity {Speed = Vector2.One*2, Type = typeof (Bouncer), Colour = Color.Green},
                    new LevelDataTransfer.LevelEntity {Speed = Vector2.One*3, Type = typeof (Bouncer)},
                    new LevelDataTransfer.LevelEntity {Speed = Vector2.One*3, Type = typeof (Circler)},
                    new LevelDataTransfer.LevelEntity {Speed = Vector2.One*3, Type = typeof (Player)},
                    new LevelDataTransfer.LevelEntity {Speed = Vector2.One*4, Type = typeof (Player)},
                },
                StartPositions = new List<LevelDataTransfer.LevelStartPosition>()
                {
                    new LevelDataTransfer.LevelStartPosition()
                        {Anchor = LevelDataTransfer.Anchor.North | LevelDataTransfer.Anchor.West},
                    new LevelDataTransfer.LevelStartPosition()
                        {Anchor = LevelDataTransfer.Anchor.North | LevelDataTransfer.Anchor.East},
                    new LevelDataTransfer.LevelStartPosition()
                        {Anchor = LevelDataTransfer.Anchor.South | LevelDataTransfer.Anchor.West},
                    new LevelDataTransfer.LevelStartPosition()
                        {Anchor = LevelDataTransfer.Anchor.South | LevelDataTransfer.Anchor.East},
                }
            };

            var jsonString = JsonConvert.SerializeObject(dt);
            LevelDataTransfer restore = JsonConvert.DeserializeObject<LevelDataTransfer>(jsonString);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var cb = Game.Window.ClientBounds;

            int startIndex;
            var pWidth = Textures[Entity.PlayerOne].Width;
            var pHeight = Textures[Entity.PlayerOne].Height;

            var startingPositions = new List<Vector2>()
            {
                new Vector2(
                    x: pWidth, 
                    y: pHeight), 
                new Vector2(
                    x: cb.Width - pWidth, 
                    y: pHeight), 
                new Vector2(
                    x: cb.Width-pWidth, 
                    y: cb.Height-pHeight), 
                new Vector2(
                    x: pWidth, 
                    y: cb.Height-pHeight),
            };

            var r = new Random();

            #region Players
            startIndex = r.Next(startingPositions.Count);
            Components.Add(new Player(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.PlayerOne],
                soundEffect: SFX[Entity.PlayerOne],
                pan: 1f,
                weaponSoundEffect: SFX[Entity.Item],
                colour: Color.Red)
            {
                ControllerIndex = PlayerIndex.One
            });

            startingPositions.RemoveAt(startIndex);
            startIndex = r.Next(startingPositions.Count);
            Components.Add(new Player(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.PlayerTwo],
                soundEffect: SFX[Entity.PlayerTwo],
                pan: -1f,
                weaponSoundEffect: SFX[Entity.Item],
                colour: Color.Blue)
            {
                ControllerIndex = PlayerIndex.Two
            });
            startingPositions.RemoveAt(startIndex);
            #endregion

            #region Enemies
            startIndex = r.Next(startingPositions.Count);
            Components.Add(new Circler(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.EnemyCircler],
                soundEffect: SFX[Entity.EnemyCircler],
                colour: Color.Green)
            {
                Speed = Vector2.UnitX * (r.Next(2) == 0 ? -1 : 1) +
                        Vector2.UnitY * (r.Next(2) == 0 ? -1 : 1)
            });
            startingPositions.RemoveAt(startIndex);

            startIndex = r.Next(startingPositions.Count);
            Components.Add(new Bouncer(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.EnemyBouncer],
                soundEffect: SFX[Entity.EnemyBouncer],
                colour: Color.Green)
            {
                Speed = Vector2.UnitX * (r.Next(2) == 0 ? -1 : 1) +
                        Vector2.UnitY * (r.Next(2) == 0 ? -1 : 1)
            });
            startingPositions.RemoveAt(startIndex);
            #endregion

            #region Walls
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
            #endregion
        }
    }
}
