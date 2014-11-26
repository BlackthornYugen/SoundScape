using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;

namespace SoundScape.Levels
{
    class SinglePlayerLevel1 : GameplayScene
    {
        public SinglePlayerLevel1(Game game, SpriteBatch sb)
            : base(game, sb)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var cb = Game.Window.ClientBounds;
            Color[] colours = new Color[]
            {
                Color.Red
                //,Color.Blue
                //,Color.Green
                //,Color.Yellow
            };
            Player player;

            var r = new Random();
            var xLimit = cb.Width - Textures[Entity.Player].Width - WallThickness;
            var yLimit = cb.Height - Textures[Entity.Player].Height - WallThickness;
            for (int i = 0; i < colours.Length; i++)
            {

                int x = r.Next(xLimit) + WallThickness;
                int y = r.Next(yLimit) + WallThickness;
                player = new Player(this, spritebatch, new Vector2(x, y), Textures[Entity.Player], SFX[Entity.Player],
                    i % 2 == 0 ? 1f : -1f, SFX[Entity.Item], colours[i % colours.Length]);
                Components.Add(player);

                player.ControllerIndex = (PlayerIndex)i;
            }
            Wall wall;

            // North Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(0, 0, cb.Width, 100));
            Components.Add(wall);

            // West Wall
            wall = new Wall(
                scene: this,
                spriteBatch: spritebatch,
                texture: Textures[Entity.Wall],
                soundEffect: SFX[Entity.Wall],
                hitbox: new Rectangle(0, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // East Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(cb.Width - WallThickness, 0, WallThickness, cb.Height));
            Components.Add(wall);

            // South Wall
            wall = new Wall(
                 scene: this,
                 spriteBatch: spritebatch,
                 texture: Textures[Entity.Wall],
                 soundEffect: SFX[Entity.Wall],
                 hitbox: new Rectangle(0, cb.Height - WallThickness, cb.Width, WallThickness));
            Components.Add(wall);

            Enemy enemy = new Bouncer(
                scene: this,
                spriteBatch: spritebatch,
                position: Vector2.One * 250,
                texture: Textures[Entity.Enemy],
                soundEffect: SFX[Entity.Enemy],
                colour: Color.Green);
            enemy.Speed = Vector2.One;
            Components.Add(enemy);
        }
    }
}
