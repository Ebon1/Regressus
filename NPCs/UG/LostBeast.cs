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
    public class LostBeast : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight)
                return 0.25f;
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("This blind beast was once a human, but after the Blind One corrupted the caverns it's an apex predator with an unsatiable hunger.\nCan easily be distracted."),
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 142;
            NPC.height = 82;
            NPC.lifeMax = 150;
            NPC.defense = 10;
            NPC.damage = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
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
                else
                    NPC.frameCounter = 0;
            }
        }
        private const int Distracted = -1;
        private const int Walk = 0;
        private const int Sniff = 1;
        private const int Alerted = 2;
        private const int Attack = 3;
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
        public override void AI()
        {
            if (AIState == Walk)
            {

            }
        }
    }
}
