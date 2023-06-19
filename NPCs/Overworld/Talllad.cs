using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Regressus.Items.Ammo;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Regressus.Dusts;
using Mono.Cecil.Cil;
using System;

namespace Regressus.NPCs.Overworld
{
    public class Talllad : ModNPC
    {
        enum StateID
        {
            Walking,
            Fallover
        };

        StateID state = StateID.Walking;

        float heightMod;
        float widthMod;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 10;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 1f;

            NPC.Size = new Vector2(76f, 140);
            NPC.scale = 1f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.sellPrice(0, 0, 0, 90);

            NPC.aiStyle = -1;
        }
        public override bool CheckDead()
        {
            if (NPC.ai[3] == 0)
            {
                state = StateID.Fallover;
                NPC.dontTakeDamage = true;
                NPC.immortal = true;
                NPC.life = 1;
                return false;
            }

            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 58, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 7; num616++)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];


            if (state == StateID.Walking)
            {
                NPC.TargetClosest(true);
                NPC.velocity.X += NPC.direction * 0.05f;

                NPC.spriteDirection = NPC.direction = NPC.Center.X < player.Center.X ? 1 : -1;
                if (NPC.collideX)
                {
                    NPC.velocity.Y = -5;
                }
            }
            else if (state == StateID.Fallover)
            {
                NPC.velocity.X = 0;
                NPC.ai[3]++;
                if (NPC.ai[3] == 1)
                {
                    heightMod = 1.5f;
                    widthMod = 0.75f;
                }
                if (NPC.ai[3] == 60)
                {
                    heightMod = 0.5f;
                    widthMod = 2;
                }
                if (NPC.ai[3] == 65)
                {
                    NPC.dontTakeDamage = false;
                    NPC.immortal = false;
                    NPC.life = 0;
                    NPC.checkDead();
                }
                heightMod += (1 - heightMod) / 15f;
                widthMod += (1 - widthMod) / 15f;
            }

        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_OnHit(NPC), projectile.Center, -projectile.oldVelocity, projectile.type, projectile.damage, projectile.knockBack);
            p.friendly = false;
            p.hostile = true;
            projectile.Kill();
        }
        public override void FindFrame(int frameHeight)
        {

            if (state == StateID.Walking)
            {
                heightMod += (1 - heightMod) / 5f;
                widthMod += (1 - widthMod) / 5f;
                if (++NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 9 * frameHeight)
                        NPC.frame.Y += frameHeight;
                    else
                        NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight * 9;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 position = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY);

            SpriteEffects spriteEffects = SpriteEffects.None;

            if (NPC.spriteDirection > 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }

            spriteBatch.Draw(texture, position, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, new Vector2(widthMod, heightMod), spriteEffects, 0);

            return false;
        }
    }
}
