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
using Regressus.Projectiles;

namespace Regressus
{
    public class BossTitleStyleID
    {
        public static readonly int Generic = -1;
        public static readonly int Oracle = 0;
        public static readonly int Vagrant = 1;
        public static readonly int SSW = 2;
        public static readonly int Luminary = 3;
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
    /*public class BiomeID
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
    }*/

    public static class RegreUtils
    {
        public static VertexPositionColorTexture AsVertex(Vector2 position, Color color, Vector2 texCoord)
        {
            return new VertexPositionColorTexture(new Vector3(position, 50), color, texCoord);
        }
        public static VertexPositionColorTexture AsVertex(Vector3 position, Color color, Vector2 texCoord)
        {
            return new VertexPositionColorTexture(position, color, texCoord);
        }
        private static int width;
        private static int height;
        private static Vector2 zoom;
        private static Matrix view;
        private static Matrix projection;
        private static bool CheckGraphicsChanged()
        {
            var device = Main.graphics.GraphicsDevice;
            bool changed = device.Viewport.Width != width
                           || device.Viewport.Height != height
                           || Main.GameViewMatrix.Zoom != zoom;

            if (!changed) return false;

            width = device.Viewport.Width;
            height = device.Viewport.Height;
            zoom = Main.GameViewMatrix.Zoom;

            return true;
        }

        public static void QuickDustLine(this Dust dust, Vector2 start, Vector2 end, float splits, Color color)
        {
            Dust.QuickDust(start, color).scale = 1f;
            Dust.QuickDust(end, color).scale = 1f;
            float num = 1f / splits;
            for (float amount = 0.0f; (double)amount < 1.0; amount += num)
                Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
        }
        public static void QuickDustLine(this Dust dust, Vector2 start, Vector2 end, float splits, Color color1, Color color2)
        {
            Dust.QuickDust(start, color1).scale = 1f;
            Dust.QuickDust(end, color2).scale = 1f;
            float num = 1f / splits;
            for (float amount = 0.0f; (double)amount < 1.0; amount += num)
            {
                Color color = Color.Lerp(color1, color2, amount);
                Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
            }
        }
        public static float CircleDividedEqually(float i, float max)
        {
            return 2f * (float)Math.PI / max * i;
        }
        public static Matrix GetMatrix()
        {
            if (CheckGraphicsChanged())
            {
                var device = Main.graphics.GraphicsDevice;
                int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                view =
                    Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up)
                    * Matrix.CreateTranslation(width / 2, height / -2, 0)
                    * Matrix.CreateRotationZ(MathHelper.Pi)
                    * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            }

            return view * projection;
        }

        private static int GetPrimitiveCount(int vertexCount, PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.LineList:
                    return vertexCount / 2;
                case PrimitiveType.LineStrip:
                    return vertexCount - 1;
                case PrimitiveType.TriangleList:
                    return vertexCount / 3;
                case PrimitiveType.TriangleStrip:
                    return vertexCount - 2;
                default: return 0;
            }
        }
        public static void DrawPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, bool drawBacksides = true)
        {
            if (vertices.Length < 6) return;
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = Regressus.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
            effect.CurrentTechnique.Passes["Default"].Apply();
            if (drawBacksides)
            {
                short[] indices = new short[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    indices[i * 2] = (short)i;
                    indices[i * 2 + 1] = (short)(i + 1);
                    indices[i * 2 + 2] = (short)(i + 2);

                    indices[i * 2 + 5] = (short)i;
                    indices[i * 2 + 4] = (short)(i + 1);
                    indices[i * 2 + 3] = (short)(i + 2);
                }

                device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                    GetPrimitiveCount(vertices.Length, type) * 2);
            }
            else
            {
                device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
            }
        }
        public static void Reload(this SpriteBatch spriteBatch, SamplerState _samplerState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = _samplerState;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void DrawTexturedPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, Texture2D texture, bool drawBacksides = true)
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = Regressus.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
            effect.Parameters["tex"].SetValue(texture);
            effect.CurrentTechnique.Passes["Texture"].Apply();
            if (drawBacksides)
            {
                short[] indices = new short[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    indices[i * 2] = (short)i;
                    indices[i * 2 + 1] = (short)(i + 1);
                    indices[i * 2 + 2] = (short)(i + 2);

                    indices[i * 2 + 5] = (short)i;
                    indices[i * 2 + 4] = (short)(i + 1);
                    indices[i * 2 + 3] = (short)(i + 2);
                }

                device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                    GetPrimitiveCount(vertices.Length, type) * 2);
            }
            else
            {
                device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
            }
        }

        public static Vector2 GetRotation(List<Vector2> oldPos, int index)
        {
            if (oldPos.Count == 1)
                return oldPos[0];

            if (index == 0)
            {
                return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);
            }

            return (index == oldPos.Count - 1
                ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
                : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
        }

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
        public static string Empty = "Regressus/Extras/Empty";
        public static string Placeholder = "Regressus/Extras/Placeholder";
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
        public static Texture2D GetExtraTexture(string tex, bool altMethod = false)
        {
            if (altMethod)
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
        //public static Player[] activePlayers = new Player[Main.maxPlayers];
        //public static Player GetRandomPlayer()
        //{
        //   return Main.player[Main.rand.Next(activePlayers.Length)];
        //}
        public static SpriteSortMode previousSortMode;
        public static BlendState previousBlendState;
        public static SamplerState previousSamplerState;
        public static DepthStencilState previousDepthStencilState;
        public static RasterizerState previousRasterizerState;
        public static Effect previousEffect;
        public static Matrix previousMatrix;

        public static void SaveCurrent(this SpriteBatch spriteBatch)
        {
            previousSortMode = SpriteSortMode.Deferred;
            previousBlendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousSamplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousDepthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousRasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousEffect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousMatrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        }

        public static void ApplySaved(this SpriteBatch spriteBatch)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = previousSortMode;
            BlendState blendState = previousBlendState;
            SamplerState samplerState = previousSamplerState;
            DepthStencilState depthStencilState = previousDepthStencilState;
            RasterizerState rasterizerState = previousRasterizerState;
            Effect effect = previousEffect;
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, previousMatrix);
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
        public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
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
                return baseVelReverse;
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
        /*public static void SetBiomeTitle(string name, Color color, string title, int biome)
        {
            RegrePlayer player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            int Progress = player.notFirstTimeEnteringBiome[biome] ? 150 : 300;
            player.biomeTextProgress = Progress;
            player.biomeMaxProgress = Progress;
            player.biomeName = name;
            player.biomeTitle = title;
            player.biomeColor = color;
        }*/
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
                    case 2:
                        BossTitles.DrawSSWTitle();
                        break;
                    case 3:
                        BossTitles.DrawLuminaryTitle();
                        break;
                }
            }
        }
        public static void DustExplosion(Vector2 pos, Vector2 size = default, bool colored = false, Color color = default, bool sound = true, bool smoke = true)
        {
            int dustType = colored ? ModContent.DustType<Dusts.ColoredFireDust>() : ModContent.DustType<Dusts.FireDust>();
            if (sound)
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, pos);
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(pos, dustType, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, color, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
            }
            if (smoke)
                for (int num905 = 0; num905 < 10; num905++)
                {
                    int num906 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[num906].position = pos + Vector2.UnitX.RotatedByRandom(3.1415927410125732) * size.X / 2f;
                    Main.dust[num906].noGravity = true;
                    Dust dust2 = Main.dust[num906];
                    dust2.velocity *= 3f;
                }
            if (smoke)
                for (int num899 = 0; num899 < 4; num899++)
                {
                    int num900 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num900].position = pos + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * size.X / 2f;
                }
        }
        /*public static void DrawBiomeTitle()
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
        }*/
        public static void Log(this Projectile obj)
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
        public static void Log(this NPC obj)
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
        public static void SpawnTelegraphLine(Vector2 position, IEntitySource source, int timeleft = 30, float rot = 0)
        {
            Projectile a = Projectile.NewProjectileDirect(source, position, -Vector2.UnitY.RotatedBy(rot), ModContent.ProjectileType<TelegraphLine>(), 0, 0);
            Projectile b = Projectile.NewProjectileDirect(source, position, Vector2.UnitY.RotatedBy(rot), ModContent.ProjectileType<TelegraphLine>(), 0, 0);
            a.timeLeft = b.timeLeft = timeleft;
        }
        public static void SpawnTelegraphLine(Vector2 position, IEntitySource source, Vector2 velocity, int timeleft = 30)
        {
            Projectile a = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<TelegraphLine>(), 0, 0);
            a.timeLeft = timeleft;
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
