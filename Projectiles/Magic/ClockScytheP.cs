using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.GameContent;

namespace Regressus.Projectiles.Magic
{
    public class ClockScytheP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Magic/ClockScythe";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clock Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        int SwingTime;
        public override void SetDefaults()
        {
            SwingTime = 200;
            Projectile.height = 108;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 62;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.scale = .75f;
            Projectile.tileCollide = false;
        }
        public float Lerp(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return (float)(x < 0.5f
              ? (Math.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (Math.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2);
        }
        Vector2 clockCenter, mousePos;
        float thing;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (rot < 359)
                rot += 5f;
            if (!runOnce)
            {
                clockCenter = Projectile.Center;
                runOnce = true;
            }
            if (Projectile.timeLeft == 72)
            {
                Projectile.velocity = new Vector2(30, 0).RotatedBy((mousePos - clockCenter).ToRotation());
            }
            if (Projectile.timeLeft > 72)
            {
                mousePos = Main.MouseWorld;
            }
            Projectile.rotation -= MathHelper.ToRadians(10);
            float progress = Utils.GetLerpValue(0, SwingTime, Projectile.timeLeft);
            thing = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
        bool runOnce;
        float rot = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 50)
            {
                float progress = Utils.GetLerpValue(0, SwingTime - 69, Projectile.timeLeft - 69);
                float clockProg = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, clockCenter - Main.screenPosition, null, Color.DeepSkyBlue * clockProg, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, .25f * clockProg, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value, clockCenter - Main.screenPosition, null, Color.DeepSkyBlue * clockProg, (mousePos - clockCenter).ToRotation() + MathHelper.PiOver2, ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value.Size() / 2, .25f * clockProg, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand1_notRotated").Value, clockCenter - Main.screenPosition, null, Color.DeepSkyBlue * clockProg, MathHelper.ToRadians(rot), ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value.Size() / 2, .25f * clockProg, SpriteEffects.None, 0);
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0)
            {
                Player player = Main.player[Projectile.owner];
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/glow").Value;
                var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
                {
                    if (i == Projectile.localAI[0])
                        continue;
                    Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DeepSkyBlue * (1f - fadeMult * i) * thing * 0.75f, Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                }

                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale * thing, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
