using Terraria;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class OracleDeathDrama : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Oracle");
            Main.npcFrameCount[NPC.type] = 21;
        }

        public override void SetDefaults()
        {
            NPC.width = 90;
            NPC.height = 104;
            NPC.damage = 0;
            NPC.dontTakeDamage = true;
            NPC.defense = 6;
            NPC.lifeMax = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter < 5)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 10)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 15)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 20)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            else if (NPC.frameCounter < 25)
            {
                NPC.frame.Y = 4 * frameHeight;
            }
            else if (NPC.frameCounter < 30)
            {
                NPC.frame.Y = 5 * frameHeight;
            }
            else if (NPC.frameCounter < 35)
            {
                NPC.frame.Y = 6 * frameHeight;
            }
            else if (NPC.frameCounter < 40)
            {
                NPC.frame.Y = 7 * frameHeight;
            }
            else if (NPC.frameCounter < 45)
            {
                NPC.frame.Y = 8 * frameHeight;
            }
            else if (NPC.frameCounter < 50)
            {
                NPC.frame.Y = 9 * frameHeight;
            }
            else if (NPC.frameCounter < 55)
            {
                NPC.frame.Y = 10 * frameHeight;
            }
            else if (NPC.frameCounter < 60)
            {
                NPC.frame.Y = 11 * frameHeight;
            }
            else if (NPC.frameCounter < 65)
            {
                NPC.frame.Y = 12 * frameHeight;
            }
            else if (NPC.frameCounter < 70)
            {
                NPC.frame.Y = 13 * frameHeight;
            }
            else if (NPC.frameCounter < 75)
            {
                NPC.frame.Y = 14 * frameHeight;
            }
            else if (NPC.frameCounter < 80)
            {
                NPC.frame.Y = 15 * frameHeight;
            }
            else if (NPC.frameCounter < 85)
            {
                NPC.frame.Y = 16 * frameHeight;
            }
            else if (NPC.frameCounter < 90)
            {
                NPC.frame.Y = 17 * frameHeight;
            }
            else if (NPC.frameCounter < 95)
            {
                NPC.frame.Y = 18 * frameHeight;
            }
            else if (NPC.frameCounter < 100)
            {
                NPC.frame.Y = 19 * frameHeight;
            }
            else if (NPC.frameCounter < 105)
            {
                NPC.frame.Y = 20 * frameHeight;
            }
            else
            {
                NPC.life = 0;
            }
        }
    }
}