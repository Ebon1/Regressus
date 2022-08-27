﻿using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Dev;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using System.IO;
using System.Text;
using Regressus.Items.Dev;
using static Regressus.MiscDrawingMethods;
using static Terraria.ModLoader.ModContent;
namespace Regressus
{
    public class BossTitleStyleID
    {
        public static readonly int Generic = -1;
        public static readonly int Oracle = 0;
        public static readonly int Vagrant = 1;
    }
    public struct Text
    {
        public string text;
        public Rectangle rect;
        public string wrappedString;
        public Text(Rectangle rect, DynamicSpriteFont font, string text)
        {
            this.text = text;
            this.rect = rect;
            this.wrappedString = RegreUtils.WrapText(font, this.text, this.rect.Width);
        }
    }
    public class BiomeID
    {
        public static readonly int Forest = 0;
        public static readonly int Jungle = 1;
        public static readonly int InfectionBiomes = 2;
        public static readonly int Hallow = 3;
        public static readonly int Mushroom = 4;
        public static readonly int Caves = 5;
        public static readonly int Dungeon = 6;
        public static readonly int Desert = 7;
        public static readonly int Tundra = 8;
        public static readonly int Ocean = 9;
        public static readonly int Granite = 10;
        public static readonly int Marble = 11;
        public static readonly int Temple = 12;
        public static readonly int Chronolands = 13;
        public static readonly int Space = 14;
        public static readonly int Meteorite = 15;
        public static readonly int Hell = 16;
    }

    public static class RegreUtils
    {
        public static string WrapText(DynamicSpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
        public static string BuffPlaceholder = "Regressus/Buffs/Debuffs/DecryptCooldown";
        public static class TRay
        {
            public static Vector2 Cast(Vector2 start, Vector2 direction, float length)
            {
                direction = direction.SafeNormalize(Vector2.UnitY);
                Vector2 output = start;
                for (int i = 0; i < length; i++)
                {
                    if (Collision.CanHitLine(output, 0, 0, output + direction, 0, 0))
                    {
                        output += direction;
                    }
                    else
                    {
                        break;
                    }
                }
                return output;
            }
            public static float CastLength(Vector2 start, Vector2 direction, float length)
            {
                Vector2 end = Cast(start, direction, length);
                return (end - start).Length();
            }
        }
        public static List<int> devItems = new List<int>
        {
            ModContent.ItemType<DecryptItem>(),
            ModContent.ItemType<VadeItem>(),
            ModContent.ItemType<PokerfaceItem>(),
            ModContent.ItemType<Items.Tiles.EbonianModReal>(),
            ModContent.ItemType<CorruptedTriangle>(),
        };
        public static Texture2D GetExtraTexture(string tex, bool uh = false)
        {
            if (uh)
                return GetTextureAlt("Extras/" + tex);
            return GetTexture("Extras/" + tex);
        }
        public static Texture2D GetTexture(string path)
        {
            return ModContent.Request<Texture2D>("Regressus/" + path).Value;
        }
        public static Texture2D GetThisTexture(Item obj)
        {
            return TextureAssets.Item[obj.type].Value;
        }
        public static Texture2D GetThisTexture(NPC obj)
        {
            return TextureAssets.Npc[obj.type].Value;
        }
        public static Texture2D GetThisTexture(Projectile obj)
        {
            return TextureAssets.Projectile[obj.type].Value;
        }
        public static Texture2D GetTextureAlt(string path)
        {
            return Regressus.Instance.Assets.Request<Texture2D>(path).Value;
        }
        public static Vector4 ColorToVector4(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static Vector4 ColorToVector4(Vector4 color)
        {
            return new Vector4(color.X / 255f, color.Y / 255f, color.Z / 255f, color.W / 255f);
        }
        public static Vector4 ColorToVector4(Vector3 color)
        {
            return new Vector4(color.X / 255f, color.Y / 255f, color.Z / 255f, 1);
        }
        public static Player[] activePlayers = new Player[Main.maxPlayers];
        public static Player GetRandomPlayer()
        {
            return Main.player[Main.rand.Next(activePlayers.Length)];
        }
        public static void Reload(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, Effect effect = null)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void SineMovement(Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
        {
            projectile.ai[1]++;
            float wave = (float)Math.Sin(projectile.ai[1] * frequencyMultiplier);
            Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
            vector.Normalize();
            wave *= projectile.ai[0];
            wave *= amplitude;
            Vector2 offset = vector * wave;
            projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
            projectile.Center = projectile.Center + offset;
        }
        public static Vector2 FromAToB(Vector2 a, Vector2 b, bool normalize = true, bool reverse = false)
        {
            Vector2 baseVel = b - a;
            if (normalize)
                baseVel.Normalize();
            if (reverse)
            {
                Vector2 baseVelReverse = a - b;
                if (normalize)
                    baseVelReverse.Normalize();
            }
            return baseVel;
        }
        public static void SetBossTitle(int progress, string name, Color color, string title = null, int style = -1)
        {
            RegrePlayer player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            player.bossTextProgress = progress;
            player.bossMaxProgress = progress;
            player.bossName = name;
            player.bossTitle = title;
            player.bossColor = color;
            player.bossStyle = style;
        }
        public static void SetBiomeTitle(string name, Color color, string title, int biome)
        {
            RegrePlayer player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            int Progress = player.notFirstTimeEnteringBiome[biome] ? 150 : 300;
            player.biomeTextProgress = Progress;
            player.biomeMaxProgress = Progress;
            player.biomeName = name;
            player.biomeTitle = title;
            player.biomeColor = color;
        }
        public static void DrawBossTitle()
        {
            var player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            if (player.bossTextProgress > 0)
            {
                switch (player.bossStyle)
                {
                    case -1:
                        float progress = Utils.GetLerpValue(0, player.bossMaxProgress, player.bossTextProgress);
                        float alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, player.bossName, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.bossName).X / 2, Main.screenHeight * 0.25f), player.bossColor * alpha, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        if (player.bossTitle != null)
                        {
                            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, player.bossTitle, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.bossTitle).X / 2, Main.screenHeight * 0.225f), player.bossColor * alpha, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        }
                        break;

                    case 0:
                        BossTitles.DrawOracleTitle();
                        break;
                    case 1:
                        BossTitles.DrawVagrantTitle();
                        break;
                }
            }
        }
        public static void DrawBiomeTitle()
        {
            var player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            if (player.biomeTextProgress > 0)
            {
                float progress = Utils.GetLerpValue(0, player.biomeMaxProgress, player.biomeTextProgress);
                float alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI), 0, 1);
                Vector2 namePos = new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.biomeName).X / 2, Main.screenHeight * 0.25f);
                Vector2 extrapos1 = new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.biomeName).X / 1.65f, Main.screenHeight * 0.275f);
                Vector2 extrapos2 = new Vector2(Main.screenWidth / 2 + FontAssets.DeathText.Value.MeasureString(player.biomeName).X / 1.65f, Main.screenHeight * 0.275f);
                Vector2 titlePos = new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.biomeTitle).X / 2, Main.screenHeight * 0.225f);
                Vector2 scale = Vector2.One;
                if (player.biomeMaxProgress == 150)
                {
                    namePos = new Vector2(0, Main.screenHeight - 40);
                    titlePos = new Vector2(0, Main.screenHeight - 55);
                    scale = Vector2.One / 1.5f;
                }
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, player.biomeName, namePos, player.biomeColor * alpha, 0, new Vector2(0.5f, 0.5f), scale);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, player.biomeTitle, titlePos, player.biomeColor * alpha, 0, new Vector2(0.5f, 0.5f), scale);
                if (player.biomeMaxProgress != 150)
                {
                    float rot = MathHelper.ToRadians(90);
                    Reload(Main.spriteBatch, BlendState.Additive);
                    Main.spriteBatch.Draw(GetExtraTexture("Extras2/slash_02"), extrapos1, null, player.biomeColor * alpha, rot, GetExtraTexture("Extras2/slash_02").Size() / 2, .5f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(GetExtraTexture("Extras2/slash_02"), extrapos2, null, player.biomeColor * alpha, MathHelper.ToRadians(-90), GetExtraTexture("Extras2/slash_02").Size() / 2, .5f, SpriteEffects.None, 0);
                    Reload(Main.spriteBatch, BlendState.AlphaBlend);
                }
            }
        }
        public static void Log(Projectile obj)
        {
            Main.NewText("Friendly?" + obj.friendly);
            Main.NewText("Hostile?" + obj.hostile);
            Main.NewText("Object:" + obj.Name);
            Main.NewText("Timeleft:" + obj.timeLeft);
            Main.NewText("Damage:" + obj.damage);
            Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + "]");
            Main.NewText("Direction:" + obj.direction);
            Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + "]");
            Main.NewText("Velocity:" + obj.velocity);
            Main.NewText("Owner:" + obj.owner);
        }
        public static void Log(NPC obj)
        {
            Main.NewText("Friendly?" + obj.friendly);
            Main.NewText("Object:" + obj.TypeName);
            Main.NewText("Timeleft:" + obj.timeLeft);
            Main.NewText("Damage:" + obj.damage);
            Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + ", " + obj.ai[2] + ", " + obj.ai[3] + "]");
            Main.NewText("Direction:" + obj.direction);
            Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + ", " + obj.localAI[2] + ", " + obj.localAI[3] + "]");
            Main.NewText("Velocity:" + obj.velocity);
        }
        public static void CollisionTPNoDust(Vector2 targetPosition, Player player)
        {
            int num = 150;
            Vector2 vector = player.position;
            Vector2 vector2 = player.velocity;
            for (int i = 0; i < num; i++)
            {
                vector2 = (vector + player.Size / 2f).DirectionTo(targetPosition).SafeNormalize(Vector2.Zero) * 12f;
                Vector2 vector3 = Collision.TileCollision(vector, vector2, player.width, player.height, fallThrough: true, fall2: true, (int)player.gravDir);
                vector += vector3;
            }
            _ = vector - player.position;
            TPNoDust(vector, player);
            NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, vector.X, vector.Y, 0);
        }
        public static void TPNoDust(Vector2 newPos, Player player)
        {
            try
            {
                player._funkytownAchievementCheckCooldown = 100;
                player.environmentBuffImmunityTimer = 4;
                player.RemoveAllGrapplingHooks();
                player.StopVanityActions();
                float num = MathHelper.Clamp(1f - player.teleportTime * 0.99f, 0.01f, 1f);
                Vector2 otherPosition = player.position;
                float num2 = Vector2.Distance(player.position, newPos);
                PressurePlateHelper.UpdatePlayerPosition(player);
                player.position = newPos;
                player.fallStart = (int)(player.position.Y / 16f);
                if (player.whoAmI == Main.myPlayer)
                {
                    bool flag = false;
                    if (num2 < new Vector2(Main.screenWidth, Main.screenHeight).Length() / 2f + 100f)
                    {
                        int time = 0;
                        Main.SetCameraLerp(0.1f, time);
                        flag = true;
                    }
                    else
                    {
                        NPC.ResetNetOffsets();
                        Main.BlackFadeIn = 255;
                        Lighting.Clear();
                        Main.screenLastPosition = Main.screenPosition;
                        Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
                        Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
                        Main.instantBGTransitionCounter = 10;
                        player.ForceUpdateBiomes();
                    }
                }
                PressurePlateHelper.UpdatePlayerPosition(player);
                player.ResetAdvancedShadows();
                for (int i = 0; i < 3; i++)
                {
                    player.UpdateSocialShadow();
                }
                player.oldPosition = player.position + player.BlehOldPositionFixer;
            }
            catch
            {
            }
        }
    }
}
