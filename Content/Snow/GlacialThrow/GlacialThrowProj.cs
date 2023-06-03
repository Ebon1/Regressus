using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Regressus.Common.Projectiles;

namespace Regressus.Content.Snow.GlacialThrow
{
    //This is intended to be looked at after looking at BaseFlail.cs, so check that first.
    //In a different class like this, we can use our previously made flail AI and add some stats to it
    public class GlacialThrowProj : FlailProj
    {
        //This line is vital, it assigns the neccesary stats to our flail.
        public GlacialThrowProj() : base("GlacialThrowProj", 26, 8, 15f, 120f) { }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Throw");
        }
        public override void ExtraDefaults()
        {
            Projectile.light = .5f;
        }
        public override void OnSpinning()
        {
            if (Main.rand.NextBool(5))
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric);
            }
        }
        public override void ExtraTileCollide(Vector2 oldvel)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric);
            }
        }
    }
}