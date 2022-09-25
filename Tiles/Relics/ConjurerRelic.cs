using Terraria.ModLoader;
using Regressus.Items.Tiles.Relics;

namespace Regressus.Tiles.Relics
{
    public class ConjurerRelic : RelicBase
    {
        public override float Offset => 0;
        protected override int ItemType => ModContent.ItemType<ConjurerRelicItem>();
    }
}
