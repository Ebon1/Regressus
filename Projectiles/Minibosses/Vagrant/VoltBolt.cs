using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Regressus.Effects.Prims;
using Regressus.Projectiles.Oracle;

namespace Regressus.Projectiles.Minibosses.Vagrant
{
    public class VoltBolt : ModProjectile
    {

        protected bool RunOnce = true;
        int MAX_TIME = 15;
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 5000;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        // Cross Product: Where a = line point 1; b = line point 2; c = point to check against.
        public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Bolt");
        }
        int damage;
        Vector2 vel;
        public override void AI()
        {
            if (RunOnce)
            {
                damage = Projectile.damage;
                MAX_TIME = Projectile.timeLeft;
                RunOnce = false;
            }
            if (Projectile.localAI[1] != 0)
            {
                Projectile.damage = 0;
                Projectile.timeLeft = MAX_TIME;
                Projectile.localAI[1]--;
            }
            else
            {
                Projectile.damage = damage;
            }
            vel = Projectile.velocity;
            vel.Normalize();
            Vector2 end = Projectile.Center + vel * RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, 5000);

            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.velocity = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * (MAX_TIME <= 25 ? 2 : 2 * (Projectile.ai[0] + 1)), 0, 1);
        }
        public Color BeamColor = new Color(20, 63, 128);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + vel * RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, 5000);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, 25 * Projectile.scale, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            Texture2D bolt = RegreUtils.GetExtraTexture("Extras2/spark_05");
            Texture2D bolt1 = RegreUtils.GetExtraTexture("Extras2/spark_06");
            Texture2D bolt2 = RegreUtils.GetExtraTexture("Extras2/spark_07");

            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            //float scale = Projectile.scale * 2 * mult;
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + vel * RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, 5000);
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

            BeamColor = Color.Blue;
            BeamPacket.SetTexture(0, bolt1);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Color.Blue;
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

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/light_01").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Blue, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/light_01").Value;
            for (int i = 0; i < 3; i++)
                Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
}
