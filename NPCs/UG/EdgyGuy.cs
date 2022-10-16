using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Regressus.Projectiles.Enemy.UG;
using SteelSeries.GameSense.DeviceZone;

namespace Regressus.NPCs.UG
{
    public class EdgyGuy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hunter");
            Main.npcFrameCount[Type] = 12;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 100;
            NPC.defense = 5;
            NPC.Size = new Vector2(84, 88);
            NPC.dontTakeDamage = false;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void FindFrame(int frameHeight)
        {
            height = frameHeight;
            NPC.frameCounter++;
            if (AIState == Idle)
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 0 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 1 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 2 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 3 * frameHeight;
                else
                    NPC.frameCounter = 0;
            }
            else if (AIState == Prepare)
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 4 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 5 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 6 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 7 * frameHeight;
                if (NPC.frameCounter == 20)
                {
                    Player player = Main.player[NPC.target];
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - Vector2.UnitY * 6f, Vector2.Zero, ModContent.ProjectileType<PhantasmalArrow>(), 10, 0f, player.whoAmI, NPC.whoAmI);
                }
            }
            else
            {
                if (NPC.frameCounter < 5)
                    NPC.frame.Y = 8 * frameHeight;
                else if (NPC.frameCounter < 10)
                    NPC.frame.Y = 9 * frameHeight;
                else if (NPC.frameCounter < 15)
                    NPC.frame.Y = 10 * frameHeight;
                else if (NPC.frameCounter < 20)
                    NPC.frame.Y = 11 * frameHeight;
                else
                {
                    AIState = Idle;
                    NPC.frameCounter = 0;
                }
            }
        }
        const int Idle = 0;
        const int Prepare = 1;
        const int Shoot = 2;
        int height;
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
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D a = RegreUtils.GetTexture("NPCs/UG/EdgyGuy_Glow");
            spriteBatch.Draw(a, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, 1, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override void OnKill()
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = 2f * (float)Math.PI / 6f * i;
                Vector2 vel = Vector2.One.RotatedBy(angle);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel * 15f, ModContent.ProjectileType<PhantasmalArrow2>(), 15, 0f);
            }
        }
        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];
            if (AIState == Idle)
            {
                if (player.Center.Distance(NPC.Center) < 1920f)
                {
                    AITimer++;
                    if (AITimer < 300)
                        NPC.velocity = RegreUtils.FromAToB(NPC.Center, player.Center, true) * 2.5f;
                }
                if (AITimer > 300)
                    NPC.velocity *= 0.96f;
                if (AITimer >= 350)
                {
                    NPC.velocity = Vector2.Zero;
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = Prepare;
                }
            }
            else if (AIState == Prepare)
            {

                if (NPC.frame.Y == height * 7)
                    AITimer++;
                if (AITimer >= 50)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.frameCounter = 0;
                    AIState = Shoot;
                }
            }
        }
    }
}
