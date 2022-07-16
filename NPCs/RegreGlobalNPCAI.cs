using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Regressus.NPCs.Bosses.Oracle;
namespace Regressus.NPCs
{
    public class RegreGlobalNPCAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == 4)
                {
                    NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
                    {
                        PortraitScale = 1,
                        PortraitPositionYOverride = 5f
                    };
                    Main.npcFrameCount[npc.type] = 9;
                }
            }
        }
        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npc.aiStyle = 0;
                npc.width = 150;
                npc.height = 112;
                npc.dontTakeDamage = true;
                Main.npcFrameCount[npc.type] = 9;
                NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            }
            if (npc.type == NPCID.ServantofCthulhu)
            {
                npc.aiStyle = 0;
                npc.width = 68;
                npc.height = 34;
                npc.life = 250;
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (npc.ai[0] == 0 && npc.ai[1] == 4)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                    Texture2D vignette1 = ModContent.Request<Texture2D>("Regressus/Extras/Vignette_intense").Value;
                    spriteBatch.Draw(vignette1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * EoCVignetteAlphaValue);
                    Main.spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
        }
        Rectangle frame;
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                bool isDashing = (npc.ai[0] != 0); //add dash check here
                if (npc.IsABestiaryIconDummy)
                {
                    npc.rotation = 0;
                    npc.scale = 0.75f;
                    frame.Y = 8 * 114;
                }
                SpriteEffects effects = npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D eoc = RegreUtils.GetExtraTexture("Sprites/ChadEoC");
                Texture2D eoc_glow = RegreUtils.GetExtraTexture("Sprites/ChadEoC_Glow");
                int frameHeight = 114;
                frame.X = 0;
                frame.Width = 210;
                frame.Height = frameHeight;
                if (npc.ai[0] == 0f)
                {
                    if (npc.frameCounter < 5)
                    {
                        frame.Y = 0;
                    }
                    else if (npc.frameCounter < 10)
                    {
                        frame.Y = frameHeight;
                    }
                    else if (npc.frameCounter < 15)
                    {
                        frame.Y = frameHeight * 2;
                    }
                    else
                    {
                        npc.frameCounter = 0;
                    }
                }
                else if (npc.ai[0] == 1f)
                {
                    if (npc.frameCounter < 5)
                    {
                        frame.Y = frameHeight * 3;
                    }
                    else if (npc.frameCounter < 10)
                    {
                        frame.Y = frameHeight * 4;
                    }
                    else if (npc.frameCounter < 15)
                    {
                        frame.Y = frameHeight * 5;
                    }
                    else
                    {
                        npc.frameCounter = 0;
                    }
                }
                else if (npc.ai[0] == 2f)
                {
                    if (npc.frameCounter < 5)
                    {
                        frame.Y = frameHeight * 6;
                    }
                    else if (npc.frameCounter < 10)
                    {
                        frame.Y = frameHeight * 7;
                    }
                    else if (npc.frameCounter < 15)
                    {
                        frame.Y = frameHeight * 8;
                    }
                    else
                    {
                        npc.frameCounter = 0;
                    }
                }
                spriteBatch.Draw(eoc, npc.Center - screenPos, frame, (npc.ai[0] == 0 && npc.ai[1] == 4 ? Color.Black : drawColor), npc.rotation + (npc.spriteDirection == -1 ? MathHelper.Pi : 0), new Vector2(210, 112) / 2, npc.scale, effects, 0f);
                if (npc.ai[0] == 0f)
                    spriteBatch.Draw(eoc_glow, npc.Center - screenPos, frame, (npc.ai[0] == 0 && npc.ai[1] == 4 ? Color.Black : Color.White), npc.rotation + (npc.spriteDirection == -1 ? MathHelper.Pi : 0), new Vector2(210, 112) / 2, npc.scale, effects, 0f);
                return false;
            }
            if (npc.type == NPCID.ServantofCthulhu)
            {
                if (npc.IsABestiaryIconDummy)
                    npc.rotation = 0;
                SpriteEffects effects = npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D eoc = RegreUtils.GetExtraTexture("Sprites/ChadEoCServant");
                Texture2D eoc_glow = RegreUtils.GetExtraTexture("Sprites/ChadEoCServant_Glow");
                npc.frame = new Rectangle(0, 0, 68, 34);
                spriteBatch.Draw(eoc, npc.Center - screenPos, npc.frame, drawColor, npc.rotation, npc.Size / 2, npc.scale, effects, 0f);
                spriteBatch.Draw(eoc_glow, npc.Center - screenPos, npc.frame, Color.White, npc.rotation, npc.Size / 2, npc.scale, effects, 0f);
                return false;
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            base.FindFrame(npc, frameHeight);
            if (npc.type == NPCID.EyeofCthulhu)
            {
            }
        }
        float EoCVignetteAlphaValue;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            base.OnSpawn(npc, source);
            if (npc.type == NPCID.EyeofCthulhu)
            {
                int num = (Main.expertMode ? 15 : 10);
                for (int i = 0; i < num; i++)
                {
                    float angle = 2f * (float)Math.PI / num * i;
                    NPC servant = NPC.NewNPCDirect(npc.GetSource_FromThis(), npc.Center, NPCID.ServantofCthulhu, 0, npc.whoAmI, angle);
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.ServantofCthulhu)
            {
                NPC parent = Main.npc[(int)npc.ai[0]];
                parent.life -= 100;
                int num = (Main.expertMode ? 15 : 10);
            }
        }
        Vector2 EoCarenaCenter;
        public override void AI(NPC npc)
        {
            base.AI(npc);
            if (npc.type == NPCID.EyeofCthulhu)
            {
                //ai 0 = phase 1 = state 2 = timer 3 = extra
                Player player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.TargetClosest(false);
                    player = Main.player[npc.target];
                    if (!player.active || player.dead)
                    {
                        npc.ai[0] = -1;
                        npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                        npc.velocity = new Vector2(0, -15f);
                        if (npc.timeLeft > 10)
                        {
                            npc.timeLeft = 10;
                        }
                        return;
                    }
                }
                if (!(npc.ai[0] == 0 && npc.ai[1] == 4))
                    if (Main.rand.NextBool(5))
                    {
                        int num9 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f);
                        Main.dust[num9].velocity.X *= 0.5f;
                        Main.dust[num9].velocity.Y *= 0.1f;
                    }
                if (!NPC.AnyNPCs(NPCID.ServantofCthulhu) && npc.ai[0] == 0)
                {
                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    npc.velocity = Vector2.Zero;
                    npc.dontTakeDamage = false;
                }
                if (npc.life < npc.lifeMax / 3 && npc.ai[0] == 1)
                {
                    npc.ai[0] = 2;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    npc.velocity = Vector2.Zero;
                }
                switch (npc.ai[0])
                {
                    case 0:
                        switch (npc.ai[1])
                        {
                            case 0: //idle
                                Vector2 idlePos = player.Center - new Vector2(-600 * player.direction, 150);
                                Vector2 moveToIdle = idlePos - npc.Center;
                                float factor = 0.035f;
                                npc.velocity = moveToIdle * factor;
                                npc.ai[2]++;
                                if (npc.ai[2] >= 120)
                                {
                                    int[] attacks = new int[]
                                    {
                                        1, 3, 4
                                    };
                                    int a = Main.rand.Next(attacks);
                                    while (a == npc.localAI[0])
                                    {
                                        a = Main.rand.Next(attacks);
                                    }
                                    npc.ai[1] = (npc.localAI[1] == 1 ? a : 1);
                                    npc.ai[2] = 0;
                                    npc.velocity = Vector2.Zero;
                                }
                                npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                                npc.damage = 0;
                                break;
                            case 1: //dash
                                npc.damage = 15;
                                if (++npc.ai[2] == 20)
                                {
                                    Vector2 vector9 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
                                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                                    npc.velocity.X = (float)(Math.Cos(rotation2) * 27) * -1;
                                    npc.velocity.Y = (float)(Math.Sin(rotation2) * 27) * -1;
                                    SoundEngine.PlaySound(SoundID.ForceRoar, npc.Center);
                                    npc.ai[1] = 2;
                                    npc.ai[2] = 0;
                                    npc.localAI[0] = 1;

                                }
                                npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                                break;
                            case 2: //stop
                                npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                                npc.ai[2] += 1f;
                                if (npc.ai[2] >= 30f)
                                {
                                    npc.velocity *= 0.95f;
                                }
                                if (npc.ai[2] >= 60f)
                                    if (npc.ai[3] < 3)
                                    {
                                        npc.ai[3]++;
                                        npc.ai[2] = 0;
                                        npc.ai[1] = 1;
                                        npc.velocity = Vector2.Zero;
                                    }
                                    else
                                    {
                                        npc.velocity = Vector2.Zero;
                                        npc.ai[3] = 0;
                                        npc.ai[2] = 0;
                                        npc.ai[1] = (npc.localAI[1] == 0 ? 3 : 0);
                                    }
                                break;
                            case 3: //launch servants
                                npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                                npc.ai[2]++;
                                if (npc.ai[2] >= 350)
                                {
                                    npc.ai[1] = (npc.localAI[1] == 0 ? 4 : 0);
                                    npc.ai[2] = 0;
                                    npc.localAI[0] = 3;
                                }
                                break;
                            case 4: //boo
                                npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                                if (npc.ai[2] < 100 && EoCVignetteAlphaValue < 1)
                                    EoCVignetteAlphaValue += 0.01f;
                                else if (npc.ai[2] >= 600 && EoCVignetteAlphaValue > 0)
                                    EoCVignetteAlphaValue -= 0.01f;
                                npc.ai[2]++;
                                if (npc.ai[2] == 1)
                                {
                                    EoCarenaCenter = player.Center;
                                }
                                player.eyeHelper.BlinkBecausePlayerGotHurt();
                                Vector2 moveTo = player.Center - npc.Center;
                                float Bfactor = 0.0067f;
                                if (Bfactor != 0)
                                    npc.velocity = moveTo * Bfactor;

                                if (npc.ai[2] >= 700)
                                {
                                    npc.ai[1] = 0;
                                    npc.ai[2] = 0;
                                    npc.ai[3] = 0;
                                    EoCVignetteAlphaValue = 0;
                                    npc.localAI[1] = 1;
                                    npc.localAI[0] = 4;
                                    EoCarenaCenter = Vector2.Zero;
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (npc.ai[1])
                        {

                        }
                        break;
                    case 2:
                        switch (npc.ai[1])
                        {

                        }
                        break;
                }
            }
            if (npc.type == NPCID.ServantofCthulhu)
            {
                //ai 0 = parent 1 = angle 2 = state 3 = timer
                Player player = Main.player[npc.target];
                NPC parent = Main.npc[(int)npc.ai[0]];
                int dist = 200;
                Vector2 center = parent.Center;
                bool pulsate = true;
                npc.damage = 0;
                switch (parent.ai[1])
                {
                    case 0:
                        npc.dontTakeDamage = false;
                        npc.ai[3] = 0;
                        npc.localAI[0] = 0;
                        npc.localAI[1] = 0;
                        break;
                    case 1:
                        npc.dontTakeDamage = false;
                        dist = 75;
                        pulsate = false;
                        break;
                    case 2:
                        npc.dontTakeDamage = false;
                        dist = 75;
                        pulsate = false;
                        break;
                    case 3:
                        npc.dontTakeDamage = false;
                        if (parent.ai[2] >= 20)
                        {
                            npc.damage = 15;
                        }
                        center = Vector2.Zero;
                        npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                        Vector2 moveTo = player.Center - npc.Center;
                        if (++npc.ai[3] >= 30)
                        {
                            npc.ai[3] = 0;
                            npc.localAI[0] = Main.rand.NextFloat(0.5f, 1f);
                        }
                        float factor = 0.0267f * npc.localAI[0];
                        npc.velocity = moveTo * factor;
                        break;
                    case 4:
                        npc.dontTakeDamage = true;
                        if (parent.ai[2] == 1)
                        {
                            npc.localAI[0] = player.Center.X;
                            npc.localAI[1] = player.Center.Y;
                            RegreSystem.ChangeCameraPos(new Vector2(npc.localAI[0], npc.localAI[1]), 650, 1);
                        }
                        if (parent.ai[2] >= 35)
                            npc.damage = 999;
                        if (parent.ai[2] > 1)
                            center = new Vector2(npc.localAI[0], npc.localAI[1]);
                        dist = 620;
                        pulsate = false;
                        break;
                }
                if (!parent.active || parent.type != NPCID.EyeofCthulhu)
                    center = Vector2.Zero;
                if (center != Vector2.Zero)
                {
                    npc.ai[1] += 2f * (float)Math.PI / 600f * 3f;
                    npc.ai[1] %= 2f * (float)Math.PI;
                    float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.3f);
                    Vector2 vel = center + (dist + (pulsate ? (125 * mult) : 0)) * new Vector2((float)Math.Cos(npc.ai[1]), (float)Math.Sin(npc.ai[1]));
                    Vector2 moveTo = vel - npc.Center;
                    float factor = 0.195f;
                    npc.velocity = moveTo * factor;
                    npc.rotation = RegreUtils.FromAToB(npc.Center, player.Center).ToRotation();
                }
                if (npc.spriteDirection == -1)
                {
                    npc.rotation += MathHelper.Pi;
                }
            }
        } /*
			else if (aiStyle == 4)
			{
				bool flag2 = false;
				if (Main.expertMode && (double)life < (double)lifeMax * 0.12)
				{
					flag2 = true;
				}
				bool flag3 = false;
				if (Main.expertMode && (double)life < (double)lifeMax * 0.04)
				{
					flag3 = true;
				}
				float num4 = 20f;
				if (flag3)
				{
					num4 = 10f;
				}
				if (target < 0 || target == 255 || player.dead || !player.active)
				{
					TargetClosest();
				}
				bool dead = player.dead;
				float num5 = position.X + (float)(width / 2) - player.position.X - (float)(player.width / 2);
				float num6 = position.Y + (float)height - 59f - player.position.Y - (float)(player.height / 2);
				float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
				if (num7 < 0f)
				{
					num7 += 6.283f;
				}
				else if ((double)num7 > 6.283)
				{
					num7 -= 6.283f;
				}
				float num8 = 0f;
				if (npc.ai[0] == 0f && npc.ai[1] == 0f)
				{
					num8 = 0.02f;
				}
				if (npc.ai[0] == 0f && npc.ai[1] == 2f && npc.ai[2] > 40f)
				{
					num8 = 0.05f;
				}
				if (npc.ai[0] == 3f && npc.ai[1] == 0f)
				{
					num8 = 0.05f;
				}
				if (npc.ai[0] == 3f && npc.ai[1] == 2f && npc.ai[2] > 40f)
				{
					num8 = 0.08f;
				}
				if (npc.ai[0] == 3f && npc.ai[1] == 4f && npc.ai[2] > num4)
				{
					num8 = 0.15f;
				}
				if (npc.ai[0] == 3f && npc.ai[1] == 5f)
				{
					num8 = 0.05f;
				}
				if (Main.expertMode)
				{
					num8 *= 1.5f;
				}
				if (flag3 && Main.expertMode)
				{
					num8 = 0f;
				}
				if (npc.rotation < num7)
				{
					if ((double)(num7 - npc.rotation) > 3.1415)
					{
						rotation -= num8;
					}
					else
					{
						rotation += num8;
					}
				}
				else if (npc.rotation > num7)
				{
					if ((double)(npc.rotation - num7) > 3.1415)
					{
						rotation += num8;
					}
					else
					{
						rotation -= num8;
					}
				}
				if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				{
					rotation = num7;
				}
				if (npc.rotation < 0f)
				{
					rotation += 6.283f;
				}
				else if ((double)rotation > 6.283)
				{
					rotation -= 6.283f;
				}
				if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				{
					rotation = num7;
				}
				if (Main.rand.Next(5) == 0)
				{
					int num9 = Dust.NewDust(new Vector2(position.X, position.Y + (float)height * 0.25f), width, (int)((float)height * 0.5f), 5, velocity.X, 2f);
					Main.dust[num9].velocity.X *= 0.5f;
					Main.dust[num9].velocity.Y *= 0.1f;
				}
				if (Main.dayTime || dead)
				{
					velocity.Y -= 0.04f;
					EncourageDespawn(10);
					return;
				}
				if (npc.ai[0] == 0f)
				{
					if (npc.ai[1] == 0f)
					{
						float num10 = 5f;
						float num11 = 0.04f;
						if (Main.expertMode)
						{
							num11 = 0.15f;
							num10 = 7f;
						}
						if (Main.getGoodWorld)
						{
							num11 += 0.05f;
							num10 += 1f;
						}
						Vector2 vector = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
						float num12 = player.position.X + (float)(player.width / 2) - vector.X;
						float num13 = player.position.Y + (float)(player.height / 2) - 200f - vector.Y;
						float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
						float num15 = num14;
						num14 = num10 / num14;
						num12 *= num14;
						num13 *= num14;
						if (velocity.X < num12)
						{
							velocity.X += num11;
							if (velocity.X < 0f && num12 > 0f)
							{
								velocity.X += num11;
							}
						}
						else if (velocity.X > num12)
						{
							velocity.X -= num11;
							if (velocity.X > 0f && num12 < 0f)
							{
								velocity.X -= num11;
							}
						}
						if (velocity.Y < num13)
						{
							velocity.Y += num11;
							if (velocity.Y < 0f && num13 > 0f)
							{
								velocity.Y += num11;
							}
						}
						else if (velocity.Y > num13)
						{
							velocity.Y -= num11;
							if (velocity.Y > 0f && num13 < 0f)
							{
								velocity.Y -= num11;
							}
						}
						npc.ai[2] += 1f;
						float num16 = 600f;
						if (Main.expertMode)
						{
							num16 *= 0.35f;
						}
						if (npc.ai[2] >= num16)
						{
							npc.ai[1] = 1f;
							npc.ai[2] = 0f;
							npc.ai[3] = 0f;
							target = 255;
							netUpdate = true;
						}
						else if ((position.Y + (float)height < player.position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f))
						{
							if (!player.dead)
							{
								npc.ai[3] += 1f;
							}
							float num17 = 110f;
							if (Main.expertMode)
							{
								num17 *= 0.4f;
							}
							if (Main.getGoodWorld)
							{
								num17 *= 0.8f;
							}
							if (npc.ai[3] >= num17)
							{
								npc.ai[3] = 0f;
								rotation = num7;
								float num18 = 5f;
								if (Main.expertMode)
								{
									num18 = 6f;
								}
								float num19 = player.position.X + (float)(player.width / 2) - vector.X;
								float num20 = player.position.Y + (float)(player.height / 2) - vector.Y;
								float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
								num21 = num18 / num21;
								Vector2 vector2 = vector;
								Vector2 vector3 = default(Vector2);
								vector3.X = num19 * num21;
								vector3.Y = num20 * num21;
								vector2.X += vector3.X * 10f;
								vector2.Y += vector3.Y * 10f;
								if (Main.netMode != 1)
								{
									int num22 = NewNPC((int)vector2.X, (int)vector2.Y, 5);
									Main.npc[num22].velocity.X = vector3.X;
									Main.npc[num22].velocity.Y = vector3.Y;
									if (Main.netMode == 2 && num22 < 200)
									{
										NetMessage.SendData(23, -1, -1, null, num22);
									}
								}
								SoundEngine.PlaySound(3, (int)vector2.X, (int)vector2.Y);
								for (int m = 0; m < 10; m++)
								{
									Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
								}
							}
						}
					}
					else if (npc.ai[1] == 1f)
					{
						rotation = num7;
						float num23 = 6f;
						if (Main.expertMode)
						{
							num23 = 7f;
						}
						if (Main.getGoodWorld)
						{
							num23 += 1f;
						}
						Vector2 vector4 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
						float num24 = player.position.X + (float)(player.width / 2) - vector4.X;
						float num25 = player.position.Y + (float)(player.height / 2) - vector4.Y;
						float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
						num26 = num23 / num26;
						velocity.X = num24 * num26;
						velocity.Y = num25 * num26;
						npc.ai[1] = 2f;
						netUpdate = true;
						if (netSpam > 10)
						{
							netSpam = 10;
						}
					}
					else if (npc.ai[1] == 2f)
					{
						npc.ai[2] += 1f;
						if (npc.ai[2] >= 40f)
						{
							velocity *= 0.98f;
							if (Main.expertMode)
							{
								velocity *= 0.985f;
							}
							if (Main.getGoodWorld)
							{
								velocity *= 0.99f;
							}
							if ((double)velocity.X > -0.1 && (double)velocity.X < 0.1)
							{
								velocity.X = 0f;
							}
							if ((double)velocity.Y > -0.1 && (double)velocity.Y < 0.1)
							{
								velocity.Y = 0f;
							}
						}
						else
						{
							rotation = (float)Math.Atan2(velocity.Y, velocity.X) - 1.57f;
						}
						int num27 = 150;
						if (Main.expertMode)
						{
							num27 = 100;
						}
						if (Main.getGoodWorld)
						{
							num27 -= 15;
						}
						if (npc.ai[2] >= (float)num27)
						{
							npc.ai[3] += 1f;
							npc.ai[2] = 0f;
							target = 255;
							rotation = num7;
							if (npc.ai[3] >= 3f)
							{
								npc.ai[1] = 0f;
								npc.ai[3] = 0f;
							}
							else
							{
								npc.ai[1] = 1f;
							}
						}
					}
					float num28 = 0.5f;
					if (Main.expertMode)
					{
						num28 = 0.65f;
					}
					if ((float)life < (float)lifeMax * num28)
					{
						npc.ai[0] = 1f;
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						netUpdate = true;
						if (netSpam > 10)
						{
							netSpam = 10;
						}
					}
					return;
				}
				if (npc.ai[0] == 1f || npc.ai[0] == 2f)
				{
					if (npc.ai[0] == 1f)
					{
						npc.ai[2] += 0.005f;
						if ((double)npc.ai[2] > 0.5)
						{
							npc.ai[2] = 0.5f;
						}
					}
					else
					{
						npc.ai[2] -= 0.005f;
						if (npc.ai[2] < 0f)
						{
							npc.ai[2] = 0f;
						}
					}
					rotation += npc.ai[2];
					npc.ai[1] += 1f;
					if (Main.expertMode && npc.ai[1] % 20f == 0f)
					{
						float num29 = 5f;
						Vector2 vector5 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
						float num30 = Main.rand.Next(-200, 200);
						float num31 = Main.rand.Next(-200, 200);
						float num32 = (float)Math.Sqrt(num30 * num30 + num31 * num31);
						num32 = num29 / num32;
						Vector2 vector6 = vector5;
						Vector2 vector7 = default(Vector2);
						vector7.X = num30 * num32;
						vector7.Y = num31 * num32;
						vector6.X += vector7.X * 10f;
						vector6.Y += vector7.Y * 10f;
						if (Main.netMode != 1)
						{
							int num33 = NewNPC((int)vector6.X, (int)vector6.Y, 5);
							Main.npc[num33].velocity.X = vector7.X;
							Main.npc[num33].velocity.Y = vector7.Y;
							if (Main.netMode == 2 && num33 < 200)
							{
								NetMessage.SendData(23, -1, -1, null, num33);
							}
						}
						for (int n = 0; n < 10; n++)
						{
							Dust.NewDust(vector6, 20, 20, 5, vector7.X * 0.4f, vector7.Y * 0.4f);
						}
					}
					if (npc.ai[1] >= 100f)
					{
						npc.ai[0] += 1f;
						npc.ai[1] = 0f;
						if (npc.ai[0] == 3f)
						{
							npc.ai[2] = 0f;
						}
						else
						{
							SoundEngine.PlaySound(3, (int)position.X, (int)position.Y);
							for (int num34 = 0; num34 < 2; num34++)
							{
								Gore.NewGore(position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
								Gore.NewGore(position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
								Gore.NewGore(position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
							}
							for (int num35 = 0; num35 < 20; num35++)
							{
								Dust.NewDust(position, width, height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
							}
							SoundEngine.PlaySound(15, (int)position.X, (int)position.Y, 0);
						}
					}
					Dust.NewDust(position, width, height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
					velocity.X *= 0.98f;
					velocity.Y *= 0.98f;
					if ((double)velocity.X > -0.1 && (double)velocity.X < 0.1)
					{
						velocity.X = 0f;
					}
					if ((double)velocity.Y > -0.1 && (double)velocity.Y < 0.1)
					{
						velocity.Y = 0f;
					}
					return;
				}
				defense = 0;
				int num36 = 23;
				int num37 = 18;
				if (Main.expertMode)
				{
					if (flag2)
					{
						defense = -15;
					}
					if (flag3)
					{
						num37 = 20;
						defense = -30;
					}
				}
				damage = GetAttackDamage_LerpBetweenFinalValues(num36, num37);
				damage = GetAttackDamage_ScaledByStrength(damage);
				if (npc.ai[1] == 0f && flag2)
				{
					npc.ai[1] = 5f;
				}
				if (npc.ai[1] == 0f)
				{
					float num38 = 6f;
					float num39 = 0.07f;
					Vector2 vector8 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
					float num40 = player.position.X + (float)(player.width / 2) - vector8.X;
					float num41 = player.position.Y + (float)(player.height / 2) - 120f - vector8.Y;
					float num42 = (float)Math.Sqrt(num40 * num40 + num41 * num41);
					if (num42 > 400f && Main.expertMode)
					{
						num38 += 1f;
						num39 += 0.05f;
						if (num42 > 600f)
						{
							num38 += 1f;
							num39 += 0.05f;
							if (num42 > 800f)
							{
								num38 += 1f;
								num39 += 0.05f;
							}
						}
					}
					if (Main.getGoodWorld)
					{
						num38 += 1f;
						num39 += 0.1f;
					}
					num42 = num38 / num42;
					num40 *= num42;
					num41 *= num42;
					if (velocity.X < num40)
					{
						velocity.X += num39;
						if (velocity.X < 0f && num40 > 0f)
						{
							velocity.X += num39;
						}
					}
					else if (velocity.X > num40)
					{
						velocity.X -= num39;
						if (velocity.X > 0f && num40 < 0f)
						{
							velocity.X -= num39;
						}
					}
					if (velocity.Y < num41)
					{
						velocity.Y += num39;
						if (velocity.Y < 0f && num41 > 0f)
						{
							velocity.Y += num39;
						}
					}
					else if (velocity.Y > num41)
					{
						velocity.Y -= num39;
						if (velocity.Y > 0f && num41 < 0f)
						{
							velocity.Y -= num39;
						}
					}
					npc.ai[2] += 1f;
					if (npc.ai[2] >= 200f)
					{
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						if (Main.expertMode && (double)life < (double)lifeMax * 0.35)
						{
							npc.ai[1] = 3f;
						}
						target = 255;
						netUpdate = true;
					}
					if (Main.expertMode && flag3)
					{
						TargetClosest();
						netUpdate = true;
						npc.ai[1] = 3f;
						npc.ai[2] = 0f;
						npc.ai[3] -= 1000f;
					}
				}
				else if (npc.ai[1] == 1f)
				{
					SoundEngine.PlaySound(36, (int)position.X, (int)position.Y, 0);
					rotation = num7;
					float num43 = 6.8f;
					if (Main.expertMode && npc.ai[3] == 1f)
					{
						num43 *= 1.15f;
					}
					if (Main.expertMode && npc.ai[3] == 2f)
					{
						num43 *= 1.3f;
					}
					if (Main.getGoodWorld)
					{
						num43 *= 1.2f;
					}
					Vector2 vector9 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
					float num44 = player.position.X + (float)(player.width / 2) - vector9.X;
					float num45 = player.position.Y + (float)(player.height / 2) - vector9.Y;
					float num46 = (float)Math.Sqrt(num44 * num44 + num45 * num45);
					num46 = num43 / num46;
					velocity.X = num44 * num46;
					velocity.Y = num45 * num46;
					npc.ai[1] = 2f;
					netUpdate = true;
					if (netSpam > 10)
					{
						netSpam = 10;
					}
				}
				else if (npc.ai[1] == 2f)
				{
					float num47 = 40f;
					npc.ai[2] += 1f;
					if (Main.expertMode)
					{
						num47 = 50f;
					}
					if (npc.ai[2] >= num47)
					{
						velocity *= 0.97f;
						if (Main.expertMode)
						{
							velocity *= 0.98f;
						}
						if ((double)velocity.X > -0.1 && (double)velocity.X < 0.1)
						{
							velocity.X = 0f;
						}
						if ((double)velocity.Y > -0.1 && (double)velocity.Y < 0.1)
						{
							velocity.Y = 0f;
						}
					}
					else
					{
						rotation = (float)Math.Atan2(velocity.Y, velocity.X) - 1.57f;
					}
					int num48 = 130;
					if (Main.expertMode)
					{
						num48 = 90;
					}
					if (npc.ai[2] >= (float)num48)
					{
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						target = 255;
						rotation = num7;
						if (npc.ai[3] >= 3f)
						{
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
							if (Main.expertMode && Main.netMode != 1 && (double)life < (double)lifeMax * 0.5)
							{
								npc.ai[1] = 3f;
								npc.ai[3] += Main.rand.Next(1, 4);
							}
							netUpdate = true;
							if (netSpam > 10)
							{
								netSpam = 10;
							}
						}
						else
						{
							npc.ai[1] = 1f;
						}
					}
				}
				else if (npc.ai[1] == 3f)
				{
					if (npc.ai[3] == 4f && flag2 && base.Center.Y > player.Center.Y)
					{
						TargetClosest();
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						netUpdate = true;
						if (netSpam > 10)
						{
							netSpam = 10;
						}
					}
					else if (Main.netMode != 1)
					{
						TargetClosest();
						float num49 = 20f;
						Vector2 vector10 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
						float num50 = player.position.X + (float)(player.width / 2) - vector10.X;
						float num51 = player.position.Y + (float)(player.height / 2) - vector10.Y;
						float num52 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) / 4f;
						num52 += 10f - num52;
						if (num52 < 5f)
						{
							num52 = 5f;
						}
						if (num52 > 15f)
						{
							num52 = 15f;
						}
						if (npc.ai[2] == -1f && !flag3)
						{
							num52 *= 4f;
							num49 *= 1.3f;
						}
						if (flag3)
						{
							num52 *= 2f;
						}
						num50 -= player.velocity.X * num52;
						num51 -= player.velocity.Y * num52 / 4f;
						num50 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
						num51 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
						if (flag3)
						{
							num50 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
							num51 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
						}
						float num53 = (float)Math.Sqrt(num50 * num50 + num51 * num51);
						float num54 = num53;
						num53 = num49 / num53;
						velocity.X = num50 * num53;
						velocity.Y = num51 * num53;
						velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
						velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
						if (flag3)
						{
							velocity.X += (float)Main.rand.Next(-50, 51) * 0.1f;
							velocity.Y += (float)Main.rand.Next(-50, 51) * 0.1f;
							float num55 = Math.Abs(velocity.X);
							float num56 = Math.Abs(velocity.Y);
							if (base.Center.X > player.Center.X)
							{
								num56 *= -1f;
							}
							if (base.Center.Y > player.Center.Y)
							{
								num55 *= -1f;
							}
							velocity.X = num56 + velocity.X;
							velocity.Y = num55 + velocity.Y;
							velocity.Normalize();
							velocity *= num49;
							velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
							velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
						}
						else if (num54 < 100f)
						{
							if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
							{
								float num57 = Math.Abs(velocity.X);
								float num58 = Math.Abs(velocity.Y);
								if (base.Center.X > player.Center.X)
								{
									num58 *= -1f;
								}
								if (base.Center.Y > player.Center.Y)
								{
									num57 *= -1f;
								}
								velocity.X = num58;
								velocity.Y = num57;
							}
						}
						else if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
						{
							float num59 = (Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / 2f;
							float num60 = num59;
							if (base.Center.X > player.Center.X)
							{
								num60 *= -1f;
							}
							if (base.Center.Y > player.Center.Y)
							{
								num59 *= -1f;
							}
							velocity.X = num60;
							velocity.Y = num59;
						}
						npc.ai[1] = 4f;
						netUpdate = true;
						if (netSpam > 10)
						{
							netSpam = 10;
						}
					}
				}
				else if (npc.ai[1] == 4f)
				{
					if (npc.ai[2] == 0f)
					{
						SoundEngine.PlaySound(36, (int)position.X, (int)position.Y, -1);
					}
					float num61 = num4;
					npc.ai[2] += 1f;
					if (npc.ai[2] == num61 && Vector2.Distance(position, player.position) < 200f)
					{
						npc.ai[2] -= 1f;
					}
					if (npc.ai[2] >= num61)
					{
						velocity *= 0.95f;
						if ((double)velocity.X > -0.1 && (double)velocity.X < 0.1)
						{
							velocity.X = 0f;
						}
						if ((double)velocity.Y > -0.1 && (double)velocity.Y < 0.1)
						{
							velocity.Y = 0f;
						}
					}
					else
					{
						rotation = (float)Math.Atan2(velocity.Y, velocity.X) - 1.57f;
					}
					float num62 = num61 + 13f;
					if (npc.ai[2] >= num62)
					{
						netUpdate = true;
						if (netSpam > 10)
						{
							netSpam = 10;
						}
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						if (npc.ai[3] >= 5f)
						{
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
						}
						else
						{
							npc.ai[1] = 3f;
						}
					}
				}
				else if (npc.ai[1] == 5f)
				{
					float num63 = 600f;
					float num64 = 9f;
					float num65 = 0.3f;
					Vector2 vector11 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
					float num66 = player.position.X + (float)(player.width / 2) - vector11.X;
					float num67 = player.position.Y + (float)(player.height / 2) + num63 - vector11.Y;
					float num68 = (float)Math.Sqrt(num66 * num66 + num67 * num67);
					num68 = num64 / num68;
					num66 *= num68;
					num67 *= num68;
					if (velocity.X < num66)
					{
						velocity.X += num65;
						if (velocity.X < 0f && num66 > 0f)
						{
							velocity.X += num65;
						}
					}
					else if (velocity.X > num66)
					{
						velocity.X -= num65;
						if (velocity.X > 0f && num66 < 0f)
						{
							velocity.X -= num65;
						}
					}
					if (velocity.Y < num67)
					{
						velocity.Y += num65;
						if (velocity.Y < 0f && num67 > 0f)
						{
							velocity.Y += num65;
						}
					}
					else if (velocity.Y > num67)
					{
						velocity.Y -= num65;
						if (velocity.Y > 0f && num67 < 0f)
						{
							velocity.Y -= num65;
						}
					}
					npc.ai[2] += 1f;
					if (npc.ai[2] >= 70f)
					{
						TargetClosest();
						npc.ai[1] = 3f;
						npc.ai[2] = -1f;
						npc.ai[3] = Main.rand.Next(-3, 1);
						netUpdate = true;
					}
				}
				if (flag3 && npc.ai[1] == 5f)
				{
					npc.ai[1] = 3f;
				}
			}*/
    }
}
