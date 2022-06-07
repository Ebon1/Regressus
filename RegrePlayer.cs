using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.GameInput;
using Regressus.Dusts;

namespace Regressus
{
    public class RegrePlayer : ModPlayer
    {
        public int bossTextProgress, bossMaxProgress;
        public string bossName;
        public string bossTitle;
        public int bossStyle;
        public Color bossColor;
        public Vector2[] oldCenter = new Vector2[950];
        public int[] oldLife = new int[950], oldDir = new int[950];
        public bool reverseTime;
        public bool starshroomed;
        int thing;
        public override void UpdateBadLifeRegen()
        {
            if (starshroomed)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= 16;
            }
        }
        public override void ResetEffects()
        {
            starshroomed = false;
            reverseTime = false;
        }
        public override void PostUpdate()
        {
            if (bossTextProgress > 0)
                bossTextProgress--;
            if (bossTextProgress == 0)
            {
                bossName = null;
                bossTitle = null;
                bossMaxProgress = 0;
                bossStyle = -1;
                bossColor = Color.White;
            }
            if (!reverseTime)
            {
                thing = 0;
                for (int num16 = oldCenter.Length - 1; num16 > 0; num16--)
                {
                    oldCenter[num16] = oldCenter[num16 - 1];
                }
                oldCenter[0] = Player.Center;
                for (int num16 = oldLife.Length - 1; num16 > 0; num16--)
                {
                    oldLife[num16] = oldLife[num16 - 1];
                }
                oldLife[0] = Player.statLife;
                for (int num16 = oldDir.Length - 1; num16 > 0; num16--)
                {
                    oldDir[num16] = oldDir[num16 - 1];
                }
                oldDir[0] = Player.direction;
            }
            else
            {
                thing++;
                if (thing < oldCenter.Length)
                {
                    if (oldCenter[thing] != Vector2.Zero)
                        Player.Center = oldCenter[thing];
                }
                if (thing < oldLife.Length)
                {
                    if (oldLife[thing] != 0)
                        Player.statLife = oldLife[thing];
                }
                if (thing < oldDir.Length)
                {
                    if (oldDir[thing] != 0)
                        Player.direction = oldDir[thing];
                }
            }
        }
    }
}
