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
using Terraria.Utilities;

namespace Regressus.NPCs.Bosses.Vagrant
{
    public class VoltageVagrant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voltage Vagrant");
            Main.npcFrameCount[Type] = 11;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 114;
            NPC.lifeMax = 2500;
            NPC.defense = 10;
            NPC.aiStyle = 0;
            NPC.damage = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = false;
            if (Main.getGoodWorld)
                NPC.scale = 0.5f;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (!stunned)
            {
                if (NPC.frameCounter >= 5)
                {
                    if (NPC.frame.Y < frameHeight * 4)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                    else
                    {
                        NPC.frame.Y = 0;
                    }
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = frameHeight * 5;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = frameHeight * 6;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = frameHeight * 7;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                else if (NPC.frameCounter < 25)
                {
                    NPC.frame.Y = frameHeight * 9;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = frameHeight * 10;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Rain,

                new FlavorTextBestiaryInfoElement("A gigantic moth that can conjure storms in a mere instant. Abandoned after the Great War, it wanders the lands in search of purpose."),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowTex = RegreUtils.GetTexture("NPCs/Bosses/Vagrant/VoltageVargant_Glow");
            Texture2D glow = RegreUtils.GetExtraTexture("Spotlight");
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
            for (int i = 0; i < NPCID.Sets.TrailCacheLength[Type]; i++)
            {
                spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + NPC.Size / 2, NPC.frame, Color.White * (1f - fadeMult * i), NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            }

            if (angery && !ded)
            {
                const float TwoPi = (float)Math.PI * 2f;
                float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
                for (int i = 0; i < 4; i++)
                    spriteBatch.Draw(texture, NPC.Center - screenPos + (Vector2.UnitX * 20 * scale).RotatedBy(MathHelper.ToRadians((90 * scale) * i)), NPC.frame, Color.Cyan * 0.5f * scale, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            if (AIState == Thunderbolts || AIState == Thunderbolts2 || AIState == Angry)
            {
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(glow, NPC.Center - screenPos, null, Color.White * 0.25f, NPC.rotation, glow.Size() / 2, NPC.scale / 0.25f, effects, 0);
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }
            //spriteBatch.Draw(glowTex, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            if (AIState == Hail)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.Black * alpha, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
                spriteBatch.Draw(glowTex, NPC.Center - screenPos, NPC.frame, Color.White * alpha, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            }
            return false;
        }
        int[] sparkVariation = new int[4];
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                AIState = Death;
                RegreSystem.ScreenShakeAmount = 15f;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.dontTakeDamage = true;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        bool ded;
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Angry = -3, Death = -4;
        private const int Idle = -1;
        private const int Intro = -2;
        private const int PreProvokation = 0;
        private const int Hail = 1;
        private const int Thunderbolts = 2;
        private const int Rain = 3;
        private const int Hail2 = 4;
        private const int Thunderbolts2 = 5;
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
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        bool stunned;
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (AIState == Rain && AITimer > 100 && !stunned)
            {
                NPC.velocity = Vector2.Zero;
                NPC.damage = 0;
                stunned = true;
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                NPC.ai[3] = 200;
                NPC.frameCounter = 0;
            }
        }
        int firstDir;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.ZonePurity && !NPC.AnyNPCs(ModContent.NPCType<VoltageVagrant>()) ? 0.01f : 0;
        }
        bool yes;
        int nextAttack = Rain;
        Vector2[] random = new Vector2[11];
        Vector2[] random2 = new Vector2[11]; //up to 3 = first lightning attack, 4 to 8 = second lightning attack 9 to 11 = hail
        Vector2 arena;
        int aa;
        bool angery;
        float alpha;
        float lightningRot;
        int lightningInterval;
        float lightningAlpha;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*float idk = (float)Math.Sin(Main.GlobalTimeWrappedHourly);
            float alpha = (idk + 1) / 2;
            float idk2 = (float)Math.Sin(-Main.GlobalTimeWrappedHourly);
            float alpha2 = (idk2 + 1) / 2;*/
            //Main.NewText(alpha + " and " + idk);
            if (AIState == Rain || AIState == Angry)
            {
                spriteBatch.Reload(BlendState.Additive);
                Texture2D glow = RegreUtils.GetExtraTexture("Spotlight");
                spriteBatch.Draw(glow, NPC.Center - screenPos, null, Color.White * 0.25f, NPC.rotation, glow.Size() / 2, NPC.scale / 0.25f, SpriteEffects.None, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
                for (int i = 0; i < 2; i++)
                {
                    Texture2D a = RegreUtils.GetExtraTexture("Extras2/spark_" + sparkVariation[i]);
                    spriteBatch.Reload(BlendState.Additive);
                    spriteBatch.Draw(a, NPC.Center - screenPos, null, Color.White * lightningAlpha, lightningRot, a.Size() / 2, 0.35f, SpriteEffects.None, 0);
                    spriteBatch.Draw(a, NPC.Center - screenPos, null, Color.DeepSkyBlue * lightningAlpha, lightningRot, a.Size() / 2, 0.35f, SpriteEffects.None, 0);
                    spriteBatch.Reload(BlendState.AlphaBlend);
                }
            }
            /*for (int i = 2; i < 4; i++)
            {
                Texture2D a = RegreUtils.GetExtraTexture("Extras2/spark_" + sparkVariation[i]);
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(a, NPC.Center - screenPos, null, Color.White * alpha2 * 0.5f, 0.025f * Main.GameUpdateCount, a.Size() / 2, 0.25f, SpriteEffects.None, 0);
                spriteBatch.Draw(a, NPC.Center - screenPos, null, Color.DeepSkyBlue * alpha2 * 0.5f, 0.025f * Main.GameUpdateCount, a.Size() / 2, 0.25f, SpriteEffects.None, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }*/
        }

        void LightningStuff()
        {
            if (lightningAlpha > 0)
                lightningAlpha -= 0.08f;
            lightningInterval++;
            if (lightningInterval >= 12)
            {
                lightningRot = MathHelper.ToRadians(Main.rand.Next(360));
                lightningInterval = 0;
                lightningAlpha = 1f;
                //Main.NewText("help");
                for (int i = 0; i < 2; i++)
                {
                    sparkVariation[i] = Main.rand.Next(4);
                }
            }
        }
        public override void AI()
        {
            if (AIState == Rain || AIState == Angry)
            {
                LightningStuff();
            }
            else
            {
                lightningAlpha = 0;
            }
            if (NPC.life < (NPC.lifeMax / 2) + NPC.lifeMax / 5 && !angery && Main.expertMode)
            {
                AITimer = 0;
                AITimer2 = 0;
                AIState = Angry;
                angery = true;
            }
            if (AIState != Rain)
                NPC.damage = 0;
            Player player = Main.player[NPC.target];
            if (Main.dayTime)
            {
                AIState = -1;
                stunned = false;
                NPC.velocity = new Vector2(0, -20f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            if (yes)
            {
                Main.StartRain();
                Main._shouldUseStormMusic = true;
                Main.windSpeedTarget = 1f;
            }
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
            if (stunned)
            {
                NPC.ai[3]--;
                if (NPC.ai[3] <= 0)
                {
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    stunned = false;
                    NPC.frameCounter = 0;
                    NPC.velocity = Vector2.Zero;
                }
            }
            if (AIState == PreProvokation)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.0025f);
                if (NPC.life < NPC.lifeMax)
                {
                    Main.StartRain();
                    Main._shouldUseStormMusic = true;
                    Main.windSpeedTarget = 1f;
                    AIState = Intro;
                    NPC.boss = true;
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/voltageVagrant");
                }
            }
            if (AIState == Intro)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    NPC.boss = true;
                    Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/voltageVagrant");
                    yes = true;
                    Main.StartRain();
                    Main._shouldUseStormMusic = true;
                    Main.windSpeedTarget = 1f;
                    RegreSystem.ScreenShakeAmount = 20f;
                    Main.NewLightning();
                    RegreUtils.SetBossTitle(160, "Voltage Vagrant", Color.White, "Bringer of Storms", BossTitleStyleID.Vagrant);
                    RegreSystem.ChangeCameraPos(NPC.Center, 160);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                }
                if (AITimer >= 160)
                {
                    AITimer = 0;
                    AIState = Idle;
                }
            }
            else if (AIState == Death)
            {
                Music = 0;
                AITimer2++;
                AITimer++;
                SoundStyle a = SoundID.NPCHit1;
                a.Volume = 0;
                NPC.HitSound = a; Dust dust;
                Vector2 position = NPC.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width / 2, NPC.height / 2, ModContent.DustType<Dusts.MothDust>(), 0f, 1f, 0, new Color(255, 255, 255), 1.2790698f)];

                if (AITimer < 120)
                {
                    if (AITimer2 == 5)
                    {
                        Vector2 rand = new Vector2(NPC.position.X + NPC.width * Main.rand.NextFloat(), NPC.position.Y + NPC.height * Main.rand.NextFloat());
                        Projectile.NewProjectile(NPC.GetSource_Death(), rand, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 0, 1);
                    }
                    if (AITimer2 >= 10)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_LightningBugHurt);
                        Vector2 rand = new Vector2(NPC.position.X + NPC.width * Main.rand.NextFloat(), NPC.position.Y + NPC.height * Main.rand.NextFloat());
                        Projectile.NewProjectile(NPC.GetSource_Death(), rand, Vector2.Zero, ModContent.ProjectileType<HailExplosion>(), 0, 1);
                        AITimer2 = 0;
                    }
                }
                if (AITimer == 110)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.damage = 0;
                    stunned = true;
                    NPC.noTileCollide = false;
                    NPC.noGravity = false;
                    NPC.ai[3] = 140;
                    NPC.frameCounter = 0;
                }
                if (AITimer == 250)
                    aa = NPC.direction;
                if (AITimer > 250)
                {
                    NPC.aiStyle = -1;
                    NPC.direction = aa;
                    NPC.velocity = new Vector2(4 * aa, -10);
                }
                if (AITimer >= 400)
                {
                    NPC.immortal = false;
                    NPC.StrikeNPC(NPC.lifeMax, 0, 0);
                }
            }
            else if (AIState == Angry)
            {
                NPC.frameCounter++;
                AITimer++;
                if (AITimer == 1)
                {
                    RegreSystem.ChangeCameraPos(NPC.Center, 70);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                }
                if (AITimer > 100)
                {
                    AITimer2++;
                    if (AITimer2 == 5)
                    {
                        random[3] = player.Center;
                        /*for (int i = 0; i < 3; i++)
                        {
                            random[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), player.Center.Y);
                            RegreUtils.SpawnTelegraphLine(random[i], NPC.GetSource_FromAI());
                        }*/
                        RegreUtils.SpawnTelegraphLine(random[3], NPC.GetSource_FromAI());
                    }
                    if (AITimer2 == 10)
                    {
                        RegreSystem.ScreenShakeAmount = 5f;
                        AITimer2 = 0;
                        /*for (int i = 0; i < 3; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), random[i], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                        }*/
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random[3], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                    }
                }
                if (AITimer >= 330)
                {
                    AIState = Idle;
                    nextAttack = Thunderbolts2;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.015f);
                if (AITimer >= 180 - (angery ? 100 : 0))
                {
                    AIState = nextAttack;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Hail)
            {
                if (alpha < 1 && AITimer < 10)
                    alpha += 0.05f;
                if (!NPC.AnyNPCs(ModContent.NPCType<OrbitingHail>()))
                    AITimer++;
                /*if (angery)
                    AITimer3++;
                if (AITimer3 == 30)
                {
                    random[9] = new Vector2(player.Center.X + Main.rand.NextFloat(-250, 250), player.Center.Y);
                    RegreUtils.SpawnTelegraphLine(random[9], NPC.InheritSource(NPC), 20);
                }
                if (AITimer3 >= 60)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), random[9], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                    AITimer3 = 0;
                }*/
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center - Vector2.UnitY * 200, 0.045f);
                NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<OrbitingHail>());
                if (AITimer == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = 2f * (float)Math.PI / 4f * i;
                        Point pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100).ToPoint();
                        NPC.NewNPC(NPC.GetSource_FromAI(), pos.X, pos.Y, ModContent.NPCType<OrbitingHail>(), 0, NPC.whoAmI, angle, i);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                    AITimer++;
                }
                AITimer2++;
                if (angery)
                    AITimer2++;
                if (AITimer2 >= 40)
                    AITimer2 = -40;
                if (AITimer2 == 20)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 randomVel = new(Main.rand.NextFloat(-15f, 15f), -7.5f);
                        Vector2 randomVel2 = new(Main.rand.NextFloat(-15f, 15f), -7.5f);
                        Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, randomVel, ModContent.ProjectileType<Hail2>(), 10, 0, player.whoAmI);
                        Projectile proj2 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, randomVel2, ModContent.ProjectileType<Hail3>(), 15, 0, player.whoAmI);
                        proj.aiStyle = proj2.aiStyle = 2;
                    }
                    Projectile proj3 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitY * -7.5f, ModContent.ProjectileType<Hail2>(), 10, 0, player.whoAmI);
                    proj3.aiStyle = 2;
                }
                if (AITimer > 10 && alpha > 0)
                    alpha -= 0.05f;
                if (AITimer >= 60)
                {
                    AIState = Idle;
                    nextAttack = Thunderbolts;
                    NPC.dontTakeDamage = false;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Rain)
            {
                if (!stunned)
                {
                    AITimer++;
                    if (angery)
                        AITimer++;
                }
                else
                    AITimer = 239;
                if (AITimer > 1)
                {
                    if (AITimer % 5 == 0)
                    {
                        Vector2 arena2 = arena + new Vector2(Main.screenWidth, 0);
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), arena2, Vector2.Zero, ModContent.ProjectileType<Lightning>(), 1000, 0);
                        Projectile b = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), arena, Vector2.Zero, ModContent.ProjectileType<Lightning>(), 1000, 0);
                        a.ai[1] = b.ai[1] = 1.75f;
                    }
                }
                if (AITimer == 1 + (angery ? 1 : 0))
                {
                    arena = new Vector2(Main.screenPosition.X, player.Center.Y);
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 101 + (angery ? 1 : 0))
                {
                    NPC.ai[3] = -1;
                }
                else if (AITimer == 201 + (angery ? 1 : 0))
                {
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 301 + (angery ? 1 : 0))
                {
                    NPC.ai[3] = -1;
                }
                if (AITimer < 100 || (AITimer > 140 && AITimer < 200) || (AITimer > 250 && AITimer < 300) || (AITimer > 360 && AITimer < 400))
                {
                    NPC.damage = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + Vector2.UnitX * 550 * NPC.ai[3], 0.035f);
                }
                //NPC.Center = new Vector2(NPC.Center.X, Vector2.Lerp(NPC.Center, player.Center, 0.035f).Y);
                if (AITimer >= 35)
                {
                    if (++AITimer2 >= 8)
                    {
                        AITimer2 = 0;
                        int rain = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                rain = ModContent.ProjectileType<Rain1>();
                                break;
                            case 1:
                                rain = ModContent.ProjectileType<Rain2>();
                                break;
                            case 2:
                                rain = ModContent.ProjectileType<Rain3>();
                                break;
                        }
                        Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(Main.windSpeedCurrent * 2, 4), rain, 10, 0, Main.myPlayer);
                    }
                }

                if ((AITimer == 130 || AITimer == 240 || AITimer == 350 || AITimer == 460) && !stunned)
                {
                    NPC.rotation = 0;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                    NPC.damage = 15;
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 55) * -1;
                    //NPC.velocity.Y = (float)(Math.Sin(rotation2) * 65) * -1;
                    //NPC.velocity += new Vector2(40 * (NPC.direction), 0);
                }
                if (AITimer >= 240 + (angery ? 250 : 0))
                {
                    stunned = false;
                    NPC.damage = 10;
                    AIState = Idle;
                    nextAttack = Hail;
                    NPC.rotation = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Thunderbolts)
            {
                NPC.frameCounter++;
                AITimer++;
                if (AITimer < 30)
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.025f);
                if (AITimer == 40)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                if (AITimer > 50)
                {
                    AITimer2++;
                    if (AITimer2 == 10)
                    {
                        random2[10] = player.Center;
                        for (int i = 0; i < 4 + (angery ? 2 : 0); i++)
                        {
                            random2[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), player.Center.Y);
                            RegreUtils.SpawnTelegraphLine(random2[i], NPC.GetSource_FromAI());
                        }
                        RegreUtils.SpawnTelegraphLine(random2[10], NPC.GetSource_FromAI());
                    }
                    if (AITimer2 == (angery ? 45 : 60))
                    {
                        RegreSystem.ScreenShakeAmount = 5f;
                        AITimer2 = 0;
                        for (int i = 0; i < 4 + (angery ? 2 : 0); i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), random2[i], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random2[10], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                    }
                }
                if (AITimer >= 290 - (angery ? 10 : 0))
                {
                    AIState = Idle;
                    nextAttack = Hail2;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Hail2)
            {
                AITimer++;
                if (++AITimer2 >= 100 && AITimer < 300)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)Math.PI / 7f * i;
                        Vector2 pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100);
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<OrbitingHailP>(), 15, 0, player.whoAmI, 0, angle);
                        a.ai[0] = (i + 1) * 20;
                        a.ai[1] = angle;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                    AITimer2 = 0;
                }


                if (AITimer >= (angery ? 400 : 290))
                {
                    AIState = Idle;
                    if (angery)
                        nextAttack = Thunderbolts2;
                    else
                        nextAttack = Rain;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Thunderbolts2)
            {
                NPC.frameCounter++;
                AITimer++;
                if (AITimer == 1)
                {
                    AITimer2 = 1;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                }
                if (AITimer >= 50)
                {
                    if (++AITimer2 >= (angery ? 8 : 10))
                    {
                        AITimer2 = 0;
                        int rain = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                rain = ModContent.ProjectileType<Hail1>();
                                break;
                            case 1:
                                rain = ModContent.ProjectileType<Hail2>();
                                break;
                            case 2:
                                rain = ModContent.ProjectileType<Hail3>();
                                break;
                        }
                        Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y - 300);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(0, 10), rain, 10, 0, Main.myPlayer);
                    }
                    if (AITimer == 50 || AITimer == 100 || AITimer == 150 || AITimer == 200 || AITimer == 250)
                    {
                        random[7] = player.Center;
                        RegreUtils.SpawnTelegraphLine(random[7], NPC.GetSource_FromAI());
                    }
                    if (AITimer == 75 || AITimer == 125 || AITimer == 175 || AITimer == 225 || AITimer == 275)
                    {
                        RegreSystem.ScreenShakeAmount = 10f;
                        for (int i = 0; i < 2; i++)
                        {
                            Projectile c = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), random[7], Vector2.Zero, ModContent.ProjectileType<Lightning>(), 15, 0);
                            c.timeLeft = 60;
                        }
                    }
                }

                if (AITimer >= 375)
                {
                    AIState = Idle;
                    nextAttack = Rain;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
        }
    }
}