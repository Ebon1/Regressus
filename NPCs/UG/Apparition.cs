using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;

namespace Regressus.NPCs.UG
{
    public class Apparition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banshee");
            Main.npcFrameCount[Type] = 18;
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.1f;
            return 0;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White * (AIState == RunAway ? NPC.ai[2] : 1f);
        }
        public override void SetDefaults()
        {
            NPC.width = 66;
            NPC.height = 132;
            NPC.lifeMax = 200;
            NPC.defense = 10;
            NPC.damage = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.dontTakeDamage = true;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (AIState == TP || AIState == RunAway)
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 0 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 1 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 2 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 3 * frameHeight;
                else if (NPC.frameCounter < 25)
                    NPC.frame.Y = 4 * frameHeight;
                else if (NPC.frameCounter < 30)
                    NPC.frame.Y = 5 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 6 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 7 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 8 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 9 * frameHeight;
                else if (NPC.frameCounter < 25)
                    NPC.frame.Y = 10 * frameHeight;
                else if (NPC.frameCounter < 30)
                    NPC.frame.Y = 11 * frameHeight;
                else if (NPC.frameCounter < 35)
                    NPC.frame.Y = 12 * frameHeight;
                else if (NPC.frameCounter < 40)
                    NPC.frame.Y = 13 * frameHeight;
                else if (NPC.frameCounter < 45)
                    NPC.frame.Y = 14 * frameHeight;
                else if (NPC.frameCounter < 50)
                    NPC.frame.Y = 15 * frameHeight;
                else if (NPC.frameCounter < 55)
                    NPC.frame.Y = 16 * frameHeight;
                else if (NPC.frameCounter < 60)
                    NPC.frame.Y = 17 * frameHeight;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            var fadeMult = 1f / NPCID.Sets.TrailCacheLength[Type];
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(tex, NPC.oldPos[i] - Main.screenPosition + NPC.Size / 2, NPC.frame, Color.White * (1f - fadeMult * i) * (AIState == RunAway ? NPC.ai[2] : 1), NPC.oldRot[i], NPC.Size / 2, NPC.scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }

            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AIState == Screech)
            {
                Texture2D texture = RegreUtils.GetExtraTexture("PulseCircle");
                RegreUtils.Reload(spriteBatch, BlendState.Additive);
                for (int i = 1; i < 4; i++)
                {
                    float scale = i * NPC.ai[3] * 0.4f;
                    float alpha = MathHelper.Clamp((float)Math.Sin(NPC.ai[3] * Math.PI), 0, 1);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.White * alpha, 0, texture.Size() / 2, scale, SpriteEffects.None, 0f);
                }
                RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A wandering apparition that hunts and kills living beings by shrieking behind them. While impervious to weapons, their form becomes unstable when watched."),
            });
        }
        private const int TP = 0;
        private const int Screech = 1;
        private const int RunAway = 2;
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
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (AIState == TP)
            {
                AITimer++;
                if (AITimer == 30)
                {
                    NPC.Center = player.Center - Vector2.UnitX * player.direction * 145;
                }
                if (AITimer >= 100)
                {
                    AITimer = 0;
                    if (player.direction != NPC.direction)
                    {
                        AIState = RunAway;
                        NPC.ai[2] = 1;
                    }
                    else
                    {
                        NPC.frameCounter = 0;
                        AIState = Screech;
                    }
                }
            }
            else if (AIState == Screech)
            {
                AITimer++;
                NPC.ai[3] += 0.06f;
                if (NPC.ai[3] > 1.2f)
                {
                    NPC.ai[3] = 0;
                }
                if (AITimer == 1)
                {
                    RegreSystem.ScreenShakeAmount = 10f;
                    Terraria.Audio.SoundStyle ae = new Terraria.Audio.SoundStyle("Regressus/Sounds/Custom/GhostScream");
                    Terraria.Audio.SoundEngine.PlaySound(ae);
                }
                if (AITimer == 30)
                {
                    player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " was consumed by the banshee!"), 20, 0);
                }
                if (AITimer >= 60)
                {
                    AITimer = 0;
                    AIState = TP;
                }
            }
            else if (AIState == RunAway)
            {
                NPC.ai[2] -= 0.05f;
                if (NPC.ai[2] < 0)
                {
                    NPC.life = 0;
                }
            }
        }
    }
}