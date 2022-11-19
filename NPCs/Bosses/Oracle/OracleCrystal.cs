using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using ReLogic.Content;
using Terraria.Initializers;
using Terraria.Graphics;
using Regressus.Projectiles.Oracle;
using Regressus.Projectiles;
using Terraria.GameContent.Bestiary;

namespace Regressus.NPCs.Bosses.Oracle
{
    public class OracleCrystal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.hide)
            {
                SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, NPC.height * 0.5f);
                spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos, NPC.frame, (NPC.ai[2] != 1 ? Color.White : Color.White * 0.5f), NPC.rotation, drawOrigin, NPC.scale, effects, 0);
                if (NPC.ai[2] != 1)
                {
                    RegreUtils.Reload(spriteBatch, BlendState.Additive);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/NPCs/Bosses/Oracle/OracleCrystal_Glow").Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, effects, 0);
                    RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
                }
                return false;
            }
            return false;
        }
        /*public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[2] == 1)
            {
                Vector2 vector56 = NPC.Center - screenPos;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                float num186 = 0f;
                Terraria.Graphics.Effects.Filters.Scene["Nebula"].GetShader().UseIntensity(1f + num186).UseProgress(0f);
                DrawData value58 = new DrawData(ModContent.Request<Texture2D>("Regressus/Empty", (AssetRequestMode)1).Value, vector56, new Microsoft.Xna.Framework.Rectangle(0, 0, NPC.width * 3, NPC.height * 2), Microsoft.Xna.Framework.Color.DeepSkyBlue * (NPC.ShieldStrengthTowerMax * 0.8f + 0.2f), NPC.rotation, new Vector2(NPC.width * 1.5f, NPC.height), NPC.scale * (1f + num186 * 0.05f), SpriteEffects.None, 0);
                GameShaders.Misc["ForceField"].UseColor(new Vector3(1f + num186 * 0.5f));
                GameShaders.Misc["ForceField"].Apply(value58);
                value58.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }*/
        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 90;
            NPC.aiStyle = -1;
            NPC.lifeMax = 4500;
            NPC.knockBackResist = 0;
            NPC.lavaImmune = true;
            NPC.defense = 20;
            NPC.damage = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage >= NPC.life)
            {
                NPC.life = NPC.lifeMax;
                damage = 0;
                NPC.dontTakeDamage = true;
                NPC.ai[2] = 1;
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            NPC center = Main.npc[(int)NPC.ai[0]];
            if (damage >= NPC.life)
            {
                NPC.life = NPC.lifeMax;
                damage = 0;
                NPC.dontTakeDamage = true;
                NPC.ai[2] = 1;
            }
        }
        public override bool CheckDead()
        {
            NPC.life = NPC.lifeMax;
            NPC.dontTakeDamage = true;
            NPC.ai[2] = 1;
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2] != 1)
            {
                if (++NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    if (NPC.frame.Y < frameHeight * 3)
                        NPC.frame.Y += frameHeight;
                    else
                        NPC.frame.Y = 0;
                }
            }
        }
        float aiTimer
        {
            get => NPC.localAI[0];
            set => NPC.localAI[0] = value;
        }
        Vector2 pos;
        int useless, projectileNum = 1;
        float[] thing = new float[4];
        public override void AI()
        {
            NPC.dontTakeDamage = true;
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            NPC center = Main.npc[(int)NPC.ai[0]];
            if (++useless == 1)
            {
                NPC.Center = center.Center;
            }
            NPC.ai[1] = center.ai[0];
            if (!center.active || center.type != ModContent.NPCType<TheOracle>())
            {
                NPC.life = 0;
            }
            #region "ai"
            Vector2 moveTo = pos - NPC.Center;
            float factor = 0.38f;
            if (pos != Vector2.Zero && NPC.ai[2] != 1)
                NPC.velocity = moveTo * factor;
            if (NPC.ai[1] == -9)
            {
                NPC.ai[3] = 0;
                NPC.ai[2] = 0;
                if (NPC.Distance(center.Center) < 100)
                {
                    center.ai[0] = -1;
                    NPC.life = 0;
                }
            }
            if (NPC.ai[1] == 3)
            {
                if (aiTimer == 1)
                    projectileNum += 1;
                if (aiTimer <= 40)
                    pos = player.Center - new Vector2(0, 400);
                if (aiTimer == 40)
                {
                    NPC.velocity = Vector2.Zero;
                    for (int i = (projectileNum - projectileNum - projectileNum); i <= projectileNum; i++)
                    {
                        Vector2 vel = Utils.RotatedBy(Vector2.UnitY, (double)(MathHelper.ToRadians(21f) * (float)i));
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 1.5f * vel, ModContent.ProjectileType<TelegraphLine>(), 0, 0, player.whoAmI, NPC.whoAmI, 1)];
                        projectile.timeLeft = 26;
                    }
                }
                if (++aiTimer >= 65)
                {
                    RegreSystem.ScreenShakeAmount = 5f;
                    aiTimer = 0;
                    for (int i = (projectileNum - projectileNum - projectileNum); i <= projectileNum; i++)
                    {
                        Vector2 vel = Utils.RotatedBy(Vector2.UnitY, (double)(MathHelper.ToRadians(21f) * (float)i));
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 1.5f * vel, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 2)];
                        projectile.timeLeft = 20;
                    }
                }
            }
            else
            {
                projectileNum = 1;
            }
            if (NPC.ai[1] == -2)
            {
                center.dontTakeDamage = true;
                NPC.dontTakeDamage = true;
            }
            else if (center.ai[0] == 1)
            {
                NPC.dontTakeDamage = true;
            }
            /*if (NPC.ai[1] == 4)
            {
                aiTimer++;
                if (aiTimer == 20)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 vel = Utils.RotatedBy(NPC.DirectionTo(player.Center), (double)(MathHelper.ToRadians(21f) * (float)i));
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, 1.5f * vel, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 2)];
                        projectile.timeLeft = 240;
                    }
                }
                pos = player.Center + new Vector2(0, 175 * 3);
                NPC.rotation = 0;
            }
            if (NPC.ai[1] == 5)
            {
                factor = .8f;
                aiTimer++;
                if (aiTimer >= 200)
                {
                    thing[0] -= 0.03f;
                }
                if (aiTimer <= 200 - 60)
                {
                    thing[0] += 0.03f;
                }
                pos = player.Center + Vector2.One.RotatedBy(thing[0]) * 450;
                if (aiTimer >= 25)
                {
                    NPC.rotation = (player.Center - NPC.Center).ToRotation() - MathHelper.PiOver2;
                    if (aiTimer <= 200 - 60 || aiTimer >= 200)
                        if (++thing[1] >= 3)
                        {
                            Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, 1.15f * Utils.RotatedBy(NPC.DirectionTo(player.Center), 0), ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 2)];
                            projectile.localAI[1] = 15;
                            projectile.timeLeft = 20;
                            Projectile projectile2 = Main.projectile[Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, 1.15f * Utils.RotatedBy(NPC.DirectionTo(player.Center), 0), ModContent.ProjectileType<TelegraphLine>(), 0, 0)];
                            projectile2.timeLeft = 20;
                            thing[1] = 0;
                        }
                }
            }*/
            if (NPC.ai[1] == -2 || NPC.ai[1] == 1 || NPC.ai[1] == -4 || NPC.ai[1] == 4 || NPC.ai[1] == 2 || NPC.ai[1] == -9)
            {
                aiTimer = 0;
                pos = center.Center + new Vector2(0, -86);
                NPC.rotation = 0;
            }
            #endregion
        }
    }
}

