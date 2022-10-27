﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Regressus.Projectiles
{
    class Ripple : ModProjectile
    { //this one is for oracle
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override string Texture => "Regressus/Extras/test";
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
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = 0.05f;
            Projectile.ai[0] += Projectile.ai[1];
            Projectile.ai[1] *= 1.1f;
        }

    }
    class RippleSmol : ModProjectile
    {
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override string Texture => "Regressus/Extras/ripple";
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
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = 0.025f;
            Projectile.ai[0] += Projectile.ai[1];
            Projectile.ai[1] *= 1.005f;
        }

    }
}
