using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Projectiles
{
    class Lens : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.penetrate = -1;
            base.SetDefaults();
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 260)
                Projectile.ai[0] += 0.003f;
            if (Projectile.timeLeft < 40)
                Projectile.ai[0] -= 0.003f;
        }

    }
}
