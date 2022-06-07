using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Regressus.Items.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Tiles.Misc {
    public class PetrifiedWood : ModTile {
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.BorealWood;
            ItemDrop = ModContent.ItemType<PetrifiedWoodItem>();
            AddMapEntry(new Color(73, 57, 44), Language.GetText("Petrified Wood"));
            AnimationFrameHeight = 0;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
    }
}