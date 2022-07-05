using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Regressus.Items.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Regressus.Tiles.Desert
{
    public class SolStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(255, 149, 0), Language.GetText("Sol Crystal"));
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
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<SolCrystal>());
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int height = tile.TileFrameY % AnimationFrameHeight == 36 ? 18 : 16;
            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, Regressus.CrystalShine);
            Regressus.CrystalShine.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            Regressus.CrystalShine.Parameters["uOpacity"].SetValue(1);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.Tile[Type].Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
            return false;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                r = 1f;
                g = 0.58f;
                b = 0f;
            }
        }
    }
    public class SolStoneTileSmall : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(255, 149, 0), Language.GetText("Sol Crystal"));
            AnimationFrameHeight = 198;
            HitSound = SoundID.Tink;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<SolCrystal>());
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, Regressus.CrystalShine);
            Regressus.CrystalShine.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            Regressus.CrystalShine.Parameters["uOpacity"].SetValue(1);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.Tile[Type].Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(0, 0, 16, 16), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
            return false;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0)
            {
                r = 1f;
                g = 0.58f;
                b = 0f;
            }
        }
    }
}