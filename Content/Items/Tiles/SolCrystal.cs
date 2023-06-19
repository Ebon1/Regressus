using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Desert;
using Terraria;

namespace Regressus.Content.Items.Tiles
{
    public class SolCrystal : ModItem
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
            Item.createTile = ModContent.TileType<SolStoneTile>();
        }
        int a;
        public override void HoldItem(Player player)
        {
            if (a == 0)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.createTile = ModContent.TileType<SolStoneTile>();
                }
                else
                {
                    Item.createTile = ModContent.TileType<SolStoneTileSmall>();
                }
                a = 1;
            }
        }
        public override void UseItemFrame(Player player)
        {
            a = 0;
        }
    }
}