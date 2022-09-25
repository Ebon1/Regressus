using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Melee.LichKnife {
    public class LichKnife : ModItem {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lich Dagger");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChainKnife);
            Item.damage=20;
            Item.shoot=ModContent.ProjectileType<LichKnifeProjectile>();
            Item.rare = 2;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.DamageType=DamageClass.Melee;
            Item.noUseGraphic = true;
            Item.shootSpeed = 12f;

        }

        
    }
}