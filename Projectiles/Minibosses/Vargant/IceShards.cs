using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Regressus.Projectiles.Minibosses.Vargant
{
    public class Hail1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice shard");
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 42;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.velocity *= 1.01f;
        }
    }
    public class Hail2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice shard");
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.velocity *= 1.01f;
        }
    }
    public class Hail3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice shard");
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.velocity *= 1.01f;
        }
    }
}
