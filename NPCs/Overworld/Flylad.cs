using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace Regressus.NPCs.Overworld
{
    public class Flylad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetDefaults()
        {
            NPC.height = 90;
            NPC.width = 68;
            NPC.damage = 10;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.friendly = false;
            NPC.lifeMax = 200;
            NPC.defense = 2;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }
        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < frameHeight * 5)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;
            }
        }
        public override bool CheckDead()
        {
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 58, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 7; num616++)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
            return true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.AnyNPCs(ModContent.NPCType<Starlad>()) /*&& ssw event check here*/ ? 0.3f : 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, TorchID.UltraBright);
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if (NPC.ai[0] == 0)
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.type == ModContent.NPCType<Starlad>() && npc.Center.Distance(player.Center) < 700)
                        NPC.ai[0] = 1;
                }
            if (!NPC.AnyNPCs(ModContent.NPCType<Starlad>()))
                NPC.ai[0] = 0;
            if (NPC.ai[0] == 1)
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - Vector2.UnitY * 100, false) * 0.065f;
            else
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - Vector2.UnitY, true) * 3;
        }
    }
}
