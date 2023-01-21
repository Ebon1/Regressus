using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Ranged
{
    public class FrigidArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Frigid Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 2, 2, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.position, TorchID.Ice);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<RegrePlayer>().itemCombo++;
            player.GetModPlayer<RegrePlayer>().itemComboReset = 480;

            for (int num331 = 0; num331 < 20; num331++)
            {
                RegreDustHelper.DrawCircle(Projectile.Center, DustID.BlueCrystalShard, 2, 4, 4, 1, 2, nogravity: true);
            }
        }
    }
}
