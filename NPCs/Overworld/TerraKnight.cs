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
            Main.npcFrameCount[Type] = 8;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D a = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            Texture2D b = RegreUtils.GetExtraTexture("glow");
            spriteBatch.Draw(a, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);
            /* RegreUtils.Reload(spriteBatch, BlendState.Additive);
            spriteBatch.Draw(b, NPC.Center - screenPos, null, new Color(0, 255, Main.DiscoB) * 0.75f, NPC.rotation, new Vector2(512, 512) / 2, NPC.scale / 3, effects, 0f);
            RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);*/
            return false;
        }
        int height;
        public override void FindFrame(int frameHeight)
        {
            height = frameHeight;
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
        const int Walk = 0;
        const int Slash = 1;
        const int Dash = 2;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
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
                    AITimer2 = 0;
                    NPC.velocity.Y -= 6.7f;
                }

                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Slash;
                }
            }
            else if (AIState == Slash)
            {
                AITimer++;
                AITimer2++;
                if (AITimer2 == 20)
                {
                    AITimer2 = 0;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, RegreUtils.FromAToB(NPC.Center, player.Center, true) * 7f, ModContent.ProjectileType<TerraKnightSlash>(), 20, 1);
                }
                if (AITimer >= 60)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Dash;
                }
            }
            else if (AIState == Dash)
            {
                AITimer++;
                if (AITimer == 100)
                {
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 20) * -1;
                }
                if (AITimer > 115)
                    NPC.velocity.X *= 0.9f;
                if (AITimer == 130)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Walk;
                }
            }
        }
    }
}
