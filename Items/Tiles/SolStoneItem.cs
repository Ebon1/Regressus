using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Desert;

namespace Regressus.Items.Tiles {
    public class SolStoneItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Sun Crystal");
        }
        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.useTurn = true;
            Item.rare = 0;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<SolStoneTile>();
        }
    }
}