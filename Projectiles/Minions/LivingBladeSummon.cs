
/*
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Minions
{
    internal class LivingBladeSummon : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Melee/LivingBlade";
        public override void SetDefaults()
        {
            Projectile.timeLeft = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.width = Projectile.height = 64;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && !player.dead && player.GetModPlayer<RegrePlayer>().bladeSummon)
            {
                Projectile.timeLeft = 2;
            }
            int ai1 = (int)Projectile.ai[1];
            if (ai1 > 0 && Main.npc[ai1].CanBeChasedBy(Projectile) && ++Projectile.ai[0] % 90 > 45)
            {
                NPC target = Main.npc[ai1];
                if (Projectile.ai[0] % 90 == 46)
                    Projectile.velocity = Projectile.DirectionTo(target.Center) * 40f;
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(target.Center).ToRotation(), 0.3f).ToRotationVector2() * Projectile.velocity.Length();
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center - Vector2.UnitY * 110f, 0.3f);
                Projectile.rotation = Projectile.rotation.AngleTowards(MathHelper.PiOver2 + MathHelper.PiOver4, 0.1f);
                float maxDist = 600f;
                int id = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile))
                    {
                        float dist = npc.Distance(player.Center);
                        if (dist < maxDist)
                        {
                            maxDist = dist;
                            id = npc.whoAmI;
                        }
                    }
                }
                Projectile.ai[1] = id;
            }
        }
    }
}*/
