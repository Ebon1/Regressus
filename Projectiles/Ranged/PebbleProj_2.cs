using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Regressus.Projectiles.Ranged
{
    public class PebbleProj_2 : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Ranged/Pebble";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pebble");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone,
                              -Projectile.velocity.X * 0.3f, -Projectile.velocity.Y * 0.3f, Scale: 2);

            }
            for (int i = 0; i < 3; i++)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f))) * Main.rand.NextFloat(0.8f, 1.1f);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<PebbleP>(), (int)(Projectile.damage * 0.66f), 1f, Projectile.owner);
                }
            }

        }
    }
}
