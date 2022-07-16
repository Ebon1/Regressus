using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Tiles.Misc;

namespace Regressus.Items.Tiles
{
    public class EbonianModReal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Eye");
            Tooltip.SetDefault("Behold! Calamity 2\nDedicated to Ebon.");
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
            Item.rare = ItemRarityID.Purple;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<EbonianModTile>();
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line);
                return false;
            }
            if (line.Text == "Dedicated to Ebon.")
            {
                MiscDrawingMethods.DrawDevName(line);
                return false;
            }
            return true;
        }
    }
}
