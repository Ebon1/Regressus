using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Regressus.Effects.Prims;
using Regressus.Projectiles.Oracle;
using IL.Terraria.GameContent.ObjectInteractions;

namespace Regressus.Projectiles.Minibosses.Conjurer
{
    public class ConjuCandle : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        Vector2 random;
        float alpha;
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 200, Projectile.timeLeft);
            alpha = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);

            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] < 30)
                Projectile.rotation = RegreUtils.FromAToB(Projectile.Center, player.Center).ToRotation();
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
                for (int i = 0; i < 35; i++)
                {
                    random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y + Main.screenHeight * Main.rand.NextFloat());
                    if (random.Distance(player.Center) < 40)
                        continue;
                    else
                        break;
                }
            if (Projectile.ai[0] < 10 && Projectile.ai[0] != 1)
            {
                Projectile.velocity = RegreUtils.FromAToB(Projectile.Center, random, false);
            }
            if (Projectile.ai[0] == 10)
            {
                Projectile.velocity = Vector2.Zero;
            }
            if (Projectile.ai[0] == 30)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.UnitX.RotatedBy(Projectile.rotation), ModContent.ProjectileType<OracleTelegraphLine>(), 0, 0);
            }
            if (Projectile.ai[0] == 60)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.UnitX.RotatedBy(Projectile.rotation), ModContent.ProjectileType<ConjurerBeam>(), 15, 0);
            }
            if (Projectile.ai[0] == 100)
            {
                Projectile.ai[0] = 0;
            }
        }
    }
    public class ConjurerBeam : OracleBeamTarget
    {
        int MAX_TIME = 30;
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
            DisplayName.SetDefault("Blood Beam");
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
            Vector2 end = Projectile.Center + vel * 5000;

            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.velocity = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * (MAX_TIME <= 25 ? 2 : 2 * (Projectile.ai[0] + 1)), 0, 1);
        }
        public Color BeamColor = new Color(20, 63, 128);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + vel * 5000;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, 25 * Projectile.scale, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            //float scale = Projectile.scale * 2 * mult;ddddddddddddd
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            Texture2D bolt = RegreUtils.GetExtraTexture("laser2");
            Texture2D bolt1 = RegreUtils.GetExtraTexture("laser");
            Texture2D bolt2 = RegreUtils.GetExtraTexture("Tentacle");

            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + vel * 5000;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles i want to kill myself
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;

            float off = -Main.GlobalTimeWrappedHourly % 1;
            BeamColor = Color.Crimson;
            BeamPacket.SetTexture(0, bolt1);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Color.Crimson;
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
            BeamColor = Color.White;
            BeamPacket.SetTexture(0, bolt);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Deferred);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/Spotlight").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Crimson, 0, new Vector2(texture.Width, texture.Height) / 2, scale * 0.5f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Regressus/Extras/crosslight").Value;
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.Crimson, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.25f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, end - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, scale * 0.15f, SpriteEffects.None, 0f);



            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
}
