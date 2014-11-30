using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    class MultiplayerLevel1 : GameplayScene
    {
        public MultiplayerLevel1(GameLoop game, SpriteBatch sb)
            : base(game, sb)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var cb = Game.Window.ClientBounds;
            var colours = new Color[]
            {
                Color.Red,
                Color.Blue,
            };

            int startIndex;
            var pWidth = Textures[Entity.Player].Width;
            var pHeight = Textures[Entity.Player].Height;

            #region Players
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

            for (int i = 0; i < colours.Length; i++)
            {
                startIndex = r.Next(startingPositions.Count);
                Components.Add(new Player(
                    scene: this, 
                    spriteBatch: _spritebatch, 
                    position: startingPositions[startIndex], 
                    texture: Textures[Entity.Player], 
                    soundEffect: SFX[Entity.Player], 
                    pan: i % 2 == 0 ? 1f : -1f, 
                    weaponSoundEffect: SFX[Entity.Item], 
                    colour: colours[i % colours.Length])
                {
                    ControllerIndex = (PlayerIndex)i
                });
                startingPositions.RemoveAt(startIndex);
            }
            #endregion

            #region Enemies
            startIndex = r.Next(startingPositions.Count);
            Components.Add(new Circler(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
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
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
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
