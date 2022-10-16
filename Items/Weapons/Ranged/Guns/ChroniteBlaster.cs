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
using Regressus.Projectiles.Dev;

namespace Regressus.Items.Weapons.Ranged.Guns
{
    public class ChroniteBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 15;
            Item.damage = 25;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 25f;
            Item.shoot = ProjectileID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .3f;
        }
        //int H;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /* H++;
             if (H == 5)
             {
                 H = 0;
                 Projectile.NewProjectile(source, position, velocity * 0.5f, ModContent.ProjectileType<DecryptItemPBall2>(), damage * 2, knockback, player.whoAmI);
                 return false;
             }*/
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ChroniteBullet>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}
