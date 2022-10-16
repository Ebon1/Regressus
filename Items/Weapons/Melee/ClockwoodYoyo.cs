using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Regressus.Projectiles.Melee;

namespace Regressus.Items.Weapons.Melee
{
    public class ClockwoodYoyo : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.damage = 10;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.noMelee = Item.noUseGraphic = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 15);
            Item.shoot = ModContent.ProjectileType<ClockwoodYoyoP>();
            Item.shootSpeed = 16f;
        }
    }
}
