using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Forest;
using Terraria;

namespace Regressus.Tiles.Forest
{
    public class Test : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.White;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<BerryBush>();
        }
    }
}