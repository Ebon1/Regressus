using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Regressus.Projectiles.Whips;

namespace Regressus.Items.Weapons.Summon.Whips
{
    public class StarLasso : ModItem
    {
        public override void SetStaticDefaults() => Tooltip.SetDefault("Drags smaller enemies closer to you");
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<StarLassoP>(), 25, 0, 4, 35);
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Items.Ammo.Starspore>(35)
                .AddIngredient(ItemID.Mushroom, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
