using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vagrant;
using Terraria.DataStructures;
using Regressus.Projectiles;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Oracle;
using Terraria.Audio;

namespace Regressus.NPCs.Bosses.Starshroom
{
    public class StarshroomWitch : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Starshroom Witch");
            //Main.npcFrameCount[Type] = 11;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.5f + bossLifeScale * 0.3f));
        }
        public override void SetDefaults()
        {
            NPC.width = 136;
            NPC.height = 114;
            NPC.lifeMax = 4000;
            /*if (Main.expertMode)
                NPC.lifeMax = 5000;
            if (Main.masterMode)
                NPC.lifeMax = 5900;*/
            NPC.defense = 5;
            NPC.aiStyle = 0;
            NPC.damage = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,

                new FlavorTextBestiaryInfoElement("Bitch."),
            });
        }
        const int Idle = 1;
        const int Intro = 0;
        const int Helix = 1;
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public override void AI()
        {
            if (AITimer == 0)
            {
                AITimer = 1;
                RegreUtils.SetBossTitle(160, "The Starshroom Witch", Color.Gold, "Spacefaring Nomad", BossTitleStyleID.SSW);
            }
        }
    }
}
