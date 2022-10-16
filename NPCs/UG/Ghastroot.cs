using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Minibosses.Vagrant;

namespace Regressus.NPCs.UG
{
    public class Ghastroot : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 20;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 100;
            NPC.defense = 5;
            NPC.Size = new Vector2(104, 86);
            NPC.dontTakeDamage = false;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = RegreUtils.GetExtraTexture("Sprites/Ghastroot_Body");
            const float TwoPi = (float)Math.PI * 2f;
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, NPC.Center - screenPos + (Vector2.UnitX * 5 * scale), null, Color.White * 0.35f * scale, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos + (Vector2.UnitX * -5 * scale), null, Color.White * 0.35f * scale, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos + (Vector2.UnitY * 10 * scale), null, Color.White * 0.35f * scale, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            spriteBatch.Draw(texture, NPC.Center - screenPos + (Vector2.UnitY * -10 * scale), null, Color.White * 0.35f * scale, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (AIState == Idle)
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 0 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 1 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 2 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 3 * frameHeight;
                else if (NPC.frameCounter < 25)
                    NPC.frame.Y = 4 * frameHeight;
                else if (NPC.frameCounter < 30)
                    NPC.frame.Y = 5 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else if (AIState == Suck)
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 19 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 18 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 17 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 16 * frameHeight;
                else if (NPC.frameCounter < 25)
                    NPC.frame.Y = 15 * frameHeight;
                else if (NPC.frameCounter < 30)
                    NPC.frame.Y = 14 * frameHeight;
                else if (NPC.frameCounter < 35)
                    NPC.frame.Y = 13 * frameHeight;
                else if (NPC.frameCounter < 40)
                    NPC.frame.Y = 12 * frameHeight;
                else if (NPC.frameCounter < 45)
                    NPC.frame.Y = 11 * frameHeight;
                else if (NPC.frameCounter < 50)
                    NPC.frame.Y = 10 * frameHeight;
                else if (NPC.frameCounter < 55)
                    NPC.frame.Y = 9 * frameHeight;
                else if (NPC.frameCounter < 60)
                    NPC.frame.Y = 8 * frameHeight;
                else if (NPC.frameCounter < 65)
                    NPC.frame.Y = 7 * frameHeight;
                else if (NPC.frameCounter < 70)
                    NPC.frame.Y = 6 * frameHeight;
                else
                {
                    AIState = Attack;
                    //shouldAttackNow = true;
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 6 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 7 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 8 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 9 * frameHeight;
                else if (NPC.frameCounter < 25)
                    NPC.frame.Y = 10 * frameHeight;
                else if (NPC.frameCounter < 30)
                    NPC.frame.Y = 11 * frameHeight;
                else if (NPC.frameCounter < 35)
                    NPC.frame.Y = 12 * frameHeight;
                else if (NPC.frameCounter < 40)
                    NPC.frame.Y = 13 * frameHeight;
                else if (NPC.frameCounter < 45)
                    NPC.frame.Y = 14 * frameHeight;
                else if (NPC.frameCounter < 50)
                    NPC.frame.Y = 15 * frameHeight;
                else if (NPC.frameCounter < 55)
                    NPC.frame.Y = 16 * frameHeight;
                else if (NPC.frameCounter < 60)
                    NPC.frame.Y = 17 * frameHeight;
                else if (NPC.frameCounter < 65)
                    NPC.frame.Y = 18 * frameHeight;
                else if (NPC.frameCounter < 70)
                    NPC.frame.Y = 19 * frameHeight;
                else
                {
                    AIState = Idle;
                    NPC.frameCounter = 0;
                }
            }
        }
        const int Idle = 0;
        const int Suck = 1;
        const int Attack = 2;
        Vector2 center;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.21f;
            return 0;
        }
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        bool shouldAttackNow;
        Vector2 mouthPos;
        public override void OnKill()
        {
            Color newColor7 = Color.CornflowerBlue;
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 2f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Blue).noGravity = true;
            }
            for (int num901 = 0; num901 < 10; num901++)
            {
                int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 200, default(Color), 2.7f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                Main.dust[num902].noGravity = true;
                Dust dust2 = Main.dust[num902];
                dust2.velocity *= 3f;
                num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                dust2 = Main.dust[num902];
                dust2.velocity *= 2f;
                Main.dust[num902].noGravity = true;
                Main.dust[num902].fadeIn = 2.5f;
            }
            for (int num903 = 0; num903 < 10; num903++)
            {
                int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 0, default(Color), 2.7f);
                Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                Main.dust[num904].noGravity = true;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
            for (int num901 = 0; num901 < 15; num901++)
            {
                int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 200, default(Color), 1.25f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                Main.dust[num902].noGravity = true;
                Main.dust[num902].velocity = RegreUtils.FromAToB(Main.dust[num902].position, mouthPos, reverse: true) * 3.5f;
                Dust dust2 = Main.dust[num902];
                dust2.velocity *= 3f;
            }
            for (int num903 = 0; num903 < 10; num903++)
            {
                int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                Main.dust[num904].noGravity = true;
                Main.dust[num904].velocity = RegreUtils.FromAToB(Main.dust[num904].position, mouthPos, reverse: true) * 4.5f;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
            Gore.NewGore(NPC.GetSource_FromThis(), mouthPos, Vector2.UnitY, ModContent.GoreType<GhastrootGore>());

        }

        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];
            mouthPos = NPC.Center - Vector2.UnitY * 23;
            Lighting.AddLight(NPC.Center, TorchID.UltraBright);
            if (AIState == Idle)
            {
                if (player.Center.Distance(NPC.Center) < 500f)
                    AITimer++;
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    NPC.frameCounter = 0;
                    /*if (shouldAttackNow)
                    {
                        AIState = Attack;
                        shouldAttackNow = false;
                    }
                    else*/
                    AIState = Suck;
                }
            }
            else if (AIState == Suck)
            {
                if (NPC.frameCounter < 25)
                {
                    if (NPC.frameCounter % 5 == 0)
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9);
                    for (int num901 = 0; num901 < 5; num901++)
                    {
                        int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 200, default(Color), 1.25f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width * 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].velocity = RegreUtils.FromAToB(Main.dust[num902].position, mouthPos) * 6.5f;
                        Dust dust2 = Main.dust[num902];
                        dust2.velocity *= 3f;
                        /*num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 100, default(Color), 0.7f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width * 2f;
                        dust2 = Main.dust[num902];
                        dust2.velocity *= 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].fadeIn = 2.5f;
                        Main.dust[num902].velocity = RegreUtils.FromAToB(Main.dust[num902].position, mouthPos) * 6.5f;*/
                    }
                    for (int num903 = 0; num903 < 2; num903++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 15, 0f, 0f, 0, default(Color), 1.25f);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width * 2f;
                        Main.dust[num904].noGravity = true;
                        Main.dust[num904].velocity = RegreUtils.FromAToB(Main.dust[num904].position, mouthPos) * 6.5f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity *= 3f;
                    }
                }
            }
            else if (AIState == Attack)
            {
                /*if (NPC.frameCounter == 20)
                {
                    center = player.Center;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), mouthPos, RegreUtils.FromAToB(NPC.Center, player.Center), ModContent.ProjectileType<Projectiles.Oracle.TelegraphLine>(), 0, 1.5f, player.whoAmI);
                }*/
                if (NPC.frameCounter == 37)
                {
                    float rotation = MathHelper.ToRadians(45);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9);
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 perturbedSpeed = (RegreUtils.FromAToB(NPC.Center, player.Center) * 1.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), mouthPos, perturbedSpeed, ModContent.ProjectileType<Projectiles.Enemy.UG.MagerootProj>(), 15, 1.5f, player.whoAmI, 1);
                    }
                    //Projectile.NewProjectile(NPC.GetSource_FromAI(), mouthPos, RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f, ModContent.ProjectileType<Projectiles.Enemy.UG.MagerootProj>(), 15, 1.5f, player.whoAmI);
                    //Projectile.NewProjectile(NPC.GetSource_FromAI(), mouthPos, RegreUtils.FromAToB(NPC.Center, center), ModContent.ProjectileType<Projectiles.Oracle.OracleBeam>(), 15, 1.5f, player.whoAmI);
                }
            }
        }
    }
}
