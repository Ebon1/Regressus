using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace Regressus.Projectiles.Melee
{
    public class ClockwoodYoyoP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = 3.25f;
            ProjectileID.Sets.YoyosMaximumRange[Type] = 145;
            ProjectileID.Sets.YoyosTopSpeed[Type] = 10f;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.Size = Vector2.One * 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }
    }
}
