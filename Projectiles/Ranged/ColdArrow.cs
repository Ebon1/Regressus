using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;

namespace Regressus.Projectiles.Ranged
{
    public class ColdArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cold Arrow");
           
        }
        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 18;

            Projectile.penetrate = 2;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            int num384 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BlueCrystalShard);
            Main.dust[num384].velocity *= 0f;
            Main.dust[num384].noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Snow, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, Color.White, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<RegrePlayer>().itemCombo++;
            player.GetModPlayer<RegrePlayer>().itemComboReset = 480;
        }
    }
}

