using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Ranged
{
    public class PlantGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Folirage");
            Tooltip.SetDefault("Fires a torrent of leaves with a small spread. May also fire a high-velocity spore.");
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 30;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 1000000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                int proj;
                if (Main.rand.NextBool(6))
                {
                    proj = ModContent.ProjectileType<Projectiles.Curserisen.CurserisenSeed>();
                    velocity *= 1.2f;
                }
                else
                {
                    proj = ModContent.ProjectileType<Projectiles.Curserisen.CurserisenLeaves>();
                }
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(6));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Wood, 15)
            .AddIngredient(ItemID.DirtBlock, 20)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
