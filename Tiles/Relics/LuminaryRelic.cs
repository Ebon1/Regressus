using Terraria.ModLoader;
using Regressus.Items.Tiles.Relics;
using System;

namespace Regressus.Tiles.Relics
{
    public class LuminaryRelic : RelicBase
    {
        public override float Offset => 0;
        protected override int ItemType => ModContent.ItemType<LuminaryRelicItem>();
    }
}
