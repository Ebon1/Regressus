using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Regressus.Items.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using System;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Tiles.Misc
{
    public class EbonianModTile : ModTile
    {
        public override void SetStaticDefaults()
        {

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileSolid[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            Texture2D glow = RegreUtils.GetTexture("Tiles/Misc/EbonianModTile_Glow");
            TileObjectData.newTile.Origin = (glow.Size() * 0.5f).ToPoint16();
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(0, 0, 0), Language.GetText("The Eye"));
            DustType = 7;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = Terraria.GameContent.TextureAssets.Tile[Type].Value;
            Texture2D glow = RegreUtils.GetTexture("Tiles/Misc/EbonianModTile_Glow");
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Main.spriteBatch.Draw(
            texture,
             new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
            new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0f);
            //float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 0.3f);
            RegreUtils.Reload(spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(
            glow,
             new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
            new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
            new Color(Main.DiscoR, 0, 0),
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0f);
            RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
}
