using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Melee
{
    public class GlacialThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Throw");
            Tooltip.SetDefault("Swing around a hunk of ice.\nIt is not a popsicle, do not stick your tongue to it.");
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 4f;
            Item.damage = 9;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GlacialThrowProj>();
            Item.shootSpeed = 15.1f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
        }
    }
}