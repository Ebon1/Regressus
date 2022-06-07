using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Melee;
using Terraria.GameContent;

namespace Regressus.Projectiles.Melee
{
    public class ForeshadowP : HeldSword
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/Foreshadow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foreshadow");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            SwingTime = 25;
            holdOffset = 65f;
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 v = Vector2.UnitY.RotatedBy(Utils.RotatedByRandom(Projectile.Center, MathHelper.ToRadians(180)).ToRotation());
            Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld, v, ModContent.ProjectileType<ForeshadowP2>(), Projectile.damage / 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI)];
            p.velocity = Vector2.UnitX.RotatedBy(v.ToRotation());
            p.rotation = v.ToRotation();
            //p.localAI[0] = v.X;
            //p.localAI[1] = v.Y;
        }
        public override float Lerp(float x)
        {
            return (float)(x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Math.Pow(2, 20 * x - 10) / 2 : (2 - Math.Pow(2, -20 * x + 10)) / 2);
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.75f) - mult * (holdOffset * 0.75f));
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(70, 10, 102) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), new Color(70, 10, 102), Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
            /*Projectile.localAI[0]++;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;*/
        }
    }
    public class ForeshadowP3 : HeldSword
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/Foreshadow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foreshadow");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            SwingTime = 15;
            holdOffset = 65f;
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }
        int a;
        float aa, b;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (!Main.mouseRight && a < 60)
            {
                Projectile.Kill();
            }
            if (Main.mouseRight && a < 60)
            {
                a++;
                Projectile.timeLeft = SwingTime;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos1 = player.Center + new Vector2(100, 0).RotatedByRandom(MathHelper.ToRadians(359));
                    Vector2 vel = player.Center - pos1;
                    vel.Normalize();
                    vel *= 5f;
                    Dust.NewDustDirect(pos1, 1, 1, ModContent.DustType<Dusts.TestDust>(), vel.X, vel.Y);
                }
            }
            if (a == 59)
            {
                //TP(Main.MouseWorld);
            }
            if (Projectile.timeLeft == 7)
            {
                Vector2 v = Vector2.UnitX.RotatedBy(b);
                Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, v, ModContent.ProjectileType<ForeshadowP2>(), Projectile.damage * 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 1)];
                p.velocity = Vector2.UnitX.RotatedBy(v.ToRotation());
                p.rotation = v.ToRotation();
            }
            float start = Projectile.velocity.ToRotation() - ((MathHelper.PiOver2) - 0.2f);
            float end = Projectile.velocity.ToRotation() + ((MathHelper.PiOver2) - 0.2f);
            float progress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float rot = Projectile.ai[0] == 1 ? start.AngleLerp(end, progress) : start.AngleLerp(end, 1f - progress);
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rot * player.direction;
            pos += rot.ToRotationVector2() * holdOffset;
            Projectile.rotation = (pos - player.Center).ToRotation() + MathHelper.PiOver4;
            Projectile.Center = pos;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
        public override float Lerp(float x)
        {
            return (float)(x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Math.Pow(2, 20 * x - 10) / 2 : (2 - Math.Pow(2, -20 * x + 10)) / 2);
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (a < 60)
            {
                aa = RegreUtils.FromAToB(player.Center, Main.MouseWorld, reverse: true).ToRotation();
                b = RegreUtils.FromAToB(player.Center, Main.MouseWorld).ToRotation();
            }
            if (player == Main.player[Main.myPlayer])
                Utils.DrawLine(Main.spriteBatch, player.Center - new Vector2(Main.screenWidth * 2, 0).RotatedBy(aa), player.Center + new Vector2(Main.screenWidth * 2, 0).RotatedBy(b), Color.DarkViolet * 0.5f, Color.DarkViolet * 0.5f, 5);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = MathHelper.Clamp((float)Math.Sin(mult * Math.PI) * 3f, 0, 1);
            Vector2 pos = player.Center + Projectile.velocity * ((holdOffset * 0.75f) - mult * (holdOffset * 0.75f));
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(70, 10, 102) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), new Color(70, 10, 102), Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
            /*Projectile.localAI[0]++;
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;*/
        }
    }
    public class ForeshadowP2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/starSky2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tear");
        }
        int maxTime;
        public override void SetDefaults()
        {
            maxTime = 45;
            Projectile.width = 21;
            Projectile.height = 723 / 3;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 45;
            Projectile.tileCollide = false;
            Projectile.scale = .5f;
            if (Projectile.ai[0] == 1)
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 4;
            }
        }
        public float rotation = 0;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float a = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.velocity * ((Projectile.height * Projectile.scale * (Projectile.ai[0] == 1 ? 6 : 8)) / 2), Projectile.Center + Projectile.velocity * ((Projectile.height * Projectile.scale * (Projectile.ai[0] == 1 ? 6 : 8)) / 2), Projectile.width * (Projectile.ai[0] == 1 ? 0.5f : 1) * Projectile.scale * (Projectile.ai[0] == 1 ? 6 : 8), ref a);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/Extras2/scratch_02").Value, Projectile.Center - Main.screenPosition, null, Color.DarkViolet * Projectile.scale, Projectile.rotation, ModContent.Request<Texture2D>("Regressus/Extras/Extras2/scratch_02").Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        /*public override void PostDraw(Color lightColor)
        {
            Utils.DrawLine(Main.spriteBatch, Projectile.Center - Projectile.velocity * ((Projectile.height * Projectile.scale * (Projectile.ai[0] == 1 ? 6 : 8)) / 2), Projectile.Center + Projectile.velocity * ((Projectile.height * Projectile.scale * (Projectile.ai[0] == 1 ? 6 : 8)) / 2), Color.Red);
        }*/
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        bool runOnce;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!runOnce)
            {
                runOnce = true;
            }
            if (Projectile.ai[0] == 1)
                Projectile.scale = 3.25f;
            //Projectile.velocity = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            //Projectile.rotation = Projectile.velocity.ToRotation();
            float progress = Utils.GetLerpValue(0, maxTime, Projectile.timeLeft);
            Projectile.ai[1] = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 1.25f, 0, 1);
            /*Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity = Vector2.Zero;
                Vector2 v = Utils.RotatedByRandom(Projectile.Center, MathHelper.ToRadians(180));
                Projectile.rotation = v.ToRotation();
                for (int i = 0; i < 2; i++)
                {
                    float angle = 2f * (float)Math.PI / 2f * i;
                    Vector2 velocity = new Vector2(1, 1).RotatedBy(angle);
                    Projectile Projectileectile = Main.Projectileectile[Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity * v, ModContent.ProjectileType<SmolTentacleRT>(), Projectile.damage, 0f, player.whoAmI, 35, 0)];
                    Projectileectile.timeLeft = 180;
                    Projectileectile.hostile = false;
                    Projectileectile.friendly = true;
                }
            }*/
        }
    }
}
