using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using ReLogic.Graphics;
using System;
using Terraria.UI.Chat;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.Initializers;
using Terraria.GameContent.Skies;
using Terraria.GameContent.ItemDropRules;
using Regressus.Effects.Prims;
using Terraria.WorldBuilding;
using Regressus.Tiles.Desert;
using Terraria.IO;

namespace Regressus
{
    public class RegreSystem : ModSystem
    {
        public static int LavaMoss;
        public static int KryptonMoss;
        public static int ArgonMoss;
        public static int XenonMoss;

        public override void PostWorldGen()
        {
            WorldGen.TileRunner(new Random().Next(WorldGen.UndergroundDesertLocation.Location.X, (int)WorldGen.UndergroundDesertLocation.BottomRight().X),
                new Random().Next(WorldGen.UndergroundDesertLocation.Location.Y, (int)WorldGen.UndergroundDesertLocation.BottomRight().Y),
                1, 5, ModContent.TileType<SolStoneTile>(), true, 0, 0, false, false);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int textIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            int textIndex2 = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text")) - 1;
            layers.Insert(textIndex, new LegacyGameInterfaceLayer("Regressus: BossText", () =>
            {
                RegreUtils.DrawBossTitle();
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
            //Particles.Particle.UpdateParticles();
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