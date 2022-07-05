using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Items.Critters;

namespace Regressus.NPCs.Critters
{
    public class EnchantedWisp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Night Wisp");
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("Sometimes the lost souls of the caverns manage to escape the Blind's One grip of terror, and finally find peace on the surface"),
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= 5 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = NPCAIStyleID.Butterfly;
            AIType = NPCID.Butterfly;
            NPC.width = 28;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.npcSlots = 0.5f;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (player.Center.Distance(NPC.Center) <= 200)
            {
                //  NPC.ai[2] = 1;
            }
            NPC.spriteDirection = NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            if (NPC.ai[2] == 0)
            {
                NPC.aiStyle = NPCAIStyleID.Butterfly;
                AIType = NPCID.Butterfly;
            }
            /*else
            {
                NPC.aiStyle = -1;
            AIType = 0;
                NPC.Center = new Vector2(NPC.Center.X + 10, NPC.Center.Y + 0.25f);
            }*/
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !Main.dayTime && Main.hardMode ? 0.25f : 0;
        }
    }
}
