using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;

namespace SoundScape.Levels
{
    class SinglePlayerLevel2 : GameplayScene
    {
        public SinglePlayerLevel2(Game game, SpriteBatch sb)
            : base(game, sb)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var r = new Random();
            var cb = Game.Window.ClientBounds;

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

            int startIndex = r.Next(startingPositions.Count);
            player = new Player(this, spritebatch, startingPositions[startIndex], Textures[Entity.Player], SFX[Entity.Player], 0);
            startingPositions.RemoveAt(startIndex);
            Components.Add(player);

            startIndex = r.Next(startingPositions.Count);
            Enemy enemy = new Bouncer(
                scene: this,
                spriteBatch: spritebatch,
                position: startingPositions[startIndex],
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
                colour: Color.Green);
            startingPositions.RemoveAt(startIndex);
            enemy.Speed = Vector2.Zero
                + Vector2.UnitX * r.Next(1, 5) * (r.Next(2) == 0 ? -1 : 1)
                + Vector2.UnitY * r.Next(1, 5) * (r.Next(2) == 0 ? -1 : 1);
            Components.Add(enemy);

            Wall wall;

            // North Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(0, -WallThickness, cb.Width, 100));
            Components.Add(wall);

            // West Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(-WallThickness, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // East Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // South Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(0, cb.Height, cb.Width, WallThickness));
            Components.Add(wall);
        }
    }
}
