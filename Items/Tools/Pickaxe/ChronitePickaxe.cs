using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Ranged;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace Regressus.Items.Tools.Pickaxe
{
    public class ChronitePickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.NightmarePickaxe);
        }
    }
}
