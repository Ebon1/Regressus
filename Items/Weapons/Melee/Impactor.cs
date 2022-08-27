using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Melee;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Regressus.Items.Weapons.Melee
{
    public class Impactor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Impactor");
            Tooltip.SetDefault("Right click to throw the hammer\nHammer time!");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 76;
            Item.crit = 45;
            Item.damage = 34;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            //Item.reuseDelay = 45;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<ImpactorP>();
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity * 10, ModContent.ProjectileType<ImpactorPThrown>(), damage, knockback, player.whoAmI, -player.direction);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, (int)(damage * 1.5f), knockback, player.whoAmI, -player.direction);
            }
            return false;
        }
    }
}
