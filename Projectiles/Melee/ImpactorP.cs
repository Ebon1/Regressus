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
    public class ImpactorP : HeldSword
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/Impactor";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Impactor");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            SwingTime = 25;
            holdOffset = 45f;
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            stretch = Player.CompositeArmStretchAmount.Quarter;
        }
        Vector2 impactPos;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (impactPos == Vector2.Zero)
                impactPos = target.Center - (target.Center - Projectile.Center) * 0.75f;
            RegreSystem.ScreenShakeAmount = 2.5f;
        }
        public override float Lerp(float x)
        {
            return (float)(x < 0.5f ? (1 - Math.Sqrt(1 - Math.Pow(2 * x, 2))) / 2 : (Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2);
        }
        public override void PostDraw(Color lightColor)
        {
            if (impactPos != Vector2.Zero)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft);
                Texture2D circle = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_02").Value;
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/light_01").Value;
                Main.spriteBatch.Draw(circle, impactPos - Main.screenPosition, null, new Color(66, 255, 157) * Math.Clamp(progress * 2f, 0, SwingTime), 0, circle.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                Main.spriteBatch.Draw(glow, impactPos - Main.screenPosition, null, new Color(66, 255, 157) * Math.Clamp(progress * 2f, 0, SwingTime), Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Impactor_Glow").Value;
            /*var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                if (i == Projectile.localAI[0])
                    continue;
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), new Color(53, 53, 69) * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            */

            Texture2D slash = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = (float)Math.Sin(mult * Math.PI);
            Vector2 pos = player.Center + Projectile.velocity * (35f - mult * 35f);
            Main.spriteBatch.Draw(slash, pos - Main.screenPosition, null, new Color(53, 53, 69) * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 1.95f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, glow.Width, glow.Height), new Color(53, 53, 69), Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * 1.05f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }
    public class ImpactorPThrown : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/Impactor";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Impactor");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
            Projectile.hostile = false;
            Projectile.timeLeft = 250;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        Vector2 impactPos;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (impactPos == Vector2.Zero)
                impactPos = target.Center - (target.Center - Projectile.Center) * 0.75f;
            RegreSystem.ScreenShakeAmount = 2.5f;
        }
        public override void PostDraw(Color lightColor)
        {
            if (impactPos != Vector2.Zero)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 50, Projectile.timeLeft - 200);
                Texture2D circle = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_02").Value;
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/light_01").Value;
                Main.spriteBatch.Draw(circle, impactPos - Main.screenPosition, null, new Color(66, 255, 157) * Math.Clamp(progress * 3f, 0, 50), 0, circle.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                Main.spriteBatch.Draw(glow, impactPos - Main.screenPosition, null, new Color(66, 255, 157) * Math.Clamp(progress * 3f, 0, 50), Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.5f, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Impactor_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), new Color(53, 53, 69) * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }
}
