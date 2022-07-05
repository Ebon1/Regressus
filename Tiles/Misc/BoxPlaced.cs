using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Regressus.Items.Tiles;

namespace Regressus.Tiles.Misc
{
    public class BoxPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Box");
            AddMapEntry(new Color(144, 148, 144), name);
            DustType = 11;

        }
        //MAKE THIS A CHEST I DO NOT KNOW WHAT IM DOING
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemType<Box>());
        }
    }
}