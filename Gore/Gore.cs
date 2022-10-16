using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace Regressus
{
    public class GhastrootGore : ModGore
    {
        public override string Texture => "Regressus/Gore/" + this.Name;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }

    }
    public class AngelGore : ModGore
    {
        public override string Texture => "Regressus/Gore/" + this.Name;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }

    }
    public class TerraknightGore1 : ModGore
    {
        public override string Texture => "Regressus/Gore/" + this.Name;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }


    }
    public class TerraknightGore2 : ModGore
    {
        public override string Texture => "Regressus/Gore/" + this.Name;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }

    }
    public class TerraknightGore3 : ModGore
    {

        public override string Texture => "Regressus/Gore/" + this.Name;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }


    }
    public class TerraknightGore4 : ModGore
    {
        public override string Texture => "Regressus/Gore/" + this.Name;

        public override void SetStaticDefaults()
        {
            GoreID.Sets.SpecialAI[Type] = 0;
        }

    }
}
