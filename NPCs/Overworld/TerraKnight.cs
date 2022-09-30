using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Regressus.NPCs;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Enemy.Overworld;

namespace Regressus.NPCs.Overworld
{
    public class TerraKnight : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
        }
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 68;
            NPC.lifeMax = 400;
            NPC.defense = 10;
            NPC.damage = 35;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight)
                return 0.015f;
            return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement(""),
            });
        }
        float swordRot = 0;
        public float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        public float Lerp(float x)
        {
            return x < 0.5f ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;
        }
        int swordDir = 1;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/EarthDivider_Glow").Value;
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Items/Weapons/Melee/EarthDivider").Value;
            Vector2 swordPos = Vector2.Zero;
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            const float TwoPi = (float)Math.PI * 2f;
            float test = Main.GlobalTimeWrappedHourly * TwoPi / 2f;

            Texture2D a = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            spriteBatch.Draw(a, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);
            /* RegreUtils.Reload(spriteBatch, BlendState.Additive);
            spriteBatch.Draw(b, NPC.Center - screenPos, null, new Color(0, 255, Main.DiscoB) * 0.75f, NPC.rotation, new Vector2(512, 512) / 2, NPC.scale / 3, effects, 0f);
            RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);*/
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (flash != Vector2.Zero)
            {
                RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                float progress = Utils.GetLerpValue(0f, 25, AITimer2);
                Texture2D glow = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.3f);
                Main.spriteBatch.Draw(glow, flash - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.0025f, glow.Size() / 2, 0.75f * mult, SpriteEffects.None, 0f); ;
                RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            }
        }
        int height;
        Vector2 flash;
        public override void FindFrame(int frameHeight)
        {
            height = frameHeight;
            Player player = Main.player[NPC.target];
            if (AIState == Walk)
            {
                if (NPC.velocity.Y == 0 || (NPC.velocity.X >= 1 && NPC.velocity.X <= -1))
                    NPC.frameCounter++;
                else
                    NPC.frame.Y = 3 * frameHeight;
                if (NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    if (NPC.frame.Y >= 7 * frameHeight)
                        NPC.frame.Y = 0;
                    else
                        NPC.frame.Y += frameHeight;
                }
            }
            else if (AIState == DualSlash)
            {
                NPC.frameCounter++;
                if (NPC.frame.Y == 12 * frameHeight && AITimer2 < 1)
                {
                    AITimer2 = 1;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.direction * Vector2.UnitX, ModContent.ProjectileType<TerraKnightP>(), 0, 0, player.whoAmI, ai0: -1, ai1: NPC.whoAmI);
                }
                if (NPC.frame.Y == 17 * frameHeight && AITimer2 < 3)
                {
                    AITimer2 = 3;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.direction * Vector2.UnitX, ModContent.ProjectileType<TerraKnightP>(), 0, 0, player.whoAmI, ai0: 1, ai1: NPC.whoAmI);
                }
                if (NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    if (NPC.frame.Y >= 20 * frameHeight)
                        AITimer = 1;
                    else
                        NPC.frame.Y += frameHeight;
                }
            }
            else if (AIState == Dash)
            {
                if (NPC.ai[3] == 2)
                {
                    NPC.frame.Y = 21 * frameHeight;
                    NPC.frameCounter = 0;
                }
                else if (NPC.ai[3] == 0)
                {
                    NPC.frame.Y = 3 * frameHeight;
                    NPC.frameCounter = 0;
                }
                else
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter >= 5)
                    {
                        NPC.frameCounter = 0;
                        if (NPC.frame.Y >= 24 * frameHeight)
                            AITimer = 120;
                        else
                            NPC.frame.Y += frameHeight;
                    }
                }
            }
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
        float idkAnymore;
        const int NoTarget = 0;
        const int Walk = -1;
        const int DualSlash = 1;
        const int Dash = 2;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (AIState == NoTarget && NPC.Center.Distance(player.Center) < 500f)
                AIState = Walk;
            if (AIState == Walk)
            {
                AITimer++;
                AITimer3--;
                if (NPC.Center.Distance(player.Center) > 20f)
                {
                    if (NPC.collideY)
                        NPC.velocity.X += 0.01f * NPC.direction;
                    else
                        NPC.velocity.X += 0.0025f * NPC.direction;
                }
                if (Math.Sign(NPC.velocity.X) != NPC.direction || NPC.Center.Distance(player.Center) < 90f)
                {
                    NPC.frame.Y = 3 * height;
                    NPC.frameCounter = 0;
                    NPC.velocity.X *= 0.92f;
                }
                if (NPC.collideX && AITimer2 != 1 && AITimer3 <= 0)
                {
                    AITimer3 = 30;
                    AITimer2 = 1;
                }
                if (AITimer2 == 1)
                {
                    AITimer2 = 2;
                    NPC.velocity.Y -= 6.7f;
                }
                if (NPC.velocity.Y < 0 && AITimer3 == 20)
                    NPC.velocity.X = NPC.direction * 2;
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.frameCounter = 0;
                    AIState = DualSlash;
                }
            }
            else if (AIState == DualSlash)
            {
                if (AITimer2 == 1 || AITimer2 == 3)
                {
                    AITimer2++;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, RegreUtils.FromAToB(NPC.Center, player.Center, true) * 7f, ModContent.ProjectileType<TerraKnightSlash>(), 20, 1);
                }
                AITimer3++;
                if (AITimer3 == 50 && swordDir == 1)
                {
                    AITimer3 = 0;
                    swordDir = -1;
                }
                if (AITimer == 1)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Dash;
                    NPC.frameCounter = 0;
                }
            }
            else if (AIState == Dash)
            {
                if (AITimer < 111)
                    AITimer++;
                if (AITimer == 60)
                {
                    AITimer2++;
                    flash = NPC.Center - Vector2.UnitY * (NPC.height / 4);
                }
                if (AITimer2 > 1)
                    AITimer2++;
                if (AITimer == 85)
                {
                    NPC.ai[3] = 2;
                    AITimer2 = 0;
                    flash = Vector2.Zero;
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 20) * -1;
                }
                if (NPC.Center.Distance(player.Center) < 35 || AITimer >= 110)
                {
                    NPC.ai[3] = 1;
                    NPC.velocity.X *= 0.8f;
                }
                if (idkAnymore == 1)
                {

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.direction * Vector2.UnitX, ModContent.ProjectileType<TerraKnightP>(), 0, 0, player.whoAmI, ai0: 1, ai1: NPC.whoAmI);
                }
                if (NPC.ai[3] == 1)
                {
                    idkAnymore++;
                }
                /*if (AITimer == 100)
                {
                    AITimer2++;
                    flash = player.Center - Vector2.UnitY * 200;
                }
                if (AITimer == 125)
                {
                    AITimer2 = 0;
                    NPC.Center = flash;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y *= 1.2f;
                }
                if (AITimer == 126)
                {
                    flash = Vector2.Zero;
                }*/
                if (AITimer == 120)
                {
                    idkAnymore = 0;
                    NPC.ai[3] = 0;
                    AITimer = 0;
                    swordDir = 1;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Walk;
                    NPC.frameCounter = 0;
                }
            }
        }
    }
}
