using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Regressus.Projectiles.Magic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Regressus.Items.Weapons.Magic
{
    public class Vision : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a fragment of a dark future.");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 90;
            Item.crit = 45;
            Item.damage = 145;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Purple;
            Item.shootSpeed = 20.5f;
            Item.shoot = ModContent.ProjectileType<VisionP>();
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
        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltipLine = tooltips.Find((TooltipLine x) => x.Name == "ItemName");
            tooltipLine.overrideColor = new Color(148, 0, 209, Main.DiscoB);
        }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //for (int i = -1; i < 2; i++)
            Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float num2 = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
            float num3 = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
            Vector2 vector5 = new Vector2(num2, num3);
            vector5.X = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
            vector5.Y = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y - 1000f;
            player.itemRotation = (float)Math.Atan2(vector5.Y * (float)player.direction, vector5.X * (float)player.direction);
            NetMessage.SendData(13, -1, -1, null, player.whoAmI);
            NetMessage.SendData(41, -1, -1, null, player.whoAmI);
            Projectile.NewProjectile(source, position, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/ -Vector2.UnitY * 15f, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
