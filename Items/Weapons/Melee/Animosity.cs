using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Melee;

namespace Regressus.Items.Weapons.Melee
{
    public class Animosity : ModItem
    {
        int swingDir = 0;
        int mode = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("As above, So below.");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 64;
            Item.damage = 60;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.shoot = ModContent.ProjectileType<AnimosityP>();
            Item.shootSpeed = 1;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Melee;
            Item.reuseDelay = 10;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
        int dir = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, dir);
            return false;
        }
    }
}
