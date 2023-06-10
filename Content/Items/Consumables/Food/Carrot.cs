using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Regressus.Items.Consumables.Food
{
    public class Carrot : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToFood(20, 40, BuffID.WellFed, 3600);
        }
    }
}
