using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Regressus.Particles
{
    // This is the file where all the particles are stored currently
    public class Spark : CustomParticle
    {
        public override string Texture => "Regressus/Extras/Circle";
        private const float maxTime = 100;
        public Vector2 oldVelocity = Vector2.Zero;
        public override void OnSpawn()
        {
            /*float ff = MathHelper.ToRadians(30);
            particle.velocity = particle.velocity.RotatedBy(Main.rand.NextFloat(-ff, ff));*/
        }
        public override void Update()
        {
            particle.rotation = particle.velocity.ToRotation();
            particle.alpha = (maxTime - particle.activeTime) / maxTime;
            particle.scale = MathHelper.Lerp(0, particle.scale, particle.alpha);
            particle.velocity += Vector2.UnitY * 0.5f;
            if (particle.activeTime > maxTime)
            {
                particle.Kill();
            }

            if (Collision.TileCollision(particle.position, particle.velocity, 1, 1) != particle.velocity)
            {
                if (Math.Abs(particle.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    particle.velocity.X = -oldVelocity.X;
                }
                if (Math.Abs(particle.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    particle.velocity.Y = -oldVelocity.Y;
                }
            }
            oldVelocity = particle.velocity;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Particle.GetTexture(particle.type);
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha, particle.rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(particle.scale / 5, 0.01f), SpriteEffects.None, 0f);
        }
    }
    public class Glow1 : CustomParticle
    {
        public override string Texture => "Regressus/Extras/glow";
        public override void Update()
        {
            particle.velocity.X += Main.windSpeedCurrent;
            particle.velocity.Y -= 0.4f;
            float progress = Utils.GetLerpValue(0, particle.customData[0], particle.activeTime);
            if (progress > 0.8f)
                particle.alpha = (progress - 0.5f) * 2;
            if (particle.activeTime > particle.customData[0])
                particle.Kill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Particle.ParticleTextures[particle.type];
            //Texture2D texture2 = ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.StardustTowerMark);
            spriteBatch.Reload(BlendState.Additive);
            //spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha, 0f, texture.Size() / 2, particle.scale / 4, SpriteEffects.None, 0f);
            //spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha * 0.8f, 0f, texture.Size() / 2, particle.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha * 0.5f, 0f, texture.Size() / 2, particle.scale * 2, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha * 0.2f, 0f, texture.Size() / 2, particle.scale * 4, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha * 0.01f, 0f, texture.Size() / 2, particle.scale * 12, SpriteEffects.None, 0f);
            //spriteBatch.Draw(texture2, particle.position - Main.screenPosition, null, particle.color * particle.alpha * 0.5f, 0f, texture2.Size() / 2, particle.scale / 4, SpriteEffects.None, 0f);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
    }
    public class Glow2 : CustomParticle
    {
        public override string Texture => "Regressus/Extras/Empty";
        public float maxTime = 60f;
        public override void OnSpawn()
        {
            particle.customData[0] = particle.scale;
            particle.rotation += MathHelper.Pi / 2;
            particle.scale = 0f;
        }
        public override void Update()
        {
            // 0.05 == 20
            particle.velocity *= 0.98f;
            particle.alpha = Utils.GetLerpValue(0f, 0.05f, particle.activeTime / 60f, clamped: true) * Utils.GetLerpValue(1f, 0.9f, particle.activeTime / 60f, clamped: true);
            particle.scale = Utils.GetLerpValue(0f, 20f, particle.activeTime, clamped: true) * Utils.GetLerpValue(45f, 30f, particle.activeTime, clamped: true);
            if (particle.activeTime <= maxTime)
            {
                return;
            }

            particle.Kill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Main.Assets.Request<Texture2D>("Terraria/Images/Projectile_644").Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color col = Color.White * particle.alpha * 0.9f;
            col.A /= 2;
            Color col1 = particle.color * particle.alpha * 0.5f; // used
            col1.A = 0;
            Color col2 = col * 0.5f; // used
            col1 *= particle.scale;
            col2 *= particle.scale;
            Vector2 scale1 = new Vector2(0.3f, 2f) * particle.scale * particle.customData[0];
            Vector2 scale2 = new Vector2(0.3f, 1f) * particle.scale * particle.customData[0];
            Vector2 pos = particle.position - Main.screenPosition;
            SpriteEffects effects = SpriteEffects.None;
            spriteBatch.Draw(texture, pos, null, col1, (float)Math.PI / 2f + particle.rotation, origin, scale1, effects, 0f);
            spriteBatch.Draw(texture, pos, null, col1, particle.rotation, origin, scale2, effects, 0f);
            spriteBatch.Draw(texture, pos, null, col2, (float)Math.PI / 2f + particle.rotation, origin, scale1 * 0.6f, effects, 0f);
            spriteBatch.Draw(texture, pos, null, col2, particle.rotation, origin, scale2 * 0.6f, effects, 0f);
        }
    }
    public class Shockwave : CustomParticle
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustTowerMark;
        public float MaxSize { get => particle.customData[0]; set => particle.customData[0] = value; }
        private const float maxTime = 60f;
        public override void OnSpawn()
        {
            /*if (Main.rand.NextBool(3))
            {
                particle.customData[0] *= 2;
            }*/
            if (particle.customData[1] == 0)
            {
                particle.customData[1] = 1;
            }

            if (particle.customData[2] == 0)
            {
                particle.customData[2] = 1;
            }

            if (particle.customData[3] == 0)
            {
                particle.customData[3] = 1;
            }

            MaxSize = particle.scale;
            particle.scale = 0f;
        }
        public override void Update()
        {
            particle.velocity = Vector2.Zero;
            float progress = particle.activeTime / maxTime;
            particle.scale = MathHelper.Lerp(particle.scale, MaxSize, progress);
            particle.alpha = MathHelper.Lerp(particle.alpha, 0, progress);
            if (particle.activeTime <= maxTime)
            {
                return;
            }

            particle.Kill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Particle.ParticleTextures[particle.type];
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2;
            float baseScale = 512f / texture.Width;
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha, 0f, origin, baseScale * particle.scale, SpriteEffects.None, 0f);
        }
    }
    public class Shockwave2 : CustomParticle
    {
        public override string Texture => "Regressus/Extras/Perlin";
        public float MaxSize { get => particle.customData[0]; set => particle.customData[0] = value; }
        private const float maxTime = 60f;
        public override void OnSpawn()
        {
            if (particle.customData[1] == 0)
            {
                particle.customData[1] = 1;
            }

            if (particle.customData[2] == 0)
            {
                particle.customData[2] = 1;
            }

            if (particle.customData[3] == 0)
            {
                particle.customData[3] = 1;
            }

            MaxSize = particle.scale;
            particle.scale = 0f;
        }
        public override void Update()
        {
            particle.velocity = Vector2.Zero;
            float progress = particle.activeTime / maxTime;
            particle.scale = MathHelper.Lerp(particle.scale, MaxSize, progress);
            particle.alpha = MathHelper.Lerp(particle.alpha, 0, progress);
            Color col = new Color(particle.customData[1], particle.customData[2], particle.customData[3]);
            particle.color = Color.Lerp(particle.color, col, progress);
            if (particle.activeTime <= maxTime)
            {
                return;
            }

            particle.Kill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // 1.5 X scale cuz the vanilla shader halves X size
            Vector2 scale = new Vector2(particle.scale * 1.5f, particle.scale);
            // restart spritebatch
            spriteBatch.Reload(SpriteSortMode.Immediate);
            Texture2D texture = Particle.ParticleTextures[particle.type];
            // make a new drawdata(spritebatch draw but saved inside a class)
            DrawData data = new DrawData(texture,
                particle.position - Main.screenPosition,
                new Rectangle(0, 0, texture.Width, texture.Height),
                particle.color * particle.alpha,
                particle.rotation,
                new Vector2(texture.Width, texture.Height) / 2,
                scale,
                SpriteEffects.None,
            0);
            // vanilla effect used in pillar shield
            MiscShaderData effect = GameShaders.Misc["ForceField"];
            _ = effect.UseColor(particle.color);
            effect.Apply(data);
            // make it actually draw
            data.Draw(spriteBatch);
            // restart spritebatch again so effect doesnt continue to be applied
            spriteBatch.Reload(SpriteSortMode.Deferred);
        }
    }
    public class Flame : CustomParticle
    {
        public override string Texture => "Regressus/Extras/glow";

        private const float maxTime = 60f;
        public override void OnSpawn()
        {
            base.OnSpawn();
        }
        public override void Update()
        {
            particle.alpha = 1f - particle.activeTime / maxTime;
            if (particle.activeTime <= maxTime)
            {
                return;
            }

            particle.Kill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Particle.ParticleTextures[particle.type];
            spriteBatch.Reload(BlendState.Additive);
            spriteBatch.Draw(texture, particle.position - Main.screenPosition, null, particle.color * particle.alpha, particle.rotation, texture.Size() / 2, particle.scale, SpriteEffects.None, 0f);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
    }
}