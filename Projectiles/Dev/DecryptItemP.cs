using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using Regressus.Effects.Prims;
using Regressus.Projectiles.Melee;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Regressus.Projectiles.Dev
{
    public class DecryptItemP1 : HeldSword
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            SwingTime = 35;
            holdOffset = 42.5f;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }
        public override float Lerp(float x)
        {
            return (float)(x < 0.5 ? 16 * x * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 5) / 2);
        }
        int a;
        public override void ExtraAI()
        {
            if (++a >= 25)
            {
                a = 0;
                Vector2 vel = Main.MouseWorld - Projectile.Center;
                vel.Normalize();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel * 14f, ModContent.ProjectileType<DecryptItemPBall2>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/DecryptItemP1_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.45f) - mult * (holdOffset * 0.45f));
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.Blue * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, Color.Blue * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), Color.Blue, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
    public class DecryptItemP2 : HeldSword
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            SwingTime = 20;
            holdOffset = 42.5f;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }
        public override float Lerp(float x)
        {
            return (float)(x < 0.5 ? 16 * x * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 5) / 2);
        }
        int a;
        public override void ExtraAI()
        {
            if (++a >= 15)
            {
                a = 0;
                Vector2 vel = Main.MouseWorld - Projectile.Center;
                vel.Normalize();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel * 14f, ModContent.ProjectileType<DecryptItemPBall1>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/DecryptItemP2_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.45f) - mult * (holdOffset * 0.45f));
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.Red * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, Color.Red * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), Color.Red, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
    public class DecryptItemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 94;
            Projectile.height = 168;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A Dance of Fire and Ice");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override string Texture => "Regressus/Items/Dev/_DecryptItem";
        int lmao = 1;
        float a, b, c;
        public override void AI()
        {
            if (a < 55)
                a++;
            Projectile.scale = a / 55;
            c = b / (60);
            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[1] > 2)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DecryptCooldown>(), 60 * 30);
                Projectile.localAI[1] = 0;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.DecryptCooldown>()))
            {
                if (b < 60)
                    b++;
            }
            else
            {
                if (b > 0)
                    b--;
            }
            Projectile.rotation += MathHelper.ToRadians(10 * Projectile.ai[0]);
            if (Main.mouseLeft)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.hostile && p.type != ModContent.ProjectileType<Projectiles.Oracle.OracleOrbs>())
                {
                    if (p.Center.Distance(Projectile.Center) < Projectile.height)
                    {
                        if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.DecryptCooldown>()))
                        {
                            Projectile.localAI[1]++;
                            p.Kill();
                        }
                    }
                }
                else
                    continue;
            }
            AttachToPlayer();
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            Vector2 vector56 = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            float num186 = 0f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool ShouldUpdatePosition() => false;
        public void AttachToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.Center = pos;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Dev/DecryptItemP_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.White * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], SpriteEffects.None, 0f);
            }
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
    public class DecryptItemPBall1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Orb");
            Main.projFrames[Projectile.type] = 4;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 512, 512), Color.Red * 0.5f, Projectile.rotation, new Vector2(512, 512) / 2, 0.25f * Projectile.scale, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 64, 64, 64), Color.White, Projectile.rotation, new Vector2(64, 64) / 2, Projectile.scale, effects, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity *= 1.05f;
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
    public class DecryptItemPBall2 : ModProjectile
    {
        public override string Texture => "Regressus/Projectiles/Oracle/OracleEnergyOrb";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Orb");
            Main.projFrames[Projectile.type] = 4;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 512, 512), Color.Blue * 0.5f, Projectile.rotation, new Vector2(512, 512) / 2, 0.25f * Projectile.scale, effects, 0f);
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBlast_Glow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 35, 65, 35), Color.White, Projectile.rotation, new Vector2(65, 35) / 2, 1, effects, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 48, 46, 48), Color.White, Projectile.rotation, new Vector2(46, 46) / 2, Projectile.scale, effects, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity *= 1.05f;
            }
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            //Projectile.rotation = Projectile.velocity.ToRotation();
            /*if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            */
        }
    }
}
