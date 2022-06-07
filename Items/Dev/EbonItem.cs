using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;

namespace Regressus.Items.Dev
{
    public class EbonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ebonian Slayer");
            Tooltip.SetDefault("Great for uhh\nDedicated to Ebon.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = 66;
            Item.height = 100;
            Item.crit = 45;
            Item.damage = 185;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<EbonItemP>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                RegreUtils.DrawDevName(line);
                return false;
            }
            if (line.Text == "Dedicated to Ebon.")
            {
                RegreUtils.DrawDevName(line);
                return false;
            }
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*if (player.altFunctionUse == 2)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Minions.Tyrfing>(), 2);
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<VadeItemP3>(), damage, knockback, player.whoAmI);
                proj.originalDamage = Item.damage;
            }*/
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}

