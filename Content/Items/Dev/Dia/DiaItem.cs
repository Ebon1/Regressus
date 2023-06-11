using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Regressus.Common;

namespace Regressus.Content.Items.Dev.Dia
{
    public class DiaItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aurorus");
            Tooltip.SetDefault("Dedicated to Dia.");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 90;
            Item.crit = 45;
            Item.damage = 145;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.reuseDelay = 80;
            Item.mana = 150;
            Item.shootSpeed = 15f;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<DiaP>();
        }
        ParticleSystem sys = new();
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                MiscDrawingMethods.DrawDevName(line, sys);
                return false;
            }
            if (line.Text == "Dedicated to Dia.")
            {
                MiscDrawingMethods.DrawDevName(line, sys);

                return false;
            }
            return true;
        }
    }
}
