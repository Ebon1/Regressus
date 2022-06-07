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
    public class Hellshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots one thousand degree bullets to melt enemies from within.");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 25;
            Item.damage = 51;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 18.5f;
            Item.shoot = ProjectileID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HotBullet>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}
