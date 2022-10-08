using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Regressus.Items.Weapons.Ranged
{
    public class StarshroomDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Sac");
            Tooltip.SetDefault("Throws starshroom dust that applies a debuff to npcs\nContains a sample of the witch's life effence");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 5;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.useAmmo = ModContent.ItemType<Items.Ammo.Starspore>();
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12.5f;
            Item.shoot = ModContent.ProjectileType<StarshroomDustP>();
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<Items.Ammo.Starspore>(25).AddIngredient(ItemID.Mushroom, 35).AddTile(TileID.Anvils).Register();
        }
    }
}
