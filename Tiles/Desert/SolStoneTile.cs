using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Regressus.Items.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Tiles.Desert {
    public class SolStoneTile : ModTile {
        public override void SetStaticDefaults() {
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<SolStoneItem>();
            AddMapEntry(new Color(255, 149, 0), Language.GetText("Sun Crystal"));
            AnimationFrameHeight = 198;
            HitSound = SoundID.Tink;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.addTile(Type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<SolStoneItem>());
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0) {
                r = 1f;
                g = 0.58f;
                b = 0f;
            }
        }
    }
}