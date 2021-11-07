using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace Regressus.Projectiles
{
    public class LavaStream : ModProjectile
    {
        public override string Texture => "Regressus/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lava Stream");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
			Projectile.friendly = true;
			Projectile.hostile = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 1500);
		}
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 1500, quiet: false);
		}
        public override void AI()
		{
			//Bugger to hell this is all vanilla code, slightly adjusted.
			Projectile.scale -= 0.015f;
			if (Projectile.scale <= 0f)
			{
				Projectile.velocity *= 5f;
				Projectile.oldVelocity = Projectile.velocity;
				Projectile.Kill();
			}
			if (Projectile.ai[0] > 3f)
			{
				Projectile.ai[0] += Projectile.ai[1];
				if (Projectile.ai[0] > 30f)
				{
					Projectile.velocity.Y += 0.1f;
				}
                for (int k = 0; k < 6; k++)
				{
					Vector2 vector2 = Projectile.velocity * k / 6f;
                    int newdust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width - 6 * 2, Projectile.height - 6 * 2, DustID.Lava, 0f, 0f, 175, new Color(0, 80, 255, 100), 1.2f);
					Dust dust;
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[newdust];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[newdust];
						dust.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust = Main.dust[newdust];
						dust.alpha += 25;
					}
					Main.dust[newdust].noGravity = true;
					dust = Main.dust[newdust];
					dust.velocity *= 0.3f;
					dust = Main.dust[newdust];
					dust.velocity += Projectile.velocity * 0.5f;
					Main.dust[newdust].position = Projectile.Center;
					Main.dust[newdust].position.X -= vector2.X;
					Main.dust[newdust].position.Y -= vector2.Y;
					dust = Main.dust[newdust];
					dust.velocity *= 0.2f;
				}
				if (Main.rand.NextBool(4))
				{
                    int newdust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width - 6 * 2, Projectile.height - 6 * 2, DustID.Lava, 0f, 0f, 175, new Color(0, 80, 255, 100), 1.2f);
					Dust dust = Main.dust[newdust];
					dust.velocity *= 0.5f;
					dust = Main.dust[newdust];
					dust.velocity += Projectile.velocity * 0.5f;
				}
			}
			else
			{
				Projectile.ai[0] += 1f;
			}
		}
    }
}