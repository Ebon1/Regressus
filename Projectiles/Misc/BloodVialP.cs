using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles.Misc
{
    public class BloodVialP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mosquito");
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new(24, 22);
            Projectile.friendly = Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
                Dust.NewDust(Projectile.Center, 24, 22, DustID.Blood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
        }
        public override void AI()
        {
            Projectile.damage = Projectile.originalDamage = 15;
            if (++Projectile.frameCounter % 2 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }
            if (Main.player[Projectile.owner].HasBuff<Buffs.BloodVialB>())
                Projectile.timeLeft = 2;
            Vector2 center = Main.player[Projectile.owner].Center;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.Center.Distance(Projectile.Center) < 600f)
                    {
                        center = npc.Center;
                    }
                }
            }
            Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, center, false) * 0.0045f;
            Projectile.direction = center.X > Projectile.Center.X ? 1 : -1;
            Projectile.spriteDirection = Projectile.direction;
        }
    }
}
