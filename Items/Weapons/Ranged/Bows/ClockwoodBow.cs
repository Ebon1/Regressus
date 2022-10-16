using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Regressus.Items.Weapons.Ranged.Bows
{
    public class ClockwoodBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToBow(25, 10f, true);
            Item.damage = 10;
        }
    }
}
