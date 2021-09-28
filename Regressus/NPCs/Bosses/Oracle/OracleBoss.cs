using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Oracle;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;

namespace Regressus.NPCs.Bosses.Oracle
{
    [AutoloadBossHead] //my code is a huge mess lmao
    public class OracleBoss : ModNPC
    {
        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 13; 
            DisplayName.SetDefault("The Oracle");   
        } 
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleBoss").Value.Width * 0.5f, NPC.height * 0.5f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleBoss").Value, NPC.Center - Main.screenPosition, NPC.frame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleBoss_Glow").Value, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}            
        public override void BossLoot(ref string name, ref int potionType) {
            Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Weapons.Magic.SussyMogusStaff>());
            NPC.NewNPC((int)realNpcCenter.X, (int)realNpcCenter.Y, ModContent.NPCType<OracleDeathDrama>());
        }
        public override void SetDefaults()
        {   
            NPC.width = 90;
            NPC.height = 104;  
			NPC.damage = 0;
			NPC.defense = 15;
			NPC.lifeMax = 5100;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
            NPC.knockBackResist = 0f;
			NPC.aiStyle = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
            NPC.boss = true;
            Music = MusicID.Boss4;
        }
        private const int AISlot = 0;
		private const int TimerSlot = 1;

		private const int IntiatePhase2 = -1;
		private const int FlyAndShootSpikes = 0;
		private const int RingOfRunes = 1;
		private const int CrystalRain = 2;
		private const int SummonPortals = 3;
		private const int MinionSummoning = 4;
        private int dustthing;
		public float AIState {
			get => NPC.ai[AISlot];
			set => NPC.ai[AISlot] = value;
		}

		public float AITimer {
			get => NPC.ai[TimerSlot];
			set => NPC.ai[TimerSlot] = value;
		}
        public int AITimer2;
        private int funny;
        private bool gobackbecausenomorefunny;
        private int projAmount = 20;
        public static Vector2 realNpcCenter;
        private bool haveDonePhase2Transition = false;
		public override void AI() {
			Player player = Main.player[NPC.target];
            if (NPC.life <= NPC.lifeMax / 2) {
                dustthing = ModContent.DustType<Dusts.OracleRunes2>();
            }
            else {
                haveDonePhase2Transition = false;
                phaseTransitionTimer = 0;
                dustthing = ModContent.DustType<Dusts.OracleRunes>();
            }
            if (!haveDonePhase2Transition && NPC.life <= NPC.lifeMax / 2) {
                AIState = IntiatePhase2;
            }
            if (!player.active || player.dead) {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (NPC.HasValidTarget) {
                    AIState = FlyAndShootSpikes;
					AITimer = 0; 
                }
                if (!player.active || player.dead) {
                    NPC.velocity = new Vector2(0f, -20f);
                    if (NPC.timeLeft > 10) {
                        NPC.timeLeft = 10;
                    }
                    NPC.timeLeft = 1;
                    return;
                }
            }
            if (AIState == IntiatePhase2) {
		NPC.velocity *= 0.96f;
		float num1156 = 150f;
		phaseTransitionTimer++;
		if (phaseTransitionTimer >= num1156)
		{
            haveDonePhase2Transition = true;
			NPC.rotation = 0f;
            AIState = 0;
		}
		else if (phaseTransitionTimer < 40f)
		{
			NPC.rotation = Vector2.UnitY.RotatedBy(phaseTransitionTimer / 40f * ((float)Math.PI * 2f)).Y * 0.2f;
		}
		else if (phaseTransitionTimer < 80f)
		{
			NPC.rotation = Vector2.UnitY.RotatedBy(phaseTransitionTimer / 20f * ((float)Math.PI * 2f)).Y * 0.3f;
		}
		else if (phaseTransitionTimer < 120f)
		{
			NPC.rotation = Vector2.UnitY.RotatedBy(phaseTransitionTimer / 10f * ((float)Math.PI * 2f)).Y * 0.4f;
		}
		else
		{
			NPC.rotation = (phaseTransitionTimer - 120f) / 30f * ((float)Math.PI * 2f);
		}
            }
            realNpcCenter = NPC.Center + new Vector2(0, 10);
            if (NPC.life >= NPC.lifeMax / 2 || (NPC.life <= NPC.lifeMax / 2 && haveDonePhase2Transition)) {
            NPC.rotation = NPC.velocity.X * 0.05f;
            }
            if (AIState == FlyAndShootSpikes) {
                AITimer++;
                    if (haveDonePhase2Transition && NPC.life <= NPC.lifeMax / 2) {
            if (!gobackbecausenomorefunny) {
            funny += 10;
            }
            else {
                funny -= 10;
            }
            if (funny >= 600) {
                gobackbecausenomorefunny = true;
            }
            else if (funny <= 0) {
                gobackbecausenomorefunny = false;
            }
                Vector2 pos = new Vector2(player.position.X + 300 - funny, player.position.Y - 400 - (funny / 10));
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.0505f;

                if (++AITimer2 >= 60) {
			for (int i = 0; i <= 3; i++)
			{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), realNpcCenter, 6.5f * Utils.RotatedBy(NPC.DirectionTo(player.Center), (double)(MathHelper.ToRadians(Main.rand.NextFloat(15f, 34f)) * (float)i), default(Vector2)), ModContent.ProjectileType<CrystalProjectilesFromTheHitGameAmongUs>(), 20, 1f, Main.myPlayer)];
            projectile.tileCollide = true;
            projectile.timeLeft = 230;
			}
            AITimer2 = 0; 
                }

                    }
                    else {
            if (!gobackbecausenomorefunny) {
            funny += 5;
            }
            else {
                funny -= 5;
            }
            if (funny >= 600) {
                gobackbecausenomorefunny = true;
            }
            else if (funny <= 0) {
                gobackbecausenomorefunny = false;
            }
                Vector2 pos = new Vector2(player.position.X + 300 - funny, player.position.Y - 300 - (funny / 10));
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.0505f;

                if (++AITimer2 >= 60) {
			for (int i = 0; i <= 2; i++)
			{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), realNpcCenter, 6.5f * Utils.RotatedBy(NPC.DirectionTo(player.Center), (double)(MathHelper.ToRadians(Main.rand.NextFloat(15f, 34f)) * (float)i), default(Vector2)), ModContent.ProjectileType<CrystalProjectilesFromTheHitGameAmongUs>(), 20, 1f, Main.myPlayer)];
            projectile.tileCollide = true;
            projectile.timeLeft = 230;
			}
            AITimer2 = 0; 
                }

                    }
                if (AITimer >= 360) {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = RingOfRunes;
                }
            }
            else if (AIState == RingOfRunes) {
                NPC.velocity *= 0.8f;
                    AITimer++;
                    if (haveDonePhase2Transition && NPC.life <= NPC.lifeMax / 2) {
if (AITimer == 1) {
			    Vector2 center = realNpcCenter;
			    for (int k = 0; k < 8; k++) {
				float angle = 2f * (float)Math.PI / 8f * k;
				Vector2 pos = center + 150 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				var proj = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<AmongUsRunes2>(), 0, 0f, Main.myPlayer, NPC.whoAmI, angle)];
			    for (int r = 0; r < 5; r++) {
				float anglee = 2f * (float)Math.PI / 5f * r;
				Vector2 pos2 = pos + 10 * new Vector2((float)Math.Cos(anglee), (float)Math.Sin(anglee));	Dust dust;
	            dust = Terraria.Dust.NewDustPerfect(pos2, dustthing, new Vector2(10f, 10f).RotatedBy(anglee), 0, new Color(255,255,255), 1f);
                dust.scale = 2f;
                }
                proj.localAI[0] = 3;
                proj.localAI[1] = 45 * (k + 1);
			        }
			    for (int k = 0; k < 15; k++) {
				float angle = 2f * (float)Math.PI / 15f * k;
				Vector2 pos = center + 100 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				var proj = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<AmongUsRunes2>(), 0, 0f, Main.myPlayer, NPC.whoAmI, angle)];
			    for (int r = 0; r < 5; r++) {
				float anglee = 2f * (float)Math.PI / 5f * r;
				Vector2 pos2 = pos + 10 * new Vector2((float)Math.Cos(anglee), (float)Math.Sin(anglee));	Dust dust;
	            dust = Terraria.Dust.NewDustPerfect(pos2, dustthing, new Vector2(10f, 10f).RotatedBy(anglee), 0, new Color(255,255,255), 1f);
                dust.scale = 2f;
                }
                proj.localAI[0] = 0;
                proj.localAI[1] = 400;
			        }
                    }
                if (AITimer >= 400) {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = CrystalRain;
			        for (int k = 0; k < 20; k++) {
				    float angle = 2f * (float)Math.PI / 20f * k;
				    Vector2 pos = realNpcCenter + 50 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));	Dust dust;
	                dust = Terraria.Dust.NewDustPerfect(pos, dustthing, new Vector2(10f, 10f).RotatedBy(angle), 0, new Color(255,255,255), 1f);
                    dust.scale = 2f;
                    }
                }
                    }
                    else {
                    if (AITimer == 1) {
			    Vector2 center = realNpcCenter;
			    for (int k = 0; k < 5; k++) {
				float angle = 2f * (float)Math.PI / 5f * k;
				Vector2 pos = center + 150 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				var proj = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<AmongUsRunes2>(), 0, 0f, Main.myPlayer, NPC.whoAmI, angle)];
			    for (int r = 0; r < 5; r++) {
				float anglee = 2f * (float)Math.PI / 5f * r;
				Vector2 pos2 = pos + 10 * new Vector2((float)Math.Cos(anglee), (float)Math.Sin(anglee));	Dust dust;
	            dust = Terraria.Dust.NewDustPerfect(pos2, dustthing, new Vector2(10f, 10f).RotatedBy(anglee), 0, new Color(255,255,255), 1f);
                dust.scale = 2f;
                }
                proj.localAI[0] = 1;
                proj.localAI[1] = 45 * (k + 1);
			        }
			    for (int k = 0; k < 10; k++) {
				float angle = 2f * (float)Math.PI / 10f * k;
				Vector2 pos = center + 100 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				var proj = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<AmongUsRunes2>(), 0, 0f, Main.myPlayer, NPC.whoAmI, angle)];
			    for (int r = 0; r < 5; r++) {
				float anglee = 2f * (float)Math.PI / 5f * r;
				Vector2 pos2 = pos + 10 * new Vector2((float)Math.Cos(anglee), (float)Math.Sin(anglee));	Dust dust;
	            dust = Terraria.Dust.NewDustPerfect(pos2, dustthing, new Vector2(10f, 10f).RotatedBy(anglee), 0, new Color(255,255,255), 1f);
                dust.scale = 2f;
                }
                proj.localAI[0] = 0;
                proj.localAI[1] = 360;
			        }
                    }
                if (AITimer >= 400) {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = CrystalRain;
			        for (int k = 0; k < 20; k++) {
				    float angle = 2f * (float)Math.PI / 20f * k;
				    Vector2 pos = realNpcCenter + 50 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));	Dust dust;
	                dust = Terraria.Dust.NewDustPerfect(pos, dustthing, new Vector2(10f, 10f).RotatedBy(angle), 0, new Color(255,255,255), 1f);
                    dust.scale = 2f;
                    }
                }
                    }
            }
            else if (AIState == CrystalRain) {
                AITimer++;
                    if (haveDonePhase2Transition && NPC.life <= NPC.lifeMax / 2) {
                Vector2 pos = new Vector2(player.position.X, player.position.Y - 450);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.0505f;

                if (++AITimer2 >= 45) {
						float num53 = 2200f;
						float num54 = num53 / (float)projAmount;
                        projAmount++;
						for (int num56 = 0; num56 < projAmount; num56++)
						{
							Vector2 vector17 = new Vector2(player.Center.X - num53 / 2f + num54 * (float)num56, player.Center.Y - 1300f);
							Projectile projectileDemon = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), vector17.X, vector17.Y, 0, 1f, ModContent.ProjectileType<CrystalProjectilesFromTheHitGameAmongUs>(), 20, 0, Main.myPlayer, 0, 0)];
                            }
            AITimer2 = 0; 
                }
                    }
                    else {
                Vector2 pos = new Vector2(player.position.X, player.position.Y - 300);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.0505f;

                if (++AITimer2 >= 30) {
			for (int i = 0; i <= 5; i++)
			{
			Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), realNpcCenter, 6.5f * Utils.RotatedBy(NPC.DirectionTo(new Vector2(0, -1)), (double)(MathHelper.ToRadians(Main.rand.NextFloat(15f, 55f)) * (float)i), default(Vector2)), ModContent.ProjectileType<CrystalProjectilesFromTheHitGameAmongUs>(), 20, 1f, Main.myPlayer)];
            projectile.tileCollide = true;
            projectile.timeLeft = 230;
			}
            AITimer2 = 0; 
                }
                    }
                if (AITimer >= 360) {
                    AITimer = 0;
                    AITimer2 = 0;
                    projAmount = 20;
                    AIState = SummonPortals;
                }
            }
            else if (AIState == SummonPortals) {
                NPC.velocity *= 0.8f;
                    AITimer++;
                    if (haveDonePhase2Transition && NPC.life <= NPC.lifeMax / 2) {
                        Vector2 impostorlookingkindasusngl = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y + Main.screenHeight * Main.rand.NextFloat());
                        if (AITimer <= 100) {
                            if (++AITimer2 >= 15) {
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), impostorlookingkindasusngl, Vector2.Zero, ModContent.ProjectileType<Portale>(), 0, 1f, Main.myPlayer)];         
                    AITimer2 = 0;
                    }
                        }
                    }
                    else {
                    AITimer2++;
                    if (AITimer2 == 25) {
                    Vector2 sussyBaka = new Vector2(Main.screenPosition.X, Main.screenPosition.Y);
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), sussyBaka, Vector2.Zero, ModContent.ProjectileType<Portale>(), 0, 1f, Main.myPlayer)];
                    }
                    if (AITimer2 == 50) {
                    Vector2 sussyBaka = new Vector2(Main.screenPosition.X + Main.screenWidth, Main.screenPosition.Y);
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), sussyBaka, Vector2.Zero, ModContent.ProjectileType<Portale>(), 0, 1f, Main.myPlayer)];
                    }
                    if (AITimer2 == 75) {
                    Vector2 sussyBaka = new Vector2(Main.screenPosition.X + Main.screenWidth, Main.screenPosition.Y + Main.screenHeight);
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), sussyBaka, Vector2.Zero, ModContent.ProjectileType<Portale>(), 0, 1f, Main.myPlayer)];
                    }
                    if (AITimer2 == 100) {
                    Vector2 sussyBaka = new Vector2(Main.screenPosition.X, Main.screenPosition.Y + Main.screenHeight);
                    Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), sussyBaka, Vector2.Zero, ModContent.ProjectileType<Portale>(), 0, 1f, Main.myPlayer)];
                    }
                    }
                if (AITimer >= 180) {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = MinionSummoning;
                }
            }
            else if (AIState == MinionSummoning) {
                NPC.velocity *= 0.8f;
                    AITimer++;
                    if (AITimer == 1) {
                        for (int wtf = 0; wtf < 2; wtf++) {
				float angle = 2f * (float)Math.PI / 2f * wtf;
				Vector2 pos = realNpcCenter + 100 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    if (NPC.life >= NPC.lifeMax / 2) {
                NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<OracleMinion>());
                    }
                    else {
                NPC.NewNPC((int)pos.X, (int)pos.Y, ModContent.NPCType<OracleScholar>());
                    }
                        }
                    }
                if (AITimer >= 180) {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = FlyAndShootSpikes;
			        for (int k = 0; k < 20; k++) {
				    float angle = 2f * (float)Math.PI / 20f * k;
				    Vector2 pos = realNpcCenter + 50 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));	Dust dust;
	                dust = Terraria.Dust.NewDustPerfect(pos, dustthing, new Vector2(10f, 10f).RotatedBy(angle), 0, new Color(255,255,255), 1f);
                    dust.scale = 2f;
                    }
                }
            }
		}
        public override void HitEffect(int hitDirection, double damage) {
			for (int k = 0; k < damage / NPC.lifeMax * 100.0; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, dustthing, hitDirection, -1f, 0, default(Color), 1f);
			}
        }
        private int phaseTransitionTimer = 0;
        public override void FindFrame(int frameHeight) {
            if (AIState != RingOfRunes && AIState != SummonPortals && AIState != MinionSummoning) {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5) {
					NPC.frame.Y = 0 * frameHeight;
				}
				else if (NPC.frameCounter < 10) {
					NPC.frame.Y = 1 * frameHeight;
				}
				else if (NPC.frameCounter < 15) {
					NPC.frame.Y = 2 * frameHeight;
				}
                else if (NPC.frameCounter < 20) {
					NPC.frame.Y = 3 * frameHeight;
				}
                else if (NPC.frameCounter < 25) {
					NPC.frame.Y = 4 * frameHeight;
				}
				else if (NPC.frameCounter < 10) {
					NPC.frame.Y = 5 * frameHeight;
				}
				else if (NPC.frameCounter < 15) {
					NPC.frame.Y = 6 * frameHeight;
				}
                else if (NPC.frameCounter < 20) {
					NPC.frame.Y = 7 * frameHeight;
				}
				else {
					NPC.frameCounter = 0;
			}
            }
            else {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5) {
					NPC.frame.Y = 8 * frameHeight;
				}
				else if (NPC.frameCounter < 10) {
					NPC.frame.Y = 9 * frameHeight;
				}
				else if (NPC.frameCounter < 15) {
					NPC.frame.Y = 10 * frameHeight;
				}
                else if (NPC.frameCounter < 20) {
					NPC.frame.Y = 11 * frameHeight;
				}
                else if (NPC.frameCounter < 25) {
					NPC.frame.Y = 12 * frameHeight;
				}
				else {
					NPC.frameCounter = 0;
			}
            }
        }
    }
}