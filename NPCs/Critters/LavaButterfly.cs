using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Items.Critters;

namespace Regressus.NPCs.Critters
{
    public class LavaButterfly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lava Moss Glowfly");
            Main.npcFrameCount[NPC.type] = 3;
            Main.npcCatchable[NPC.type] = true;
        }
        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("Placeholder!!"),
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Butterfly);
            NPC.width = 26;
            NPC.height = 22;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.npcSlots = 0.5f;
            NPC.catchItem = (short)ItemType<LavaButterItem>();
            NPC.lavaImmune = true;
            AIType = NPCID.Butterfly;
            AnimationType = NPCID.Butterfly;
            NPC.lifeMax = 5;
            NPC.Opacity = 255;
            NPC.value = 0;
        }

        public override void AI()
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.TargetClosest(false);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && RegreSystem.LavaMoss > 5)
            {
                return 5f;
            }
            return 0f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //�: Totally legit code, all mine aha
            Texture2D glowTex = Request<Texture2D>("Regressus/NPCs/Critters/LavaButterflyGlow").Value;

            float frame = 3f / (float)Main.npcFrameCount[NPC.type];
            int height = (int)(float)((NPC.frame.Y / NPC.frame.Height) * frame) * (glowTex.Height / 3);

            Rectangle square = new Rectangle(0, height, glowTex.Width, glowTex.Height / 3);
            SpriteEffects effects = (NPC.direction != -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(glowTex, NPC.Center - Main.screenPosition + new Vector2(0f, 4), square, Color.White, NPC.rotation, Utils.Size(square) / 2f, NPC.scale, effects, 0f);
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 1;
        }
    }
}