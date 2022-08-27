using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Regressus.Buffs.Minions
{
    public class IceLanternB : ModBuff
    {
        public override string Texture => RegreUtils.BuffPlaceholder;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudburst");
            Description.SetDefault("Right click to move the hail to your cursor.");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Weapons.Summon.Minions.IceLanternP>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
