using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace Regressus.Projectiles.Oracle
{
    public class TelegraphLineOracle : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
            Projectile.timeLeft = 45;
		}
        public int timer = 0;
        private Vector2 startpos;
        public override void AI() {
            timer++;
            if (timer == 1) {
                startpos = Projectile.Center;
            }
            if (timer >= 1) {
                Projectile.Center = startpos;
            }
            Player player = Main.player[Projectile.owner];
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Oracle/OracleLine").Value;
			Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale;
			Vector2 drawScale = new Vector2(0.15f, 0.15f);
			float visualBeamLength = Main.screenWidth - 1f * Projectile.scale;
			DelegateMethods.f_1 = 1f;
			Vector2 startPosition = centerFloored - Main.screenPosition;
			Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;
			DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, ((Projectile.timeLeft % 5 == 0) ? Color.White : new Color(213, 166, 53)));
			return false;
		}
		private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
		{
			Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);
			DelegateMethods.c_1 = beamColor;
			Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
		}

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
                Texture2D line = mod.GetTexture("Projectiles/Line");
                spriteBatch.Draw(line, projectile.Center - Main.screenPosition, null, ((projectile.timeLeft % 10 == 0) ? Color.White : Color.Red), projectile.rotation, new Vector2(1, 6), new Vector2(1f, Main.screenHeight / 6), 0, 0);
            return true;
        }*/
    }
}