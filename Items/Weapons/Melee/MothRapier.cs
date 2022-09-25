using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Melee;

namespace Regressus.Items.Weapons.Melee
{
    internal class MothRapier : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deluge");
            Tooltip.SetDefault("Makes rain fall upon enemies on hit.");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 82;
            Item.crit = 45;
            Item.damage = 15;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<MothRapierP>();
        }
    }
}
