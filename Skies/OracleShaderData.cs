using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
namespace Regressus.Skies
{
    public class OracleShaderData : ScreenShaderData
    {
        public OracleShaderData(string passName)
            : base(passName)
        {
        }

        public override void Apply()
        {
            UseTargetPosition(Main.screenPosition);
            base.Apply();
        }
    }
}