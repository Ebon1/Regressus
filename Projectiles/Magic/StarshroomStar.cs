using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.Audio;
using Terraria.GameContent;

namespace Regressus.Projectiles.Magic
{
    public class StarshroomStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 7; num616++)
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length(), Utils.SelectRandom<int>(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
        }
        float time, frequencyMultiplier, amplitude;
        bool runOnce;
        Vector2 initialCenter, initialVel;
        public override void AI()
        {
            if (!runOnce)
            {
                initialCenter = Projectile.Center;
                initialVel = Projectile.velocity;
                runOnce = true;
            }
            RegreUtils.SineMovement(Projectile, initialCenter, initialVel, 0.15f, 60);
            Projectile.rotation += MathHelper.ToRadians(5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Magic/StarshroomStar_Glow").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.Yellow * (1f - fadeMult * i), Projectile.oldRot[i] + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }
}
