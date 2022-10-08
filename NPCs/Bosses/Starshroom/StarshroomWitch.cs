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
using Regressus.Projectiles.SSW;
using static Terraria.ModLoader.ModContent;

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
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
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
        Vector2 center;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (Main.dayTime)
            {
                AIState = -69420;
                NPC.velocity = new Vector2(0, -20f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            if (AIState == Intro)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    RegreUtils.SetBossTitle(160, "The Starshroom Witch", Color.Gold, "Spacefaring Nomad", BossTitleStyleID.SSW);
                }
                if (AITimer >= 180)
                {
                    AIState = Helix;
                    AITimer = 0;
                }
            }
            //do idle shit later
            else if (AIState == Helix)
            {
                AITimer++;
                switch (AITimer)
                {
                    case 30:
                        RegreUtils.SpawnTelegraphLine(NPC.Center, NPC.GetSource_FromAI(), RegreUtils.FromAToB(NPC.Center, player.Center));
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 100:
                        RegreUtils.SpawnTelegraphLine(NPC.Center, NPC.GetSource_FromAI(), RegreUtils.FromAToB(NPC.Center, player.Center));
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 170:
                        RegreUtils.SpawnTelegraphLine(NPC.Center, NPC.GetSource_FromAI(), RegreUtils.FromAToB(NPC.Center, player.Center));
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 250:
                        RegreUtils.SpawnTelegraphLine(NPC.Center, NPC.GetSource_FromAI(), RegreUtils.FromAToB(NPC.Center, player.Center));
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                }
                if (++AITimer2 >= 5 && AITimer >= 30)
                {
                    AITimer2 = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        if (i == 0)
                            continue;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, RegreUtils.FromAToB(NPC.Center, center) * 19f, ProjectileType<SSWStar>(), 15, 0, player.whoAmI, i);
                    }
                }
                if (AITimer >= 330)
                {
                    AIState = Intro;
                    AITimer = 0;
                }
            }
        }
    }
}
