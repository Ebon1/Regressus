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
using Terraria.Graphics;
using Regressus.NPCs.Bosses.Oracle;

namespace Regressus.Projectiles.Oracle
{
    public class OracleOrbs : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb");
            Main.projFrames[Projectile.type] = 6;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2((TextureAssets.Projectile[Projectile.type].Value.Width / 2) * 0.5F, (TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]) * 0.5F);
            Vector2 drawPos = new Vector2(
                Projectile.position.X - Main.screenPosition.X + (Projectile.width / 5) - (TextureAssets.Projectile[Projectile.type].Value.Width / 5) * Projectile.scale / 5 + drawOrigin.X * Projectile.scale,
                Projectile.position.Y - Main.screenPosition.Y + Projectile.height - TextureAssets.Projectile[Projectile.type].Value.Height * Projectile.scale / Main.projFrames[Projectile.type] + 4f + drawOrigin.Y * Projectile.scale + Projectile.gfxOffY
                );
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(frameX * Projectile.width, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.White, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleOrbs_Glow").Value, drawPos, new Rectangle(frameX * Projectile.width, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.White, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 136;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        float AITimer;
        bool shouldLookAtPlayer;
        int useless;
        int frameX;
        int currentState;
        Vector2 pos, lastPos;
        Vector2[] orbBlastPos = new Vector2[4];
        float meleeRot;
        int attacks;
        public override void AI()
        {
            Projectile.timeLeft = 2;
            Player player = Main.player[Projectile.owner];
            /*if (++useless >= 15 && player.ownedProjectileCounts[ModContent.ProjectileType<OracleOrbs>()] != 4)
            {
                Projectile.Kill();
            }*/
            NPC center = Main.npc[(int)Projectile.ai[0]];
            NPC crystal = Main.npc[(int)Projectile.localAI[1]];
            if (++useless == 2)
            {
                Projectile.Center = center.Center;
            }
            Vector2 moveTo = pos - Projectile.Center;
            Projectile.ai[1] = center.ai[0];
            Projectile.hide = center.hide;
            if (!center.active || center.type != ModContent.NPCType<TheOracle>())
            {
                Projectile.Kill();
            }
            if (shouldLookAtPlayer)
            {
                LookAtPlayer();
            }
            if (center.ai[1] == 0)
            {
                AITimer = 0;
            }
            #region "animation"
            if (currentState == 0)
            {
                if (++Projectile.frameCounter >= 5)
                {
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    if (frameX < 3)
                        frameX++;
                    else
                        frameX = 0;
                }
            }
            else if (currentState == 1)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter < 5)
                {
                    Projectile.frame = 0;
                    frameX = 4;
                }
                else if (Projectile.frameCounter < 10)
                {
                    Projectile.frame = 1;
                    frameX = 0;
                }
                else if (Projectile.frameCounter < 15)
                {
                    Projectile.frame = 1;
                    frameX = 1;
                }
                else if (Projectile.frameCounter < 20)
                {
                    Projectile.frame = 1;
                    frameX = 2;
                }
                else if (Projectile.frameCounter < 25)
                {
                    Projectile.frame = 1;
                    frameX = 3;
                }
                else if (Projectile.frameCounter < 30)
                {
                    Projectile.frame = 1;
                    frameX = 4;
                }
                else if (Projectile.frameCounter < 35)
                {
                    Projectile.frame = 2;
                    frameX = 0;
                }
                else if (Projectile.frameCounter < 40)
                {
                    Projectile.frame = 2;
                    frameX = 1;
                }
                else if (Projectile.frameCounter < 45)
                {
                    Projectile.frame = 2;
                    frameX = 2;
                }
                else if (Projectile.frameCounter < 50)
                {
                    Projectile.frame = 2;
                    frameX = 3;
                }
                else if (Projectile.frameCounter < 55)
                {
                    Projectile.frame = 2;
                    frameX = 4;
                }
                else if (Projectile.frameCounter < 60)
                {
                    Projectile.frame = 3;
                    frameX = 0;
                }
                else if (Projectile.frameCounter < 65)
                {
                    Projectile.frame = 3;
                    frameX = 1;
                }
                else if (Projectile.frameCounter < 70)
                {
                    Projectile.frame = 3;
                    frameX = 2;
                }
                else if (Projectile.frameCounter < 75)
                {
                    Projectile.frame = 3;
                    frameX = 3;
                }
                else
                {
                    currentState = 2;
                    Projectile.frameCounter = 0;
                }
            }
            else if (currentState == 2)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter < 5)
                {
                    Projectile.frame = 3;
                    frameX = 4;
                }
                else if (Projectile.frameCounter < 10)
                {
                    Projectile.frame = 4;
                    frameX = 0;
                }
                else if (Projectile.frameCounter < 15)
                {
                    Projectile.frame = 4;
                    frameX = 1;
                }
                else if (Projectile.frameCounter < 20)
                {
                    Projectile.frame = 4;
                    frameX = 2;
                }
                else
                {
                    Projectile.frameCounter = 0;
                }
            }
            else if (currentState == 3)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter < 5)
                {
                    Projectile.frame = 4;
                    frameX = 3;
                }
                else if (Projectile.frameCounter < 10)
                {
                    Projectile.frame = 4;
                    frameX = 4;
                }
                else if (Projectile.frameCounter < 15)
                {
                    Projectile.frame = 5;
                    frameX = 0;
                }
                else if (Projectile.frameCounter < 20)
                {
                    Projectile.frame = 5;
                    frameX = 1;
                }
                else
                {
                    Projectile.frameCounter = 0;
                }
            }
            if (Projectile.ai[1] == -1 && currentState != 2)
            {
                currentState = 1;
            }
            #endregion
            #region "positions"
            float factor = 0.18f;
            Projectile.velocity = moveTo * factor;
            if (Projectile.ai[1] == -2 || Projectile.ai[1] == -3 || Projectile.ai[1] == 5 || Projectile.ai[1] == 4 || (Projectile.ai[1] == 2 && center.ai[1] < 40) || Projectile.ai[1] == -1 || Projectile.ai[1] == 2)
            {
                switch (Projectile.localAI[0])
                {
                    case 0:
                        pos = center.Center + new Vector2(120, 50);
                        Projectile.spriteDirection = -1;
                        break;
                    case 1:
                        pos = center.Center + new Vector2(150, -100);
                        Projectile.spriteDirection = -1;
                        break;
                    case 2:
                        pos = center.Center + new Vector2(-120, 50);
                        break;
                    case 3:
                        pos = center.Center + new Vector2(-150, -100);
                        break;
                }
            }
            /*if (Projectile.ai[1] == 4)
            {
                switch (Projectile.localAI[0])
                {
                    case 0:
                        pos = player.Center + new Vector2(70 * 3, -100 * 3);
                        Projectile.spriteDirection = -1;
                        break;
                    case 1:
                        pos = player.Center + new Vector2(150 * 3, 70 * 3);
                        Projectile.spriteDirection = -1;
                        break;
                    case 2:
                        pos = player.Center + new Vector2(-70 * 3, -100 * 3);
                        break;
                    case 3:
                        pos = player.Center + new Vector2(-150 * 3, 70 * 3);
                        break;
                }
            }
            */
            if (Projectile.ai[1] == 2 && center.ai[1] > 40)
            {
                float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f); //idk if this is actually doing anything lol
                switch (Projectile.localAI[0])
                {
                    case 0:
                        pos = center.Center + new Vector2(428 * 1.75f * mult, 0).RotatedBy(2f * (float)Math.PI / 4f * Projectile.localAI[0] + Main.GameUpdateCount * 0.03f);
                        Projectile.spriteDirection = -1;
                        break;
                    case 1:
                        pos = center.Center + new Vector2(428 * 1.75f * mult, 0).RotatedBy(2f * (float)Math.PI / 4f * Projectile.localAI[0] + Main.GameUpdateCount * 0.03f);
                        Projectile.spriteDirection = -1;
                        break;
                    case 2:
                        pos = center.Center + new Vector2(428 * 1.75f * mult, 0).RotatedBy(2f * (float)Math.PI / 4f * Projectile.localAI[0] + Main.GameUpdateCount * 0.03f);
                        break;
                    case 3:
                        pos = center.Center + new Vector2(428 * 1.75f * mult, 0).RotatedBy(2f * (float)Math.PI / 4f * Projectile.localAI[0] + Main.GameUpdateCount * 0.03f);
                        break;
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (orbBlastPos[0] != Vector2.Zero)
                {
                    switch (Projectile.localAI[0])
                    {
                        case 0:
                            pos = orbBlastPos[0];
                            Projectile.spriteDirection = -1;
                            break;
                        case 1:
                            pos = orbBlastPos[1];
                            Projectile.spriteDirection = -1;
                            break;
                        case 2:
                            pos = orbBlastPos[2];
                            break;
                        case 3:
                            pos = orbBlastPos[3];
                            break;
                    }
                }
                else
                {
                    switch (Projectile.localAI[0])
                    {
                        case 0:
                            pos = center.Center + new Vector2(120, 50);
                            Projectile.spriteDirection = -1;
                            break;
                        case 1:
                            pos = center.Center + new Vector2(150, -100);
                            Projectile.spriteDirection = -1;
                            break;
                        case 2:
                            pos = center.Center + new Vector2(-120, 50);
                            break;
                        case 3:
                            pos = center.Center + new Vector2(-150, -100);
                            break;
                    }
                }
            }
            if (Projectile.ai[1] == 3)
            {
                factor = 1;
                switch (Projectile.localAI[0])
                {
                    case 0:
                        pos = player.Center + new Vector2(-300, 0);
                        Projectile.spriteDirection = -1;
                        break;
                    case 1:
                        pos = player.Center + new Vector2(300, 0);
                        Projectile.spriteDirection = -1;
                        break;
                    case 2:
                        pos = player.Center + new Vector2(200, -140);
                        break;
                    case 3:
                        pos = player.Center + new Vector2(-200, -140);
                        break;
                }
            }
            #endregion
            #region "ai"
            if (Projectile.ai[1] == 1)
            {
                AITimer++;
                if (AITimer == 20)
                {
                    if (attacks < 3)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            orbBlastPos[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y + Main.screenHeight * Main.rand.NextFloat());
                        }
                    }
                    else
                    {
                        bool a = Main.rand.NextBool();
                        orbBlastPos[0] = player.Center + new Vector2(300, 300);
                        orbBlastPos[1] = player.Center + new Vector2(300, -300);
                        orbBlastPos[2] = player.Center + new Vector2(-300, 300);
                        orbBlastPos[3] = player.Center + new Vector2(-300, -300);
                    }
                }
                if (AITimer == 50)
                {
                    if (attacks < 3)
                    {
                        attacks++;
                        AITimer = 0;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 1.5f * Utils.RotatedBy(Projectile.DirectionTo(player.Center), 0), ModContent.ProjectileType<OracleBlast>(), 45, 0, player.whoAmI, 1);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 1.5f * Utils.RotatedBy(Projectile.DirectionTo(player.Center), 0), ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0, player.whoAmI);
                    }
                    else
                    {
                        lastPos = player.Center;
                        Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 1.5f * Utils.RotatedBy(Projectile.DirectionTo(player.Center), 0), ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0, player.whoAmI)];
                        proj.timeLeft = 25;
                    }
                }
                if (AITimer >= 75)
                {
                    RegreSystem.ScreenShakeAmount = 10f;
                    Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 1.5f * Utils.RotatedBy(Projectile.DirectionTo(lastPos), 0), ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 2)];
                    proj.timeLeft = 20;
                    AITimer = 0;
                }
            }
            else
            {
                attacks = 0;
            }
            /*if (Projectile.ai[1] == 4)
            {
                AITimer++;
                if (AITimer == 20)
                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 vel = Utils.RotatedBy(Projectile.DirectionTo(player.Center), (double)(MathHelper.ToRadians(21f) * (float)i));
                        Projectile projectile = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 1.5f * vel, ModContent.ProjectileType<OracleBeam>(), 45, 0, player.whoAmI, 2)];
                        projectile.timeLeft = 240;
                    }
            }*/
            /*if (Projectile.ai[1] == 2)
            {
                meleeRot += MathHelper.Lerp(2.5f, 10f, center.ai[1] / 400);
                if (++AITimer >= 35 && center.ai[1] >= 40)
                {
                    AITimer = 0;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, .5f * Utils.RotatedBy(Projectile.DirectionTo(player.Center), 0), ModContent.ProjectileType<OracleOrbs2>(), 45, 0, player.whoAmI, 1);
                }
            }
            else
            {
                meleeRot = 0;
            }*/
            #endregion
        }
        private void LookAtPlayer()
        {
            Vector2 look = Main.player[Projectile.owner].Center - Projectile.Center;
            LookInDirection(look);
        }
        private void LookInDirection(Vector2 look)
        {
            float angle = 0.5f * (float)Math.PI;
            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }
            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }
            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }
            Projectile.rotation = angle;
        }
    }
}

