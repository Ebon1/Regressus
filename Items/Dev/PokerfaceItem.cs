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
    public class PokerfaceItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celeste Prism");
            Tooltip.SetDefault("Dedicated to Pokerface.");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 90;
            Item.crit = 45;
            Item.damage = 145;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.mana = 2;
            Item.shootSpeed = 43f;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<PokerfaceP>();
        }
        /*public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = false;
                Item.shootSpeed = 15f;
                Item.channel = false;
                Item.mana = 5;
                Item.shoot = ModContent.ProjectileType<PokerfaceP2>();
            }
            else
            {
                Item.noUseGraphic = true;
                Item.mana = 2;
                Item.shootSpeed = 43f;
                Item.channel = true;
                Item.shoot = ModContent.ProjectileType<PokerfaceP>();
            }
            return base.CanUseItem(player);
        }*/
        ParticleSystem sys = new();
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Pokerface.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            return true;
        }
        /*public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = -3; i < 4; i++)
                {
                    if (i == 0)
                        continue;
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, i);
                }
                return false;
            }
            else
                return true;
        }*/
    }
}
