using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.Audio;

namespace Regressus.NPCs.Overworld
{
    public class Facade : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(ModContent.NPCType<Starlad>());
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0;
            NPC.hide = true;
            NPC.Size = new Vector2(46, 30);
        }
        public override void OnKill()
        {
            for (int i = 0; i < 5; i++)
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade_Death1>());
            for (int i = 0; i < 3; i++)
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade_Death2>());
        }
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int Hiding = 0, FuckingExplode = 1, PreAttack = 2, Attack = 3;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (AIState == Hiding)
            {
                if (NPC.frameCounter % 5 == 0)
                    if (NPC.frame.Y == 0)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0;
            }
            else if (AIState == PreAttack)
            {
                if (NPC.frameCounter % 5 == 0)
                    if (NPC.frame.Y < 5 * NPC.height && NPC.frame.Y > NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 2 * NPC.height;
            }
            else if (AIState == Attack)
            {
                if (NPC.frameCounter % 5 == 0)
                    if (NPC.frame.Y < 7 * NPC.height && NPC.frame.Y > 5 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 6 * NPC.height;
            }
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }
        public override void PostDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D a = RegreUtils.GetExtraTexture("PulseCircle");
            if (AIState == Attack)
            {
                //sb.Reload(BlendState.Additive);
                //float alpha = MathHelper.Clamp((float)Math.Sin(AITimer3 * Math.PI), 0, 1);
                //if (AITimer2 < 1)
                //  sb.Draw(a, NPC.Center - screenPos, null, Color.Yellow * alpha, 0, a.Size() / 2, 0.4f * AITimer3, SpriteEffects.None, 0f);
                //sb.Reload(BlendState.AlphaBlend);
            }
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (AIState == Hiding)
            {
                if (player.Center.Distance(NPC.Center) < 100)
                    AIState = FuckingExplode;
            }
            else if (AIState == FuckingExplode)
            {
                NPC.frame.Y = NPC.height * 2;
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade1>());
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade2>());
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade3>());
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), ModContent.GoreType<Facade4>());
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), GoreID.Rat1);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), GoreID.CrimsonBunnyHead);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), GoreID.Ladybug1);
                Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Unit(), GoreID.Owl2);
                for (int i = 0; i < 8; i++)
                    Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.BrownMoss);
                AIState = Attack;
            }
            else if (AIState == PreAttack)
            {
                AITimer++;
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AIState = Attack;
                }
            }
            else if (AIState == Attack)
            {
                AITimer++;
                if (AITimer == 1)
                    SoundEngine.PlaySound(SoundID.Zombie7);
                if (AITimer3 < 1f)
                    AITimer3 += 0.02f;
                if (AITimer == 30)
                {
                    for (int i = -2; i < 3; i++)
                    {
                        Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(i, -1), ModContent.ProjectileType<FacadeTentacle>(), 15, 0, NPC.target);
                        a.ai[0] = 10;
                        a.damage = 15;
                        a.ai[1] = 20;
                    }
                }
                if (AITimer >= 300)
                {
                    AITimer = 0;
                    AITimer2 = AITimer3 = 0;
                    AIState = PreAttack;
                }
            }
        }
    }
    public class FacadeTentacle : ModProjectile
    {
        private List<float> rots;

        public int len;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.width = 1;
            Projectile.hide = true;
            Projectile.height = 1;
            Projectile.timeLeft = 300;
            Projectile.damage = 1000;
            Projectile.penetrate = -1;
            rots = new List<float>();
            len = 0;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.PaleGoldenrod);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }
        private float Timer;

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        float value;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = 10;
            //for (int i = 0; i < 3; i++)
            //{
            value += Projectile.ai[1];
            float factor = 1f;
            Vector2 velocity = base.Projectile.velocity * factor * 4f;
            Projectile.rotation = 0.3f * (float)Math.Sin((double)(value / 100f)) + velocity.ToRotation();
            rots.Insert(0, Projectile.rotation);
            while (rots.Count > Projectile.ai[0])
            {
                rots.RemoveAt(rots.Count - 1);
            }

            if (len < Projectile.ai[0] && Projectile.timeLeft > 150 && Projectile.timeLeft % 3 == 0)// && Projectile.timeLeft > 50)
            {
                len++;
            }
            /*if (len >= 0 && Projectile.timeLeft <= 50)
            {
                len -= 5;
            }
            */
            if (len >= 0 && Projectile.timeLeft < 150 && Projectile.timeLeft % 3 == 0)
            {
                len--;
            }
            //}
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 1; i < len; i++)
            {
                float factor = (float)i / (float)len;
                float w = (Projectile.ai[0] == 400 ? 28 : 32f) * MathHelper.SmoothStep(0.8f, 0.1f, factor);
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - new Vector2(w, w) + Utils.RotatedBy(new Vector2((float)(16 * i), 0f), rots[i]), new Vector2(w, w) * 2f))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = Projectile.Center;
            Timer++;
            for (int i = 1; i < len; i++)
            {
                float factor = (float)i / (float)len;
                Vector2 v0 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(16 * (i - 1)), 0f), rots[i - 1]);
                Vector2 v1 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(16 * i), 0f), rots[i]);
                Vector2 normaldir = v1 - v0;
                normaldir = new Vector2(normaldir.Y, 0f - normaldir.X);
                normaldir.Normalize();
                float w = (Projectile.ai[0] == 400 ? 28 : 32f) * MathHelper.SmoothStep(0.8f, 0.1f, factor);


                Rectangle frame = new Rectangle(0, 0, 10, 26);
                Vector2 origin = new Vector2(5, 8);
                //float scale = 1;
                frame.Width = 18;
                if (i < len - 1)
                {
                    frame.Y = 32 * 2;
                    frame.Height = 32;
                }
                else
                {
                    frame.Y = Timer < 5 ? 0 : 32;
                    frame.Height = 32;


                    //Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    //float t = Timer / timeToFlyOut;
                    //scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }

                Vector2 element = v0;
                Vector2 diff = v1 - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                float prog = Utils.GetLerpValue(0, 250, Projectile.timeLeft);
                float alpha = Math.Clamp((float)Math.Sin(prog * Math.PI) * 3, 0, 1);
                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color/* * factor * alpha*/, rotation, origin, 1, flip, 0);

                pos += diff;
            }
            if (Timer > 10)
                Timer = 0;
            return false;
        }
    }
}
