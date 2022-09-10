using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Magic
{
    public class StarshroomWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The Witch's most beloved wand.");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 72;
            Item.crit = 25;
            Item.damage = 18;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12.5f;
            Item.shoot = ModContent.ProjectileType<StarshroomStar>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i++)
            {
                if (i == 0)
                    continue;
                Projectile.NewProjectile(source, position, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/velocity, type, damage, knockback, player.whoAmI, i);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Items.Ammo.Starspore>(25)
                .AddIngredient(ItemID.Mushroom, 35)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
