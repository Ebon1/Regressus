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
using Regressus.Projectiles.Pets;

namespace Regressus.NPCs.Bosses.Starshroom
{
    public class StarshroomWitch : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Starshroom Witch");
            Main.npcFrameCount[Type] = 12;
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
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = false;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = false;
            NPC.boss = true;
        }
        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < 11 * frameHeight)
                    NPC.frame.Y += frameHeight;
                else NPC.frame.Y = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AIState == Staff && AITimer < 240 && AITimer > 150)
            {
                Texture2D tex = RegreUtils.GetExtraTexture("MagicCircle_2");
                spriteBatch.Reload(BlendState.Additive);
                Player player = Main.player[NPC.target];
                float progress = Utils.GetLerpValue(0, 90, AITimer3);
                float scale = Math.Clamp((float)(Math.Sin(progress * MathHelper.Pi) * 2), 0, 1);
                spriteBatch.Draw(tex, NPC.Center + (Vector2.UnitX * 50).RotatedBy(RegreUtils.FromAToB(NPC.Center, player.Center).ToRotation()) - screenPos, null, Color.White * scale, RegreUtils.FromAToB(NPC.Center, player.Center).ToRotation(), new Vector2(tex.Width * 0.1f, tex.Height / 2), new Vector2(0.2f, 1f), SpriteEffects.None, 0f);
                spriteBatch.Draw(tex, NPC.Center + (Vector2.UnitX * 50).RotatedBy(RegreUtils.FromAToB(NPC.Center, player.Center).ToRotation()) - screenPos, null, Color.Gold * scale, RegreUtils.FromAToB(NPC.Center, player.Center).ToRotation(), new Vector2(tex.Width * 0.1f, tex.Height / 2), new Vector2(0.2f, 1f), SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }
        }
        /*public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }*/
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,

                new FlavorTextBestiaryInfoElement("Bitch."),
            });
        }
        const int Idle = -1;
        const int Intro = 0;
        const int Sacs = 1;
        const int Staff = 2;
        const int Flail = 3;
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
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        Vector2 center;
        bool flying;
        Vector2 cachePos;
        public override void AI()
        {
            NPC.noGravity = flying;
            NPC.noTileCollide = flying;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (flying)
                NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
            else
                NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
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
                if (NPC.collideY)
                    AITimer++;
                if (AITimer == 1)
                {
                    RegreSystem.ChangeCameraPos(NPC.Center, 180);
                    RegreUtils.SetBossTitle(160, "The Starshroom Witch", Color.Gold, "Spacefaring Nomad", BossTitleStyleID.SSW);
                }
                if (AITimer >= 180)
                {
                    AIState = Flail;
                    AITimer = 0;
                }
            }
            //do idle shit later
            else if (AIState == Sacs)
            {
                AITimer++;
                flying = true;
                if (AITimer < 180 && AITimer > 100)
                    if (AITimer % 10 == 0)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-1, 1f) * 5, 5f), ModContent.ProjectileType<Sporesac>(), 15, 0);
                if (AITimer < 100)
                {
                    cachePos = player.Center;
                    NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - new Vector2(Main.screenWidth / 2 + 500, 200)) * 25;
                }
                else if (AITimer < 180)
                {
                    NPC.velocity = new Vector2(25, 0);
                }
                //if (AITimer == 200 || AITimer == 300)
                //    NPC.Center = player.Center - new Vector2(Main.screenWidth / 2, 200);
                if (AITimer >= 250)
                {
                    NPC.velocity = Vector2.Zero;
                    cachePos = Vector2.Zero;
                    flying = false;
                    AIState = Idle;
                    AITimer = 0;
                }
            }
            else if (AIState == Staff)
            {

                AITimer++;
                AITimer2++;
                if (AITimer == 80)
                {
                    AITimer3 = 90;
                }
                if (AITimer < 80)
                {
                    if (AITimer2 % 5 == 0)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            if (i == 0)
                                continue;
                            float thing = MathHelper.Clamp(AITimer * 0.01f, 0, 1f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, RegreUtils.FromAToB(NPC.Center, player.Center) * 10, ModContent.ProjectileType<SSWStar>(), 15, 0, player.whoAmI, i * thing);
                        }
                    }
                }
                else if (AITimer > 80 && AITimer < 150)
                {
                    if (AITimer2 % 5 == 0)
                        for (int i = -1; i < 2; i++)
                        {
                            if (i == 0)
                                continue;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, RegreUtils.FromAToB(NPC.Center, player.Center) * 10, ModContent.ProjectileType<SSWStarCurve>(), 15, 0, player.whoAmI, i * 3 * ((AITimer - 80) * 0.2f));
                        }
                }
                if (AITimer >= 150) AITimer3--;
                if (AITimer3 == 0 && AITimer > 150)
                {
                    float rot = Main.rand.NextFloat(0, (float)Math.PI * 2);
                    for (float k = 0; k < 6.28f; k += .1f)
                    {
                        Vector2 pos = NPC.Center;
                        float rand = 0;

                        float x = (float)Math.Cos(k + rand);
                        float y = (float)Math.Sin(k + rand);
                        float mult = (Math.Abs((k * (2.5f) % (float)Math.PI) - (float)Math.PI / 2)) + 0.5f;
                        pos += new Vector2(x, y).RotatedBy(rot) * mult * 100;
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, RegreUtils.FromAToB(NPC.Center, player.Center) * 7f, ModContent.ProjectileType<SSWStar>(), 15, 0, player.whoAmI, 0);
                        a.ai[1] = 1;
                    }
                }
                if (AITimer >= 330)
                {
                    NPC.velocity = Vector2.Zero;
                    cachePos = Vector2.Zero;
                    flying = false;
                    AIState = Flail;
                    AITimer = 0;
                }
            }
            else if (AIState == Flail)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BigAssBall>(), 20, 0, player.whoAmI, NPC.whoAmI);
                }
            }
        }
    }
}
