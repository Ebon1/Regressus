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

namespace Regressus.Buffs.Debuffs
{
    public class PilgrimBlindness : ModBuff
    {
        public override string Texture => RegreUtils.BuffPlaceholder;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashbanged");
            Description.SetDefault("Your eyes are melting.");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (!Terraria.Graphics.Effects.Filters.Scene["Regressus:Blindness"].IsActive())
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("Regressus:Blindness", player.Center);
            }
            else
            {
                float progress = Utils.GetLerpValue(0, 360, player.buffTime[buffIndex]);
                float scale = MathHelper.Clamp((float)Math.Sin(progress * MathHelper.Pi) * 3, 0, 1);
                Terraria.Graphics.Effects.Filters.Scene["Regressus:Blindness"].GetShader().UseProgress((scale) * 8);
            }
            Terraria.Graphics.Effects.Filters.Scene["Regressus:Blindness"].GetShader().UseTargetPosition(player.Center);
        }
    }
}
