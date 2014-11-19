using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleEmiter : Microsoft.Xna.Framework.GameComponent
    {
        Game game;
        SpriteBatch spriteBatch;
        Vector2 originEmision;

        public ParticleEmiter(Game game, SpriteBatch spriteBatch, Vector2 originEmision)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.originEmision = originEmision;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        List<Particle> parts = new List<Particle>();

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);

            parts.Add(new Particle(game, spriteBatch, originEmision));
            game.Components.Add(parts[parts.Count - 1]);

            CheckParticlesOnScreen();
        }

        void CheckParticlesOnScreen()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if(parts[i].DestroyMe)
                {

                }
            }
        }
    }
}
