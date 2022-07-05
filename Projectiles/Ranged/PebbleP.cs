using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Ranged
{
    public class PebbleP : ModProjectile
    {
        public override string Texture => "Regressus/Items/Weapons/Ranged/Pebble";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pebble");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig);
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
        }
    }
}
