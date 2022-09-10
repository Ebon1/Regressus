using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles;

namespace Regressus.Items
{
    public class Flashlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashlight");
            Tooltip.SetDefault("\"Let there be light!\"");
        }
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = 34;
            Item.height = 14;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.UseSound = new Terraria.Audio.SoundStyle("Regressus/Sounds/Custom/Flashlight")
            {
                PitchVariance = 0.2f,
            };
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<FlashlightP>();
        }
    }
}
