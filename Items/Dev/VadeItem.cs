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
    public class VadeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tyrfing");
            Tooltip.SetDefault("\"Would you care to see my rgb lighting, i spent $3000\"\nDedicated to Vade in a river.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = 66;
            Item.height = 100;
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
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<VadeItemP>();
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line);
                return false;
            }
            if (line.Text == "Dedicated to Vade in a river.")
            {
                MiscDrawingMethods.DrawDevName(line);
                return false;
            }
            return true;
        }
        public int dir = 1, attacks = -1;
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Minions.Tyrfing>(), 2);
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<VadeItemP3>(), damage, knockback, player.whoAmI);
                proj.originalDamage = Item.damage;
            }
            attacks++;
            if (attacks >= 3)
                attacks = 0;
            dir = -dir;
            if (attacks == 2)
                for (int i = -2; i < 3; i++)
                {
                    Projectile.NewProjectile(source, position, Utils.RotatedBy(velocity * 1.5f, (double)(MathHelper.ToRadians(16f) * (float)i)), ModContent.ProjectileType<VadeItemP2>(), damage, knockback, player.whoAmI);
                }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, dir, attacks);
            return false;
        }
    }
}
