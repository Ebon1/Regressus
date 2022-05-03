/*using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class OracleTimeFreezePlayer : ModPlayer
    {
        public bool StoppedTime;
        public override void ResetEffects()
        {
            StoppedTime = false;
        }
        public override void UpdateEquips()
        {
            if (StoppedTime)
            {
                Player.velocity = Player.oldVelocity;
                Player.position = Player.oldPosition;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlJump = false;
                Player.controlDown = false;
                Player.controlUseItem = false;
                Player.controlUseTile = false;
                Player.controlHook = false;
                Player.controlMount = false;
                if (Player.mount.Active)
                {
                    Player.mount.Dismount(Player);
                }
                Player.moonLeech = true;
                Player.potionDelay = 10;
                Main.NewText("ah shit i cant move");
            }
        }
    }
    public class OracleTimeFreezeNPCs : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool StoppedTime;
        public override void ResetEffects(NPC npc)
        {
            StoppedTime = false;
        }
        public override bool PreAI(NPC npc)
        {
            if (StoppedTime)
            {
                npc.position = npc.oldPosition;
                npc.frameCounter = 0;
                return false;
            }
            return base.PreAI(npc);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (StoppedTime)
                return false;
            return true;
        }
    }
}*/
