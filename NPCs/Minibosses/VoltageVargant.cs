using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vargant;
using Terraria.DataStructures;
using Regressus.Projectiles;
using ReLogic.Content;

namespace Regressus.NPCs.Minibosses
{
    public class VoltageVargant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 114;
            NPC.lifeMax = 1000;
            NPC.defense = 5;
            NPC.aiStyle = 0;
            NPC.damage = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                if (NPC.frame.Y < frameHeight * 4)
                {
                    NPC.frame.Y += frameHeight;
                }
                else
                {
                    NPC.frame.Y = 0;
                }
                NPC.frameCounter = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowTex = RegreUtils.GetTexture("NPCs/Minibosses/VoltageVargant_Glow");
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            spriteBatch.Draw(glowTex, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            return false;
        }
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Idle = 0;
        private const int Hail = 1;
        private const int Rain = 2;
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
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.025f);
                if (AITimer >= 180)
                {
                    AIState = Hail;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            if (AIState == Hail)
            {
                AITimer++;
                if (++AITimer2 >= 10)
                {
                    AITimer2 = 0;
                    int hail = 0;
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            hail = ModContent.ProjectileType<Hail1>();
                            break;
                        case 1:
                            hail = ModContent.ProjectileType<Hail2>();
                            break;
                        case 2:
                            hail = ModContent.ProjectileType<Hail3>();
                            break;
                    }
                    Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(2 * Main.windSpeedCurrent, 10), hail, 15, 0, Main.myPlayer);
                }
                if (AITimer >= 180)
                {
                    AIState = Rain;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            if (AIState == Rain)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center - Vector2.UnitY * 450 + Vector2.UnitX * (50 * player.direction), 0.035f);
                if (++AITimer2 >= 5)
                {
                    AITimer2 = 0;
                    int rain = 0;
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            rain = ModContent.ProjectileType<Rain1>();
                            break;
                        case 1:
                            rain = ModContent.ProjectileType<Rain2>();
                            break;
                        case 2:
                            rain = ModContent.ProjectileType<Rain3>();
                            break;
                    }
                    Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-20, 20), 0), new Vector2(0, 6), rain, 10, 0, Main.myPlayer);
                }
                if (AITimer >= 180)
                {
                    AIState = Idle;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
        }
    }
}