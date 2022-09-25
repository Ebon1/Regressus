using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using Regressus.Effects.Prims;

namespace Regressus.Projectiles.Oracle
{
    public class OracleBeamTarget : ModProjectile
    {
        protected bool RunOnce = true;
        protected int RotationDirection = 1;
        public Player Target;

        protected const float MAX_TIME = 350;

        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam");
        }
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 2650;
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
        public override void AI()
        {
            if (RunOnce)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.ai[1] = 1.75f;
                Target = Main.player[(int)Projectile.ai[0]];
                RunOnce = false;
            }

            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            RotationDirection = isLeft(Projectile.Center, end, Target.Center) ? 1 : -1;

            Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY).RotatedBy(MathHelper.ToRadians(MathHelper.SmoothStep(Projectile.ai[1], 0, Projectile.timeLeft / MAX_TIME)) * RotationDirection);

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float a = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.height, Projectile.width, ref a);
        }
        public Color BeamColor = new Color(20, 63, 128);
        float rotation;
        public override bool PreDraw(ref Color lightColor)
        {
            //Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);

            Texture2D TrailTexture1 = Mod.Assets.Request<Texture2D>("Extras/oracleBeam").Value;
            Texture2D TrailTexture2 = Mod.Assets.Request<Texture2D>("Extras/Tentacle").Value;
            Texture2D TrailTexture3 = Mod.Assets.Request<Texture2D>("Extras/laser").Value;
            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            rotation += MathHelper.ToRadians(1 * Projectile.ai[1]);
            Texture2D rune1 = ModContent.Request<Texture2D>("Regressus/Extras/rune1").Value;
            Texture2D rune2 = ModContent.Request<Texture2D>("Regressus/Extras/rune2").Value;
            Texture2D rune3 = ModContent.Request<Texture2D>("Regressus/Extras/rune3").Value;
            Texture2D rune4 = ModContent.Request<Texture2D>("Regressus/Extras/rune4").Value;
            Main.spriteBatch.Draw(rune1, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune1.Width, rune1.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune2, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune2.Width, rune2.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune3, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune3.Width, rune3.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune4, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune4.Width, rune4.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);

            //float scale = Projectile.scale * 2 * mult;
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = new Color(20, 63, 128);
            BeamPacket.SetTexture(0, TrailTexture1);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = new Color(20, 63, 128);
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();

            BeamColor = Color.White;
            BeamPacket packet3 = new BeamPacket();
            packet3.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture3);
            packet3.Add(start + offset * mult, BeamColor, new Vector2(0 + -off, 0));
            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));

            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end - offset * mult, BeamColor, new Vector2(1 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));
            packet3.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;
            for (int i = 0; i < 5; i++)
                Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, new Color(20, 63, 128), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.type == ModContent.ProjectileType<OracleBeamTarget>())
            {
                DelegateMethods.v3_1 = new Color(20, 63, 128).ToVector3();
                Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/, Projectile.width * Projectile.scale, new Terraria.Utils.TileActionAttempt(DelegateMethods.CastLight));
            }
        }
    }

    public class OracleBeamNoTarget : OracleBeamTarget
    {
        public Vector2 CrossHairTarget;
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 2650;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam test");
        }
        public override void AI()
        {
            if (RunOnce)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.ai[1] = 2.5f;
                RunOnce = false;
            }

            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            RotationDirection = Projectile.ai[0] == 1 ? -1 : 1;

            Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY).RotatedBy(MathHelper.ToRadians(MathHelper.SmoothStep(Projectile.ai[1], 0, Projectile.timeLeft / MAX_TIME)) * RotationDirection);

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
    public class OracleBeam : OracleBeamTarget
    {
        int MAX_TIME = 60;
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 2650;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam rotate");
        }
        int damage;
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

            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;

            //Projectile.velocity = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * (MAX_TIME <= 25 ? 1 : 2 * (Projectile.ai[0] + 1)), 0, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            Texture2D TrailTexture1 = Mod.Assets.Request<Texture2D>("Extras/oracleBeam").Value;
            Texture2D TrailTexture2 = Mod.Assets.Request<Texture2D>("Extras/Tentacle").Value;
            Texture2D TrailTexture3 = Mod.Assets.Request<Texture2D>("Extras/laser").Value;

            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            //float scale = Projectile.scale * 2 * mult;
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = new Color(20, 63, 128);
            BeamPacket.SetTexture(0, TrailTexture1);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = new Color(20, 63, 128);
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();

            BeamColor = Color.White;
            BeamPacket packet3 = new BeamPacket();
            packet3.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture3);
            packet3.Add(start + offset * mult, BeamColor, new Vector2(0 + -off, 0));
            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));

            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end - offset * mult, BeamColor, new Vector2(1 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));
            packet3.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;
            for (int i = 0; i < 5; i++)
                Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, new Color(20, 63, 128), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
    public class OracleBeamRT : OracleBeam
    {

        int MAX_TIME = 60;

        public override void SetDefaults()
        {
            Projectile.width = Main.screenWidth / 6;
            Projectile.height = 2650;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam pillar");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);

            Texture2D _beamTexture = ModContent.Request<Texture2D>("Regressus/Extras/oracleBeamLight").Value;
            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            //float scale = Projectile.scale * 2 * mult;
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;
            float off = 0;
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            BeamPacket packet1 = new BeamPacket();
            packet1.Pass = "Texture";

            BeamColor = Color.DeepSkyBlue;
            BeamPacket.SetTexture(0, _beamTexture);
            // draw the flame part of the beam
            packet1.Add(start + offset * 15f * mult, BeamColor, new Vector2(0 + off, 0));
            packet1.Add(start - offset * 15f * mult, BeamColor, new Vector2(0 + off, 1));
            packet1.Add(end + offset * 15f * mult, BeamColor, new Vector2(1 + off, 0));

            packet1.Add(start - offset * 15f * mult, BeamColor, new Vector2(0 + off, 1));
            packet1.Add(end - offset * 15f * mult, BeamColor, new Vector2(1 + off, 1));
            packet1.Add(end + offset * 15f * mult, BeamColor, new Vector2(1 + off, 0));
            packet1.Send();
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";

            BeamColor = Color.White;
            BeamPacket.SetTexture(0, _beamTexture);
            // draw the flame part of the beam
            packet.Add(start + offset * 14f * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 14f * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 14f * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 14f * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 14f * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 14f * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
    public class OracleBeam2 : OracleBeam
    {
        int MAX_TIME = 60;

        public override void SetDefaults()
        {
            Projectile.width = Main.screenWidth / 6;
            Projectile.height = 2650;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam pillar");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);

            Texture2D _beamTexture = ModContent.Request<Texture2D>("Regressus/Extras/oracleBeamLight").Value;
            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            //float scale = Projectile.scale * 2 * mult;
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;
            float off = 0;
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            BeamPacket packet1 = new BeamPacket();
            packet1.Pass = "Texture";

            BeamColor = Color.DeepSkyBlue;
            BeamPacket.SetTexture(0, _beamTexture);
            // draw the flame part of the beam
            packet1.Add(start + offset * 15f * mult, BeamColor, new Vector2(0 + off, 0));
            packet1.Add(start - offset * 15f * mult, BeamColor, new Vector2(0 + off, 1));
            packet1.Add(end + offset * 15f * mult, BeamColor, new Vector2(1 + off, 0));

            packet1.Add(start - offset * 15f * mult, BeamColor, new Vector2(0 + off, 1));
            packet1.Add(end - offset * 15f * mult, BeamColor, new Vector2(1 + off, 1));
            packet1.Add(end + offset * 15f * mult, BeamColor, new Vector2(1 + off, 0));
            packet1.Send();
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";

            BeamColor = Color.White;
            BeamPacket.SetTexture(0, _beamTexture);
            // draw the flame part of the beam
            packet.Add(start + offset * 14f * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 14f * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 14f * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 14f * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 14f * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 14f * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }

    public class OracleBeamConnect : OracleBeamTarget
    {
        public Projectile Target;

        private const float MAX_TIME = 20;

        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam Test");
        }
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 2650;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)MAX_TIME;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {

            if (RunOnce)
            {
                Target = Main.projectile[(int)Projectile.ai[0]];
                RunOnce = false;
            }

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 2, 0, 1);
        }

        float rotation;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);

            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.1f);
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            rotation += MathHelper.ToRadians(1);
            Texture2D rune1 = ModContent.Request<Texture2D>("Regressus/Extras/rune1").Value;
            Texture2D rune2 = ModContent.Request<Texture2D>("Regressus/Extras/rune2").Value;
            Texture2D rune3 = ModContent.Request<Texture2D>("Regressus/Extras/rune3").Value;
            Texture2D rune4 = ModContent.Request<Texture2D>("Regressus/Extras/rune4").Value;
            Texture2D TrailTexture1 = Mod.Assets.Request<Texture2D>("Extras/oracleBeam").Value;
            Texture2D TrailTexture2 = Mod.Assets.Request<Texture2D>("Extras/Tentacle").Value;
            Texture2D TrailTexture3 = Mod.Assets.Request<Texture2D>("Extras/laser").Value;
            Main.spriteBatch.Draw(rune1, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune1.Width, rune1.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune2, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune2.Width, rune2.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(rune3, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune3.Width, rune3.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune4, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune4.Width, rune4.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);

            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Target.Center;

            float width = Projectile.width * Projectile.scale;
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = new Color(20, 63, 128);
            BeamPacket.SetTexture(0, TrailTexture1);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = new Color(20, 63, 128);
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();

            BeamColor = Color.White;
            BeamPacket packet3 = new BeamPacket();
            packet3.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture3);
            packet3.Add(start + offset * mult, BeamColor, new Vector2(0 + -off, 0));
            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));

            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end - offset * mult, BeamColor, new Vector2(1 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));
            packet3.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;
            for (int i = 0; i < 5; i++)
                Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, new Color(20, 63, 128), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }

    public class OracleBeamChild : OracleBeamTarget
    {
        private NPC ParentNPC;
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy Beam Test 2");
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 2000;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 450;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {

            if (RunOnce)
            {
                ParentNPC = Main.npc[(int)Projectile.ai[0]];
                RunOnce = false;
            }

            if (ParentNPC.active)
            {
                Projectile.Center = ParentNPC.Center + new Vector2(0, -350);
            }
            else
            {
                Projectile.Kill();
            }

            //Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY).RotatedBy(MathHelper.ToRadians(MathHelper.SmoothStep(4, 1, Projectile.timeLeft / 120f)));
            float progress = Utils.GetLerpValue(0, 450, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 8, 0, 1);
        }
        float rotation;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);

            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 0.1f);
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            Texture2D TrailTexture1 = Mod.Assets.Request<Texture2D>("Extras/oracleBeam").Value;
            Texture2D TrailTexture2 = Mod.Assets.Request<Texture2D>("Extras/Tentacle").Value;
            Texture2D TrailTexture3 = Mod.Assets.Request<Texture2D>("Extras/laser").Value;
            //rotation += MathHelper.ToRadians(1 * Projectile.ai[1]);
            /*Texture2D rune1 = ModContent.Request<Texture2D>("Regressus/Extras/rune1").Value;
            Texture2D rune2 = ModContent.Request<Texture2D>("Regressus/Extras/rune2").Value;
            Texture2D rune3 = ModContent.Request<Texture2D>("Regressus/Extras/rune3").Value;
            Texture2D rune4 = ModContent.Request<Texture2D>("Regressus/Extras/rune4").Value;
            Texture2D rune1_glow = ModContent.Request<Texture2D>("Regressus/Extras/rune1_glow").Value;
            Texture2D rune2_glow = ModContent.Request<Texture2D>("Regressus/Extras/rune2_glow").Value;
            Texture2D rune3_glow = ModContent.Request<Texture2D>("Regressus/Extras/rune3_glow").Value;
            Texture2D rune4_glow = ModContent.Request<Texture2D>("Regressus/Extras/rune4_glow").Value;
            Main.spriteBatch.Draw(rune1, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune1.Width, rune1.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune2, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune2.Width, rune2.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune3, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune3.Width, rune3.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune4, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune4.Width, rune4.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune1_glow, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune1.Width, rune1.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune2_glow, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune2.Width, rune2.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune3_glow, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), -rotation, new Vector2(rune3.Width, rune3.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(rune4_glow, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), rotation, new Vector2(rune4.Width, rune4.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            */
            Texture2D rune1 = ModContent.Request<Texture2D>("Regressus/Extras/rune_alt").Value;
            for (int i = 0; i < 2; i++)
                Main.spriteBatch.Draw(rune1, Projectile.Center - Main.screenPosition, null, new Color(20, 63, 128), 0, new Vector2(rune1.Width, rune1.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * Projectile.height;
            float width = Projectile.width * Projectile.scale;
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            BeamColor = new Color(20, 63, 128);
            BeamPacket.SetTexture(0, TrailTexture1);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = new Color(20, 63, 128);
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture2);
            packet2.Add(start + offset * 2 * mult, BeamColor, new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, BeamColor, new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, BeamColor, new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, BeamColor, new Vector2(1 + off, 0));
            packet2.Send();

            BeamColor = Color.White;
            BeamPacket packet3 = new BeamPacket();
            packet3.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture3);
            packet3.Add(start + offset * mult, BeamColor, new Vector2(0 + -off, 0));
            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));

            packet3.Add(start - offset * mult, BeamColor, new Vector2(0 + -off, 1));
            packet3.Add(end - offset * mult, BeamColor, new Vector2(1 + -off, 1));
            packet3.Add(end + offset * mult, BeamColor, new Vector2(1 + -off, 0));
            packet3.Send();
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/blueFlare").Value;
            for (int i = 0; i < 5; i++)
                Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, new Color(20, 63, 128), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
}
