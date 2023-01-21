using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Items.Weapons.Magic;
using Terraria.GameContent;
using Regressus.Effects.Prims;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Regressus.Projectiles.Magic
{
    public class VisionP : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.height = 65;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 65;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 100;
            if (Projectile.ai[1] == 0)
                Projectile.penetrate = -1;
            else
                Projectile.penetrate = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        Vector2 mousePos;
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 100, 100, ModContent.DustType<Dusts.TestDust>(), 0, 0, 0, default, 1.75f * Projectile.scale);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 55 && Projectile.ai[1] == 0)
            {
                //for (int i = -1; i < 5; i++)
                //    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Main.MouseWorld.X, Main.screenPosition.Y), Utils.RotatedBy(Vector2.UnitY * 17.5f, (double)(MathHelper.ToRadians(Main.rand.NextFloat(2.5f, 4.5f)) * (float)i)), ModContent.ProjectileType<VisionP>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), new(Main.MouseWorld.X, Main.screenPosition.Y - 200), Vector2.UnitY, ModContent.ProjectileType<VisionP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.scale = 0.45f;
            }
            /*if (Projectile.velocity != Vector2.Zero)
                d.velocity = Vector2.Zero;
            else
                d.velocity = new Vector2(0, -4);*/
        }
    }
    public class VisionP2 : ModProjectile
    {
        protected bool RunOnce = true;
        public int MAX_TIME = 400;
        public override string Texture => "Regressus/Extras/Empty";
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath44);
        }
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 2000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 400;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hide = true;
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
            DisplayName.SetDefault("beam");
        }
        int damage;
        Vector2 vel;
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public override void AI()
        {
            if (RunOnce)
            {
                MAX_TIME = Projectile.timeLeft;
                RunOnce = false;
            }
            vel = Projectile.velocity;
            vel.Normalize();
            Vector2 end = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
            Projectile.Center = Vector2.Lerp(Projectile.Center, new(Main.MouseWorld.X, Main.player[Projectile.owner].Center.Y - (Main.screenHeight / 2)), 0.035f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.velocity = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));

            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 5, 0, 1);
        }
        public Color BeamColor = new Color(20, 63, 128);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, Projectile.width * Projectile.scale, ref _);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += 0.3f;

            RegreUtils.Reload(Main.spriteBatch, MiscDrawingMethods.Subtractive);
            RegreUtils.Reload(Main.spriteBatch, SpriteSortMode.Immediate);
            Texture2D bolt = RegreUtils.GetExtraTexture("laser");
            Texture2D bolt1 = RegreUtils.GetExtraTexture("laser2");
            Texture2D bolt2 = RegreUtils.GetExtraTexture("laser3");

            // make the beam slightly change scale with time
            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = Projectile.scale * 4 * mult;
            Texture2D texture = ModContent.Request<Texture2D>("Regressus/Extras/Line").Value;
            //float scale = Projectile.scale * 2 * mult;
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + vel * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity,*/ Main.screenWidth;//);
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

            BeamColor = Color.DarkMagenta;
            BeamPacket.SetTexture(0, bolt1);
            // draw the flame part of the beam
            packet.Add(start + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(start - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(end - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(end + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();

            BeamColor = Color.DarkViolet;
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


            texture = ModContent.Request<Texture2D>("Regressus/Extras/Extras2/circle_05").Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.DarkViolet, 0, new Vector2(texture.Width, texture.Height) / 2, scale, SpriteEffects.None, 0f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);

            return false;
        }
    }
}

