using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regressus.Projectiles.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regressus.Items.Weapons.Ranged.Guns
{
    public class MalignantShotgun : ModItem
    {
        int charges;

        public override bool AltFunctionUse(Player player) => true;

        public override Vector2? HoldoutOffset() => new Vector2(-10f, 0f);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malignant Shotgun");
            Tooltip.SetDefault("<left> to fire"
                + "\nFiring will charge the shotgun"
                + "\n<right> to fire a volley of grenades");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 25;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 25f;
            Item.channel = true;

            Item.width = Item.height = 16;
            Item.useTime = Item.useAnimation = 40;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("Regressus/Sounds/Custom/Shotgun");
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void HoldItem(Player player)
        {
            if (player == Main.LocalPlayer)
            {
                if (player.ItemAnimationActive) { player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 * player.direction); }
                else { player.SetCompositeArmFront(false, default, default); }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = velocity.ToRotation();

            Vector2 offset = new Vector2(0.8f, -0.1f).RotatedBy(rotation);

            for (int k = 0; k < 15; k++)
            {
                Vector2 direction = offset.RotatedByRandom(0.4f);
                Dust.NewDustPerfect(position + offset * 70, DustID.Torch, direction * Main.rand.NextFloat(8), 125, default, 1f).noGravity = true;
            }

            if (Collision.CanHit(position, 0, 0, position + offset, 0, 0)) { position += offset; }

            if (player.altFunctionUse == 2)
            {
                for (int k = 0; k < charges; k++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
                    newVelocity *= 1f - Main.rand.NextFloat(0.5f);

                    Projectile.NewProjectile(source, position, newVelocity, ModContent.ProjectileType<MalignantFission>(), damage, knockback, player.whoAmI);
                }

                charges = 0;
            }
            else
            {
                for (int k = 0; k < 5; k++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));
                    newVelocity *= 1f - Main.rand.NextFloat(0.5f);

                    Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
                }

                charges++;

                if (charges <= 6)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana);
                }
            }

            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MalignantShotgunPro>(), damage, knockback, player.whoAmI);

            Gore.NewGore(source, player.Center, new Vector2(player.direction * -1, -0.5f) * 2, Mod.Find<ModGore>("ShotgunShell").Type, 1f);

            RegreSystem.ScreenShakeAmount = 2f;

            if (charges >= 6)
            {
                charges = 6;
            }

            return false;
        }
    }

    public class MalignantShotgunPro : ModProjectile
    {
        bool initialize = true;

        float maxTimeLeft;

        float preDrawOffset;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Malignant Shotgun");
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.scale = 1f;
            Projectile.Size = Vector2.Zero;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            if (initialize)
            {
                Projectile.timeLeft = player.HeldItem.useAnimation;
                maxTimeLeft = Projectile.timeLeft;
                Projectile.netUpdate = true;
                initialize = false;
            }

            Projectile.Center = player.Center;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Lerp(-0.5f * player.direction, 0f, EaseFunction.EaseQuinticOut.Ease(1 - (Projectile.timeLeft / maxTimeLeft)));

            player.heldProj = Projectile.whoAmI;

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= maxTimeLeft / 2f)
            {
                preDrawOffset += (0 - preDrawOffset) / 5f;
            }
            else if (Projectile.ai[0] >= maxTimeLeft / 3f)
            {
                preDrawOffset += (-20 - preDrawOffset) / 5f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 drawPosition = player.Center + Projectile.rotation.ToRotationVector2() * 20f - Main.screenPosition;

            SpriteEffects drawFlipped = player.direction == -1 ? SpriteEffects.FlipVertically : 0;

            Main.spriteBatch.Draw(texture, drawPosition, sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, drawFlipped, 0f);

            texture = ModContent.Request<Texture2D>(Texture + "Pump").Value;

            sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
            origin = sourceRectangle.Size() / 2f;
            drawPosition = player.Center + Projectile.rotation.ToRotationVector2() * (20f + preDrawOffset) - Main.screenPosition;

            Main.spriteBatch.Draw(texture, drawPosition, sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, drawFlipped, 0f);

            return false;
        }
    }
}
