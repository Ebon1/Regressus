﻿using Microsoft.Xna.Framework;
using Regressus.Projectiles.Dev;
using Regressus.Projectiles.Ranged;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Ammo
{
    public class ChroniteBulletI : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ChroniteBullet>();
            Item.ammo = AmmoID.Bullet;
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}
