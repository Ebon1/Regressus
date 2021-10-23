using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Projectiles
{
	public class ForestBombThrownParticles : ModProjectile
	{
        public override string Texture => "Regressus/Items/Weapons/Ranged/ForestBomb"; 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forest Bomb");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 1;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
            Projectile.hostile = false;
			AIType = 0;
            Projectile.timeLeft = 3500; 
            Projectile.alpha = 255; 
		}
		public override void AI()
        {
    Dust dust;
	dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 3, new Vector2(0f, 0f), 0, new Color(255,255,255), 2f); 
	dust.noGravity = true; 
    }
	}
}