using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Regressus.Skies;
using Terraria;
using Regressus.NPCs.Bosses.Oracle;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Regressus.Dusts;
using Regressus.Effects.Prims;
using Terraria.ID;

namespace Regressus
{
    public class Regressus : Mod
    {
        public RenderTarget2D render, render3, render4;
        public int a;
        public static Regressus Instance;
        public static Effect BeamShader, Lens, Test1, Test2, LavaRT, Galaxy, CrystalShine, TrailShader;
        public static Effect Tentacle, TentacleBlack, TentacleRT, ScreenDistort, TextGradient, TextGradient2, TextGradientY;
        public DrawableTooltipLine[] lines = new DrawableTooltipLine[Main.maxItems];
        public DrawableTooltipLine activeLine;
        public static void GetLine(DrawableTooltipLine line, int index)
        {
            for (int i = 0; i < Main.maxItems; i++)
            {
                if (i == index)
                {
                    Regressus r = new Regressus();
                    r.lines[i] = line;
                    r.activeLine = line;
                }
            }
        }

        public override void Load()
        {
            /*Particle.Load();
            foreach (Type type in Code.GetTypes())
            {
                Particle.TryRegisteringParticle(type);
            }*/
            Instance = this;
            Test1 = ModContent.Request<Effect>("Regressus/Effects/Test1", (AssetRequestMode)1).Value;
            CrystalShine = ModContent.Request<Effect>("Regressus/Effects/CrystalShine", (AssetRequestMode)1).Value;
            TextGradient = ModContent.Request<Effect>("Regressus/Effects/TextGradient", (AssetRequestMode)1).Value;
            TextGradient2 = ModContent.Request<Effect>("Regressus/Effects/TextGradient2", (AssetRequestMode)1).Value;
            TextGradientY = ModContent.Request<Effect>("Regressus/Effects/TextGradientY", (AssetRequestMode)1).Value;
            Test2 = ModContent.Request<Effect>("Regressus/Effects/Test2", (AssetRequestMode)1).Value;
            Galaxy = ModContent.Request<Effect>("Regressus/Effects/Galaxy", (AssetRequestMode)1).Value;
            LavaRT = ModContent.Request<Effect>("Regressus/Effects/LavaRT", (AssetRequestMode)1).Value;
            BeamShader = ModContent.Request<Effect>("Regressus/Effects/Beam", (AssetRequestMode)1).Value;
            Lens = ModContent.Request<Effect>("Regressus/Effects/Lens", (AssetRequestMode)1).Value;
            Tentacle = ModContent.Request<Effect>("Regressus/Effects/Tentacle", (AssetRequestMode)1).Value;
            TentacleRT = ModContent.Request<Effect>("Regressus/Effects/TentacleRT", (AssetRequestMode)1).Value;
            ScreenDistort = ModContent.Request<Effect>("Regressus/Effects/DistortMove", (AssetRequestMode)1).Value;
            TentacleBlack = ModContent.Request<Effect>("Regressus/Effects/TentacleBlack", (AssetRequestMode)1).Value;
            TrailShader = ModContent.Request<Effect>("Regressus/Effects/TrailShader", (AssetRequestMode)1).Value;
            Filters.Scene["Regressus:Oracle"] = new Filter(new OracleShaderData("FilterMiniTower").UseColor(.16f, .42f, .87f).UseOpacity(0f), EffectPriority.Medium);
            SkyManager.Instance["Regressus:Oracle"] = new OracleSkyP1();
            Filters.Scene["Regressus:Oracle2"] = new Filter(new OracleShaderData("FilterMiniTower").UseColor(.78f, .33f, 1.11f).UseOpacity(1f), EffectPriority.Medium);
            Filters.Scene["Regressus:Oracle2Menu"] = new Filter(new OracleShaderData("FilterMiniTower").UseColor(.78f, .33f, 1.11f).UseOpacity(.9f), EffectPriority.Medium);
            SkyManager.Instance["Regressus:Oracle2"] = new OracleSkyP2();
            Filters.Scene["Regressus:OracleSummon"] = new Filter(new ScreenShaderData("FilterCrystalWin"), EffectPriority.VeryHigh);
            Filters.Scene["Regressus:OracleVoid1"] = new Filter(new ScreenShaderData("FilterCrystalWin"), EffectPriority.VeryHigh);
            Filters.Scene["Regressus:OracleVoid2"] = new Filter(new ScreenShaderData("FilterCrystalWin"), EffectPriority.VeryHigh);
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
            On.Terraria.Main.DrawProjectiles += DrawPrimitives;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            CreateRender();

            // Vanilla resprites go here
            TextureAssets.Item[ItemID.BrokenHeroSword] = ModContent.Request<Texture2D>("Regressus/Extras/Sprites/BrokenHeroSword", AssetRequestMode.ImmediateLoad);
            base.Load();
        }

        private void DrawPrimitives(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.ModProjectile is IPrimitiveDrawer)
                {
                    (projectile.ModProjectile as IPrimitiveDrawer).DrawPrimitives();
                }
            }
            orig(self);
        }

        public override void Unload()
        {
            //Particle.Unload();
            On.Terraria.Graphics.Effects.FilterManager.EndCapture -= FilterManager_EndCapture;
            On.Terraria.Main.DrawProjectiles -= DrawPrimitives;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            base.Unload();
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            CreateRender();
        }

        private void Main_LoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            if (render != null)
                CreateRender();
        }

        public void CreateRender()
        {
            Main.QueueMainThreadAction(() =>
            {
                render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                render3 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                render4 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            });
        }

        //if (proj.active && proj.type == ModContent.ProjectileType<Projectiles.Melee.ForeshadowP>())
        //{
        //Projectiles.Melee.ForeshadowP foreshadowP = new Projectiles.Melee.ForeshadowP();
        //    Color color = Color.White;
        //    proj.ModProjectile.PreDraw(ref color);
        /*RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
        Texture2D glow = ModContent.Request<Texture2D>("Regressus/Projectiles/Melee/Foreshadow_Glow").Value;
        var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[proj.type];
        for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[proj.type]; i++)
        {
            if (i == proj.localAI[0])
                continue;
            Main.spriteBatch.Draw(glow, proj.oldPos[i] - Main.screenPosition + new Vector2(proj.width / 2f, proj.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkViolet * (1f - fadeMult * i), proj.oldRot[i] + (proj.ai[0] == -1 ? 0 : MathHelper.PiOver2 * 3), glow.Size() / 2, proj.scale * (ProjectileID.Sets.TrailCacheLength[proj.type] - i) / ProjectileID.Sets.TrailCacheLength[proj.type], proj.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }

        RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
        foreshadowP.PostDraw(color);*/
        //}

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            #region "rt2d"
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<Projectiles.Melee.ForeshadowP2>())
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Extras/Extras2/scratch_03").Value, proj.Center - Main.screenPosition, null, Color.DarkViolet, proj.rotation, ModContent.Request<Texture2D>("Regressus/Extras/Extras2/scratch_03").Size() / 2, new Vector2(proj.ai[1] * proj.scale, (proj.ai[0] == 1 ? 0.5f : 1) * proj.scale), SpriteEffects.None, 0f);
                }
                if (proj.active && proj.timeLeft > 1 && proj.type == ModContent.ProjectileType<Projectiles.TentacleRT>() || proj.type == ModContent.ProjectileType<Projectiles.SmolTentacleRT>() || proj.type == ModContent.ProjectileType<Projectiles.Oracle.OracleBeamRT>())
                {
                    Color color = Color.White;
                    proj.ModProjectile.PreDraw(ref color);
                }
            }
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.Textures[1] = ModContent.Request<Texture2D>("Regressus/Extras/starSky2", (AssetRequestMode)1).Value;
            Test1.CurrentTechnique.Passes[0].Apply();
            Test1.Parameters["m"].SetValue(0.62f);
            Test1.Parameters["n"].SetValue(0.01f);
            sb.Draw(render, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.End();

            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            TestDust.DrawAll(sb);
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.Textures[1] = ModContent.Request<Texture2D>("Regressus/Extras/starSky2", (AssetRequestMode)1).Value;
            Test1.CurrentTechnique.Passes[0].Apply();
            Test1.Parameters["m"].SetValue(0.62f);
            Test1.Parameters["n"].SetValue(0.01f);
            sb.Draw(render, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            LavaDust.DrawAll(sb);
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.Textures[1] = ModContent.Request<Texture2D>("Regressus/Extras/Lava", (AssetRequestMode)1).Value;
            LavaRT.CurrentTechnique.Passes[0].Apply();
            LavaRT.Parameters["m"].SetValue(0.62f);
            LavaRT.Parameters["n"].SetValue(0.01f);
            sb.Draw(render, Vector2.Zero, Color.White);
            sb.End();
            #endregion
            #region "dev names"
            #endregion
            #region "lens"
            RenderTarget2D render2 = Main.screenTargetSwap;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<Projectiles.Lens>())
                {
                    gd.SetRenderTarget(render2);
                    gd.Clear(Color.Transparent);
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    Lens.CurrentTechnique.Passes[0].Apply();
                    Vector2 screenResolution = new Vector2(Main.screenWidth, Main.screenHeight);//·Ö±æÂÊ
                    Lens.Parameters["uScreenResolution"].SetValue(screenResolution);
                    Lens.Parameters["pos"].SetValue((proj.Center - Main.screenPosition) / screenResolution);
                    Lens.Parameters["intensity"].SetValue(5);
                    Lens.Parameters["range"].SetValue(proj.ai[0]);
                    Main.spriteBatch.Draw(render2 == Main.screenTarget ? Main.screenTargetSwap : Main.screenTarget, Vector2.Zero, Color.White);
                    Main.spriteBatch.End();

                    render2 = render2 == Main.screenTarget ? Main.screenTargetSwap : Main.screenTarget;
                }
            }
            if (render2 == Main.screenTarget)
            {
                gd.SetRenderTarget(render2);
                gd.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }
            #endregion
            #region "ripple"
            gd.SetRenderTarget(render3);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.type == ModContent.ProjectileType<Projectiles.Ripple>())
                {
                    Texture2D a = TextureAssets.Projectile[projectile.type].Value;
                    Main.spriteBatch.Draw(a, projectile.Center - Main.screenPosition, null, Color.White, 0, a.Size() / 2, projectile.ai[0], SpriteEffects.None, 0f);
                }
            }
            sb.End();
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Test2.CurrentTechnique.Passes[0].Apply();
            Test2.Parameters["tex0"].SetValue(Main.screenTargetSwap);
            Test2.Parameters["i"].SetValue(0.02f);
            sb.Draw(render3, Vector2.Zero, Color.White);
            sb.End();
            #endregion
            /*

            gd.SetRenderTarget(render4);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            var font = FontAssets.DeathText.Value;
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, "poopshitter", Vector2.Zero, Color.White, 0, Vector2.Zero, 10, SpriteEffects.None, 0f);
            sb.End();
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Regressus.TextGradient2.CurrentTechnique.Passes[0].Apply();
            Regressus.TextGradient2.Parameters["color2"].SetValue(new Vector4(0.7803921568627451f, 0.0941176470588235f, 1, 1));
            Regressus.TextGradient2.Parameters["color3"].SetValue(new Vector4(0.0509803921568627f, 1, 1, 1));
            Regressus.TextGradient2.Parameters["amount"].SetValue(Main.GlobalTimeWrappedHourly);
            sb.Draw(render4, Vector2.Zero, Color.White);
            sb.End();
            */
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
    }

    public class RegressusShaderPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            bool oracleP1 = Terraria.NPC.AnyNPCs(ModContent.NPCType<TheOracle>()) && !TheOracle._phase2;
            bool oracleP2 = Terraria.NPC.AnyNPCs(ModContent.NPCType<TheOracle>()) && TheOracle._phase2;
            Player.ManageSpecialBiomeVisuals("Regressus:Oracle", oracleP1);
            Player.ManageSpecialBiomeVisuals("Regressus:Oracle2", oracleP2);
            if (!oracleP1 && !oracleP2)
            {
                Filters.Scene["Regressus:OracleSummon"].Deactivate();
                Filters.Scene["Regressus:OracleVoid1"].Deactivate();
                Filters.Scene["Regressus:OracleVoid2"].Deactivate();
            }
        }
    }
}