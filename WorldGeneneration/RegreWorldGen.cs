using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Regressus.WorldGeneration.Passes;

namespace Regressus.WorldGeneration
{
	public class RegreWorldGen : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{

			int index = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));

			tasks.Insert(index, new AmbientPass("AmbientPass", 0.5f));
		}


	}
}