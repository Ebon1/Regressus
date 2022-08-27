using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Vargant;
using Terraria.DataStructures;
using Regressus.Projectiles;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
namespace Regressus.NPCs.Bosses.Vargant
{
    public class VoltageVargant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voltage Vagrant");
            Main.npcFrameCount[Type] = 11;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 114;
            NPC.lifeMax = 2000;
            NPC.defense = 5;
            NPC.aiStyle = 0;
            NPC.damage = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/voltageVagrant");
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

                new FlavorTextBestiaryInfoElement("A giant moth that can conjure storms in a mere instant. Abandoned after the Great War, it wanders the lands in search of purpose."),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D glowTex = RegreUtils.GetTexture("NPCs/Bosses/Vargant/VoltageVargant_Glow");
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
            for (int i = 0; i < NPCID.Sets.TrailCacheLength[Type]; i++)
            {
                spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + NPC.Size / 2, NPC.frame, Color.White * (1f - fadeMult * i), NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            //spriteBatch.Draw(glowTex, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*if (AIState == Rain)
            {
                for (int i = 0; i < 3; i++)
                {
                    Texture2D texture = RegreUtils.GetExtraTexture("cloud2");
                    float progress = Utils.GetLerpValue(0, 240, AITimer);
                    float mult = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 1.2f, 0, 1);
                    float mult2 = 1f;
                    Vector2 pos = new Vector2(Main.screenWidth - ((Main.screenWidth / 5) * mult), -100);
                    SpriteEffects effects = SpriteEffects.None;
                    if (i == 1)
                    {
                        pos.X = (Main.screenWidth / 5) * mult;
                        effects = SpriteEffects.FlipVertically;
                    }
                    else if (i == 2)
                    {
                        pos.X = Main.screenWidth / 2;
                        pos.Y = -200 + 100 * mult;
                        mult2 = .5f;
                        texture = RegreUtils.GetExtraTexture("cloud");
                    }
                    spriteBatch.Reload(BlendState.Additive);
                    if (mult2 == .5f)
                        for (int j = 0; j < 2; j++)
                            spriteBatch.Draw(texture, pos, null, Color.White * mult * mult2, 0, texture.Size() / 2, 2, effects, 0);
                    spriteBatch.Draw(texture, pos, null, Color.White * mult * mult2, 0, texture.Size() / 2, 2, effects, 0);
                    spriteBatch.Reload(BlendState.AlphaBlend);
                }
            }*/ //ugly fucking clouds
        }
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Idle = -1;
        private const int Intro = -2;
        private const int PreProvokation = 0;
        private const int Hail = 1;
        private const int Thunderbolts = 2;
        private const int Rain = 3;
        private const int Hail2 = 4;
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
            return spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.ZonePurity && !NPC.AnyNPCs(ModContent.NPCType<VoltageVargant>()) ? 0.01f : 0;
        }
        int nextAttack = Rain;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            Main.StartRain();
            Main._shouldUseStormMusic = true;
            Main.windSpeedTarget = 1f;
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
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.0005f);
                if (NPC.life < NPC.lifeMax)
                {
                    AIState = Intro;
                }
            }
            if (AIState == Intro)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    RegreSystem.ScreenShakeAmount = 20f;
                    RegreUtils.SetBossTitle(160, "Voltage Vagrant", Color.White, "Bringer of Storms", BossTitleStyleID.Vagrant);
                    RegreSystem.ChangeCameraPos(NPC.Center, 160);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                }
                if (AITimer >= 160)
                {
                    AITimer = 0;
                    AIState = Idle;
                }
            }
            if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.025f);
                if (AITimer >= 180)
                {
                    AIState = nextAttack;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Hail)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<OrbitingHail>()))
                    AITimer++;

                NPC.Center = Vector2.Lerp(NPC.Center, player.Center - Vector2.UnitY * 100, 0.025f);
                NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<OrbitingHail>());
                if (AITimer == 1)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)Math.PI / 7f * i;
                        Point pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100).ToPoint();
                        NPC.NewNPC(NPC.GetSource_FromAI(), pos.X, pos.Y, ModContent.NPCType<OrbitingHail>(), 0, NPC.whoAmI, angle);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                    AITimer++;
                }
                AITimer2++;
                if (AITimer2 >= 40)
                    AITimer2 = -40;
                if (AITimer2 == 20)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 randomVel = new(Main.rand.NextFloat(-10f, 10f), -7.5f);
                        Vector2 randomVel2 = new(Main.rand.NextFloat(-10f, 10f), -7.5f);
                        Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, randomVel, ModContent.ProjectileType<Hail2>(), 10, 0, player.whoAmI);
                        Projectile proj2 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, randomVel2, ModContent.ProjectileType<Hail3>(), 15, 0, player.whoAmI);
                        proj.aiStyle = proj2.aiStyle = 2;
                    }
                    Projectile proj3 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitY * -7.5f, ModContent.ProjectileType<Hail2>(), 10, 0, player.whoAmI);
                    proj3.aiStyle = 2;
                }
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
                    AITimer++;
                else
                    AITimer = 239;
                if (AITimer == 1)
                {
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 101)
                {
                    NPC.ai[3] = -1;
                }
                if (AITimer < 100 || (AITimer > 150 && AITimer < 200))
                {
                    NPC.damage = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + Vector2.UnitX * 550 * NPC.ai[3], 0.035f);
                }
                //NPC.Center = new Vector2(NPC.Center.X, Vector2.Lerp(NPC.Center, player.Center, 0.035f).Y);
                if (AITimer >= 35)
                {
                    if (++AITimer2 >= 6)
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(Main.windSpeedCurrent * 4, 5), rain, 10, 0, Main.myPlayer);
                    }
                }

                if ((AITimer == 100 || AITimer == 200) && !stunned)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                    NPC.damage = 15;
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 65) * -1;
                    //NPC.velocity += new Vector2(40 * (NPC.direction), 0);
                }
                if (AITimer >= 240)
                {
                    stunned = false;
                    NPC.damage = 10;
                    AIState = Idle;
                    nextAttack = Hail;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Thunderbolts)
            {
                AITimer++;
                if (AITimer < 30)
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.025f);
                if (AITimer == 40)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                if (AITimer > 50)
                {
                    if (++AITimer2 >= 15)
                    {
                        Vector2 __random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                        AITimer2 = 0;
                    }
                }
                if (AITimer >= 210)
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
                if (++AITimer2 >= 70)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)Math.PI / 7f * i;
                        Vector2 pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<OrbitingHailP>(), 15, 0, player.whoAmI, 0, angle);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoar);
                    AITimer2 = 0;
                }


                if (AITimer >= 210)
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