using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Regressus.Items.Weapons.Magic
{
    public class NorthernStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Northern Star");
            Tooltip.SetDefault("Fires piercing ice sparks.");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; //so the Item`s animation doesn`t do damage
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.NorthernStarProj>();
            Item.shootSpeed = 10f;
        }
    }
}