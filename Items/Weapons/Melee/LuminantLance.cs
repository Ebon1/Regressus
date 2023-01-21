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
using ReLogic.Graphics;
using Regressus.Items.Weapons.Magic;

namespace Regressus.Items.Weapons.Melee
{
    public class LuminantLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminant Lance");
            Tooltip.SetDefault("Flawless");
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (proj != null)
                if (proj.ai[1] <= 0)
                {
                    proj.ai[1] = 40;
                    for (int i = 0; i < 2; i++)
                    {
                        float angle = RegreUtils.CircleDividedEqually(i, 2);
                        Vector2 pos = target.Center + (Vector2.One * 150).RotatedBy(angle);
                        Vector2 vel = RegreUtils.FromAToB(pos, target.Center) * 5;
                        Projectile.NewProjectile(Projectile.InheritSource(Item), pos, vel, ModContent.ProjectileType<LuminaryPF>(), Item.damage, Item.knockBack, player.whoAmI);
                    }
                }
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 50;
            Item.crit = 45;
            Item.damage = 23;
            Item.channel = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noUseGraphic = true;
            //Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<LuminantLanceP>();
        }
        Projectile proj;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Text == "Flawless" || line.Index == 0)
            {
                MiscDrawingMethods.DrawFlawlessRarity(line);
                return false;
            }
            return true;
        }
    }
}
