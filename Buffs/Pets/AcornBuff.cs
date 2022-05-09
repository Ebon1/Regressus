using Regressus;
using Terraria;
using Terraria.ModLoader;
using Regressus.Projectiles.Pets;

namespace Regressus.Buffs.Pets
{
    public class AcornBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Acorn Fairy");
            Description.SetDefault("A forest memory is following you!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<RegrePlayer>().AcornFairyPet = true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<AcornFairyPet>()] <= 0;

            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer){
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2),
                    0f, 0f, ModContent.ProjectileType<AcornFairyPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}