using Microsoft.Xna.Framework;
using Regressus.Content.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Regressus.Common.Systems
{
    public sealed class RatVillageGenerationSystem : ModSystem
    {
        public override void PostWorldGen() => GenerateVillages();

        public override void PostUpdateEverything()
        {
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F) && !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F))
            {
                TryPlaceObject((int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f), ModContent.TileType<RatHouse2x2>());
            }
        }

        private static void GenerateVillages()
        {
            const int maxAttempts = 100;
            int attempts = 0;

            const int maxVillages = 5;
            int villages = 0;

            do
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)Main.rockLayer + 100, Main.maxTilesY - 300);

                if (TryPlaceVillage(x, y))
                {
                    villages++;
                }

                attempts++;
            } 
            while (attempts < maxAttempts && villages < maxVillages);
        }

        private static bool TryPlaceVillage(int x, int y)
        {
            bool success = true;
            int houseCount = 0;

            int[] types = new[]
            {
                ModContent.TileType<RatHouse2x2>(),
                ModContent.TileType<RatHouse3x2>()
            };

            Rectangle villageArea = new(x, y, 200, 100);

            for (int i = villageArea.X; i < villageArea.X + villageArea.Width; i++)
            {
                for (int j = villageArea.Y; j < villageArea.Y + villageArea.Height; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        if (TryPlaceObject(i, j, WorldGen.genRand.Next(types)))
                        {
                            if (WorldGen.genRand.NextBool(4))
                            {
                                TryPlaceObject(i + WorldGen.genRand.Next(-2, 3), j, ModContent.TileType<RatLantern>());
                            }

                            houseCount++;
                        }
                    }
                }
            }

            if (houseCount < 10)
            {
                success = false;
            }

            return success;
        }

        private static bool TryPlaceObject(int x, int y, int type)
        {
            bool success = true;

            int width = TileObjectData.GetTileData(type, 0).Width;
            int height = TileObjectData.GetTileData(type, 0).Height;

            for (int i = x; i < x + width; i++)
            {
                Tile groundTile = Framing.GetTileSafely(i, y);

                if (!WorldGen.InWorld(i, y) || !WorldGen.SolidTile(groundTile))
                {
                    success = false;
                    break;
                }

                for (int j = y - height; j < y; j++)
                {
                    Tile airTile = Framing.GetTileSafely(i, j);

                    if (!WorldGen.InWorld(i, j) || WorldGen.SolidTile(airTile))
                    {
                        success = false;
                        break;
                    }
                }           
            }

            if (success)
            {
                WorldGen.PlaceObject(x, y - 1, type);
            }

            return success;
        }
    }
}