using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;
using Terraria.Chat;
using Regressus.Items.Weapons.Melee;
using Terraria.UI.Chat;
using Regressus.Items.Consumables;
namespace Regressus.NPCs.Town
{
    public class Samael : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Totally Normal Guy");
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.SpawnsWithCustomName[Type] = true;
        }
        public override bool CanChat()
        {
            return true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && RegreSystem.SamaelDay == 0 && !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Samael>()))
            {
                return 0.74f;
            }
            return 0f;
        }
        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.width = 18;
            NPC.dontTakeDamage = true;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Sam",
            };
        }
        public override void OnSpawn(IEntitySource source)
        {
            RegreSystem.SamaelDay++;
            Main.NewText("Something strange is nearby.", new Color(118, 50, 173));
        }
        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add("Do you sometimes wonder about the nature of this ground we sit on? No? Just me, then.");
            chat.Add("The baubles I sell are not for the faint of heart. Some may steal it from you.");
            chat.Add("a");
            return chat;
        }
        public override void AI()
        {
            NPC.homeless = true;
        }
        public static float SamaelAlpha = 1f;
        public static string SamaelDialogue;
        public List<int> Items = new List<int>
        {
            ModContent.ItemType<BloodVial>(),
        };
        /*public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (RegreSystem.SamaelShopOpen && SamaelAlpha > 0f && (SamaelDialogue != "" || SamaelDialogue != null))
                SamaelAlpha -= 0.05f;
            if ((SamaelDialogue == "" || SamaelDialogue == null))
                SamaelAlpha = 1f;
            if (RegreSystem.SamaelShopOpen)
            {
                if (Items.Contains(Main.mouseItem.type))
                {
                    SamaelDialogue = "Fresh, tasty blood.";
                }
                else
                    SamaelDialogue = "";
            }
        }*/
        public static string GetDialogueText()
        {
            float progress = Utils.GetLerpValue(0, 1, SamaelAlpha);
            float realProg = ((MathHelper.Clamp((1f - progress), 0, 1)));
            string text = SamaelDialogue;
            int count = (int)(text.Length * realProg);
            string something = $"{text.Substring(0, count)}";
            return something;
        }
        public static void DrawDialogue()
        {
            SpriteBatch sb = Main.spriteBatch;
            float progress = Utils.GetLerpValue(0, 1, SamaelAlpha);
            string text = GetDialogueText();
            /* Main.spriteBatch.Reload(BlendState.Additive);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/textGlow").Value, new Vector2(Main.screenWidth / 4, (int)(Main.screenHeight * 0.55f)), null, Color.Gray, 0f, new Vector2(256) / 2, new Vector2(3, 3f), SpriteEffects.None, 0f);
             Main.spriteBatch.Reload(BlendState.AlphaBlend);*/
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Town/Samael").Value, new Vector2(180, (int)(Main.screenHeight * 0.65f)), null, Color.White, 0f, ModContent.Request<Texture2D>("Regressus/NPCs/Town/Samael").Value.Size() / 2, 1f, SpriteEffects.FlipHorizontally, 0f);
            //ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2(225, Main.screenHeight * 0.65f), Color.White, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f), Main.screenWidth - 100);
            Main.spriteBatch.Reload(Main.DefaultSamplerState);
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                RegreSystem.SamaelShopOpen = true;
                shop = true;
            }
        }
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<BloodVial>());
            nextSlot++;
        }
    }
}
