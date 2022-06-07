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
    public class HellshotDebuff : ModBuff
    {
        public override string Texture => RegreUtils.BuffPlaceholder;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starshroom Poisoning");
            Description.SetDefault("Losing health");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.RegreGlobalNPCMisc>().hellshot = true;
        }
    }
}