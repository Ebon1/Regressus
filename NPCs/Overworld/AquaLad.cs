using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.NPCs.Overworld
{
    public class AquaLad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Pupfish);
            NPC.Size = new Microsoft.Xna.Framework.Vector2(54, 36);
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override bool PreAI()
        {
            NPC.spriteDirection = NPC.velocity.X < 0 ? 1 : -1;
            return true;
        }
        public override void PostAI()
        {
            NPC.spriteDirection = NPC.velocity.X < 0 ? 1 : -1;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.wet)
                NPC.frame.Y = 0;
            else
                NPC.frame.Y = frameHeight;
        }
    }
}
