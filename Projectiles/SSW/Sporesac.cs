using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace Regressus.Projectiles.SSW
{
    public class Sporesac : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sporesac");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 40;
            Projectile.aiStyle = 2;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 350;
            Projectile.tileCollide = true;

        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MushExplosion>(), 15, 0);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MushExplosion>(), 15, 0);
                float progress = Utils.GetLerpValue(0, 350, Projectile.timeLeft);
                float speed = MathHelper.Lerp(3, 8, progress) * Main.rand.NextFloat(0.795f, 1f);
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity = -oldVelocity * 0.9f;
                else
                    Projectile.velocity = new Vector2(Projectile.direction * speed, speed * -2.5f);
                return false;
            }
            return true;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -20).RotatedBy(Projectile.rotation), DustID.Enchanted_Gold);
        }
    }
    public class MushExplosion : ModProjectile
    {

        public override string Texture => RegreUtils.Empty;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 65;
            Projectile.height = 65;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 26;
        }
        float AAA;
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.ColoredFireDust>(), Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, Color.Gold, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
            }
            for (int num905 = 0; num905 < 10; num905++)
            {
                int num906 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default(Color), 2.5f);
                Main.dust[num906].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Main.dust[num906].noGravity = true;
                Dust dust2 = Main.dust[num906];
                dust2.velocity *= 3f;
            }
            for (int num899 = 0; num899 < 4; num899++)
            {
                int num900 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num900].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
            }
        }

    }
}
