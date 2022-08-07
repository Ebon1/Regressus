using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;

namespace Regressus.NPCs.UG
{
    public class Mossling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.25f;
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("To protect themselves from daemonic corruption, The Mosslings have evolved regenerative capabilities that make them near invincibile"),
            });
        }
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 56;
            NPC.lifeMax = 350;
            NPC.defense = 20;
            NPC.damage = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (AIState == Run)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 3)
                    NPC.frame.Y = 0 * frameHeight;
                else if (NPC.frameCounter < 6)
                    NPC.frame.Y = 1 * frameHeight;
                else if (NPC.frameCounter < 9)
                    NPC.frame.Y = 2 * frameHeight;
                else if (NPC.frameCounter < 12)
                    NPC.frame.Y = 3 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 4 * frameHeight;
                else if (NPC.frameCounter < 18)
                    NPC.frame.Y = 5 * frameHeight;
                else if (NPC.frameCounter < 21)
                    NPC.frame.Y = 6 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else if (AIState == Idle && NPC.velocity != Vector2.Zero)
            {
                NPC.frameCounter++;
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
                else if (NPC.frameCounter < 35)
                    NPC.frame.Y = 6 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else
                NPC.frame.Y = 0 * frameHeight;
        }
        private const int AISlot = 0;
        private const int Idle = 0;
        private const int Run = 1;
        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }
        public override bool CheckDead()
        {
            for (int i = 0; i < 30; i++)
            {
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, 910, 0.7f + Main.rand.NextFloat() * 0.6f);
            }
            return true;
        }
        public override void AI()
        {
            if (NPC.life != NPC.lifeMax)
            {
                AIState = Run;
            }
            if (AIState == Idle)
            {
                NPC.ai[1]--;
                NPC.aiStyle = -1;
                NPC.direction = Main.player[NPC.target].Center.X > NPC.Center.X ? 1 : -1;
                NPC.spriteDirection = NPC.direction;
                if (NPC.Center.Distance(Main.player[NPC.target].Center) > 90f)
                {
                    if (NPC.collideY)
                        NPC.velocity.X += 0.025f * NPC.direction;
                    else
                        NPC.velocity.X += 0.005f * NPC.direction;
                }
                if (Math.Sign(NPC.velocity.X) != NPC.direction || NPC.Center.Distance(Main.player[NPC.target].Center) < 90f)
                {
                    NPC.velocity.X *= 0.92f;
                }
                if (NPC.collideX && NPC.ai[2] != 1 && NPC.ai[1] <= 0)
                {
                    NPC.ai[1] = 30;
                    NPC.ai[2] = 1;
                }
                if (NPC.ai[2] == 1)
                {
                    NPC.ai[2] = 0;
                    NPC.velocity.Y -= 6.6f;
                }
            }
            else if (AIState == Run)
            {
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.life += 10;
                    CombatText.NewText(NPC.getRect(), CombatText.HealLife, 10);
                }
                NPC.ai[1]--;
                NPC.aiStyle = -1;
                NPC.direction = Main.player[NPC.target].Center.X < NPC.Center.X ? 1 : -1;
                NPC.spriteDirection = NPC.direction;
                if (NPC.collideY)
                    NPC.velocity.X += 0.175f * NPC.direction;
                else
                    NPC.velocity.X += 0.075f * NPC.direction;
                if (Math.Sign(NPC.velocity.X) != NPC.direction)
                {
                    NPC.velocity.X *= 0.92f;
                }
                if (NPC.collideX && NPC.ai[2] != 1 && NPC.ai[1] <= 0)
                {
                    NPC.ai[1] = 30;
                    NPC.ai[2] = 1;
                }
                if (NPC.ai[2] == 1)
                {
                    NPC.ai[2] = 0;
                    NPC.velocity.Y -= 6.6f;
                }
            }
        }
    }
}
