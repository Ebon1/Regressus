using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.GameContent;

namespace Regressus.Projectiles.Magic
{
    public class VisionP : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.height = 65;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 65;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            if (Projectile.ai[1] == 0)
                Projectile.penetrate = -1;
            else
                Projectile.penetrate = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        Vector2 mousePos;
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 40, 40, ModContent.DustType<Dusts.TestDust>(), 0, 0, 0, default, 1.75f * Projectile.scale);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 75 && Projectile.ai[1] == 0)
            {
                for (int i = -1; i < 5; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Main.MouseWorld.X, Main.screenPosition.Y), Utils.RotatedBy(Vector2.UnitY * 17.5f, (double)(MathHelper.ToRadians(Main.rand.NextFloat(2.5f, 4.5f)) * (float)i)), ModContent.ProjectileType<VisionP>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1);
            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.scale = 0.45f;
            }
            /*if (Projectile.velocity != Vector2.Zero)
                d.velocity = Vector2.Zero;
            else
                d.velocity = new Vector2(0, -4);*/
        }
    }
}
