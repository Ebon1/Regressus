using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Ranged.Guns
{
    public class PebbleGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pebble Gun");
            Tooltip.SetDefault("This can give someone a concussion");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 0;
            Item.damage = 10;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.noMelee = true;
            Item.autoReuse = false;
            //Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;
            Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PebbleProj_2>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 4;

            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.StoneBlock, 5)
                .AddIngredient(ItemID.SilverBar, 10)
                .Register();


        }
    }
}
