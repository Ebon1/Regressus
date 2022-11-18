using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;

namespace Regressus.Items.Weapons.Ranged.Bows
{
    public class Eingefroren : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eingefroren");
            Tooltip.SetDefault("Shoots out cold arrows" +
                "\nHold down left click long enough to change normal arrows into frigid arrows");
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 64;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<ColdArrow>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<RegrePlayer>().itemCombo >= 0)
            {
                type = ModContent.ProjectileType<ColdArrow>();
                damage = 16;
            }
            if (player.GetModPlayer<RegrePlayer>().itemCombo >= 10)
            {
                type = ModContent.ProjectileType<FrigidArrow>();
                damage = 24;
            }

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
    }
}
