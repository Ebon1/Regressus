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
            DisplayName.SetDefault("Plasmalight");
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
    public class Flashlight2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionized Plasmalight");
            Tooltip.SetDefault("Outputs light at 5 million lumen!\nContinuous use at night may attract unwanted attention...");
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
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<FlashlightP2>();
        }
    }
}
