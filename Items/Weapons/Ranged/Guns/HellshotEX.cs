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

namespace Regressus.Items.Weapons.Ranged.Guns
{
    public class HellshotEX : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernoshot");
            Tooltip.SetDefault("It's like the hellshot but better lol");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 25;
            Item.damage = 61;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.reuseDelay = 20;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;
            Item.rare = 8;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new(-11, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position += Vector2.Normalize(velocity) * 45f;
            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10, 10))) * Main.rand.NextFloat(0.8f, 1f);
                Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<HotBulletEX>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}