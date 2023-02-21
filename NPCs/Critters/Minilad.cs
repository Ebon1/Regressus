using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Regressus.Items.Ammo;
using static System.Net.Mime.MediaTypeNames;

namespace Regressus.NPCs.Critters
{
    public class Minilad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
                return 0.15f;
            return 0;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("starlad with down syndrome"),
            });
        }
        public override void SetDefaults()
        {
            NPC.height = 32;
            NPC.width = 30;
            NPC.damage = 0;
            NPC.friendly = false;
            NPC.lifeMax = 12;
            NPC.defense = 0;
            NPC.aiStyle = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = false;
        }
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {

            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(NPC.width / 2, NPC.height / 2);
            sb.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Critters/Minilad").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);
            
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0)
            {
                if (++NPC.frameCounter >= 3)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y=(NPC.frame.Y/32+1)%8*32;
                }
                return;
            }
            NPC.frame.Y = 0;
               
        }

        public float bruh
        {
            get => NPC.ai[0];
            set => NPC.ai[1] = value;
        }





        public override void AI()
        {
            NPC.TargetClosest(true);
            
            
            Lighting.AddLight(NPC.Center, TorchID.UltraBright);
            NPC.direction = NPC.velocity.X >= 0 ? -1 : 1;


            if (NPC.collideX)
            {

                NPC.velocity.Y = -10f;
              
            } 
            
            
            if (!NPC.HasValidTarget) return;
            Player player = Main.player[NPC.target];
            Vector2 z = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity.X = z.X * 4;



            

      





        }
    }
}
