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
using System.Threading.Tasks;

namespace Regressus.Items.Weapons.Ranged
{
    public class Firestorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Firestorm Grenade");
            Tooltip.SetDefault("BURN!!! BURN!!!!!!!!!");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.crit = 10;
            Item.damage = 25;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 30;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.maxStack = 99999;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<FirestormGrenade>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, -Vector2.UnitY * 20f, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).AddIngredient(ItemID.StoneBlock).Register();
        }
    }
}
