using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Misc;

namespace Regressus.Items.Tiles
{
    public class Box : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Box");
        }

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.width = 44;
            Item.height = 44;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.rare = 10;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = 20;
            Item.createTile = ModContent.TileType<BoxPlaced>();
            Item.maxStack = 99;
        }
    }
}