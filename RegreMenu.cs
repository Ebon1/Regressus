using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace Regressus
{
    public class RegreMenu : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Regressus/Extras/Banner");
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            spriteBatch.Draw(Logo.Value, logoDrawCenter, null, Color.White, 0, Logo.Size() / 2, logoScale * 2, SpriteEffects.None, 0);
            return false;
        }
    }
}
