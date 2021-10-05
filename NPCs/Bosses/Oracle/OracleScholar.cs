using Terraria;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Regressus.Projectiles.Oracle;
using Terraria.ModLoader;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class OracleScholar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            DisplayName.SetDefault("Scholar");
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 80;
            NPC.aiStyle = 0;
            NPC.damage = 0;
            NPC.defense = 1;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        private const int AISlot = 0;
        private const int TimerSlot = 1;

        private const int Idle = 0;
        private const int Teleport = 1;
        private const int SkyFracture = 2;
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

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (AIState == 0)
            {
                AITimer++;
                if (AITimer >= 230)
                {
                    AITimer = 0;
                    AIState = 1;
                }
            }
            else if (AIState == 1)
            {
                if (AITimer == 0)
                {
                    NPC.Center = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y + Main.screenHeight * Main.rand.NextFloat());
                    AITimer = 1;
                }
            }
            else if (AIState == 2)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    float num167 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                    Vector2 velocity = NPC.DirectionTo(player.Center) * 1.1f;
                    Vector2 value17 = new Vector2(1, 1).RotatedBy(num167) * (0.95f + Main.rand.NextFloat() * 0.3f);
                    Projectile pruh = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center + value17 * 30f, velocity, ModContent.ProjectileType<AmongUsRunes>(), 8, 0, 0, 0f)];
                    pruh.friendly = false;
                    pruh.hostile = true;
                    pruh.localAI[0] = 1;
                    pruh.localAI[1] = Main.rand.Next(10);
                }
                if (AITimer == 6)
                {
                    float num167 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                    Vector2 velocity = NPC.DirectionTo(player.Center) * 1.1f;
                    Vector2 value17 = new Vector2(1, 1).RotatedBy(num167) * (0.95f + Main.rand.NextFloat() * 0.3f);
                    Projectile pruh = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center + value17 * 30f, velocity, ModContent.ProjectileType<AmongUsRunes>(), 8, 0, 0, 0f)];
                    pruh.friendly = false;
                    pruh.hostile = true;
                    pruh.localAI[0] = 1;
                    pruh.localAI[1] = Main.rand.Next(10);
                }
                if (AITimer == 12)
                {
                    float num167 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                    Vector2 velocity = NPC.DirectionTo(player.Center) * 1.1f;
                    Vector2 value17 = new Vector2(1, 1).RotatedBy(num167) * (0.95f + Main.rand.NextFloat() * 0.3f);
                    Projectile pruh = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center + value17 * 30f, velocity, ModContent.ProjectileType<AmongUsRunes>(), 8, 0, 0, 0f)];
                    pruh.friendly = false;
                    pruh.hostile = true;
                    pruh.localAI[0] = 1;
                    pruh.localAI[1] = Main.rand.Next(10);
                }
                if (AITimer == 18)
                {
                    float num167 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                    Vector2 velocity = NPC.DirectionTo(player.Center) * 1.1f;
                    Vector2 value17 = new Vector2(1, 1).RotatedBy(num167) * (0.95f + Main.rand.NextFloat() * 0.3f);
                    Projectile pruh = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center + value17 * 30f, velocity, ModContent.ProjectileType<AmongUsRunes>(), 8, 0, 0, 0f)];
                    pruh.friendly = false;
                    pruh.hostile = true;
                    pruh.localAI[0] = 1;
                    pruh.localAI[1] = Main.rand.Next(10);
                }

                if (AITimer >= 20)
                {
                    AITimer = 0;
                    AIState = 0;
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (AIState != 1)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 2 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 3 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 4 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 5 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 6 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 7 * frameHeight;
                }
                else if (NPC.frameCounter < 25)
                {
                    NPC.frame.Y = 8 * frameHeight;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = 9 * frameHeight;
                }
                else if (NPC.frameCounter < 35)
                {
                    NPC.frame.Y = 10 * frameHeight;
                }
                else if (NPC.frameCounter < 40)
                {
                    NPC.frame.Y = 11 * frameHeight;
                }
                else if (NPC.frameCounter < 45)
                {
                    NPC.frame.Y = 12 * frameHeight;
                }
                else if (NPC.frameCounter < 50)
                {
                    NPC.frame.Y = 13 * frameHeight;
                }
                else
                {
                    AITimer = 0;
                    AIState = 2;
                }
            }
        }
    }
}