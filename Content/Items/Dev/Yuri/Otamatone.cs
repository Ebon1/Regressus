using Regressus.Common;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Content.Items.Dev.Yuri
{
    public class Otamatone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Otamatone");
            Tooltip.SetDefault("Shoots Phantom otamatones and makes funny noises :)\nDedicated to Yuri O.");
        }

        public override void SetDefaults()
        {
            Item.damage = 67;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 29;
            Item.useAnimation = 28;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = 5;
            Item.knockBack = 10;
            Item.value = Item.buyPrice(0, 22, 50, 0);
            Item.rare = 9;
            Item.autoReuse = false;
            Item.shoot = Mod.Find<ModProjectile>("OtamatoneP").Type;
            Item.shootSpeed = 10f;
        }
        ParticleSystem sys = new();
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Yuri O.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);

                return false;
            }
            return true;
        }
    }
}
