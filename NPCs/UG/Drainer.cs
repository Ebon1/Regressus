using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.GameContent;

namespace Regressus.NPCs.UG
{
    public class Drainer : ModNPC
    {
        public List<Vector2> flyTowards = new List<Vector2>();
        float PI = (float)Math.PI;
        Vector2 currentVector;
        Vector2 oldPos;
        Vector2[] movementPos = new Vector2[2];
        bool alt = false;
        float rotation;
        int direction;
        Vector2[] afterIMG = new Vector2[6];
        int afterIMGIndex;
        float[] rotationIMG = new float[6];
        int rotationIndex;
        float alpha = 0;
        int[] state = new int[3];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Lich");
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.DebuffImmunitySets[Type] = new NPCDebuffImmunityData
            { ImmuneToAllBuffsThatAreNotWhips = true, ImmuneToWhips = true };
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Apparition>());
            NPC.lifeMax = 250;
            NPC.defense = 5;
            NPC.Size = new Vector2(76, 96);
            NPC.dontTakeDamage = false;
            NPC.aiStyle = 0;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.ZonePurity)
                return 0.1f;
            return 0;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter < 5)
                NPC.frame.Y = 0 * frameHeight;
            else if (NPC.frameCounter < 10)
                NPC.frame.Y = 1 * frameHeight;
            else if (NPC.frameCounter < 15)
                NPC.frame.Y = 2 * frameHeight;
            else if (NPC.frameCounter < 20)
                NPC.frame.Y = 3 * frameHeight;
            else if (NPC.frameCounter < 25)
                NPC.frame.Y = 4 * frameHeight;
            else if (NPC.frameCounter < 30)
                NPC.frame.Y = 5 * frameHeight;
            else
                NPC.frameCounter = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            var fadeMult = 1f / afterIMG.Length;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, Color.White * 1.25f * alpha, rotation, new Vector2(NPC.width / 2, NPC.height / 2), 1, direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            for (int i = 0; i < afterIMG.Length; i++)
                Main.spriteBatch.Draw(tex, afterIMG[i] - screenPos, NPC.frame, Color.White * (0.75f - fadeMult * i) * alpha, rotationIMG[i], new Vector2(NPC.width / 2, NPC.height / 2), 1, direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (NPC.ai[0] == 0)
                oldPos = NPC.Center;
            NPC.ai[0]++;
            if (state[1] == 0)
            {
                Dash();
                FindEmptySpace(alt && Vector2.Distance(NPC.Center, currentVector) > 150 ? currentVector : default);
            }
            else
                Pursuit();
            afterIMG[afterIMGIndex] = NPC.Center;
            if (afterIMGIndex++ > afterIMG.Length - 2)
                afterIMGIndex = 0;
            rotationIMG[rotationIndex] = rotation;
            if (rotationIndex++ > rotationIMG.Length - 2)
                rotationIndex = 0;
            void Pursuit()
            {
                if (state[2]++ > 45 && NPC.ai[3] > 0)
                {
                    state[2] = 0;
                    CombatText.NewText(NPC.getRect(), CombatText.HealLife, (int)NPC.ai[3]);
                    NPC.ai[3] = 0;
                }
                if (Vector2.Distance(NPC.Center, player.MountedCenter) < 125 && state[0] < 330)
                    state[0] = 330;
                bool invulnerable = alpha < 0.25f;
                NPC.dontTakeDamage = invulnerable;
                NPC.defense = 50;
                NPC.damage = invulnerable ? 0 : 50;
                if (!invulnerable)
                    SuckLife();
                NPC.ai[1] += PI / 60;
                NPC.takenDamageMultiplier = 1;
                NPC.dontTakeDamage = invulnerable;
                Vector2 towards = player.MountedCenter - NPC.Center;
                float rot = (float)Math.Atan(towards.Y / towards.X) + (towards.X < 0 ? PI : 0);
                rot += MathHelper.Lerp(-PI / 15, PI / 15, (float)Math.Sin(NPC.ai[1]));
                Vector2 displacement = new Vector2((float)Math.Cos(rot) * 3.33f, (float)Math.Sin(rot) * 3.33f);
                movementPos[0] = NPC.position;
                NPC.position += displacement;
                movementPos[1] = NPC.position;
                direction = player.MountedCenter.X < NPC.Center.X ? -1 : 1;
                rotation = (movementPos[1] - movementPos[0]).ToRotation() + (direction == -1 ? PI : PI / 4);
                Lighting.AddLight(movementPos[0], new Vector3(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B) * 0.005f * alpha);
                Lighting.AddLight(movementPos[1], new Vector3(Color.PaleVioletRed.R, Color.PaleVioletRed.G, Color.PaleVioletRed.B) * 0.01f * alpha);
                if (!invulnerable)
                    SuckLife();
                if (state[0]++ > 360)
                {
                    state = new int[3] { 0, 0, 0 };
                    NPC.ai[1] = 0;
                }
                if (state[0] < 30)
                    alpha += 1 / 30f;
                if (state[0] > 330)
                    alpha -= 1 / 30f;
            }
            void Dash()
            {
                if(currentVector != oldPos && currentVector != Vector2.Zero)
                {
                    float lerp = (float)Math.Sin(NPC.ai[1]);
                    float dist = Vector2.Distance(currentVector, oldPos) * 0.5f;
                    NPC.ai[1] += PI / (150 * ((dist / 300) + 1));
                    #region Roundybouts
                    float halve1 = (float)Math.Atan((currentVector.Y - oldPos.Y) / (currentVector.X - oldPos.X));
                    float halve2 = halve1 + PI;
                    float lerpCircle = MathHelper.Lerp(halve1, halve2, (currentVector.X - oldPos.X > 0 ? 1 : 0) - lerp * (alt ? -1 : 1));
                    Vector2 midway = new Vector2((currentVector.X + oldPos.X) / 2, (currentVector.Y + oldPos.Y) / 2);
                    float usage = dist;
                    Vector2 pos = new Vector2(usage * (float)Math.Cos(lerpCircle), usage * (float)Math.Sin(lerpCircle)) + midway;
                    movementPos[0] = NPC.position;
                    movementPos[1] = pos;
                    if (NPC.ai[2] > 1)
                    {
                        alpha = (float)(Math.Sin(NPC.ai[1] * PI) / 2);
                        bool invulnerable = alpha < 0.25f;
                        NPC.dontTakeDamage = invulnerable;
                        NPC.defense = 0;
                        NPC.takenDamageMultiplier = 2.5f;
                        NPC.damage = invulnerable ? 0 : 75;
                        if (!invulnerable)
                            SuckLife();
                    }
                    direction = currentVector.X < oldPos.X ? -1 : 1;
                    rotation = (movementPos[1] - movementPos[0]).ToRotation() + (direction == -1 ? PI / 2 : PI / 4);
                    if(alpha > 0)
                        if (Main.rand.NextBool((int)Math.Round(40 * lerp) + 1))
                            Dust.NewDustDirect(NPC.Center + new Vector2(Main.rand.Next(NPC.width / -2, NPC.width / 2), Main.rand.Next(NPC.height / -2, NPC.height / 2)), 0, 0, DustID.DungeonSpirit, movementPos[0].X - movementPos[1].X, movementPos[0].Y - movementPos[1].Y).noGravity = true;
                    NPC.position = pos;
                    Lighting.AddLight(movementPos[0], new Vector3(Color.GhostWhite.R, Color.GhostWhite.G, Color.GhostWhite.B) * 0.005f * alpha);
                    Lighting.AddLight(movementPos[1], new Vector3(Color.PaleVioletRed.R, Color.PaleVioletRed.G, Color.PaleVioletRed.B) * 0.01f * alpha);
                    #endregion
                    if (NPC.ai[1] >= Math.PI / 2)
                    {
                        if (flyTowards.Count > 1)
                            currentVector = flyTowards[1];
                        flyTowards.Remove(currentVector);
                        oldPos = NPC.position;
                        NPC.ai[1] = 0;
                        alt = !alt;
                        if (NPC.ai[2] < 2)
                            NPC.ai[2]++;
                        if (state[0]++ > 2)
                            state = new int[3] { 0, 1, 0 };
                        if (NPC.ai[3] > 0)
                        {
                            CombatText.NewText(NPC.getRect(), CombatText.HealLife, (int)NPC.ai[3]);
                            NPC.ai[3] = 0;
                        }
                    }
                }
                else if (flyTowards.Count > 1)
                    currentVector = flyTowards[1];
            }
            void SuckLife()
            {
                if(Vector2.Distance(player.MountedCenter, NPC.Center) < 200)
                {
                    if (NPC.life < NPC.lifeMax)
                    {
                        int exchange = (int)Math.Round(3 * MathHelper.Clamp(alpha, 0, 1));
                        Vector2 towards = NPC.Center - player.MountedCenter;
                        Dust.NewDustPerfect(player.MountedCenter + new Vector2(Main.rand.Next(player.width / -4, player.width / 4), Main.rand.Next(player.height / -4, player.height / 4)), DustID.LifeDrain, towards / 100);
                        player.statLife -= exchange;
                        NPC.ai[3] += exchange;
                        NPC.life += exchange;
                    }
                    else
                        NPC.life = NPC.lifeMax;
                }
            }
            void FindEmptySpace(Vector2 originTarg = default)
            {
                if (originTarg == new Vector2())
                    originTarg = player.MountedCenter;
                Vector2 literalVector = NPC.Center - originTarg;
                float targToRot = (float)Math.Atan(literalVector.Y / literalVector.X) + (literalVector.X > 0 ? PI : 0);
                float dist = Vector2.Distance(originTarg, NPC.Center);
                float lerp = (float)(Math.Sin(NPC.ai[0]) / 2) + 0.5f;
                float lerpSlow = (float)(Math.Sin(NPC.ai[0] / 2) / 2) + 0.5f;
                targToRot += MathHelper.Lerp(-PI * 0.5f, PI * 0.5f, lerp);
                dist *= lerpSlow;
                Vector2 detectPos = new Vector2(dist * (float)Math.Cos(targToRot), dist * (float)Math.Sin(targToRot));
                Point tileWorld = new Point((int)detectPos.X / 16, (int)detectPos.Y / 16);
                tileWorld += new Point((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                int tileCount = 0;
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        Point p =  tileWorld + new Point(x, y);
                        if (WorldGen.TileEmpty(p.X, p.Y))
                            tileCount++;
                    }
                }
                if(tileCount >= 8)
                {
                    Vector2 input = new Vector2(tileWorld.X * 16, tileWorld.Y * 16);
                    bool nearby = false;
                    foreach(Vector2 vec in flyTowards)
                    {
                        if (Vector2.Distance(input, vec) < 150)
                        {
                            nearby = true;
                            break;
                        }
                    }
                    if (!nearby)
                        flyTowards.Add(input);
                }
            }
        }
    }
}