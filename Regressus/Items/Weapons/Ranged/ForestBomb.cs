using Regressus.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
namespace Regressus.Items.Weapons.Ranged
{
	public class ForestBomb : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forest Bomb");
        }
		public override void SetDefaults() {
			Item.shootSpeed = 10f;
			Item.damage = 26;
			Item.knockBack = 5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 25;
            Item.reuseDelay = 30;
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 1;
			Item.rare = 2;

			Item.consumable = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;

			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(silver: 5);
			Item.shoot = ModContent.ProjectileType<ForestBombThrown>();
		}
	}
}