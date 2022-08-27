using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using Regressus.Items.Dev;

namespace Regressus
{
    public class MiscDrawingMethods
    {
        public class BossTitles
        {
            public static void DrawOracleTitle()
            {
                var player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
                Color color1 = Color.DeepSkyBlue, color2 = Color.DarkViolet;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix);
                //Regressus.TextGradient.CurrentTechnique.Passes[0].Apply();
                float progress = Utils.GetLerpValue(0, player.bossMaxProgress, player.bossTextProgress);
                float alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
                //RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/textGlow").Value, new Rectangle(-Main.screenWidth, (int)(-25), Main.screenWidth * 4, 256 * 2), player.bossColor * alpha);
                if (player.bossTitle != null)
                    DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, player.bossTitle, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.bossTitle).X / 2, Main.screenHeight * 0.225f), player.bossColor * alpha);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, player.bossName, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.bossName).X / 2, Main.screenHeight * 0.25f), player.bossColor * alpha);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
            }
            public static void DrawVagrantTitle()
            {
                var player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
                float progress = Utils.GetLerpValue(0, player.bossMaxProgress, player.bossTextProgress);
                float alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
                Main.spriteBatch.Reload(BlendState.Additive);
                Main.spriteBatch.Reload(BlendState.AlphaBlend);
                if (player.bossTitle != null)
                    DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, player.bossTitle, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.bossTitle).X / 2, Main.screenHeight * 0.225f), player.bossColor * alpha);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, player.bossName, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.bossName).X / 2, Main.screenHeight * 0.25f), player.bossColor * alpha);
            }
        }
        public static readonly BlendState Subtractive = new BlendState
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };
        public readonly static BlendState AlphaSubtractive = new BlendState
        {
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };

        public static void DrawDevName(DrawableTooltipLine line, ParticleSystem sys)
        {
            Color currentColor = Main.hslToRgb((float)Math.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f, 1f, 0.5f);

            Vector2 size = FontAssets.MouseText.Value.MeasureString(line.Text);
            Rectangle rect = new Rectangle(0, (int)(size.Y / 4), (int)size.X, (int)size.Y / 2);
            if (Main.GameUpdateCount % 2 == 0)
            {
                sys.CreateParticle((part) =>
                {
                    if (part.ai[0] > 30)
                    {
                        part.dead = true;
                    }
                    part.ai[0]++;
                    part.scale = (float)Math.Sin(part.ai[0] * Math.PI / 30) * part.ai[1];
                    part.alpha = (float)Math.Sin(part.ai[0] * Math.PI / 30);
                }, new[]
                {
                    ModContent.Request<Texture2D>("Regressus/Extras/Star").Value
                }, (part, line, spriteBatch) =>
                {
                    Color c = Main.hslToRgb((float)Math.Sin(Main.GlobalTimeWrappedHourly + part.ai[0] / 60 * Math.PI) / 2 + 0.5f, 1f, 0.5f);
                    spriteBatch.Reload(BlendState.Additive);
                    spriteBatch.Draw(part.textures[0], part.position + new Vector2(line.X, line.Y), null, c, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0f);
                    spriteBatch.Reload(BlendState.AlphaBlend);
                }, part =>
                {
                    part.ai[1] = Main.rand.NextFloat(0.05f, 0.1f);
                    part.color = Main.hslToRgb(Main.rand.NextFloat(), 1, 0.5f);
                    part.position = Main.rand.NextVector2FromRectangle(rect);
                });
            }
            sys.UpdateParticles();
            Texture2D tex = ModContent.Request<Texture2D>("Regressus/Extras/Spotlight").Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(tex, new Rectangle((int)(line.X - size.X / 2), (int)(line.Y - size.Y / 8), (int)size.X * 2, (int)(size.Y + size.Y / 4)), currentColor * 0.7f);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), currentColor);
            sys.DrawParticles(line);
        }
        public static void DrawDevNameLegacy(DrawableTooltipLine line)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Regressus.TextGradient2, Main.UIScaleMatrix);
            //Regressus.TextGradient.CurrentTechnique.Passes[0].Apply();
            Regressus.TextGradient2.Parameters["color2"].SetValue(new Vector4(0.7803921568627451f, 0.0941176470588235f, 1, 1));
            Regressus.TextGradient2.Parameters["color3"].SetValue(new Vector4(0.0509803921568627f, 1, 1, 1));
            Regressus.TextGradient2.Parameters["amount"].SetValue(Main.GlobalTimeWrappedHourly);
            var font = FontAssets.MouseText.Value;
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }
        public static void DrawFlawlessRarity(DrawableTooltipLine line)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Regressus.CrystalShine, Main.UIScaleMatrix);
            var font = FontAssets.MouseText.Value;
            Regressus.CrystalShine.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            Regressus.CrystalShine.Parameters["uOpacity"].SetValue(1);
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.Gold);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }
        public static void DrawGradientX(DrawData data, Color color1, Color color2)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Regressus.TextGradient2, Main.UIScaleMatrix);
            //Regressus.TextGradient.CurrentTechnique.Passes[0].Apply();
            Regressus.TextGradient2.Parameters["color2"].SetValue(new Vector4((float)color1.R / 255, (float)color1.G / 255, (float)color1.B / 255, (float)color1.A / 255));
            Regressus.TextGradient2.Parameters["color3"].SetValue(new Vector4((float)color2.R / 255, (float)color2.G / 255, (float)color2.B / 255, (float)color2.A / 255));
            Regressus.TextGradient2.Parameters["amount"].SetValue(Main.GlobalTimeWrappedHourly);
            data.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }
        public static void DrawGradientY(DrawData data, Color color1, Color color2)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, Regressus.TextGradientY, Main.UIScaleMatrix);
            //Regressus.TextGradient.CurrentTechnique.Passes[0].Apply();
            Regressus.TextGradientY.Parameters["color2"].SetValue(new Vector4((float)color1.R / 255, (float)color1.G / 255, (float)color1.B / 255, (float)color1.A / 255));
            Regressus.TextGradientY.Parameters["color3"].SetValue(new Vector4((float)color2.R / 255, (float)color2.G / 255, (float)color2.B / 255, (float)color2.A / 255));
            Regressus.TextGradientY.Parameters["amount"].SetValue(Main.GlobalTimeWrappedHourly);
            data.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }
    }
}
