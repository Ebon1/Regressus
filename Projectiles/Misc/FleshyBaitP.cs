using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Regressus.Projectiles.Misc
{
    public class FleshyBaitP : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
            DisplayName.SetDefault("Fleshy Bait");
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.Size = new(22, 13);
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 4;
            NPC.aiStyle = -1;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * (NPC.life - 1);
        }
        public override void AI()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.life > 0 && npc.damage > 0 && npc.lifeMax < 300 && !npc.friendly)
                {
                    if (npc.aiStyle == NPCAIStyleID.Bat || npc.aiStyle == NPCAIStyleID.Fighter || npc.aiStyle == NPCAIStyleID.Slime || npc.aiStyle == NPCAIStyleID.Unicorn)
                    {
                        if (npc.velocity.Y < 1)
                            npc.velocity.X = RegreUtils.FromAToB(npc.Center, NPC.Center).X * 5f;
                        npc.spriteDirection = npc.direction = RegreUtils.FromAToB(npc.Center, NPC.Center).X > 0 ? 1 : -1;
                        if (npc.Center.Distance(NPC.Center) < npc.width)
                        {

                            npc.velocity *= 0.01f;
                            NPC.ai[0]++;
                            for (int i = 0; i < 2; i++)
                            {
                                Dust.NewDust(NPC.Center, 22, 13, DustID.Blood, Main.rand.NextFloat(-0.4f, .4f), Main.rand.NextFloat(-0.4f, .4f));
                                Dust.NewDust(NPC.Center, 22, 13, DustID.GreenBlood, Main.rand.NextFloat(-0.4f, .4f), Main.rand.NextFloat(-0.4f, .4f));
                            }
                            if (NPC.ai[0] >= 160)
                            {
                                NPC.life--;
                                NPC.ai[0] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
