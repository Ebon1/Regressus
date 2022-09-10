using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Melee
{
    public class TheBaguette : ModItem
    {
        public override void SetStaticDefaults() => Tooltip.SetDefault("'The legendary sword of Sir Bake-Ry'\nRight click to eat a part of this baguette (you can only eat once per day).");

        public override void SetDefaults()
        {
            Item.Size = new Vector2(34, 64);
            Item.damage = 10;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 10f;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 1, 50);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && !player.GetModPlayer<RegrePlayer>().CantEatBaguette)
            {
                player.GetModPlayer<RegrePlayer>().CantEatBaguette = true;
                Item.DefaultToFood(34, 64, BuffID.WellFed3, 3600 * 8);
                Item.useStyle = ItemUseStyleID.EatFood;
                Item.consumable = false;
            }
            else
            {
                Item.Size = new Vector2(34, 64);
                Item.damage = 10;
                Item.useTime = Item.useAnimation = 10;
                Item.useStyle = 1;
                Item.knockBack = 10f;
                Item.autoReuse = true;
                Item.DamageType = DamageClass.Melee;
                Item.UseSound = SoundID.Item1;
            }
            return true;
        }
    }
}
