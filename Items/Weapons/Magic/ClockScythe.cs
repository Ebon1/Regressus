using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Magic
{
    public class ClockScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chrono-sickle");
            Tooltip.SetDefault("Ancient weapon of a forgotten titan");
        }

        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 82;
            Item.crit = 15;
            Item.damage = 43;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.UseSound = SoundID.Item43;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.mana = 25;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<ClockScytheP>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
