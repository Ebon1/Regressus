using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Regressus.Content.Dusts
{
    public class RollParticle : ModDust
    {
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            if (dust.scale <= 0)
                dust.active = false;
            dust.frame = new Rectangle(0, 0, 8, 8);
            return false;
        }
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            base.OnSpawn(dust);
        }
    }
}
