using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Regressus.Projectiles.Oracle;
using Terraria.DataStructures;
using Regressus.Projectiles;
using ReLogic.Content;
using Regressus.Buffs.Debuffs;
using ReLogic.Graphics;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class TheOracle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 202;
            NPC.height = 304;
            NPC.lifeMax = 150000;
            NPC.damage = 0;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.buffImmune[24] = true;
            NPC.noTileCollide = true;
            NPC.defense = 65;
        }
        public override void FindFrame(int frameHeight)
        {
            int frameWidth = 202;
            NPC.frame.Width = frameWidth;
            NPC.frameCounter++;
            if (!phase2)
            {
                NPC.frame.Y = 0 * frameHeight;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.X = 0 * frameWidth;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.X = 1 * frameWidth;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.X = 2 * frameWidth;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.X = 3 * frameWidth;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (phase2 && AIState != Transform)
            {
                NPC.frame.Y = frameHeight;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.X = 0 * frameWidth;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.X = 1 * frameWidth;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.X = 2 * frameWidth;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.X = 3 * frameWidth;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        float minuteHandRot, hourHandRot, clockAlpha;
        int theFinalCountdown = (3600 * 4) + (60 * 20);
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color lightColor)
        {
            if (arenaCenter != Vector2.Zero)
            {
                if (!Main.player[NPC.target].HasBuff(ModContent.BuffType<TimeReverse>()))
                {
                    minuteHandRot += 0.1666668f / (4 * 8);
                    hourHandRot += 0.0138889f / (4 * 8);
                }
                else
                {
                    minuteHandRot -= 0.1666668f / (4 * 8);
                    hourHandRot -= 0.0138889f / (4 * 8);
                }
                RegreUtils.Reload(spriteBatch, BlendState.Additive);
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
                float scale = 2 * mult;
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, arenaCenter - pos, null, Color.White * clockAlpha, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, 2.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, arenaCenter - pos, null, Color.White * clockAlpha, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, .25f, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/PulseCircle").Value, arenaCenter - pos, null, Color.White * clockAlpha, 0, ModContent.Request<Texture2D>("Regressus/Extras/PulseCircle").Value.Size() / 2, 2f * 3.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value, arenaCenter - pos, null, Color.White * clockAlpha, MathHelper.ToRadians(hourHandRot), ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value.Size() / 2, 2.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value, arenaCenter - pos, null, Color.White * clockAlpha, MathHelper.ToRadians(minuteHandRot), ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value.Size() / 2, 2.5f, SpriteEffects.None, 0);
                for (int i = 0; i < 3; i++)
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/vortex2").Value, arenaCenter - pos, null, Color.DarkViolet * clockAlpha, -Main.GameUpdateCount * 0.0075f, new Vector2(1230, 1264) / 2, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/vortex").Value, arenaCenter - pos, null, Color.DarkViolet * clockAlpha, -Main.GameUpdateCount * 0.005f, new Vector2(256, 256) / 2, scale * 2, SpriteEffects.None, 0f);
                RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
            }
            if (!NPC.hide)
            {
                if (AIState != Transform && AIState != PreIntro)
                {
                    Vector2 drawOrigin = new Vector2((ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value.Width / 2) * 0.5F, (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value.Height / Main.npcFrameCount[NPC.type]) * 0.5F);
                    Vector2 drawPos = new Vector2(
                        NPC.position.X - pos.X + (NPC.width / 4) - (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value.Width / 4) * NPC.scale / 4 + drawOrigin.X * NPC.scale,
                        NPC.position.Y - pos.Y + NPC.height - ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + drawOrigin.Y * NPC.scale + NPC.gfxOffY
                        );
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value, drawPos, NPC.frame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle_Glow").Value, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    if (AIState != Death)
                    {
                        RegreUtils.Reload(spriteBatch, BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle_Glow2").Value, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                        RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
                    }
                }
                if (AIState == Transform)
                {
                    Vector2 drawOrigin = new Vector2((ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform").Value.Width / 2) * 0.5F, (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracle").Value.Height / 5) * 0.5F);
                    Vector2 drawPos = new Vector2(
                        NPC.position.X - pos.X + (NPC.width / 5) - (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform").Value.Width / 5) * NPC.scale / 5 + drawOrigin.X * NPC.scale,
                        NPC.position.Y - pos.Y + NPC.height - ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform").Value.Height * NPC.scale / 5 + 4f + drawOrigin.Y * NPC.scale + NPC.gfxOffY
                        );
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform").Value, drawPos, transformFrame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform_Glow").Value, drawPos, transformFrame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    if (AIState != Death)
                    {
                        RegreUtils.Reload(spriteBatch, BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleTransform_Glow2").Value, drawPos, transformFrame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                        RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
                    }
                }
                if (AIState == PreIntro)
                {
                    Vector2 drawOrigin = new Vector2((ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut").Value.Width / 5) * 0.5F, (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut").Value.Height / 6) * 0.5F);
                    Vector2 drawPos = new Vector2(
                        NPC.position.X - pos.X + (NPC.width / 5) - (ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut").Value.Width / 5) * NPC.scale / 5 + drawOrigin.X * NPC.scale,
                        NPC.position.Y - pos.Y + NPC.height - ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut").Value.Height * NPC.scale / 6 + 4f + drawOrigin.Y * NPC.scale + NPC.gfxOffY
                        );
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut").Value, drawPos, savantFrame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    /*RegreUtils.Reload(spriteBatch, BlendState.Additive);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/TheOracleFakeOut_Glow").Value, drawPos, savantFrame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                    RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend); }*/
                }
            }
            return false;
        }
        public Vector2 arenaCenter;
        float arenaScale;
        bool glass;
        float glassFade;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 pos, Color drawColor)
        {
            if (phase2)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                Texture2D vignette1 = ModContent.Request<Texture2D>("Regressus/Extras/Vignette").Value;
                spriteBatch.Draw(vignette1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.DarkViolet);
                Main.spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage >= NPC.life)
            {
                NPC.life = NPC.lifeMax;
                damage = 0;
                NPC.dontTakeDamage = true;
                AIState = Death;
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            NPC center = Main.npc[(int)NPC.ai[0]];
            if (damage >= NPC.life)
            {
                NPC.life = NPC.lifeMax;
                damage = 0;
                NPC.dontTakeDamage = true;
                AIState = Death;
            }
        }
        public override bool CheckDead()
        {
            if (deathTimer2 != 1)
            {
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = true;
                AIState = Death;
            }
            return deathTimer2 == 1;
        }
        float transformFrameCounter;
        Rectangle transformFrame;
        Rectangle savantFrame;
        Rectangle savantP2Frame;
        Rectangle savantP2TransitionFrame;
        float savantFrameCounter;
        int currentTexture;
        float deathTimer, deathTimer2;
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int TimerSlot2 = 2;
        private const int AISlot2 = 3;
        bool phase2 = false;
        public static bool _phase2;
        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }

        public float AITimer
        {
            get => NPC.ai[TimerSlot];
            set => NPC.ai[TimerSlot] = value;
        }

        public float AITimer2
        {
            get => NPC.ai[TimerSlot2];
            set => NPC.ai[TimerSlot2] = value;
        }
        public float AIValue
        {
            get => NPC.ai[AISlot2];
            set => NPC.ai[AISlot2] = value;
        }
        /*
        - ball bash (make sure to make them act smart)
        - giga ray (balls start moving fast around him and he shoots big ray)
        - chrono glyphs (rune attack)
        - time stop+chrono glyphs
        - 1.if you want to keep oracle circle make the orbs follow around him, have him speed up his orbit thing then stop and fire rapid projectiles from the orbs to the middle of the circling area, where the player can flee
        - 2. use the rest of the oracle for attacks, it has an eye and a giant crystal, you can definitely use those for different things
        - ball ray
        - 3. maybe have him fire a multitude of projectiles from his 3 jet things downards, which the projectiles soon rise upwards
        either in bursts or in a constant fire
        - 4. make the crystal pulsate brightly and slow doen time significantly before creating different rings of projectiles around itself, with holes towards the player, although the holes shift a bit every ring
        have time slowly go back to normal as the player dodged the rings
        - 5. make the oracle get above the player and make the orbs slowly spirak to his middle before pulling them away quickly, opening big rift thingy that sucks the player in and possibly uses the tentacle shaders you learnt to try to swipe at the player ( or possibly spiral)

        - 6. make it align the orbs above the player and fire railguns downards, make it repeat this like 4 times rapidly
        - 7. have it create a big glyph around itself, with its orbs circling around it rapidly before having it shoot a large continuous deathray that slowly follows you alongside a burst of projectiles all around

         the bg light pillars fade out right before the attack then fade back in violently (maybe in different places :)
         */
        public const int Death = -3;
        public const int Transform = -1;
        public const int Intro = -2;
        private const int MusicIntroSync = -4;
        public const int PreIntro = 0;
        private const int OrbBlast = 1;
        private const int BeamStar = 2;
        private const int Railgun = 3;
        private const int GroundLasers = 4;
        bool hasTransformed;
        float rotAngle;
        NPC crystal;
        Vector2[] random = new Vector2[7];
        public static int _finalCountdown;
        public override void AI()
        {
            #region "random stuff"
            Player player = Main.player[NPC.target];
            if (AIState != PreIntro && !phase2)
            {
                if (!Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].IsActive())
                {
                    Terraria.Graphics.Effects.Filters.Scene.Activate("Regressus:OracleSummon", NPC.Center);
                }
                else
                {
                    Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].GetShader().UseProgress(0.5f);
                }
                Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].GetShader().UseTargetPosition(NPC.Center);
            }
            if (phase2)
            {
                theFinalCountdown--;
                if (Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].IsActive())
                {
                    Terraria.Graphics.Effects.Filters.Scene.Deactivate("Regressus:OracleSummon");
                }
            }
            _finalCountdown = theFinalCountdown;
            if (NPC.life <= NPC.lifeMax / 2 && !hasTransformed)
            {
                AIState = -9;
                AITimer = 0;
                if (crystal != null)
                    crystal.localAI[0] = 0;
                hasTransformed = true;
            }
            _phase2 = phase2;
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            if (phase2 && AIState != Transform)
            {
                if (player.Center.Distance(arenaCenter) >= (1322 / 2) * (2.5f))
                {
                    RegreUtils.TPNoDust(arenaCenter, player);
                    //player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " left the boundaries."), 999999999, 0);
                }
            }
            if (player.chaosState && player.ownedProjectileCounts[ModContent.ProjectileType<OracleRoDRift>()] < 1)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), (Main.MouseWorld.Distance(arenaCenter) < (1322 / 2) * 2.5f ? Main.MouseWorld : arenaCenter), Vector2.Zero, ModContent.ProjectileType<OracleRoDRift>(), 0, 0, player.whoAmI);
            }
            if (!phase2 && !NPC.AnyNPCs(ModContent.NPCType<OracleCrystal>()) && AIState != PreIntro && AIState != Transform)
                crystal = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OracleCrystal>(), ai0: NPC.whoAmI, Target: player.whoAmI)];

            #endregion
            if (AIState == Intro)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<OracleOrbs>()] <= 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OracleOrbs>(), 0, 0, Main.myPlayer, NPC.whoAmI)];
                            projectile.localAI[0] = i;
                            projectile.localAI[1] = crystal.whoAmI;
                        }
                    }
                }
                if (AITimer == 2)
                {
                    RegreSystem.ChangeCameraPos(NPC.Center, 215, 1.5f);
                    RegreUtils.SetBossTitle(215, "The Oracle", Color.DarkCyan, "--Warden of Time--", BossTitleStyleID.Oracle);
                }
                if (AITimer >= 220)
                {
                    AITimer = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIState = MusicIntroSync;
                    NPC.dontTakeDamage = false;
                }
            }
            else if (AIState == PreIntro)
            {
                #region "Pre Intro"
                NPC.dontTakeDamage = true;
                int frameWidth = 202, frameHeight = 306;
                savantFrame.Width = frameWidth;
                savantFrame.Height = frameHeight;
                savantFrameCounter++;
                AITimer++;
                if (AITimer > 50)
                {
                    RegreSystem.ChangeCameraPos(NPC.Center, 10, 1.25f);
                }
                if (AITimer == 1)
                {
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oldOracle");
                    NPC.Center = new Vector2(player.position.X, player.position.Y - 200);
                    RegreSystem.ChangeCameraPos(NPC.Center, 50, 1);
                    RegreUtils.SetBossTitle(120, "The Oracle", Color.LightSkyBlue, "--Construct of Insight--");
                }
                if (savantFrameCounter < 5)
                {
                    savantFrame.Y = 0;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 10)
                {
                    savantFrame.Y = 0;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 15)
                {
                    savantFrame.Y = 0;
                    savantFrame.X = 2 * frameWidth;
                }
                else if (savantFrameCounter < 20)
                {
                    savantFrame.Y = 0;
                    savantFrame.X = 3 * frameWidth;
                }
                else if (savantFrameCounter < 25)
                {
                    savantFrame.Y = 0;
                    savantFrame.X = 4 * frameWidth;
                }
                else if (savantFrameCounter < 30)
                {
                    savantFrame.Y = frameHeight;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 35)
                {
                    savantFrame.Y = frameHeight;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 40)
                {
                    savantFrame.Y = frameHeight;
                    savantFrame.X = 2 * frameWidth;
                    if (AITimer2 < 4)
                    {
                        savantFrameCounter = 0;
                        AITimer2++;
                    }
                }
                else if (savantFrameCounter < 45)
                {
                    savantFrame.Y = frameHeight;
                    savantFrame.X = 3 * frameWidth;
                }
                else if (savantFrameCounter < 50)
                {
                    savantFrame.Y = frameHeight;
                    savantFrame.X = 4 * frameWidth;
                }
                else if (savantFrameCounter < 55)
                {
                    savantFrame.Y = 2 * frameHeight;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 60)
                {
                    savantFrame.Y = 2 * frameHeight;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 65)
                {
                    savantFrame.Y = 2 * frameHeight;
                    savantFrame.X = 2 * frameWidth;
                }
                else if (savantFrameCounter < 70)
                {
                    savantFrame.Y = 2 * frameHeight;
                    savantFrame.X = 3 * frameWidth;
                }
                else if (savantFrameCounter < 75)
                {
                    savantFrame.Y = 2 * frameHeight;
                    savantFrame.X = 4 * frameWidth;
                }
                else if (savantFrameCounter < 80)
                {
                    savantFrame.Y = 3 * frameHeight;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 85)
                {
                    savantFrame.Y = 3 * frameHeight;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 90)
                {
                    savantFrame.Y = 3 * frameHeight;
                    savantFrame.X = 2 * frameWidth;
                }
                else if (savantFrameCounter < 95)
                {
                    savantFrame.Y = 3 * frameHeight;
                    savantFrame.X = 3 * frameWidth;
                }
                else if (savantFrameCounter < 100)
                {
                    savantFrame.Y = 3 * frameHeight;
                    savantFrame.X = 4 * frameWidth;
                }
                else if (savantFrameCounter < 105)
                {
                    savantFrame.Y = 4 * frameHeight;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 110)
                {
                    savantFrame.Y = 4 * frameHeight;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 115)
                {
                    savantFrame.Y = 4 * frameHeight;
                    savantFrame.X = 2 * frameWidth;
                }
                else if (savantFrameCounter < 120)
                {
                    savantFrame.Y = 4 * frameHeight;
                    savantFrame.X = 3 * frameWidth;
                }
                else if (savantFrameCounter < 125)
                {
                    savantFrame.Y = 4 * frameHeight;
                    savantFrame.X = 4 * frameWidth;
                }
                else if (savantFrameCounter < 130)
                {
                    savantFrame.Y = 5 * frameHeight;
                    savantFrame.X = 0 * frameWidth;
                }
                else if (savantFrameCounter < 135)
                {
                    savantFrame.Y = 5 * frameHeight;
                    savantFrame.X = 1 * frameWidth;
                }
                else if (savantFrameCounter < 140)
                {
                    savantFrame.Y = 5 * frameHeight;
                    savantFrame.X = 2 * frameWidth;
                }
                else if (savantFrameCounter < 145)
                {
                    savantFrame.Y = 5 * frameHeight;
                    savantFrame.X = 3 * frameWidth;
                }
                if (savantFrameCounter >= 195)
                {
                    AIState = Intro;
                    AITimer = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIValue = 0;
                    AITimer2 = 0;
                    Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].Deactivate();
                    savantFrameCounter = 0;
                }
                if (savantFrameCounter > 105)
                {
                    if (!Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].IsActive())
                    {
                        Terraria.Graphics.Effects.Filters.Scene.Activate("Regressus:OracleSummon", NPC.Center);
                    }
                    else
                    {
                        Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].GetShader().UseProgress(AIValue);
                    }
                    Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleSummon"].GetShader().UseTargetPosition(NPC.Center);
                }
                if (savantFrameCounter > 105 && savantFrameCounter < 165)
                {
                    AIValue = MathHelper.Lerp(AIValue, 8f, (savantFrameCounter - 105) / 165);
                }
                if (savantFrameCounter >= 105 && savantFrameCounter <= 125)
                    Main.musicFade[Main.curMusic] = MathHelper.Lerp(1, 0, (savantFrameCounter - 105) / 20);
                if (savantFrameCounter >= 136 && savantFrameCounter <= 145)
                {
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oracle");
                    Main.musicFade[Main.curMusic] = MathHelper.Lerp(0, 1, (savantFrameCounter - 136) / 9);
                }
                if (savantFrameCounter > 165)
                {
                    AIValue = MathHelper.Lerp(AIValue, 0f, (savantFrameCounter - 165) / 194);
                }
                #endregion
            }
            else if (AIState == Transform)
            {
                #region "transform"
                int frameWidth = 202, frameHeight = NPC.frame.Height;
                transformFrame.Width = frameWidth;
                transformFrame.Height = frameHeight;
                transformFrameCounter++;
                float progress = MathHelper.Lerp(0, 1, transformFrameCounter / 430);
                //arenaScale = Math.Clamp(progress * 5f, 0, 2f);
                clockAlpha = Math.Clamp(progress * 6.5f, 0, .75f);
                if (transformFrameCounter == 1)
                {
                    arenaCenter = NPC.Center;
                    NPC.velocity = Vector2.Zero;
                    NPC.dontTakeDamage = true;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].hostile && Main.projectile[i].type != ModContent.ProjectileType<OracleOrbs>())
                        {
                            Main.projectile[i].timeLeft = 1;
                        }
                    }
                    RegreSystem.ChangeCameraPos(NPC.Center, 430, 1.45f);
                }
                if (transformFrameCounter == 80)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        float angle = 2f * (float)Math.PI / 15f * i;
                        Vector2 velocity = new Vector2(1, 1).RotatedBy(angle);
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<TentacleBlack>(), 0, 0f, player.whoAmI, 200, .75f)];
                        projectile.timeLeft = 300;
                    }
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oracle2");
                }
                if (transformFrameCounter == 330)
                {
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Ripple>(), 0, 0f, player.whoAmI, ai1: 0.01f)];
                }
                if (transformFrameCounter < 5)
                {
                    transformFrame.Y = 0;
                    transformFrame.X = 0 * frameWidth;
                }
                else if (transformFrameCounter < 10)
                {
                    transformFrame.Y = 0;
                    transformFrame.X = 1 * frameWidth;
                }
                else if (transformFrameCounter < 15)
                {
                    transformFrame.Y = 0;
                    transformFrame.X = 2 * frameWidth;
                }
                else if (transformFrameCounter < 20)
                {
                    transformFrame.Y = 0;
                    transformFrame.X = 3 * frameWidth;
                }
                else if (transformFrameCounter < 25)
                {
                    transformFrame.Y = 0;
                    transformFrame.X = 4 * frameWidth;
                }
                else if (transformFrameCounter < 30)
                {
                    transformFrame.Y = frameHeight;
                    transformFrame.X = 0 * frameWidth;
                }
                else if (transformFrameCounter < 35)
                {
                    transformFrame.Y = frameHeight;
                    transformFrame.X = 1 * frameWidth;
                }
                else if (transformFrameCounter < 40)
                {
                    transformFrame.Y = frameHeight;
                    transformFrame.X = 2 * frameWidth;
                }
                else if (transformFrameCounter < 45)
                {
                    transformFrame.Y = frameHeight;
                    transformFrame.X = 3 * frameWidth;
                }
                else if (transformFrameCounter < 50)
                {
                    transformFrame.Y = frameHeight;
                    transformFrame.X = 4 * frameWidth;
                }
                else if (transformFrameCounter < 55)
                {
                    transformFrame.Y = 2 * frameHeight;
                    transformFrame.X = 0 * frameWidth;
                }
                else if (transformFrameCounter < 60)
                {
                    transformFrame.Y = 2 * frameHeight;
                    transformFrame.X = 1 * frameWidth;
                }
                else if (transformFrameCounter < 180 + 200)
                {
                    phase2 = true;
                    transformFrame.Y = 2 * frameHeight;
                    transformFrame.X = 2 * frameWidth;
                }
                else if (transformFrameCounter < 185 + 200)
                {
                    transformFrame.Y = 2 * frameHeight;
                    transformFrame.X = 3 * frameWidth;
                }
                else if (transformFrameCounter < 190 + 200)
                {
                    transformFrame.Y = 2 * frameHeight;
                    transformFrame.X = 4 * frameWidth;
                }
                else if (transformFrameCounter < 195 + 200)
                {
                    transformFrame.Y = 3 * frameHeight;
                    transformFrame.X = 0 * frameWidth;
                }
                else if (transformFrameCounter < 200 + 200)
                {
                    transformFrame.Y = 3 * frameHeight;
                    transformFrame.X = 1 * frameWidth;
                }
                else if (transformFrameCounter < 205 + 200)
                {
                    transformFrame.Y = 3 * frameHeight;
                    transformFrame.X = 2 * frameWidth;
                }
                else if (transformFrameCounter < 210 + 200)
                {
                    transformFrame.Y = 3 * frameHeight;
                    transformFrame.X = 3 * frameWidth;
                }
                else if (transformFrameCounter < 215 + 200)
                {
                    transformFrame.Y = 3 * frameHeight;
                    transformFrame.X = 4 * frameWidth;
                }
                else if (transformFrameCounter < 220 + 200)
                {
                    transformFrame.Y = 4 * frameHeight;
                    transformFrame.X = 0 * frameWidth;
                }
                else if (transformFrameCounter < 225 + 200)
                {
                    transformFrame.Y = 4 * frameHeight;
                    transformFrame.X = 1 * frameWidth;
                }
                else if (transformFrameCounter < 230 + 200)
                {
                    transformFrame.Y = 4 * frameHeight;
                    transformFrame.X = 2 * frameWidth;
                }
                else
                {
                    arenaCenter = NPC.Center;
                    AIState = OrbBlast;
                    AITimer = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    hasTransformed = true;
                    NPC.dontTakeDamage = false;
                    transformFrameCounter = 0;
                }
                #endregion
            }
            else if (AIState == MusicIntroSync)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center + new Vector2(0, -300), AITimer / 235);
                if (AITimer >= 400)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIState = OrbBlast;
                }
            }
            else if (AIState == Death)
            {
                deathTimer++;
                if (deathTimer > 40)
                {
                    float progress = MathHelper.Lerp(0, 1, deathTimer / 500);
                    AIValue = Math.Clamp(-progress * 7.5f, -10f, 0f);
                    if (!Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleVoid1"].IsActive())
                    {
                        Terraria.Graphics.Effects.Filters.Scene.Activate("Regressus:OracleVoid1", NPC.Center);
                    }
                    else
                    {
                        Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleVoid1"].GetShader().UseProgress(AIValue);
                    }
                    Terraria.Graphics.Effects.Filters.Scene["Regressus:OracleVoid1"].GetShader().UseTargetPosition(NPC.Center);
                }
                if (deathTimer == 500)
                {
                    deathTimer2 = 1;
                    CheckDead();
                }
                if (deathTimer == 1)
                {
                    AIValue = 0;
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/oracleVoid");
                    NPC.Center = arenaCenter;
                    RegreSystem.ChangeCameraPos(NPC.Center, 430, 1.45f);
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].hostile && Main.projectile[i].type != ModContent.ProjectileType<OracleOrbs>())
                        {
                            Main.projectile[i].timeLeft = 1;
                        }
                    }
                }
            }
            else if (AIState == OrbBlast)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center + new Vector2(0, -300), AITimer / 235);
                if (AITimer >= 235)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIState = BeamStar;
                }
            }
            else if (AIState == BeamStar)
            {
                AITimer++;
                if (AITimer < 40)
                {
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + new Vector2(0, -300), AITimer / 39);
                }
                if (AITimer == 10)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        float angle = 2f * (float)Math.PI / 5f * k;
                        Vector2 velocity = new Vector2(15, 15).RotatedBy(angle);
                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0, player.whoAmI, NPC.whoAmI, 1);
                        Main.projectile[projectile].tileCollide = false;
                    }
                }
                if (AITimer == 40)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        float angle = 2f * (float)Math.PI / 5f * k;
                        Vector2 velocity = new Vector2(15, 15).RotatedBy(angle);
                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<OracleBeamNoTarget>(), 45, 0, player.whoAmI, 0, 0.89f);
                        Main.projectile[projectile].tileCollide = false;
                    }
                }
                if (AITimer >= 70 && AITimer <= 340)
                {
                    AIValue += 46f / 2;
                    Vector2 velocity = new Vector2(1.5f, 1.5f).RotatedBy(MathHelper.ToRadians(AIValue));
                    if (velocity.Length() < 3) velocity = Vector2.Normalize(velocity) * 3f;
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity * 2.5f, ModContent.ProjectileType<OracleEnergyOrb>(), 45, 0);
                    }
                }
                if (AITimer >= 400)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIValue = 0;
                    AIState = Railgun;
                }
            }
            else if (AIState == Railgun)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center + new Vector2(0, -540), AITimer / 350);
                AITimer++;
                if (AITimer >= 65 * 5)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIState = GroundLasers;
                }
            }
            else if (AIState == GroundLasers)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center + new Vector2(0, -340), AITimer / (45 * 12));
                AITimer++;
                AITimer2++;
                if (AITimer2 == 10)
                {
                    random[6] = new Vector2(player.Center.X, Main.screenPosition.Y + Main.screenHeight + 200);
                    for (int i = 0; i < 6; i++)
                    {
                        random[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y + Main.screenHeight + 200);
                        int projectilea = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[i], -Vector2.UnitY, ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0, player.whoAmI, NPC.whoAmI);
                        Main.projectile[projectilea].timeLeft = 36;
                    }
                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[6], -Vector2.UnitY, ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0, player.whoAmI, NPC.whoAmI);
                    Main.projectile[projectile].timeLeft = 36;
                }
                if (AITimer2 == 45)
                {
                    RegreSystem.ScreenShakeAmount = 5f;
                    AITimer2 = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        int projectilea = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[i], -Vector2.UnitY, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 0, 1.875f);
                        Main.projectile[projectilea].timeLeft = 20;
                    }
                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[6], -Vector2.UnitY, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 0, 1.875f);
                    Main.projectile[projectile].timeLeft = 20;

                    for (int i = 0; i < 6; i++)
                    {
                        int projectilea = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[i] - Vector2.UnitY * (Main.screenHeight + 400), Vector2.UnitY, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 0, 1.875f);
                        Main.projectile[projectilea].timeLeft = 20;
                    }
                    int projectile2 = Projectile.NewProjectile(NPC.GetSource_FromThis(), random[6] - Vector2.UnitY * (Main.screenHeight + 400), Vector2.UnitY, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 0, 1.875f);
                    Main.projectile[projectile2].timeLeft = 20;
                }
                if (AITimer >= 45 * 12)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    if (crystal != null)
                        crystal.localAI[0] = 0;
                    AIState = MusicIntroSync;
                }
            }
        }
    }
}
