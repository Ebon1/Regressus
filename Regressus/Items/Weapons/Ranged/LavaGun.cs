using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Regressus.Items.Weapons.Ranged
{
    public class LavaGun : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("The Fire Hose");
			Tooltip.SetDefault("Fires out streams of lava to incinerate foes.");
        }
        public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Orange;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.width = 38;
			Item.height = 10;
			Item.damage = 20;
			Item.scale = 0.9f;
			Item.shoot = ModContent.ProjectileType<Projectiles.LavaStream>();
			Item.shootSpeed = 8f;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 1, 50);
		}
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(2272, 1)
            .AddIngredient(173, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}