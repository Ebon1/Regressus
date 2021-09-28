using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles.Oracle
{
    public class CrystalProjectilesFromTheHitGameAmongUs : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<OracleCrystalExplosionSus>(), 20, 0, 0);
        }
        public override void AI()
        {
            if ((Main.player[Projectile.owner].position - Projectile.Center).LengthSquared() < 18 * 18)
            {
                Projectile.Kill();
            }
            if (Projectile.position.Y + (float)Projectile.height < Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - 16f)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.785f;
        }
    }
}