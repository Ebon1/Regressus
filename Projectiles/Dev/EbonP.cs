using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using Regressus.Effects.Prims;
using Regressus.Projectiles.Melee;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.IO;
using Regressus.NPCs.Minibosses;

namespace Regressus.Projectiles.Dev
{
    public class EbonP1 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(134, 148);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
        }
        bool a;
        float mult;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D b = RegreUtils.GetExtraTexture("Sprites/Exol");
            Texture2D c = RegreUtils.GetExtraTexture("Sprites/Exol_Glow");
            Main.EntitySpriteDraw(b, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), lightColor * mult, Projectile.rotation, Projectile.Size / 2, 1, SpriteEffects.None, 0);
            sb.Reload(BlendState.Additive);
            Main.EntitySpriteDraw(c, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White * mult, Projectile.rotation, Projectile.Size / 2, 1, SpriteEffects.None, 0);
            sb.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, player.Center - Vector2.UnitY * 250, false) * 0.018f;
            if (!a)
            {
                for (int i = 0; i < 8; i++)
                {
                    float angle = 2f * (float)Math.PI / 8f * i;
                    Vector2 pos = Projectile.Center + 130 * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Projectile poop = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), pos, Vector2.Zero, ModContent.ProjectileType<EbonP1_2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, angle, Projectile.whoAmI);
                    if (i != 0)
                        poop.localAI[0] = i;
                    else
                        poop.localAI[0] = 0.5f;
                }
                a = true;
            }

            float progress = Utils.GetLerpValue(0, 600, Projectile.timeLeft);
            mult = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 10, 0, 1);
        }
    }
    public class EbonP1_2 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol1";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(50, 50);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 500;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }
        bool a;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile owner = Main.projectile[(int)Projectile.ai[1]];
            if (Projectile.timeLeft > 50 * Projectile.localAI[0] && !a)
            {
                Projectile.ai[0] += 2f * (float)Math.PI / 600f * 10;
                Projectile.ai[0] %= 2f * (float)Math.PI;
                Projectile.Center = owner.Center + 130 * new Vector2((float)Math.Cos(Projectile.ai[0]), (float)Math.Sin(Projectile.ai[0]));
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (!a)
                {
                    Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, Main.MouseWorld) * 25f;
                    Projectile.timeLeft = 500;
                    a = true;
                }
            }
        }
    }
    public class EbonP2 : LuminaryBeamBase
    {
        public override string Texture => RegreUtils.Empty;
        public override void Extra()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            MAX_TIME = 180;
            Projectile.timeLeft = 180;
            Projectile.width = 200;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D bolt1 = RegreUtils.GetExtraTexture("laser2");
            Texture2D bolt2 = RegreUtils.GetExtraTexture("oracleBeamLight");

            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 5 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            Texture2D bolt = RegreUtils.GetExtraTexture("laser4");
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
            float num = Vector2.Distance(start, end);
            Vector2 vector = (end - start) / num;
            Vector2 vector2 = start;
            float rotation = vector.ToRotation();
            for (int i = 0; i < num; i++)
            {
                Main.spriteBatch.Draw(bolt, vector2 - Main.screenPosition, null, Main.DiscoColor, rotation, bolt.Size() / 2, new Vector2(1, Projectile.scale * 5), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(bolt, vector2 - Main.screenPosition, null, Color.White, rotation, bolt.Size() / 2, new Vector2(1, Projectile.scale * 5), SpriteEffects.None, 0f);
                vector2 = start + i * vector;
            }
            //float scale = Projectile.scale * 2 * mult;
            /*BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 vel = Projectile.velocity;
            vel.Normalize();
            Vector2 end = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ /*Main.screenWidth;//);
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles i want to kill myself
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = Color.White;
            BeamPacket.SetTexture(0, bolt);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Main.DiscoColor;
            BeamPacket.SetTexture(0, bolt1);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Main.DiscoColor;
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, bolt2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);
            */
            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.9f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Main.DiscoColor, 0, new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.9f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Main.DiscoColor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
    public class EbonP3 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP4 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
    public class EbonP5 : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Sprites/Exol";
    }
}
