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
using ReLogic.Graphics;

namespace Regressus.Items.Weapons.Ranged.Bows
{
    public class Omniscience : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots arrows that pierce through multiple timelines");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 92;
            Item.crit = 25;
            Item.damage = 51;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 18.5f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Index == 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                Regressus.Galaxy.CurrentTechnique.Passes[0].Apply();
                Regressus.Galaxy.Parameters["galaxy"].SetValue(ModContent.Request<Texture2D>("Regressus/Extras/starSky2").Value);
                var font = FontAssets.MouseText.Value;
                //Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White, 1);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, font, line.Text, new Vector2(line.X, line.Y), Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HotBullet>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}
