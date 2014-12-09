using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.GameplaySceneComponents;
using SoundScape.GameplaySceneComponents.Enemies;

namespace SoundScape.Levels
{
    class LevelDebug : GameplayScene
    {
        public LevelDebug(GameLoop game, SpriteBatch sb, GameOptions options)
            : base(game, sb, options)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Vector2 middle = Vector2.UnitY * Game.Window.ClientBounds.Height / 2;
            middle += Vector2.UnitX * Game.Window.ClientBounds.Width / 2;
            Components.Add(new Player(
                scene: this,
                spriteBatch: _spritebatch,
                position: middle,
                texture: Textures[Entity.PlayerOne],
                soundEffect: SFX[Entity.PlayerOne],
                pan: 0,
                weaponSoundEffect: SFX[Entity.Sonar],
                colour: Color.Red)
            {
                Controller = Game.PlayerOne,
                SonarTexture = Textures[Entity.Sonar],
            });

            var r = new Random();
            var n = Enum.GetValues(typeof(Entity)).Length;

            Components.Add(new Bouncer(this, _spritebatch, middle + Vector2.UnitX * 300, Textures[(Entity)r.Next(n)], SFX[(Entity)r.Next(n)]));
            Components.Add(new Bouncer(this, _spritebatch, middle - Vector2.UnitX * 300, Textures[(Entity)r.Next(n)], SFX[(Entity)r.Next(n)]));
            Components.Add(new Bouncer(this, _spritebatch, middle + Vector2.UnitY * 300, Textures[(Entity)r.Next(n)], SFX[(Entity)r.Next(n)]));
            Components.Add(new Bouncer(this, _spritebatch, middle - Vector2.UnitY * 300, Textures[(Entity)r.Next(n)], SFX[(Entity)r.Next(n)]));
        }
    }
}
