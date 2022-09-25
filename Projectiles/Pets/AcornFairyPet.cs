using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Regressus.Projectiles.Pets;

namespace Regressus.Projectiles.Pets
{
    public class AcornFairyPet : ModFlyingPet
    {
        public override float TeleportThreshold => 1200f;

        public override bool ShouldFlyRotate => false;

        public override void SetStaticDefaults()
        {
            PetSetStaticDefaults(lightPet: false);
            DisplayName.SetDefault("Acorn Fairy");
            Main.projFrames[Projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            PetSetDefaults();
            Projectile.width = 46;
            Projectile.height = 44;
            Projectile.ignoreWater = true;
        }

        public override void PetFunctionality(Player player)
        {
            RegrePlayer modPlayer = player.GetModPlayer<RegrePlayer>();

            if (player.dead)
                Projectile.timeLeft = 0;

            if (!modPlayer.AcornFairyPet && !modPlayer.AcornFairyPet)
                Projectile.timeLeft = 0;

            if (modPlayer.AcornFairyPet || modPlayer.AcornFairyPet)
                Projectile.timeLeft = 2;

            Projectile.rotation = 0;
        }

        public override void Animation(int state)
        {
            SimpleAnimation(speed: 2);
        }
    }
}