using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Consumables
{
    public class LivingBladePickup : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heartfruit");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.rare = 1;
        }
        public override bool OnPickup(Player player)
        {
            player.Heal(15);
            Item.active = false;
            Item.TurnToAir();
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}