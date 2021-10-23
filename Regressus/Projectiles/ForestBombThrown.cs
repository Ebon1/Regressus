using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
namespace Regressus.Projectiles
{
	public class ForestBombThrown : ModProjectile
	{
        public override string Texture => "Regressus/Items/Weapons/Ranged/ForestBomb";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ebonian Scythe");
		}

		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.timeLeft = 130;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.velocity *= 0.1f;
			return false;
		}
        public override void Kill(int timeLeft) 
        {
            Player player = Main.player[Projectile.owner];
            RegreSystem.ScreenShakeAmount = 5;
            
                for (int i = 0; i < 7; i++) {
					float speedX = Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-8f, 8f);
					float speedY = -5;
					Projectile sussy = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.position.X + speedX, Projectile.position.Y + speedY, speedX, speedY, ModContent.ProjectileType<ForestBombThrownParticles>(), 30, 0, Projectile.owner, 0, 0)];
					sussy.hostile = false;
					sussy.friendly = true;
            }
        }
	}
}