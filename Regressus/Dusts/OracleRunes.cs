using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Dusts
{
	public class OracleRunes : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			dust.frame = new Rectangle(0, Main.rand.Next(10) * 8, 8, 8);
		}
		public override bool Update(Dust dust) {
			dust.scale -= 0.05f;
			if (dust.scale < 0.75f) {
				dust.active = false;
			}
			return false;
		}
	}
	public class OracleRunes2 : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			dust.frame = new Rectangle(0, Main.rand.Next(10) * 8, 8, 8);
		}
		public override bool Update(Dust dust) {
			dust.scale -= 0.05f;
			if (dust.scale < 0.75f) {
				dust.active = false;
			}
			return false;
		}
	}
}