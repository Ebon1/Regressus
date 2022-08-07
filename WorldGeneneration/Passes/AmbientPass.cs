using Regressus.Tiles.Forest;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Regressus.WorldGeneration.Passes
{
	public class AmbientPass : GenPass
	{
		public AmbientPass(string name, float loadWeight) : base(name, loadWeight) { }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Placing bushes..."; // uhh replace this

			int skipX = 0;
			for (int tileX = 1; tileX < Main.maxTilesX - 1; tileX++)
			{
				progress.Set(tileX / Main.maxTilesX);

				if (skipX > 0)
				{
					skipX--;
					continue;
				}

				if (WorldGen.genRand.NextBool(30)) 
				{
					for (int tileY = 1; tileY < Main.worldSurface; tileY++)
					{
						// on a solid block, check if 2 tiles are grass and the 2x2 area above is clear 
						if(Main.tile[tileX, tileY].BlockType == BlockType.Solid && WorldGen.EmptyTileCheck(tileX, tileX + 1, tileY - 1, tileY - 2) && Main.tile[tileX, tileY].TileType == TileID.Grass && Main.tile[tileX+1, tileY].TileType == TileID.Grass)
						{
							WorldGen.PlaceObject(tileX, tileY - 1, ModContent.TileType<BerryBush>(), random: WorldGen.genRand.Next(0,2));
							skipX = WorldGen.genRand.Next(6, 200); // skip some tiles after placing 
							break;
						}
					}
				}
			}
		}
	}
}