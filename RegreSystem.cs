using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.UI.Chat;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Initializers;
using Terraria.GameContent.Skies;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Regressus.Effects.Prims;
namespace Regressus
{
    public class RegreSystem : ModSystem
    {
        public static int LavaMoss;
        public static int KryptonMoss;
        public static int ArgonMoss;
        public static int XenonMoss;
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            var player = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            int textIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            int textIndex2 = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text")) - 1;
            layers.Insert(textIndex, new LegacyGameInterfaceLayer("Regressus: BossText", () =>
            {
                if (player.bossTextProgress > 0)
                {
                    if (!player.bossDramatic)
                    {
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, player.bossName, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.bossName).X / 2, Main.screenHeight * 0.25f), player.bossColor, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        if (player.bossTitle != null)
                        {
                            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, player.bossTitle, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.bossTitle).X / 2, Main.screenHeight * 0.225f), player.bossColor, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        }
                    }
                    else
                    {
                        float progress = Utils.GetLerpValue(0, player.bossMaxProgress, player.bossTextProgress);
                        float alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
                        RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/textGlow").Value, new Rectangle(-Main.screenWidth, (int)(-25), Main.screenWidth * 4, 256 * 2), player.bossColor * alpha);
                        if (player.bossTitle != null)
                            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, player.bossTitle, new Vector2(Main.screenWidth / 2 - FontAssets.MouseText.Value.MeasureString(player.bossTitle).X / 2, Main.screenHeight * 0.225f), player.bossColor * alpha, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, player.bossName, new Vector2(Main.screenWidth / 2 - FontAssets.DeathText.Value.MeasureString(player.bossName).X / 2, Main.screenHeight * 0.25f), player.bossColor * alpha, 0, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f));
                        RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
                    }
                }
                return true;
            }, InterfaceScaleType.UI));
        }
        public static int ShakeTimer = 0;
        public static float ScreenShakeAmount = 0;
        public override void ModifyScreenPosition()
        {
            Player player = Main.LocalPlayer;
            if (!isChangingCameraPos)
            {
                zoomBefore = Main.GameZoomTarget;
            }
            if (isChangingCameraPos)
            {
                if (CameraChangeLength > 0)
                {
                    if (zoomAmount != 1 && zoomAmount < zoomBefore)
                    {
                        Main.GameZoomTarget = Utils.Clamp(Main.GameZoomTarget + 0.05f, 1f, zoomAmount);
                    }
                    if (CameraChangeTransition <= 1f)
                    {
                        Main.screenPosition = Vector2.SmoothStep(cameraChangeStartPoint, CameraChangePos, CameraChangeTransition += 0.025f);
                    }
                    else
                    {
                        Main.screenPosition = CameraChangePos;
                    }
                    CameraChangeLength--;
                }
                else if (CameraChangeTransition >= 0)
                {
                    if (Main.GameZoomTarget != zoomBefore)
                    {
                        Main.GameZoomTarget -= 0.05f;
                    }
                    Main.screenPosition = Vector2.SmoothStep(player.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), CameraChangePos, CameraChangeTransition -= 0.05f);
                }
                else
                {
                    isChangingCameraPos = false;
                }
            }
            if (!Main.gameMenu)
            {
                ShakeTimer++;
                if (ScreenShakeAmount >= 0 && ShakeTimer >= 5)
                {
                    ScreenShakeAmount -= 0.1f;
                }
                if (ScreenShakeAmount < 0)
                {
                    ScreenShakeAmount = 0;
                }
                Main.screenPosition += new Vector2(ScreenShakeAmount * Main.rand.NextFloat(), ScreenShakeAmount * Main.rand.NextFloat());
            }
            else
            {
                ScreenShakeAmount = 0;
                ShakeTimer = 0;
            }
        }
        float zoomBefore;
        public static float zoomAmount;
        public static Vector2 cameraChangeStartPoint;
        public static Vector2 CameraChangePos;
        public static float CameraChangeTransition;
        public static int CameraChangeLength;
        public static bool isChangingCameraPos;

        public override void ResetNearbyTileEffects()
        {
            LavaMoss = 0;
            KryptonMoss = 0;
            ArgonMoss = 0;
            XenonMoss = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            LavaMoss = tileCounts[TileID.LavaMoss] + tileCounts[TileID.LavaMossBrick];
            KryptonMoss = tileCounts[TileID.KryptonMoss] + tileCounts[TileID.KryptonMossBrick];
            ArgonMoss = tileCounts[TileID.ArgonMoss] + tileCounts[TileID.ArgonMossBrick];
            XenonMoss = tileCounts[TileID.XenonMoss] + tileCounts[TileID.XenonMossBrick];
        }

        public override void PostUpdateEverything()
        {
            Particles.Particle.UpdateParticles();
        }
        public static void ChangeCameraPos(Vector2 pos, int length, float zoom = 1.65f)
        {
            cameraChangeStartPoint = Main.screenPosition;
            CameraChangeLength = length;
            CameraChangePos = pos - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            isChangingCameraPos = true;
            CameraChangeTransition = 0;
            if (Main.GameZoomTarget < zoom)
                zoomAmount = zoom;
        }
    }
}