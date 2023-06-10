using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Regressus.Items.Consumables.Food
{
    public class WyvernSteak : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToFood(20, 40, BuffID.WellFed2, 3600 * 4);
        }
    }
}
