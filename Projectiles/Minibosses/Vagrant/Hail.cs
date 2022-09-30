using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.NPCs.Bosses.Vagrant;
using Terraria.Audio;

namespace Regressus.Projectiles.Minibosses.Vagrant
{
    public class HailExplosion : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Ranged/AzazelP2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 65;
            Projectile.height = 65;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 26;
        }
        float AAA;
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item14);
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                for (int num899 = 0; num899 < 4; num899++)
                {
                    int num900 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num900].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                }
                for (int num901 = 0; num901 < 10; num901++)
                {
                    int num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, 0f, 0f, 200, default(Color), 2.7f);
                    Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                    Main.dust[num902].noGravity = true;
                    Dust dust2 = Main.dust[num902];
                    dust2.velocity *= 3f;
                    num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                    dust2 = Main.dust[num902];
                    dust2.velocity *= 2f;
                    Main.dust[num902].noGravity = true;
                    Main.dust[num902].fadeIn = 2.5f;
                }
                for (int num903 = 0; num903 < 5; num903++)
                {
                    int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 15, 0f, 0f, 0, default(Color), 2.7f);
                    Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                    Main.dust[num904].noGravity = true;
                    Dust dust2 = Main.dust[num904];
                    dust2.velocity *= 3f;
                }
                for (int num905 = 0; num905 < 10; num905++)
                {
                    int num906 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default(Color), 1.5f);
                    Main.dust[num906].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                    Main.dust[num906].noGravity = true;
                    Dust dust2 = Main.dust[num906];
                    dust2.velocity *= 3f;
                }
                Projectile.timeLeft = 25;
            }
            /*if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.Kill();
                }
            }
            Main.NewText(AAA);
            float progress = Utils.GetLerpValue(0, 25, Projectile.timeLeft);
            AAA = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);*/
        }
        public override bool PreDraw(ref Color lightColor) => false;
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = RegreUtils.GetTexture("Projectiles/Ranged/AzazelP2");
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.LightBlue * AAA, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 65, 65, 65), Color.White * AAA, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }*/
    }
    public class OrbitingHailP : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Minibosses/Vagrant/Hail1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 42;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 0;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
        }
        float balls;
        bool maybe;
        public override void OnSpawn(IEntitySource source)
        {
            maybe = Main.rand.NextBool();
            balls = Main.rand.NextFloat(1f, 3f);
        }
        Vector2 center;
        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * balls * (maybe ? -1 : 1);
            Projectile.ai[1] %= 2f * (float)Math.PI;
            if (++Projectile.ai[0] < 100)
                center = p.Center;

            Projectile.Center = center + Projectile.timeLeft * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            Projectile.rotation += MathHelper.ToRadians(5);
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
    public class Hail1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 42;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 0;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail1_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.velocity *= 1.01f;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
    public class Hail2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail2").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail2_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 0;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            if (Projectile.aiStyle != 2 && Projectile.friendly)
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            if (Projectile.aiStyle != 1)
                Projectile.velocity *= 1.01f;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
    public class Hail3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hail");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            if (Projectile.aiStyle != 2 && Projectile.friendly)
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 15, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail3").Value;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Minibosses/Vagrant/Hail3_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.LightBlue * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 0;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            if (Projectile.aiStyle != 1)
                Projectile.velocity *= 1.01f;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
}
