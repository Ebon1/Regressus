

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Regressus
{
    public delegate void UpdateFunction(Particle particle);
    public delegate void UpdateFunction2(Particle2 particle);
    public delegate void DrawFunction(Particle particle, DrawableTooltipLine line, SpriteBatch spriteBatch);
    public delegate void DrawFunction2(Particle2 particle, SpriteBatch spriteBatch, Vector2 position);
    public delegate void InitializeFunction(Particle particle);
    public delegate void InitializeFunction2(Particle2 particle);
    public class Particle
    {
        public ParticleSystem parent;
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Color color = Color.White;
        public float alpha = 1f;
        public float scale = 1f;
        public float rotation = 0f;
        public Texture2D[] textures;
        public float[] ai;
        public bool dead;
        public UpdateFunction Update;
        public DrawFunction Draw;
    }
    public class Particle2
    {
        public ParticleSystem2 parent;
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public Color color = Color.White;
        public float alpha = 1f;
        public float scale = 1f;
        public float rotation = 0f;
        public Texture2D[] textures;
        public float[] ai;
        public bool dead;
        public UpdateFunction2 Update;
        public DrawFunction2 Draw;
    }
    public class ParticleSystem
    {
        readonly List<Particle> particles;
        public ParticleSystem()
        {
            particles = new();
        }

        public static void DefaultDrawHook(Particle part, DrawableTooltipLine line, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(part.textures[0], new Vector2(line.X, line.Y) + part.position, null, part.color * part.alpha, 0f,
                part.textures[0].Size() / 2, part.scale * 0.5f, SpriteEffects.None, 0f);
        }
        public void UpdateParticles()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle part = particles[i];
                part.Update(part);
                if (part.dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void DrawParticles(DrawableTooltipLine line)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(particle, line, Main.spriteBatch);
            }
        }
        public void CreateParticle(UpdateFunction update, Texture2D[] textures, DrawFunction draw, InitializeFunction init = default)
        {
            Particle particle = new Particle
            {
                Update = update,
                Draw = draw,
                textures = textures,
                ai = new float[4],
                parent = this
            };
            init?.Invoke(particle);
            particles.Add(particle);
        }
    }
    public class ParticleSystem2
    {
        readonly List<Particle2> particles;
        public ParticleSystem2()
        {
            particles = new();
        }

        public static void DefaultDrawHook(Particle2 part, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(part.textures[0], part.position, null, part.color * part.alpha, 0f,
                part.textures[0].Size() / 2, part.scale * 0.5f, SpriteEffects.None, 0f);
        }
        public void UpdateParticles()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle2 part = particles[i];
                part.Update(part);
                part.position += part.velocity;
                if (part.dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void DrawParticles()
        {
            foreach (Particle2 particle in particles)
            {
                particle.Draw(particle, Main.spriteBatch, particle.position);
            }
        }
        public void CreateParticle(UpdateFunction2 update, Texture2D[] textures, DrawFunction2 draw, Vector2 _velocity, InitializeFunction2 init = default)
        {
            Particle2 particle = new Particle2
            {
                Update = update,
                Draw = draw,
                textures = textures,
                ai = new float[4],
                parent = this,
                velocity = _velocity
            };
            init?.Invoke(particle);
            particles.Add(particle);
        }
    }
}
