using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Regressus.NPCs.Bosses.Oracle;

namespace Regressus.Buffs.Debuffs
{
    public class TimeReverse : ModBuff
    {
        public override string Texture => RegreUtils.BuffPlaceholder;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Time Reverse");
            Description.SetDefault("Going back in time");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.mount.Active)
                player.mount.Dismount(player);

            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlDown = false;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlHook = false;
            player.controlMount = false;
            player.GetModPlayer<RegrePlayer>().reverseTime = true;
            /*if (player.buffTime[buffIndex] == 250)
            {
                if (!Main.dedServ)
                    Terraria.Audio.SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/Reverse"));
            }*/
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.GetGlobalNPC<NPCs.RegreGlobalNPC>().reverseTime = true;
        }
    }
}