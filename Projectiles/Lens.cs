using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
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
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.penetrate = -1;
            base.SetDefaults();
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 80)
                Projectile.ai[0] += 0.003f;
            if (Projectile.timeLeft < 40)
                Projectile.ai[0] -= 0.003f;
        }

    }
    class Lens2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 0;
            base.SetDefaults();
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0;
            Projectile.ai[1] = Main.rand.Next(1, 4);
            Projectile.ai[0] = Main.rand.NextFloat(0.5f, 1);
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 80)
                Projectile.scale += 0.003f;
            if (Projectile.timeLeft < 40)
                Projectile.scale -= 0.003f;
        }

    }
}
