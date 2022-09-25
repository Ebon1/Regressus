using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Melee;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vagrant;
namespace Regressus.Projectiles.Melee
{
    public class MothRapierP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/MothRapier";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moth Rapier");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        float holdOffset;
        int SwingTime;
        public override void SetDefaults()
        {
            SwingTime = 15;
            holdOffset = 39;
            Projectile.width = Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
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
        public override void AI()
        {
            AttachToPlayer();
        }
        bool runOnce;
        float rot;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 3; i++)
            {
                int rain = 0;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        rain = ModContent.ProjectileType<Rain1>();
                        break;
                    case 1:
                        rain = ModContent.ProjectileType<Rain2>();
                        break;
                    case 2:
                        rain = ModContent.ProjectileType<Rain3>();
                        break;
                }
                Vector2 random = new Vector2(target.Center.X + 50 * Main.rand.NextFloat(-1f, 1f), Main.screenPosition.Y);
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), random, new Vector2(Main.windSpeedCurrent * 1.1f, 5), rain, 10, 0, Main.myPlayer);
                a.hostile = false;
                a.friendly = true;
            }
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
            if (!runOnce)
            {
                rot = Projectile.velocity.ToRotation();
                runOnce = true;
            }
            float progress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float _holdOffset = holdOffset * (progress + 0.25f);
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rot * player.direction;
            pos += rot.ToRotationVector2() * _holdOffset;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rot - MathHelper.PiOver2);
            Projectile.rotation = (pos - player.Center).ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
            Projectile.Center = pos;
            player.itemTime = 2;
            player.itemAnimation = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0)
            {
                Player player = Main.player[Projectile.owner];
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (Projectile.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), texture.Size() / 2, Projectile.scale, Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
