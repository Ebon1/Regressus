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
using Regressus.NPCs.Chronolands;

namespace Regressus.Items.Weapons.Ranged
{
    public class Pebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Just an ordinary pebble.\nCan be used as a distraction for blind enemies.");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.crit = 0;
            Item.damage = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.maxStack = 99999;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PebbleP>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(5).AddIngredient(ItemID.StoneBlock).Register();
        }
    }
}
