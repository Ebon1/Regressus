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
            NPC.lifeMax = 350;
            NPC.defense = 10;
            NPC.damage = 35;
            NPC.knockBackResist = 0.2f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime && NPC.downedMechBoss3 && NPC.downedMechBoss2 && NPC.downedMechBoss1)
                return 0.25f;
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
            spriteBatch.Draw(a, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);
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
        public override void AI()
        {
            NPC.ai[1]--;
            NPC.aiStyle = -1;
            NPC.direction = Main.player[NPC.target].Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (NPC.Center.Distance(Main.player[NPC.target].Center) > 90f)
            {
                if (NPC.collideY)
                    NPC.velocity.X += 0.01f * NPC.direction;
                else
                    NPC.velocity.X += 0.0025f * NPC.direction;
            }
            if (Math.Sign(NPC.velocity.X) != NPC.direction || NPC.Center.Distance(Main.player[NPC.target].Center) < 90f)
            {
                NPC.frame.Y = 3 * height;
                NPC.frameCounter = 0;
                NPC.velocity.X *= 0.92f;
            }
            if (NPC.collideX && NPC.ai[2] != 1 && NPC.ai[1] <= 0)
            {
                NPC.ai[1] = 30;
                NPC.ai[2] = 1;
            }
            if (NPC.ai[2] == 1)
            {
                NPC.ai[2] = 0;
                NPC.velocity.Y -= 6.7f;
            }

            if (NPC.Center.Distance(Main.player[NPC.target].Center) < 750f && NPC.ai[0] == 0)
            {
                NPC.ai[0] = 1;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 70, Vector2.Zero, ModContent.ProjectileType<TerraKnightSword>(), NPC.damage / 2, 1f, Main.player[NPC.target].whoAmI, NPC.whoAmI);
            }
        }
    }
}
