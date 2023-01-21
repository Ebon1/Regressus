/*using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Regressus.Skies;
using Regressus.NPCs.Bosses.Oracle;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework.Media;
using Regressus.Dusts;
using Regressus.Effects.Prims;

namespace Regressus
{
    public class RegreMenuOracle : ModMenu
    {
        public override string DisplayName => "Oracle";
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Regressus/Extras/Banner2");
        public override int Music => MusicLoader.GetMusicSlot("Regressus/Sounds/Music/oracle2_alt");
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<EmptyBG>();
        float minuteHandRot, hourHandRot;
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {

            //bg draw
            Texture2D b = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            Main.spriteBatch.Draw(b, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
            Main.spriteBatch.Reload(BlendState.Additive);
            Effect effect = Regressus.ScreenDistort;
            Main.spriteBatch.Reload(effect);
            Filters.Scene["Regressus:Oracle2Menu"].Activate(new Vector2(0, 0));
            Texture2D a = ModContent.Request<Texture2D>("Regressus/Extras/starSky2").Value;
            effect.Parameters["screenPosition"].SetValue(Main.screenPosition);
            effect.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("Regressus/Extras/seamlessNoise").Value);
            effect.Parameters["distortionMultiplier"].SetValue(0.75f);
            effect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) * -15f);
            effect.Parameters["alpha"].SetValue(0.55f);
            effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            Main.spriteBatch.Draw(a, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.DarkViolet);
            Main.spriteBatch.End();
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);

            /*//*//*clock
            minuteHandRot += 0.1666668f / 4;
hourHandRot += 0.0138889f / 4;
Vector2 arenaCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
RegreUtils.Reload(spriteBatch, BlendState.Additive);
float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*//*) *0.1f);
            float scale = 0.5f * mult;
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clock").Value, arenaCenter, null, Color.White, 0, ModContent.Request<Texture2D>("Regressus/Extras/clock").Value.Size() / 2, .25f, SpriteEffects.None, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value, arenaCenter, null, Color.White, MathHelper.ToRadians(hourHandRot), ModContent.Request<Texture2D>("Regressus/Extras/clockHand1").Value.Size() / 2, 2.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value, arenaCenter, null, Color.White, MathHelper.ToRadians(minuteHandRot), ModContent.Request<Texture2D>("Regressus/Extras/clockHand2").Value.Size() / 2, 2.5f, SpriteEffects.None, 0);
            RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
            */
/*
            //logo draw
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D c = ModContent.Request<Texture2D>("Regressus/Extras/Banner2_Glow").Value;
            spriteBatch.Draw(Logo.Value, logoDrawCenter, null, Color.White, 0, Logo.Size() / 2, 0.85f, SpriteEffects.None, 0);
            spriteBatch.Draw(c, logoDrawCenter, null, Color.White, 0, Logo.Size() / 2, 0.85f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);


            return false;
        }
    }
    public class EmptyBG : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Extras/Empty");
        }
        public override int ChooseFarTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Extras/Empty");
        }
        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Extras/Empty");
        }
    }
}*/
