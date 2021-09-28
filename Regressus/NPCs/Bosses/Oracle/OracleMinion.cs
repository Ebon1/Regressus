using Terraria;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Regressus.Projectiles.Oracle;
using Terraria.ModLoader;

namespace Regressus.NPCs.Bosses.Oracle
{
	public class OracleMinion : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[NPC.type] = 6;
            DisplayName.SetDefault("Savant");
		}
		public override void SetDefaults() {
			NPC.width = 72;
			NPC.height = 72;
			NPC.aiStyle = 5;
            AIType = 205;
			NPC.damage = 0;
			NPC.defense = 2;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color lightColor)
        {
		Texture2D texture = ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleMinion_Rune").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin;
				spriteBatch.Draw(texture, drawPos, texture.Frame(1, 10, 0, thing), Color.White, NPC.rotation, texture.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0f);
			return true;
		}
        
        private int AITimer = 0;
		private int thing = Main.rand.Next(10);
		private float memes = Main.rand.NextFloat(0.0495f, 0.0535f);
        public override void AI() {
            Player player = Main.player[NPC.target];
                NPC.spriteDirection = Main.player[NPC.target].Center.X > NPC.Center.X ? 1 : -1;
                NPC.direction = Main.player[NPC.target].Center.X > NPC.Center.X ? 1 : -1;
                AITimer++;
				NPC.rotation = 0;
				if (++AITimer >= 200) {
					for(int i = 0; i < 200; i++)
            {    Player target = Main.player[i];
                float shootToX = target.position.X + (float)target.width * 0.5f - NPC.Center.X;
                float shootToY = target.position.Y - NPC.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
              
				if(target.active)
                {
                    distance = 3f / distance;
                    shootToX *= distance * 1.5f;
                    shootToY *= distance * 1.5f;
                    int proj = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center.X, NPC.Center.Y, shootToX, shootToY, ModContent.ProjectileType<AmongUsRunes>(), 20, 0, Main.myPlayer, 0f, 0f);
                    Main.projectile[proj].timeLeft = 300;
                    Main.projectile[proj].localAI[0] = 69;
                    Main.projectile[proj].localAI[1] = thing;
                    Main.projectile[proj].netUpdate = true;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].friendly = false;
			AITimer = 0;
					thing = Main.rand.Next(10);
                }
            }
				}
        }
        public override void FindFrame(int frameHeight) 
        {
            NPC.frameCounter++;
                if (NPC.frameCounter < 5) {
				NPC.frame.Y = 1 * frameHeight;
			}
			else if (NPC.frameCounter < 10) {
				NPC.frame.Y = 2 * frameHeight;
			}
			else if (NPC.frameCounter < 15) {
				NPC.frame.Y = 3 * frameHeight;
			}
			else if (NPC.frameCounter < 20) {
				NPC.frame.Y = 4 * frameHeight;
			}
			else if (NPC.frameCounter < 25) {
				NPC.frame.Y = 5 * frameHeight;
			}
			else {
                NPC.frameCounter = 0;
			}
        }
	}
}