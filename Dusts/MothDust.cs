using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
namespace Regressus.Dusts
{
    public class MothDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = 110;
        }
    }
}