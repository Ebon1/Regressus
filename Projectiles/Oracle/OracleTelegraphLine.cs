using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Regressus.Effects.Prims;
using System;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Oracle
{
    public class OracleTelegraphLine : OracleBeamTarget
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Telegraph line");
        }
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.timeLeft = 30;
        }
        int MAX_TIME;
        public override void AI()
        {
            if (Projectile.ai[1] == 1)
            {
                NPC npc = Main.npc[(int)Projectile.ai[0]];
                Projectile.Center = npc.Center;
            }
            if (!RunOnce)
            {
                MAX_TIME = Projectile.timeLeft;
                RunOnce = true;
            }
            float progress = Utils.GetLerpValue(0, MAX_TIME, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Extras/laser3").Value;
            Vector2 startPosition = Projectile.Center;
            Vector2 endPosition = Projectile.Center + Projectile.velocity * /*RegreUtils.TRay.CastLength(Projectile.Center, Projectile.velocity, */2650/*)*/;
            float width = Projectile.width * Projectile.scale;
            // offset so i can make the triangles
            RegreUtils.Reload(Main.spriteBatch, BlendState.Additive);
            Vector2 offset = (startPosition - endPosition).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * Projectile.width;

            float mult = (0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly/* * 2*/) * 0.1f);
            Color BeamColor = Color.White * 0.5f /*new Color(20, 63, 128)*/;
            BeamPacket.SetTexture(0, texture);
            float off = -Main.GlobalTimeWrappedHourly % 1;
            // draw the flame part of the beam
            BeamPacket packet = new BeamPacket();
            packet.Pass = "Texture";
            packet.Add(startPosition + offset * 3 * mult, BeamColor, new Vector2(0 + off, 0));
            packet.Add(startPosition - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(endPosition + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));

            packet.Add(startPosition - offset * 3 * mult, BeamColor, new Vector2(0 + off, 1));
            packet.Add(endPosition - offset * 3 * mult, BeamColor, new Vector2(1 + off, 1));
            packet.Add(endPosition + offset * 3 * mult, BeamColor, new Vector2(1 + off, 0));
            packet.Send();
            //DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, (Projectile.timeLeft % 10 == 0) ? Color.White * 0.5f : new Color(20, 63, 128) * 0.5f);
            RegreUtils.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
        /*private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }*/
    }
}

