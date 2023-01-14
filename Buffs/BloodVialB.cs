using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Regressus.Projectiles.Misc;

namespace Regressus.Buffs
{
    public class BloodVialB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Vial");
            Description.SetDefault("You are attracting dangerous mosquitoes");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] % 30 == 0 && player.buffTime[buffIndex] > 5)
            {
                Vector2 center = player.Center + (Vector2.One * 80).RotatedByRandom(7f);
                for (int i = 0; i < 4; i++)
                    Dust.NewDust(center, 24, 22, DustID.Blood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), center, Vector2.Zero, ModContent.ProjectileType<BloodVialP>(), 15, 0f, player.whoAmI);
            }
        }
    }
}
