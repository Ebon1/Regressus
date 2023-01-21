/*using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Regressus.WorldGeneration
{
    public class ShrineBiomeSystem : ModSystem
    {
        private static List<Vector2> shrinePos;
        private static List<Vector2> shrinePosGen = new();

        private static List<Rectangle> shrineRectangles = new();

        public override void Load()
        {
            //On.Terraria.GameContent.Biomes.EnchantedSwordBiome.Place += HookCountShrines;
        }

        // adds to a list the coordinates of every succesful shrine placement 
        private static bool HookCountShrines(On.Terraria.GameContent.Biomes.EnchantedSwordBiome.orig_Place orig, EnchantedSwordBiome self, Point origin, StructureMap structures)
        {
            if (orig(self, origin, structures))
            {
                shrinePosGen.Add(new Vector2(origin.X, origin.Y));
                return true;
            }

            return false;
        }

        public override void OnWorldLoad()
        {
            shrinePos = new();
        }

        public override void PostWorldGen()
        {
            shrinePos = shrinePosGen;

            // must reset this in the case of user generating multiple worlds 
            shrinePosGen = new();
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("shrinePos", out shrinePos))
            {
                ComputeShrineRectangles();
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (shrinePos.Count > 0)
            {
                tag["shrinePos"] = shrinePos;
            }
        }

        public static bool IsInOrNearShrine(Player player)
        {
            for (int i = 0; i < shrineRectangles.Count; i++)
            {
                // if any shrine rectangle intersects with the player's tile coordinates 
                if (shrineRectangles[i].Intersects(new Rectangle((int)(player.position.X / 16), (int)(player.position.Y / 16), 2, 3)))
                {
                    return true;
                }
            }

            return false;
        }

        private void ComputeShrineRectangles()
        {
            for (int i = 0; i < shrinePos.Count; i++)
            {
                // 50x50 tile coordinate rectangle around the shrine (might need a bigger one)
                shrineRectangles.Add(new Rectangle((int)(shrinePos[i].X) - 25, (int)(shrinePos[i].Y) - 25, 50, 50));
            }
        }

    }
}*/