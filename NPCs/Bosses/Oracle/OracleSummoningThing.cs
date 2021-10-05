using Terraria;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Regressus.Projectiles.Oracle;
using Terraria.ModLoader;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class OracleSummoningThing : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
        }
        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 38;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 1;
            NPC.lifeMax = 1000;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }
        public override bool CheckDead()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<OracleBoss>()))
            {
                NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OracleBoss>());
                Main.NewText("The Oracle has awoken!", new Color(175, 75, 255));
            }
            return true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3 && spawnInfo.player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<OracleSummoningThing>()) && !NPC.AnyNPCs(ModContent.NPCType<OracleBoss>()))
            {
                return .05f;
            }
            else
            {
                return 0;
            }
        }
    }
}