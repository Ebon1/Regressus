using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;
using Regressus.Projectiles.SSW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.NPCs.Overworld
{
    public class Astrolad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.Size = new Vector2(118, 156);
            NPC.damage = 0;
            NPC.friendly = false;
            NPC.lifeMax = 1;
            NPC.defense = 0;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (!NPC.IsABestiaryIconDummy && NPC.frameCounter % 5 == 0)
            {
                for (int i = 0; i < lads.Length; i++)
                {
                    if (lads[i].hyper)
                    {
                        if (lads[i].frame.Y < lads[i].frame.Height * 3)
                            lads[i].frame.Y += lads[i].frame.Height;
                        else
                            lads[i].frame.Y = 0;
                    }
                    else
                    {
                        if (lads[i].frame.Y < lads[i].frame.Height * 7)
                            lads[i].frame.Y += lads[i].frame.Height;
                        else
                            lads[i].frame.Y = 0;
                    }
                }
            }
            if (NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < frameHeight * 4)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;
            }
        }
        public struct Lad
        {
            public Vector2 position;
            public bool hyper;
            public Rectangle frame;
            public Vector2 velocity;
            public float rotation;
            public Lad(Vector2 _position, bool _hyper, Rectangle _frame, Vector2 _velocity)
            {
                velocity = _velocity;
                position = _position;
                frame = _frame;
                hyper = _hyper;
                rotation = 0;
            }
        }
        public Lad[] lads = new Lad[4];
        public override void OnSpawn(IEntitySource source)
        {
            NPC.velocity = -Vector2.UnitY * 0.1f;
            for (int i = 0; i < lads.Length; i++)
            {
                Vector2 pos = Main.rand.NextVector2Circular(25, 25);
                Rectangle frame = new Rectangle(0, 0, 30, 32);
                bool hyper = Main.rand.NextBool(20);
                lads[i] = new Lad(pos, hyper, hyper ? new Rectangle(0, 0, 28, 34) : frame, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 1)); ;
            }
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < lads.Length; i++)
            {
                Texture2D tex = RegreUtils.GetTexture("NPCs/Critters/Minilad");
                Texture2D hyper = RegreUtils.GetTexture("NPCs/Overworld/Hyperlad");
                Vector2 origin = lads[i].hyper ? new Vector2(28, 34) / 2 : new Vector2(30, 32) / 2;
                spriteBatch.Draw(lads[i].hyper ? hyper : tex, NPC.Center + lads[i].position - screenPos, lads[i].frame, Color.White, lads[i].rotation, origin, 1, SpriteEffects.None, 0);
            }
            Texture2D npc = TextureAssets.Npc[Type].Value;
            Texture2D vfx = ModContent.Request<Texture2D>(Texture + "_vfx").Value;
            spriteBatch.Draw(npc, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, new Vector2(widthMod, heightMod), SpriteEffects.None, 0);
            Color color = Color.Lerp(Color.White, Color.DarkOrange, Main.GlobalTimeWrappedHourly * 2) * (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.15f;
            spriteBatch.Draw(vfx, NPC.Center - screenPos, NPC.frame, color, NPC.rotation, NPC.Size / 2, new Vector2(widthMod, heightMod), SpriteEffects.None, 0);
            return false;
        }
        public void UpdateLads()
        {
            for (int i = 0; i < lads.Length; i++)
            {
                for (int j = 0; j < lads.Length; j++)
                {
                    if (j == i)
                        continue;
                    if (lads[i].position.Distance(lads[j].position) <= 2)
                    {
                        lads[i].velocity = -lads[j].velocity;
                    }
                }
                lads[i].rotation = MathHelper.Lerp(lads[i].rotation, lads[i].velocity.ToRotation() + MathHelper.PiOver2, 0.1f);
                if ((lads[i].position + NPC.Center).Distance(NPC.Center - new Vector2(0, 15).RotatedBy(NPC.rotation)) > 25 * widthMod)
                    lads[i].velocity = RegreUtils.FromAToB((lads[i].position + NPC.Center), NPC.Center - new Vector2(0, 15).RotatedBy(NPC.rotation)).RotatedByRandom(0.5f);
                lads[i].position += lads[i].velocity;
            }
        }
        float heightMod = 1f;
        float widthMod = 1f;

        bool bounced;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (damage > NPC.life)
            {
                for (int i = 0; i < lads.Length; i++)
                {
                    Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_Death(), NPC.Center + lads[i].position, RegreUtils.FromAToB(lads[i].position, Main.player[NPC.target].Center) * 10, ModContent.ProjectileType<MissileLad>(), lads[i].hyper ? 50 : 20, 0, Main.player[NPC.target].whoAmI);
                    a.frame = lads[i].hyper ? 1 : 0;
                }

                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<StarExplosion>(), NPC.damage, 0);
            }
        }
        public override void AI()
        {
            UpdateLads();

            Player player = Main.LocalPlayer;

            NPC.velocity *= 0.98f;

            Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);

            NPC.ai[0]++;

            if (NPC.ai[0] >= Main.rand.NextFloat(180f, 420f))
            {
                if (!player.HasBuff(BuffID.Shine) || !player.hasMagiluminescence)
                {
                    NPC.velocity = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                }
                else if (player.HasBuff(BuffID.Shine) || player.hasMagiluminescence)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * 5f;
                }

                bounced = false;

                NPC.ai[0] = 0f;
            }

            if (player.itemAnimation == player.itemAnimationMax - 1f && NPC.Center.Distance(player.Center) <= 200f && (!player.HasBuff(BuffID.Shine) || !player.hasMagiluminescence))
            {
                NPC.velocity = NPC.DirectionFrom(player.Center) * 5f;
                bounced = false;

                NPC.ai[0] = 0f;
            }

            if (NPC.collideX)
            {
                NPC.velocity.X = -NPC.oldVelocity.X;
                bounced = true;

                SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/ObeseladBounce"), NPC.Center);
            }

            if (NPC.collideY)
            {
                NPC.velocity.Y = -NPC.oldVelocity.Y;
                bounced = true;

                SoundEngine.PlaySound(new SoundStyle("Regressus/Sounds/Custom/ObeseladBounce"), NPC.Center);
            }

            if (!bounced)
            {
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

                heightMod = 1f + (NPC.velocity.Length() * 0.1f);
                widthMod = 1f - (NPC.velocity.Length() * 0.1f);
            }
            else
            {
                NPC.rotation += NPC.velocity.Length() * 0.2f;

                heightMod = 1f;
                widthMod = 1f;
            }

        }
    }
}
