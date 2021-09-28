using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles.Oracle
{
	public class Portale : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portal");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
		}
		public override Color? GetAlpha(Color lightColor) {
			return Color.White * ((255 - Projectile.alpha) / 255f);
		}
		public override void AI() {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.52342123123f;
            if (++Projectile.ai[0] >= 20) {
                float rotation = (float)Math.Atan2(Projectile.Center.Y - (player.position.Y + (player.height * 0.5f)), Projectile.Center.X -(player.position.X + (player.width * 0.5f)));
            Projectile projectilee = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, (float)((Math.Cos(rotation) * 1f) * -1), (float)((Math.Sin(rotation) * 1f) * -1), ModContent.ProjectileType<AmongUsRunes>(), 20, 1f, Main.myPlayer)];
            Projectile projectilee2 = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, (float)((Math.Cos(rotation) * 1f) * -1), (float)((Math.Sin(rotation) * 1f) * -1), ModContent.ProjectileType<TelegraphLineOracle>(), 20, 1f, Main.myPlayer)];
            Projectile.ai[0] = 0;
            projectilee.hostile = true;
            projectilee.friendly = false;
            projectilee.timeLeft = 200;
            projectilee2.timeLeft = 10;
            }
		}
	}
}