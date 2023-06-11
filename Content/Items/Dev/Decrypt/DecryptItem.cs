using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Regressus.Common;
using Microsoft.Xna.Framework;

namespace Regressus.Content.Items.Dev.Decrypt
{
    public class DecryptItem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A Dance of Fire and Ice");
            Tooltip.SetDefault("Right click for an alt attack\n\"Helikopter, helikopter\"\nDedicated to Decrypt.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = 34;
            Item.height = 60;
            Item.crit = 45;
            Item.damage = 145;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = .5f;
            Item.shoot = ModContent.ProjectileType<DecryptItemP>();
        }
        ParticleSystem sys = new();
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Decrypt.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            return true;
        }
        public int dir = 1;
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, (dir == 1 ? ModContent.ProjectileType<DecryptItemP1>() : ModContent.ProjectileType<DecryptItemP2>()), damage, knockback, player.whoAmI, dir);
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, dir);
            return false;
        }
    }
}
