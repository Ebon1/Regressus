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
        }
        public static void DrawDevName(DrawableTooltipLine line)
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
