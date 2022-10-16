using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.Effects.Prims;
using System.Security.Cryptography.X509Certificates;

namespace Regressus.Projectiles.Ranged
{
    public class FirestormGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.CritChance = 0;
            Projectile.hostile = false;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[1] = 1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            Projectile.CritChance = 0;
            float progress = Utils.GetLerpValue(0, 100, Projectile.timeLeft);

            float scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            Projectile.rotation += MathHelper.ToRadians(36 * scale);
            Projectile.velocity *= 0.95f;
            if (Projectile.ai[0] % 10 == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 5f * Projectile.ai[1], ModContent.ProjectileType<FirestormGrenade2>(), Projectile.damage, 0f, Projectile.owner, Projectile.whoAmI);
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.FireDust>(), Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, Color.White, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
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
    public class FirestormGrenade2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            owner.ai[1] += 0.25f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            default(FirestormTrail).Draw(Main.projectile[Projectile.whoAmI]);
            Texture2D a = RegreUtils.GetExtraTexture("crosslight");
            Main.spriteBatch.Draw(a, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, a.Size() / 2, 0.25f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(a, Projectile.Center - Main.screenPosition, null, Color.OrangeRed, Projectile.rotation, a.Size() / 2, 0.35f, SpriteEffects.None, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.CritChance = 0;
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            if (owner.type != ModContent.ProjectileType<FirestormGrenade>())
                return;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 move = Vector2.Zero;
            float distance = 5050f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target && Projectile.timeLeft > 45)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (11 * Projectile.velocity + move) / 5f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 11f)
            {
                vector *= 11f / magnitude;
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.CritChance = 0;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.FireDust>(), Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, Color.White, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
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
