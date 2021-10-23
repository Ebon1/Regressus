using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace Regressus.Projectiles
{
    public class NorthernStarProj : ModProjectile
    {
        public override string Texture => "Regressus/Empty";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.light = .5f;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Electric, 0f, 0f, 0, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = Projectile.position;
            Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
            Main.dust[dust].velocity *= 0.2f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 27);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric);
            }
        }
    }
}