using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Regressus.Items.Weapons.Magic
{
	public class SussyMogusStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Rune Staff");
			Tooltip.SetDefault("Shoots Piercing Runes");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 23;
			Item.width = 40;
			Item.height = 40;
            Item.mana = 6;
			Item.useTime = 20;
			Item.DamageType = DamageClass.Magic;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 10;Item.rare = 8;
			Item.value = 100000;
			Item.rare = 6;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Oracle.AmongUsRunes>();
			Item.shootSpeed = 3f;
		}

         public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(15);
			position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }
	}
}