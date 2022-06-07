
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Effects.Prims;


namespace Regressus.Projectiles.Dev
{
    public class PokerfacePBeam : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        private const float PiBeamDivisor = MathHelper.Pi / PokerfaceP.NumBeams;


        private const float MaxDamageMultiplier = 1.5f;


        private const float MaxBeamScale = 1.8f;


        private const float MaxBeamSpread = 2f;


        private const float MaxBeamLength = 2400f;



        private const float BeamTileCollisionWidth = 1f;



        private const float BeamHitboxCollisionWidth = 22f;



        private const int NumSamplePoints = 3;





        private const float BeamLengthChangeFactor = 0.75f;


        private const float VisualEffectThreshold = 0.1f;


        private const float OuterBeamOpacityMultiplier = 0.75f;
        private const float InnerBeamOpacityMultiplier = 0.1f;


        private const float BeamLightBrightness = 0.75f;





        private const float BeamColorHue = 0.4694444444444444f;
        private const float BeamHueVariance = 0.18f;
        private const float BeamColorSaturation = 0.66f;
        private const float BeamColorLightness = 0.53f;


        private float BeamID
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }


        private float HostPrismIndex
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        private float BeamLength
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;


            Projectile.tileCollide = false;


            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }


        public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
        public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadSingle();

        public override void AI()
        {

            Projectile hostPrism = Main.projectile[(int)HostPrismIndex];
            if (Projectile.type != ModContent.ProjectileType<PokerfacePBeam>() || !hostPrism.active || hostPrism.type != ModContent.ProjectileType<PokerfaceP>())
            {
                Projectile.Kill();
                return;
            }


            Vector2 hostPrismDir = Vector2.Normalize(hostPrism.velocity);
            float chargeRatio = MathHelper.Clamp(hostPrism.ai[0] / PokerfaceP.MaxCharge, 0f, 1f);


            Projectile.damage = (int)(hostPrism.damage * GetDamageMultiplier(chargeRatio));


            Projectile.friendly = hostPrism.ai[0] > PokerfaceP.DamageStart;


            float beamIdOffset = BeamID - PokerfaceP.NumBeams / 2f + 0.5f;
            float beamSpread;
            float spinRate;
            float beamStartSidewaysOffset;
            float beamStartForwardsOffset;


            if (chargeRatio < 1f)
            {
                Projectile.scale = MathHelper.Lerp(0f, MaxBeamScale, chargeRatio);
                beamSpread = MathHelper.Lerp(MaxBeamSpread, 0f, chargeRatio);
                beamStartSidewaysOffset = MathHelper.Lerp(20f, 6f, chargeRatio);
                beamStartForwardsOffset = MathHelper.Lerp(-21f, -17f, chargeRatio);



                if (chargeRatio <= 0.66f)
                {
                    float phaseRatio = chargeRatio * 1.5f;
                    Projectile.Opacity = MathHelper.Lerp(0f, 0.4f, phaseRatio);
                    spinRate = MathHelper.Lerp(20f, 16f, phaseRatio);
                }



                else
                {
                    float phaseRatio = (chargeRatio - 0.66f) * 3f;
                    Projectile.Opacity = MathHelper.Lerp(0.4f, 1f, phaseRatio);
                    spinRate = MathHelper.Lerp(16f, 6f, phaseRatio);
                }
            }


            else
            {
                Projectile.scale = MaxBeamScale;
                Projectile.Opacity = 1f;
                beamSpread = 0f;
                spinRate = 6f;
                beamStartSidewaysOffset = 6f;
                beamStartForwardsOffset = -17f;
            }


            float deviationAngle = (hostPrism.ai[0] + beamIdOffset * spinRate) / (spinRate * PokerfaceP.NumBeams) * MathHelper.TwoPi;


            Vector2 unitRot = Vector2.UnitY.RotatedBy(deviationAngle);
            Vector2 yVec = new Vector2(4f, beamStartSidewaysOffset);
            float hostPrismAngle = hostPrism.velocity.ToRotation();
            Vector2 beamSpanVector = (unitRot * yVec).RotatedBy(hostPrismAngle);
            float sinusoidYOffset = unitRot.Y * PiBeamDivisor * beamSpread;


            Projectile.Center = hostPrism.Center;

            Projectile.position += hostPrismDir * 66f + new Vector2(0f, -hostPrism.gfxOffY);

            Projectile.position += hostPrismDir * beamStartForwardsOffset;

            Projectile.position += beamSpanVector;


            Projectile.velocity = hostPrismDir.RotatedBy(sinusoidYOffset);
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();


            float hitscanBeamLength = PerformBeamHitscan(hostPrism, chargeRatio >= 1f);
            BeamLength = MathHelper.Lerp(BeamLength, hitscanBeamLength, BeamLengthChangeFactor);


            Vector2 beamDims = new Vector2(Projectile.velocity.Length() * BeamLength, Projectile.width * Projectile.scale);


            Color beamColor = GetOuterBeamColor();
            if (chargeRatio >= VisualEffectThreshold)
            {
                //ProduceBeamDust(beamColor);


                if (Main.netMode != NetmodeID.Server)
                {
                    ProduceWaterRipples(beamDims);
                }
            }



            DelegateMethods.v3_1 = beamColor.ToVector3() * BeamLightBrightness * chargeRatio;
        }


        private float GetDamageMultiplier(float chargeRatio)
        {
            float f = chargeRatio * chargeRatio * chargeRatio;
            return MathHelper.Lerp(1f, MaxDamageMultiplier, f);
        }

        private float PerformBeamHitscan(Projectile prism, bool fullCharge)
        {


            Vector2 samplingPoint = Projectile.Center;
            if (fullCharge)
            {
                samplingPoint = prism.Center;
            }



            Player player = Main.player[Projectile.owner];
            if (!Collision.CanHitLine(player.Center, 0, 0, prism.Center, 0, 0))
            {
                samplingPoint = player.Center;
            }




            float[] laserScanResults = new float[NumSamplePoints];
            Collision.LaserScan(samplingPoint, Projectile.velocity, BeamTileCollisionWidth * Projectile.scale, MaxBeamLength, laserScanResults);
            float averageLengthSample = 0f;
            for (int i = 0; i < laserScanResults.Length; ++i)
            {
                averageLengthSample += laserScanResults[i];
            }
            averageLengthSample /= NumSamplePoints;

            return averageLengthSample;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }


            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + Projectile.velocity * RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, MaxBeamLength);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, BeamHitboxCollisionWidth * Projectile.scale, ref _);
        }
        public Color BeamColor = new Color(20, 63, 128);


        public override bool PreDraw(ref Color lightColor)
        {
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);

            Texture2D TrailTexture1 = Mod.Assets.Request<Texture2D>("Extras/oracleBeam").Value;
            Texture2D TrailTexture2 = Mod.Assets.Request<Texture2D>("Extras/Tentacle").Value;
            Texture2D TrailTexture3 = Mod.Assets.Request<Texture2D>("Extras/laser").Value;
            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            //float scale = Projectile.scale * 2 * mult;
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, MaxBeamLength);
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            BeamColor = new Color(20, 63, 128);
            BeamPacket.SetTexture(0, TrailTexture1);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, GetOuterBeamColor(), new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, GetOuterBeamColor(), new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, GetOuterBeamColor(), new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, GetOuterBeamColor(), new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, GetOuterBeamColor(), new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, GetOuterBeamColor(), new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = new Color(20, 63, 128);
            BeamPacket packet2 = new BeamPacket();
            packet2.Pass = "Texture";
            BeamPacket.SetTexture(0, TrailTexture2);
            packet2.Add(start + offset * 2 * mult, GetOuterBeamColor(), new Vector2(0 + off, 0));
            packet2.Add(start - offset * 2 * mult, GetOuterBeamColor(), new Vector2(0 + off, 1));
            packet2.Add(end + offset * 2 * mult, GetOuterBeamColor(), new Vector2(1 + off, 0));

            packet2.Add(start - offset * 2 * mult, GetOuterBeamColor(), new Vector2(0 + off, 1));
            packet2.Add(end - offset * 2 * mult, GetOuterBeamColor(), new Vector2(1 + off, 1));
            packet2.Add(end + offset * 2 * mult, GetOuterBeamColor(), new Vector2(1 + off, 0));
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

            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.05f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/flare").Value;
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.1f, SpriteEffects.None, 0f);

            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }

        private Color GetOuterBeamColor()
        {

            float hue = (BeamID / PokerfaceP.NumBeams) % BeamHueVariance + BeamColorHue;


            Color c = Main.hslToRgb(hue, BeamColorSaturation, BeamColorLightness);


            c.A = 64;
            return c;
        }


        private Color GetInnerBeamColor() => Color.White;

        /*private void ProduceBeamDust(Color beamColor)
        {

            const int type = 15;
            Vector2 endPosition = Projectile.Center + Projectile.velocity * (BeamLength - 14.5f * Projectile.scale);



            float angle = Projectile.rotation + (Main.rand.NextBool() ? 1f : -1f) * MathHelper.PiOver2;
            float startDistance = Main.rand.NextFloat(1f, 1.8f);
            float scale = Main.rand.NextFloat(0.7f, 1.1f);
            Vector2 velocity = angle.ToRotationVector2() * startDistance;
            Dust dust = Dust.NewDustDirect(endPosition, 0, 0, type, velocity.X, velocity.Y, 0, beamColor, scale);
            dust.color = beamColor;
            dust.noGravity = true;


            if (Projectile.scale > 1f)
            {
                dust.velocity *= Projectile.scale;
                dust.scale *= Projectile.scale;
            }
        }*/

        private void ProduceWaterRipples(Vector2 beamDims)
        {
            WaterShaderData shaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();


            float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
            Vector2 ripplePos = Projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Projectile.rotation);


            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation);
        }
    }
}
