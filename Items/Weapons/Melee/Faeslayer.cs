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
    public class Faeslayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A weapon forged from light and faeblood.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 70;
            Item.crit = 45;
            Item.damage = 75;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noUseGraphic = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 12.5f;
            Item.shoot = ModContent.ProjectileType<FaeslayerP>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld + 450 * new Vector2((float)Math.Cos(MathHelper.ToRadians(Main.rand.Next(361))), (float)Math.Sin(MathHelper.ToRadians(Main.rand.Next(361))));
            Projectile.NewProjectile(source, position, (Vector2.UnitX * Item.shootSpeed).RotatedBy(position.DirectionTo(Main.MouseWorld).ToRotation()), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
