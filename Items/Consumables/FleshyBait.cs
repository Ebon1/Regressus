using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Regressus.Projectiles.Misc;

namespace Regressus.Items.Consumables
{
    public class FleshyBait : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToThrownWeapon(ProjectileID.Bubble, 40, 2.5f);
            Item.damage = 0;
            Item.shoot = 0;
        }
        public override bool? UseItem(Player player)
        {
            NPC.NewNPCDirect(player.GetSource_FromThis(), player.Center, ModContent.NPCType<FleshyBaitP>());
            return true;
        }
    }
}
