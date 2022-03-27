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
    public class EarthDivider : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Earth Divider");
            Tooltip.SetDefault("5 swords fall from the sky on the 5th strike\n\"The second part of a fractured weapon\"");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 82;
            Item.crit = 45;
            Item.damage = 90;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<EarthDividerP>();
        }
        public int dir = 1, attacks = -1;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltipLine = tooltips.Find((TooltipLine x) => x.Name == "ItemName");
            tooltipLine.overrideColor = new Color(0, 255, Main.DiscoB);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (++attacks >= 5)
                attacks = 0;
            dir = -dir;
            if (attacks == 4)
            {
                for (int i = -2; i < 3; i++)
                    Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X + i * 25, Main.screenPosition.Y - 100), Vector2.UnitY * 1.5f, type, damage, knockback, player.whoAmI, dir, attacks);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, dir, attacks);
            }
            return false;
        }
    }
}
