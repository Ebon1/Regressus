using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Relics;

namespace Regressus.Items.Tiles.Relics
{
    public class ConjurerRelicItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Conjurer Relic");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.useTurn = true;
            Item.rare = 0;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Master;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ConjurerRelic>();
        }
    }
}
