using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Projectiles.Oracle
{
    public class OracleBeamSmall : ModProjectile
    {
        private const float MoveDistance = 15f;
        public float Distance;
        public float rot = 588;
        public bool opposite;
        public int timer;

        public NPC shooter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oracle Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.hide = false;
            Projectile.scale = 1f;
        }

        public bool runOnce = true;

        public override void AI()
        {
            shooter = Main.npc[(int)Projectile.ai[0]];

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[Projectile.owner];
            if (!shooter.active || shooter.life <= 0)
            {
                Projectile.Kill();
            }
            Vector2 velocity = new Vector2(1, 0).RotatedBy(rot);
            opposite = Projectile.localAI[1] == 1;
            if (opposite)
            {
                rot = -shooter.rotation - MathHelper.PiOver2;
            }
            else
            {
                rot = shooter.rotation - MathHelper.PiOver2;
            }
            velocity.Normalize();
            Projectile.velocity = velocity;
            Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;

            Projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * MoveDistance;
            Projectile.timeLeft = 2;
            int dir = Projectile.direction;

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = Projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * Distance;
            }
        }

        public int colorCounter;
        public Color lineColor;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLaser(Main.spriteBatch, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, new Vector2(shooter.Center.X, shooter.Center.Y),
                Projectile.velocity, 10, Projectile.damage, -1.57f, 1f, 4000f, Color.DeepSkyBlue, (int)MoveDistance);

            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 5200f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.DeepSkyBlue;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 35, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);
            }
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 35, 26), Color.DeepSkyBlue, r, new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);

            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 35, 26), Color.DeepSkyBlue, r, new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);
            RegreUtils.Reload(spriteBatch, BlendState.Additive);
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.DeepSkyBlue;
                origin = start + i * unit;
                spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBeamSmall_Glow").Value, origin - Main.screenPosition,
                    new Rectangle(0, 26, 35, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBeamSmall_Glow").Value, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 35, 26), Color.DeepSkyBlue, r, new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);

            spriteBatch.Draw(ModContent.Request<Texture2D>("Regressus/Projectiles/Oracle/OracleBeamSmall_Glow").Value, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 35, 26), Color.DeepSkyBlue, r, new Vector2(35 * .5f, 26 * .5f), scale, 0, 0);
            RegreUtils.Reload(spriteBatch, BlendState.AlphaBlend);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 3;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}