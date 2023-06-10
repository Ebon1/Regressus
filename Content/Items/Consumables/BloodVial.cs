using System;
using Terraria;
using Terraria.ModLoader;
using Regressus.Buffs;

namespace Regressus.Items.Consumables
{
    public class BloodVial : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToFood(20, 20, ModContent.BuffType<BloodVialB>(), 60 * 15, true);
        }
    }
}
