using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Regressus.Content.Tiles
{
    public sealed class RatHouse3x2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(143, 86, 59));

            DustType = DustID.WoodFurniture;
            SoundType = SoundID.Dig;
            MineResist = 2f;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 2;
    }
}