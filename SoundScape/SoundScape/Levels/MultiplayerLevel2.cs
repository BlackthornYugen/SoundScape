using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    class MultiplayerLevel2 : GameplayScene
    {
        public MultiplayerLevel2(Game game, SpriteBatch sb)
            : base(game, sb)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var cb = Game.Window.ClientBounds;
            Color[] colours = new Color[]
            {
                Color.Red,
                Color.Blue,
            };

            Player player;

            var pWidth = Textures[Entity.Player].Width;
            var pHeight = Textures[Entity.Player].Height;
            List<Vector2> startingPositions = new List<Vector2>()
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

            int startIndex;
            for (int i = 0; i < colours.Length; i++)
            {
                startIndex = r.Next(startingPositions.Count);
                player = new Player(this, _spritebatch, startingPositions[startIndex], Textures[Entity.Player], SFX[Entity.Player],
                    i % 2 == 0 ? 1f : -1f, SFX[Entity.Item], colours[i % colours.Length]);
                startingPositions.RemoveAt(startIndex);
                Components.Add(player);

                player.ControllerIndex = (PlayerIndex)i;
            }
            Wall wall;

            // North Wall
            wall = new Wall(
                scene: this,
                spriteBatch: _spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(0, -WallThickness, cb.Width, 100));
            Components.Add(wall);

            // West Wall
            wall = new Wall(
                scene: this,
                spriteBatch: _spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(-WallThickness, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // East Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // South Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(0, cb.Height, cb.Width, WallThickness));
            Components.Add(wall);

            // Middle Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: _spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width / 2 - WallThickness * 2, cb.Height / 2 - WallThickness * 2 , WallThickness * 4, WallThickness * 4));
            Components.Add(wall);

            startIndex = r.Next(startingPositions.Count);
            Enemy enemy = new Bouncer(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
                colour: Color.Green);
            enemy.Speed =
                Vector2.UnitX * (r.Next(2) == 0 ? -1 : 1) +
                Vector2.UnitY * (r.Next(2) == 0 ? -1 : 1);
            startingPositions.RemoveAt(startIndex);
            Components.Add(enemy);

            startIndex = r.Next(startingPositions.Count);
            enemy = new Bouncer(
                scene: this,
                spriteBatch: _spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
                colour: Color.Green);
            enemy.Speed =
                Vector2.UnitX * (r.Next(2) == 0 ? -1 : 1) +
                Vector2.UnitY * (r.Next(2) == 0 ? -1 : 1);
            startingPositions.RemoveAt(startIndex);
            Components.Add(enemy);
        }
    }
}
