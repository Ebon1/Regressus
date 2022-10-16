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

using Regressus.Buffs.Debuffs;

namespace Regressus.NPCs
{
    public class Pilgrim : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 68;
            NPC.lifeMax = 65;
            NPC.defense = 4;
            NPC.damage = 15;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
                return 0.025f;
            return 0;
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
        const int FlashbangPrep = 1;
        const int Flashbang = 2;
        const int Walk = -1;
        Vector2 pos;
        public override Color? GetAlpha(Color drawColor)
        {
            if (AIState == Flashbang || AIState == FlashbangPrep)
                return Color.White * AITimer3;
            return Color.White;
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D a = RegreUtils.GetExtraTexture("PulseCircle");
            if (AIState == FlashbangPrep)
            {
                sb.Reload(BlendState.Additive);
                float alpha = MathHelper.Clamp((float)Math.Sin(AITimer2 * Math.PI), 0, 1);
                if (AITimer2 < 1)
                    sb.Draw(a, pos - screenPos, null, Color.White * alpha, 0, a.Size() / 2, 0.4f * AITimer2, SpriteEffects.None, 0f);
                sb.Reload(BlendState.AlphaBlend);
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (AIState == Flashbang && AITimer > 10 && AITimer < 50)
                if (!target.HasBuff(ModContent.BuffType<PilgrimBlindness>()))
                    target.AddBuff(ModContent.BuffType<PilgrimBlindness>(), 360);
        }
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
                    AITimer3 = 1;
                    NPC.velocity = Vector2.Zero;
                    AIState = FlashbangPrep;
                }
            }
            else if (AIState == FlashbangPrep)
            {
                NPC.damage = 0;
                AITimer++;
                if (AITimer3 > 0f)
                    AITimer3 -= 0.05f;

                if (AITimer2 < 1f)
                    AITimer2 += 0.02f;
                if (AITimer == 1)
                    pos = player.Center - Vector2.UnitY * 20;
                if (AITimer >= 100)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    NPC.velocity = Vector2.Zero;
                    AIState = Flashbang;
                }
            }
            else if (AIState == Flashbang)
            {
                AITimer++;
                if (AITimer < 30 && AITimer > 10)
                    AITimer3 += 0.05f;
                if (AITimer > 20)
                    NPC.damage = 15;
                if (AITimer == 10)
                {
                    NPC.Center = pos;
                }
                if (AITimer >= 190)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 1;
                    NPC.velocity = Vector2.Zero;
                    AIState = Walk;
                }
            }
        }
    }
}
