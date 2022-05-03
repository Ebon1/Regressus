using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Enemy.Snow;

namespace Regressus.NPCs.Snow
{
    public class SnowballWisp : ModNPC
    {
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSnow && Main.dayTime)
                return 0.42f;
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement(""),
            });
        }
        public override void SetDefaults()
        {
            NPC.height = 34;
            NPC.width = 34;
            NPC.damage = 10;
            NPC.friendly = false;
            NPC.lifeMax = 65;
            NPC.defense = 0;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Idle = 0;
        private const int PlayerFound = 1;
        private const int Attack = 2;
        private const int Roll = 3;
        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }

        public float AITimer
        {
            get => NPC.ai[TimerSlot];
            set => NPC.ai[TimerSlot] = value;
        }
        float distance = 1750f;
        int ballAmnt;
        bool justCollided;
        Projectile[] balls = new Projectile[6];
        public override bool CheckDead()
        {
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SnowBlock, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(NPC.Center, DustID.SnowBlock, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Center, DustID.SnowBlock, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            return true;
        }
        public override void AI()
        {
            ballAmnt = 4;
            if (Main.expertMode && !Main.masterMode)
                ballAmnt = 5;
            else if (Main.masterMode)
                ballAmnt = 6;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if (Main.rand.NextFloat() < 0.11627907f)
            {
                Dust dust;
                Vector2 position = NPC.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width / 2, NPC.height / 2, DustID.SnowBlock, 0f, 0f, 0, new Color(255, 255, 255), 1.3953489f)];
            }
            if (AIState == Idle)
            {
                if (NPC.Center.Distance(player.Center) < distance)
                {
                    AIState = PlayerFound;
                }
            }
            if (AIState == Idle || AIState == PlayerFound || AIState == Attack)
            {
                NPC.rotation = (NPC.Center - player.Center).ToRotation();
                if (NPC.spriteDirection == 1)
                {
                    NPC.rotation -= MathHelper.Pi;
                }
            }
            if (AIState == PlayerFound)
            {
                NPC.velocity *= 0.98f;
                AITimer++;
                if (AITimer >= 100)
                {
                    for (int i = 0; i < ballAmnt; i++)
                    {
                        int _type = Main.rand.Next(3);
                        int type = 0;
                        int damage = 0;
                        switch (_type)
                        {
                            case 0:
                                type = ModContent.ProjectileType<Snowball1>();
                                damage = 10;
                                break;
                            case 1:
                                type = ModContent.ProjectileType<Snowball2>();
                                damage = 3;
                                break;
                            case 2:
                                type = ModContent.ProjectileType<Snowball3>();
                                damage = 5;
                                break;
                        }
                        float angle = 2f * (float)Math.PI / (float)ballAmnt * i;
                        balls[i] = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, type, damage, 1, player.whoAmI, NPC.whoAmI, angle)];
                    }
                    AIState = Attack;
                    AITimer = 0;
                }
            }
            if (AIState == Attack)
            {
                Vector2 moveTo = player.Center - NPC.Center;
                if (NPC.Center.Distance(player.Center) < distance)
                {
                    float factor = 0.015f;
                    NPC.velocity = (moveTo * factor).SafeNormalize(moveTo * factor) * 3.5f;
                }
                AITimer++;
                if (AITimer == 120)
                {
                    for (int i = 0; i < ballAmnt; i++)
                    {
                        if (balls[i].type != ModContent.ProjectileType<Snowball1>() && balls[i].type != ModContent.ProjectileType<Snowball2>() && balls[i].type != ModContent.ProjectileType<Snowball3>())
                            AITimer = 0;
                        if (!balls[i].active || balls[i].localAI[0] == 1)
                            continue;
                        else
                        {
                            balls[i].localAI[0] = 1;
                            balls[i].velocity = (moveTo * 0.032f).SafeNormalize(moveTo * 0.032f) * 12f;
                            break;
                        }
                    }
                    AITimer = 0;
                }
                if (balls[ballAmnt - 1].localAI[0] == 1 || !balls[ballAmnt - 1].active)
                {
                    AITimer = 0;
                    AIState = Roll;
                }
            }
            if (AIState == Roll)
            {
                AITimer--;
                NPC.velocity.X += 0.35f * NPC.direction;
                if (Math.Sign(NPC.velocity.X) != NPC.direction)
                {
                    NPC.velocity.X *= 0.92f;
                }
                if (NPC.collideX && !justCollided && AITimer <= 0)
                {
                    AITimer = 80;
                    justCollided = true;
                }
                if (justCollided)
                {
                    justCollided = false;
                    NPC.velocity.Y -= 5.4f;
                }
                NPC.rotation += NPC.velocity.X * 0.05f;
                NPC.spriteDirection = -NPC.direction;
                NPC.noGravity = false;
            }
        }
    }
}
