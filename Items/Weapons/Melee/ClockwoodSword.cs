using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Melee
{
    public class ClockwoodSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(34, 64);
            Item.damage = 10;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 15);
        }
    }
}
