using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Regressus.Items.Tiles;

namespace Regressus.Tiles.Misc
{
    public class EnergyPlantPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 2;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Energy Plant");
            AddMapEntry(new Color(26, 184, 231), name);
            DustType = 11;
            AnimationFrameHeight = 54;


        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Tiles/Misc/EnergyPlantPlaced").Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("Regressus/Tiles/Misc/EnergyPlantPlaced_Glow").Value;

            // If you are using ModTile.SpecialDraw or PostDraw or PreDraw, use this snippet and add zero to all calls to spriteBatch.Draw
            // The reason for this is to accommodate the shift in drawing coordinates that occurs when using the different Lighting mode
            // Press Shift+F9 to change lighting modes quickly to verify your code works for all lighting modes
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            // Because height of third tile is different we change it
            int height = tile.TileFrameY % AnimationFrameHeight == 54 ? 27 : 16;

            // Offset along the Y axis depending on the current frame
            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;

            // Firstly we draw the original texture and then glow mask texture
            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            // Make sure to draw with Color.White or at least a color that is fully opaque
            // Achieve opaqueness by increasing the alpha channel closer to 255. (lowering closer to 0 will achieve transparency)
            spriteBatch.Draw(
                glowTexture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Return false to stop vanilla draw
            return false;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 5) //make this number lower/bigger for faster/slower animation
            {
                frameCounter = 0;
                frame++;
                if (frame > 14)
                {
                    frame = 0;
                }
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemType<EnergyPlant>());
        }
    }
}