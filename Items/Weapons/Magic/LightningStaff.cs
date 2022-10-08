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
using Regressus.Projectiles.Minibosses.Vagrant;

namespace Regressus.Items.Weapons.Magic
{
    public class LightningStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormstaff");
            Tooltip.SetDefault("Invokes the power of zeus!");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 90;
            Item.crit = 15;
            Item.damage = 13;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.reuseDelay = 20;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 20.5f;
            Item.shoot = ModContent.ProjectileType<LightningSummon>();
        }
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
            Projectile.NewProjectile(source, Main.MouseWorld, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/ Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
    public class LightningSummon : ModProjectile
    {
        public override string Texture => "Regressus/Extras/Empty";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = true;
        }
        Vector2 center;
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 0;
                center = Projectile.Center;
            }
            if (++Projectile.ai[0] >= 5)
            {
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), center, Vector2.Zero, ModContent.ProjectileType<LightningF>(), Projectile.damage, 0, Projectile.owner);
                Projectile.ai[0] = 0;
            }
        }
    }
}
