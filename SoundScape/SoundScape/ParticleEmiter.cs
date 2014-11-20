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
    public class ParticleEmiter : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private GameComponentCollection componentsParticles;

        List<Particle> parts = new List<Particle>();
        int counter;
        List<Texture2D> particlesTex;

        GameScene gameScene;
        SpriteBatch spriteBatch;
        Vector2 originEmision;
        int interval;
        bool create;
        bool isEmitting = true;
        float scale;

        public ParticleEmiter(GameScene gameScene, SpriteBatch spriteBatch, List<Texture2D> particlesTex, 
            Vector2 originEmision, int interval, float scale)
            : base(gameScene.Game)
        {
            componentsParticles = new GameComponentCollection();
            this.particlesTex = particlesTex;
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

            foreach (GameComponent item in componentsParticles)
            {
                if (item.Enabled)
                    item.Update(gameTime);
            }

            // TODO: Add your update code here

            base.Update(gameTime);

            if (create && isEmitting)
            {
                parts.Add(new Particle(gameScene, spriteBatch, originEmision, particlesTex,
                    speed: new Vector2(1, 0), iniScale: scale));
                componentsParticles.Add(parts[parts.Count - 1]);
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
                    parts.RemoveAt(i);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawableGC;
            foreach (GameComponent item in componentsParticles)
            {
                if (item is DrawableGameComponent)
                {
                    drawableGC = item as DrawableGameComponent;
                    if (drawableGC.Visible)
                        drawableGC.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        public virtual void Show()
        {
            isEmitting = true;
            this.Enabled = true;
            this.Visible = true;
        }

        public virtual void Hide()
        {
            parts.Clear();
            isEmitting = false;
            this.Enabled = false;
            this.Visible = false;
        }
    }
}
