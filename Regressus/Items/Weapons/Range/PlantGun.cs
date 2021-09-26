using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Ranged
{
	public class PlantGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Edit this with your tooltip.");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true; 
			item.width = 46;
			item.height = 30; 
			item.useTime = 15; 
			item.useAnimation = 15; 
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = 1000000; 
			item.rare = 2; 
			item.UseSound = SoundID.Item40;
			item.autoReuse = true;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 10;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (type == ProjectileID.Bullet)
			{
				type = ProjectileID.Bullet;
			}
			return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 15);
			recipe.AddIngredient(ItemID.DirtBlock, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
