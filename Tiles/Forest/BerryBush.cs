using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Regressus.Tiles.Forest
{
    public class BerryBush : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.RandomStyleRange = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.JunglePlants;
            HitSound = SoundID.Grass;

            AddMapEntry(new Color(113, 183, 43), Language.GetText("Berry Bush"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // spawn slyphs 
        }

    }

}
