using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Regressus.Projectiles.Minibosses.Conjurer;
using Terraria.DataStructures;
using Regressus.NPCs.UG;
using Regressus.Projectiles;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Oracle;
using Regressus.Effects.Prims;
using Terraria.Audio;
using IL.Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria.Utilities;
using Regressus.Buffs.Debuffs;
using static System.Formats.Asn1.AsnWriter;
using IL.Terraria.GameContent.Events;
using System.IO;
using Terraria.GameContent.UI;

namespace Regressus.NPCs.Minibosses
{
    public class PreHMLuminary : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminary");
            Main.npcFrameCount[Type] = 9;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.Size = new(118, 84);
            NPC.lifeMax = 2000;
            NPC.defense = 14;
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCHit1;
            SoundStyle style = SoundID.NPCDeath7;
            style.Pitch = -.6f;
            style.Volume = 0;
            NPC.DeathSound = style;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Luminary");
        }
        public override void OnSpawn(IEntitySource source)
        {
            Main.windSpeedTarget = 0;
            EmoteBubble.MakeLocalPlayerEmote(EmoteID.EmotionAlert);
            Main.NewText("Something divine approaches...", new Color(118, 50, 173));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.hide)
            {
                Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
                var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Main.spriteBatch.Draw(tex, NPC.oldPos[i] - Main.screenPosition + NPC.Size / 2, NPC.frame, Color.HotPink * (1f - fadeMult * i), NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0f);
                }
                Texture2D a = TextureAssets.Npc[Type].Value;
                Texture2D b = RegreUtils.GetTexture("NPCs/Minibosses/PreHMLuminary_White");
                Main.EntitySpriteDraw(a, NPC.Center - screenPos, NPC.frame, Color.White * alpha, NPC.rotation, NPC.Size / 2, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(b, NPC.Center - screenPos, NPC.frame, Color.White * whiteAlpha, NPC.rotation, NPC.Size / 2, 1f, SpriteEffects.None, 0);
            }
            return false;
        }
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
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("These beings are made out of pure light and pieces of holy metal. While they are usually sent for minor inconveniences, they are a force to be reckoned with"),
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (AIState != Death)
                NPC.frameCounter++;
            if (AIState == Idle || AIState == Spawn || AIState == Death || NPC.IsABestiaryIconDummy)
            {
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 5 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0;
                }
            }
            else
            {
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 8 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 6 * NPC.height;
                }
            }
        }
        const int Death = -1, Spawn = -2, PreSpawn = 0, Idle = 1, Scythe = 2, Beam = 3, Boomerang = 4, NoMovement = 5;
        float alpha = 1f;
        int nextAttack = Scythe;
        Vector2 vel;
        bool ded;
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                SoundStyle style = new SoundStyle("Regressus/Sounds/Custom/thunder");
                style.PitchVariance = 1;
                style.MaxInstances = 0;
                style.Volume = 2f;
                SoundEngine.PlaySound(style, NPC.Center);
                NPC.life = 1;
                AIState = Death;
                RegreSystem.ScreenShakeAmount = 15f;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.dontTakeDamage = true;
                RegreSystem.ChangeCameraPos(NPC.Center, 600);
                RegreSystem.ScreenShakeAmount = 20;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                SoundEngine.PlaySound(SoundID.NPCDeath56);
                return false;
            }
            return true;
        }
        float whiteAlpha;
        float rayAlpha;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Reload(BlendState.Additive);
            Texture2D cone = RegreUtils.GetExtraTexture("cone4");
            Texture2D cone2 = RegreUtils.GetExtraTexture("cone2");
            UnifiedRandom rand = new UnifiedRandom(912301);
            for (int i = 0; i < 8; i++)
            {
                float speed = rand.NextFloat(-0.7f, 0.7f);
                float rot = Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 4 * speed;
                float alpha = (rayAlpha * 2 - i / 7f);
                float scale = rand.NextFloat(2f);
                // texture is current problem
                spriteBatch.Draw(cone, NPC.Center - screenPos, null, Color.White * alpha, MathHelper.TwoPi / 8 * i + rot, new(0, cone.Height / 2), scale, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(cone2, new(Main.screenWidth / 2, -200), null, Color.Pink * spawnAlpha, MathHelper.ToRadians(90), new Vector2(0, cone2.Height / 2), 1.1f, SpriteEffects.None, 0);
            spriteBatch.Draw(cone2, new(Main.screenWidth / 2, -200), null, Color.White * spawnAlpha, MathHelper.ToRadians(90), new Vector2(0, cone2.Height / 2), 1f, SpriteEffects.None, 0);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
        bool toTheSide;
        Vector2 Beamposition;
        bool phase2;
        float spawnAlpha;
        bool hasDoneIntro;
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, TorchID.UltraBright);
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            NPC.timeLeft = 2;
            if (player.dead)
            {
                NPC.velocity = -Vector2.UnitY * 15;
                if (NPC.timeLeft > 35)
                    NPC.timeLeft = 35;
            }
            if (NPC.life < NPC.lifeMax / 2 && !phase2)
            {
                RegreUtils.DustExplosion(NPC.Center, NPC.Size, true, Color.DeepPink);
                Main.NewText("The wind is blowing...", new Color(118, 50, 173));
                phase2 = true;
                hasDoneIntro = true;
                AIState = Boomerang;
                AITimer = 0;
                AITimer2 = 0;
                AITimer3 = 0;
                Main.windSpeedTarget = 4f * Main.rand.Next(new int[] { 1, -1 });
                RegreSystem.ScreenShakeAmount = 20f;
            }
            if (phase2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust;
                    Vector2 position = NPC.Center;
                    dust = Terraria.Dust.NewDustDirect(position, 0, 0, 71, 0, 0, 0, new Color(255, 255, 255), 1f);
                    dust.noGravity = true;
                }
            }
            if (!hasDoneIntro)
            {
                float progress = Utils.GetLerpValue(0, 200 + 8 * 60, AITimer2);
                spawnAlpha = Math.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            }
            else
                spawnAlpha = 0;
            if (AIState == PreSpawn)
            {
                NPC.boss = true;
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Luminary");
                AITimer++;
                if (AITimer >= 10 * 60)
                    AITimer2++;
                NPC.Center = player.Center - Vector2.UnitY * (Main.screenHeight);
                if (AITimer >= 18 * 60)
                {
                    AITimer = 0;
                    AIState = Spawn;
                }
            }
            else if (AIState == Spawn)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center - Vector2.UnitY * 200, AITimer / 50);
                AITimer++;
                AITimer2++;
                //if (AITimer >= 190)
                //    alpha += 0.1f;
                if (NPC.Center.Distance(player.Center - Vector2.UnitY * 200) < 5)
                    AITimer = 50;
                if (AITimer >= 50)
                {
                    RegreUtils.SetBossTitle(150, "lol this doesnt even matter", Color.DeepPink, "sex penis", BossTitleStyleID.Luminary);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust;
                        Vector2 position = NPC.Center;
                        dust = Terraria.Dust.NewDustDirect(position, 0, 0, 71, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 0, new Color(255, 255, 255), 1f);
                    }
                    SoundEngine.PlaySound(SoundID.ForceRoar);
                    AITimer = 0;
                    AIState = Idle;
                    RegreSystem.ScreenShakeAmount = 20f;
                    RegreSystem.ChangeCameraPos(NPC.Center, 150);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RippleSmol>(), 0, 0f);
                }
            }
            else if (AIState == Death)
            {
                AITimer++;
                NPC.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-10, 10f));
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, AITimer / 500);
                if (AITimer == 1)
                    Main.NewLightning();
                if (AITimer < 75)
                    NPC.frameCounter += 2;
                else
                    NPC.frameCounter++;
                if (AITimer == 75)
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = RegreUtils.CircleDividedEqually(i, 6);
                        Projectile lumiClone = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LuminaryP2>(), 0, 0);
                        lumiClone.ai[1] = angle;
                        lumiClone.ai[0] = NPC.whoAmI;
                        lumiClone.timeLeft = 200 + (i * 20);
                    }
                if (AITimer > 100 && whiteAlpha < 1 && AITimer < 400)
                {
                    whiteAlpha += 0.005f;
                }
                if (AITimer > 250)
                {
                    rayAlpha = (float)Math.Sin((AITimer - 250) / 250f);
                }
                if (whiteAlpha > 0 && AITimer > 400)
                {
                    whiteAlpha -= 0.01f;
                    alpha -= 0.01f;
                }

                if (AITimer == 450)
                {
                    // what if both
                    SoundStyle style = SoundID.NPCDeath7;
                    style.Pitch = -.6f;
                    SoundEngine.PlaySound(style);
                    player.GetModPlayer<RegrePlayer>().FlashScreen(NPC.Center, 50);
                    player.AddBuff(ModContent.BuffType<PilgrimBlindness>(), 100);
                }

                if (AITimer >= 500)
                {
                    Main.windSpeedTarget = 0;
                    Main.NewText("The divine creature has been defeated.", new Color(118, 50, 173));
                    //player.DelBuff(player.FindBuffIndex(ModContent.BuffType<PilgrimBlindness>()));
                    NPC.immortal = false;
                    NPC.life = 0;
                    NPC.checkDead();
                    //NPC.StrikeNPC(NPC.lifeMax, 0, 0, true, true);
                }
            }
            else if (AIState == Idle)
            {
                AITimer++;
                AITimer2++;
                if (AITimer == 150)
                    hasDoneIntro = true;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.velocity.X * 0.05f, AITimer / 250);
                AITimer3 += 0.05f;
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - new Vector2(200 * (float)Math.Sin(AITimer3), 200f), false) * 0.05f;
                if (AITimer >= 250)
                {
                    AITimer = 0;
                    AIState = nextAttack;
                    AITimer2 = AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.frame.Y = 6 * NPC.height;
                }
            }
            else if (AIState == Scythe)
            {
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, AITimer / 300);
                AITimer++;
                if (AITimer >= 30 && AITimer <= 400)
                {
                    AITimer2++;
                    if (AITimer2 >= 40)
                    {
                        Vector2 vel2 = Main.rand.NextVector2Unit();
                        for (int i = 0; i < 15; i++)
                        {
                            Dust dust;
                            Vector2 position = NPC.Center;
                            dust = Terraria.Dust.NewDustDirect(position, 0, 0, 71, vel2.X, vel2.Y, 0, new Color(255, 255, 255), 1f);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2CircularEdge(100, 100), vel2 * 4, ModContent.ProjectileType<HomingScythe>(), 15, 0);
                        AITimer2 = 0;
                    }
                }
                if (AITimer >= 460)
                {
                    AITimer = 0;
                    AIState = Beam;
                    //AIState = Idle;
                    AITimer2 = AITimer3 = 0;
                }
            }
            else if (AIState == Beam)
            {
                AITimer++;
                if (AITimer < 50)
                {
                    vel = RegreUtils.FromAToB(NPC.Center, player.Center);
                    NPC.rotation = MathHelper.Lerp(NPC.rotation, (RegreUtils.FromAToB(NPC.Center, player.Center).ToRotation() + MathHelper.PiOver2), AITimer / 250);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust;
                        Vector2 position = NPC.Center;
                        dust = Terraria.Dust.NewDustDirect(position, 30, 30, 71, 0, 0, 0, new Color(255, 255, 255), 1f);
                        dust.velocity = RegreUtils.FromAToB(dust.position, NPC.Center);
                    }
                }
                if (AITimer == 55)
                {

                    RegreUtils.SpawnTelegraphLine(NPC.Center, NPC.GetSource_FromAI(), vel);
                }
                if (AITimer == 65)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<LuminaryBeam>(), 15, 0);
                }
                if (AITimer >= 200)
                {
                    AITimer = 0;
                    if (NPC.life < NPC.lifeMax / 2)
                        nextAttack = Boomerang;
                    else
                        nextAttack = Scythe;
                    AIState = Idle;
                    AITimer2 = AITimer3 = 0;
                }
            }
            else if (AIState == Boomerang)
            {
                AITimer++;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.velocity.X * 0.05f, AITimer / 250);
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - Vector2.UnitY * 200f, false) * 0.007f;
                if (AITimer < 45)
                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust;
                        Vector2 position = NPC.Center;
                        dust = Terraria.Dust.NewDustDirect(position, 0, 0, 71, 0, 0, 0, new Color(255, 255, 255), 1f);
                    }
                if (AITimer == 55)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitX * 10f * i, ModContent.ProjectileType<LuminaryP>(), 15, 0, NPC.target);
                        a.ai[0] = NPC.whoAmI;
                        a.ai[1] = RegreUtils.CircleDividedEqually(i, 2);
                    }
                }
                if (AITimer >= 420) //funny number lmao :rofl:
                {
                    AITimer = 0;
                    nextAttack = NoMovement;
                    AIState = Idle;
                    AITimer2 = AITimer3 = 0;
                }
            }
            else if (AIState == NoMovement)
            {
                AITimer++;
                AITimer2++;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.velocity.X * 0.05f, AITimer / 400);
                AITimer3 += 0.05f;
                NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center - new Vector2(200 * (float)Math.Sin(AITimer3), 200f), false) * 0.05f;
                if (AITimer2 == 30)
                {
                    toTheSide = Main.rand.NextBool();
                    Vector2 vell = new(toTheSide ? 1 : 0, toTheSide ? 0 : 1);
                    Beamposition = new(toTheSide ? Main.screenPosition.X : player.Center.X, toTheSide ? player.Center.Y : Main.screenPosition.Y);
                    if (!toTheSide)
                        for (int i = -2; i < 3; ++i)
                        {
                            float x = (Beamposition + Vector2.UnitX * 150 * i).X;
                            float y = (Beamposition + Vector2.UnitX * 150 * i).Y;
                            RegreUtils.SpawnTelegraphLine(Beamposition + Vector2.UnitX * 150 * i, NPC.GetSource_FromThis(), vell);
                            Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LuminaryP3>(), 0, 0, NPC.target);
                            a.ai[0] = x;
                            a.ai[1] = y;
                            a.localAI[0] = NPC.whoAmI;
                        }
                    else
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            Vector2 pos = i == 0 ? Beamposition : Beamposition + Vector2.UnitX * Main.screenWidth;
                            RegreUtils.SpawnTelegraphLine(Beamposition, NPC.GetSource_FromThis(), vell);
                            Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LuminaryP3>(), 0, 0, NPC.target);
                            a.ai[0] = pos.X;
                            a.ai[1] = pos.Y;

                            a.localAI[0] = NPC.whoAmI;
                        }
                    }
                }
                if (AITimer2 == 60)
                {
                    AITimer2 = 0;
                    Vector2 vell = new(toTheSide ? 1 : 0, toTheSide ? 0 : 1);
                    if (!toTheSide)
                        for (int i = -2; i < 3; ++i)
                        {
                            float x = (Beamposition + Vector2.UnitX * 150 * i).X;
                            float y = (Beamposition + Vector2.UnitX * 150 * i).Y;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), Beamposition + Vector2.UnitX * 150 * i, vell, ModContent.ProjectileType<LuminaryBeam3>(), 15, 0);
                        }
                    else
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            Vector2 pos = i == 0 ? Beamposition : Beamposition + Vector2.UnitX * Main.screenWidth;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, vell, ModContent.ProjectileType<LuminaryBeam3>(), 15, 0);
                        }
                    }
                }
                if (AITimer >= 60 * 5)
                {
                    AITimer = 0;
                    nextAttack = Scythe;
                    AIState = Idle;
                    AITimer2 = AITimer3 = 0;
                }
            }
        }
    }
    public class LuminaryP3 : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new(118, 84);
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 170;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.frame = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = TextureAssets.Npc[ModContent.NPCType<PreHMLuminary>()].Value;
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 84, 118, 84), Color.DeepPink * alpha, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        float alpha = 1;
        int startingTimeLeft;
        float thing;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        Vector2 targetPos;
        Vector2 basePos;
        public override void OnSpawn(IEntitySource source)
        {
            basePos = Projectile.Center;
            targetPos = new(Projectile.ai[0], Projectile.ai[1]);
        }
        public override void AI()
        {
            targetPos = new(Projectile.ai[0], Projectile.ai[1]);
            if (++Projectile.frameCounter % 5 == 0)
            {
                if (Projectile.frame < 5)
                    Projectile.frame++;
                else
                    Projectile.frame = 0;
            }
            if (targetPos == Vector2.Zero)
                return;
            NPC owner = Main.npc[(int)Projectile.localAI[0]];
            /*Projectile.ai[1] += 2f * (float)Math.PI / 600f * 4 * alpha * thing;
            Projectile.ai[1] %= 2f * (float)Math.PI;
            Projectile.Center = npc.Center + (200 * alpha) * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));*/
            if (Projectile.timeLeft > 100)
                Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, targetPos, false) * 0.1f;
            else
            {
                alpha -= 0.05f;
                Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, owner.Center, false) * 0.1f;
            }

        }
    }
    public class LuminaryP : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new(118, 84);
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.frame = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = TextureAssets.Npc[ModContent.NPCType<PreHMLuminary>()].Value;
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 84, 118, 84), Color.DeepPink * alpha, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        float alpha;
        public override void AI()
        {
            if (++Projectile.frameCounter % 5 == 0)
            {
                if (Projectile.frame < 8)
                    Projectile.frame++;
                else
                    Projectile.frame = 5;
            }
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.ai[0] == -1)
                Projectile.Kill();
            if (!npc.active)
                return;
            if (Projectile.timeLeft == 385)
                Projectile.velocity = Vector2.Zero;
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * 5 * alpha;
            Projectile.ai[1] %= 2f * (float)Math.PI;
            Projectile.Center = npc.Center + (200 * alpha) * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            if (Projectile.timeLeft == 365)
            {
                Projectile aaa = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY, ModContent.ProjectileType<LuminaryBeam2>(), 15, 0);
                aaa.ai[1] = Projectile.whoAmI;

            }
            float progress = Utils.GetLerpValue(0, 400, Projectile.timeLeft);
            alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 0.75f);
        }
    }
    public class LuminaryP2 : ModProjectile
    {
        public override string Texture => RegreUtils.Empty;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new(118, 84);
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 200;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.frame = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D a = TextureAssets.Npc[ModContent.NPCType<PreHMLuminary>()].Value;
            Main.EntitySpriteDraw(a, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 84, 118, 84), Color.DeepPink * alpha, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);
            return false;
        }
        float alpha;
        int startingTimeLeft;
        float thing;
        public override void AI()
        {
            if (startingTimeLeft == 0)
                startingTimeLeft = Projectile.timeLeft;
            if (++Projectile.frameCounter % 5 == 0)
            {
                if (Projectile.frame < 5)
                    Projectile.frame++;
                else
                    Projectile.frame = 0;
            }
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active)
                return;
            thing += 0.025f;
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * 4 * alpha * thing;
            Projectile.ai[1] %= 2f * (float)Math.PI;
            Projectile.Center = npc.Center + (200 * alpha) * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
            float progress = Utils.GetLerpValue(0, startingTimeLeft, Projectile.timeLeft);
            alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 0.75f);

            if (alpha < 1 && Projectile.timeLeft < 75)
            {
                RegreUtils.DustExplosion(Projectile.Center, Projectile.Size, true, Color.DeepPink);
                Projectile.Kill();
            }
        }
    }
    public class HomingScythe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(new("Regressus/Sounds/Custom/LuminaryConjure"));
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.direction = Projectile.spriteDirection = -1;
            if (Projectile.timeLeft > 45)
            {
                if (++Projectile.frameCounter % 5 == 0 && Projectile.frame < 7)
                    Projectile.frame++;
            }
            else
            {
                if (++Projectile.frameCounter % 5 == 0 && Projectile.frame > 0)
                    Projectile.frame--;
            }
            Vector2 move = Vector2.Zero;
            float distance = 5050f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.player[k].active)
                {
                    Vector2 newMove = Main.player[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (++Projectile.ai[0] % 5 == 0 && target && Projectile.timeLeft > 45 && Projectile.timeLeft < 255)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (6.2f * Projectile.velocity + move) / 6.2f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6.2f)
            {
                vector *= 6.2f / magnitude;
            }
        }
    }
    public class LuminaryBeam2 : LuminaryBeamBase
    {
        public override void Extra()
        {
            MAX_TIME = 370;
            Projectile.timeLeft = (int)MAX_TIME;
        }
        public override void ExtraAI()
        {
            Projectile npc = Main.projectile[(int)Projectile.ai[1]];
            if (!npc.active || npc.type != ModContent.ProjectileType<LuminaryP>())
                Projectile.Kill();
            Projectile.Center = npc.Center;
        }
    }
    public class LuminaryBeam : LuminaryBeamBase
    {
        public override void Extra()
        {
            MAX_TIME = 70;
            Projectile.timeLeft = (int)MAX_TIME;
        }
    }
    public class LuminaryBeam3 : LuminaryBeamBase
    {
        public override void Extra()
        {
            MAX_TIME = 26;
            Projectile.timeLeft = (int)MAX_TIME;
        }
    }
    public abstract class LuminaryBeamBase : ModProjectile
    {

        protected bool RunOnce = true;
        public int MAX_TIME = 400;
        public override string Texture => "Regressus/Extras/Empty";
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath44);
        }
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 2000;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hide = true;
            Extra();
        }
        public virtual void Extra()
        {
            Projectile.timeLeft = (int)MAX_TIME;
        }
        public virtual void ExtraAI()
        {

        }
        // Cross Product: Where a = line point 1; b = line point 2; c = point to check against.
        public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Bolt");
        }
        int damage;
        Vector2 vel;
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public override void AI()
        {
            ExtraAI();
            if (RunOnce)
            {
                damage = Projectile.damage;
                MAX_TIME = Projectile.timeLeft;
                RunOnce = false;
            }
            if (Projectile.localAI[1] != 0)
            {
                Projectile.damage = 0;
                Projectile.timeLeft = MAX_TIME;
                Projectile.localAI[1]--;
            }
            else
            {
                Projectile.damage = damage;
            }
            vel = Projectile.velocity;
            vel.Normalize();
            Vector2 end = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);

            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.velocity = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * (MAX_TIME <= 25 ? 2 : 2 * (Projectile.ai[0] + 1)), 0, 1);
        }
        public Color BeamColor = new Color(20, 63, 128);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, 25 * Projectile.scale, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            Texture2D bolt = RegreUtils.GetExtraTexture("laser");
            Texture2D bolt1 = RegreUtils.GetExtraTexture("laser2");
            Texture2D bolt2 = RegreUtils.GetExtraTexture("laser3");

            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            //float scale = Projectile.scale * 2 * mult;
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles i want to kill myself
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = Color.White;
            BeamPacket.SetTexture(0, bolt);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Color.DeepPink;
            BeamPacket.SetTexture(0, bolt1);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Color.HotPink;
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, bolt2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.DeepPink, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.DeepPink, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
}
