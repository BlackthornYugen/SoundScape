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
using XNALib.Scenes;

namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleEmiter : Microsoft.Xna.Framework.GameComponent
    {

        List<Particle> parts = new List<Particle>();
        int counter;

        GameScene gameScene;
        SpriteBatch spriteBatch;
        Vector2 originEmision;
        int interval;
        bool create;
        float scale;

        public ParticleEmiter(GameScene gameScene, SpriteBatch spriteBatch, 
            Vector2 originEmision, int interval, float scale)
            : base(gameScene.Game)
        {
            this.gameScene = gameScene;
            this.spriteBatch = spriteBatch;
            this.originEmision = originEmision;
            this.interval = interval;
            this.scale = scale;
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);

            if (create)
            {
                parts.Add(new Particle(gameScene.Game, spriteBatch, originEmision,
                    speed: new Vector2(1, 0), iniScale: scale));
                gameScene.Game.Components.Add(parts[parts.Count - 1]);
                create = false;
            }
            else
            {
                counter++;
                if(interval == counter)
                {
                    create = true;
                    counter = 0;
                }
            }
            CheckParticlesOnScreen();
        }


        void CheckParticlesOnScreen()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if(parts[i].DestroyMe)
                {
                    for (int j = 0; j < gameScene.Game.Components.Count; j++)
                    {
                        if(parts[i] == gameScene.Game.Components[j])
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
