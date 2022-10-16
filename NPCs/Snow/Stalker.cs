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

using Terraria.Audio;

namespace Regressus.NPCs.Snow
{
    public class Stalker : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 62;
            NPC.lifeMax = 50;
            NPC.defense = 2;
            NPC.damage = 15;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
            NPC.value = 1;
            NPC.lavaImmune = false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSnow)
                return 0.015f;
            return 0;
        }
        SpriteEffects[] flyDir = new SpriteEffects[5];
        Vector2[] flyPos = new Vector2[5], flyVel = new Vector2[5];
        /*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (AIState == Idle)
            {

                Texture2D fly = RegreUtils.GetExtraTexture("Sprites/Fly");
                Rectangle rect = NPC.getRect();
                float a = (float)Math.Sin(thing * Math.PI / 60);
                int YFrame = 0;
                if (thing % 2 == 0)
                    YFrame = 26;
                for (int i = 0; i < 5; i++)
                {
                    if (thing == 0)
                    {
                        flyDir[i] = Main.rand.NextBool() ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                        flyPos[i] = Main.rand.NextVector2FromRectangle(rect);
                        flyVel[i] = (Vector2.One * 0.5f).RotatedByRandom(MathHelper.ToRadians(360));
                    }
                    flyPos[i] += flyVel[i];
                    spriteBatch.Draw(fly, flyPos[i] - Main.screenPosition, new Rectangle(0, YFrame, 22, 26), Color.White * a, 0f, fly.Size() / 2, a * 0.5f, flyDir[i], 0f);
                }

                return false;
            }
            return true;
        }*/
        float thing = -1;
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
        const int Idle = 0;
        const int Attack = 1;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (AIState == Idle)
                return false;
            return true;
        }
        public override void AI()
        {
            thing++;
            if (thing >= 60)
                thing = 0;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (AIState == Idle)
            {
                NPC.noTileCollide = true;
                NPC.hide = true;
                NPC.behindTiles = true;
                SoundEngine.PlaySound(SoundID.Dig);
                Dust.NewDustPerfect(NPC.Center, DustID.Snow, new Vector2(Main.rand.NextFloat(-1f, 1f), -2f));
                if (AITimer < 240)
                    NPC.velocity.X = RegreUtils.FromAToB(NPC.Center, player.Center).X * 5f;
                NPC.Center = new Vector2(NPC.Center.X, (player.Center + Vector2.UnitY * RegreUtils.TRay.CastLength(player.Center, Vector2.UnitY, 1000f)).Y - (NPC.height / 2));

                if (RegreUtils.TRay.CastLength(NPC.Center, Vector2.UnitY, 1000f) > 16f)
                    NPC.Center += Vector2.UnitY * (RegreUtils.TRay.CastLength(NPC.Center, Vector2.UnitY, 1000f) - 32f);
                AITimer++;
                if (AITimer >= 270)
                {
                    AITimer = 0;
                    AIState++;
                }
            }
            else if (AIState == Attack)
            {
                AITimer++;
                if (RegreUtils.TRay.CastLength(NPC.Center, Vector2.UnitY, 1000f) < 16f)
                    NPC.Center -= Vector2.UnitY * 16f;
                if (AITimer == 20)
                {
                    NPC.noTileCollide = false;
                    NPC.hide = false;
                    NPC.behindTiles = false;
                    NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center) * 10f;
                }
                if (NPC.collideY)
                    NPC.velocity.X *= 0.85f;
                if (AITimer > 40 && NPC.collideY)
                {
                    AITimer = 0;
                    AIState = Idle;
                }
            }
        }
    }
}
