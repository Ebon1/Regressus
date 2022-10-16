using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.GameContent;
using Terraria.DataStructures;
using Regressus.Projectiles.Dev;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace Regressus.Projectiles.Ranged
{
    public class ChroniteBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.rand.NextBool(7))
            {
                Projectile.NewProjectile(source, Projectile.Center, Projectile.velocity, ModContent.ProjectileType<DecryptItemPBall2>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                Projectile.Kill();
            }
        }
        public override void AI()
        {
            Projectile.velocity *= 1.025f;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;

            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
