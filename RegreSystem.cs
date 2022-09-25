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
using static Terraria.ModLoader.ModContent;
using Regressus.NPCs.Minibosses;

namespace Regressus
{
    public class RegreSystem : ModSystem
    {
        public static int LavaMoss;
        public static int KryptonMoss;
        public static int ArgonMoss;
        public static int XenonMoss;
        public static int CloudTiles;
        public static int TempleBricks;
        public override void PostUpdatePlayers()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !(player.dead || player.ghost) && player != null)
                {
                    RegreUtils.activePlayers[i] = player;
                }
            }
        }
        public override void PostWorldGen()
        {
            WorldGen.TileRunner(new Random().Next(WorldGen.UndergroundDesertLocation.Location.X, (int)WorldGen.UndergroundDesertLocation.BottomRight().X),
                new Random().Next(WorldGen.UndergroundDesertLocation.Location.Y, (int)WorldGen.UndergroundDesertLocation.BottomRight().Y),
                1, 5, ModContent.TileType<SolStoneTile>(), true, 0, 0, false, false);

            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 13 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ItemType<Items.Consumables.Food.WyvernSteak>());
                            break;
                        }
                    }
                }
            }
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
            layers.Insert(textIndex, new LegacyGameInterfaceLayer("Regressus: OracleTimer", () =>
            {
                DynamicSpriteFont a = Mod.Assets.Request<DynamicSpriteFont>("Extras/OracleFont").Value;
                if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Oracle.TheOracle>()) && NPCs.Bosses.Oracle.TheOracle._phase2)
                    DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, a, "" + NPCs.Bosses.Oracle.TheOracle._finalCountdown / 60, new Vector2(Main.screenWidth / 2 - a.MeasureString("" + NPCs.Bosses.Oracle.TheOracle._finalCountdown / 60).X, Main.screenHeight * 0.05f), Color.DarkViolet, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                return true;
            }, InterfaceScaleType.UI));
            layers.Insert(textIndex2, new LegacyGameInterfaceLayer("Regressus: BiomeText", () =>
            {
                RegreUtils.DrawBiomeTitle();
                return true;
            }, InterfaceScaleType.UI));
            layers.Insert(textIndex2, new LegacyGameInterfaceLayer("Regressus: Page", () =>
            {

                /*Texture2D book = RegreUtils.GetExtraTexture("paper");
                //"The following codex contains all of the knowledge gathered from the Galaxy Omega-4, the closest presumed galaxy to the Primordial Chaos in which the Aeons reside. The codex must be kept under strict surveillance and it must not be revealed to the masses due to it's arcane prowess."
                DynamicSpriteFont a = Mod.Assets.Request<DynamicSpriteFont>("Extras/Handwriting").Value;
                Main.spriteBatch.Reload(BlendState.Additive);
                Main.spriteBatch.Draw(book, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), null, Color.White, 0, book.Size() / 2, 1.1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Reload(BlendState.AlphaBlend);
                Text text = new Text(new Rectangle(0, 0, book.Width, book.Height), a, pageText);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, a, text.wrappedString, new Vector2(Main.screenWidth / 2.6f, Main.screenHeight / 5), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);*/
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
                    if (zoomAmount != 1 && zoomAmount > zoomBefore)
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
            TempleBricks = 0;
            CloudTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            LavaMoss = tileCounts[TileID.LavaMoss] + tileCounts[TileID.LavaMossBrick];
            KryptonMoss = tileCounts[TileID.KryptonMoss] + tileCounts[TileID.KryptonMossBrick];
            ArgonMoss = tileCounts[TileID.ArgonMoss] + tileCounts[TileID.ArgonMossBrick];
            XenonMoss = tileCounts[TileID.XenonMoss] + tileCounts[TileID.XenonMossBrick];
            CloudTiles = tileCounts[TileID.Cloud];
            TempleBricks = tileCounts[TileID.LihzahrdBrick];
        }
        public static int AerialBudsCooldown;
        public override void PostUpdateEverything()
        {
            RegrePlayer regrePlayer = Main.LocalPlayer.GetModPlayer<RegrePlayer>();
            if (!Main.dayTime)
            {
                if (Main.time > 32399.0)
                {
                    if (regrePlayer.CantEatBaguette)
                        regrePlayer.CantEatBaguette = false;
                    if (AerialBudsCooldown > 0)
                    {
                        AerialBudsCooldown--;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        if (regrePlayer.AerialBudItem[i] != 0)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc("Aerial Bud"), Main.LocalPlayer.Center - new Vector2(0, Main.screenHeight / 2), Vector2.Zero, ModContent.ProjectileType<NPCs.Sky.AerialBudGiveBack>(), 0, 0, Main.myPlayer, i);
                        }
                    }
                }
            }
            if (AerialBudsCooldown == 0 && regrePlayer.AerialBudsGiven >= regrePlayer.AerialBudsMax)
            {
                AerialBudsCooldown = 5;
                regrePlayer.AerialBudsGiven = 0;
            }
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