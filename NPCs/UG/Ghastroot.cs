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
    public class Ghastroot : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 20;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 100;
            NPC.defense = 5;
            NPC.Size = new Vector2(104, 86);
            NPC.dontTakeDamage = false;
            NPC.aiStyle = 0;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }
        public override void FindFrame(int frameHeight)
        {
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
                else if (NPC.frameCounter < 65)
                    NPC.frame.Y = 18 * frameHeight;
                else if (NPC.frameCounter < 70)
                    NPC.frame.Y = 19 * frameHeight;
                else
                {
                    AIState = Idle;
                    NPC.frameCounter = 0;
                }
            }
        }
        const int Idle = 0;
        const int Attack = 1;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.21f;
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
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (AIState == Idle)
            {
                AITimer++;
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    NPC.frameCounter = 0;
                    AIState = Attack;
                }
            }
            else
            {
                if (NPC.frameCounter == 37)
                {
                    float rotation = MathHelper.ToRadians(45);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9);
                    Vector2 pos = NPC.Center - Vector2.UnitY * 23;
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 perturbedSpeed = (RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, perturbedSpeed, ModContent.ProjectileType<Projectiles.Enemy.UG.MagerootProj>(), 15, 1.5f, player.whoAmI);
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, RegreUtils.FromAToB(NPC.Center, player.Center) * 9.5f, ModContent.ProjectileType<Projectiles.Enemy.UG.MagerootProj>(), 15, 1.5f, player.whoAmI);
                }
            }
        }
    }
}
