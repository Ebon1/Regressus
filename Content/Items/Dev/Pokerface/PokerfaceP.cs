using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using Regressus.Effects.Prims;

namespace Regressus.Projectiles.Dev
{
    public class PokerfaceP : ModProjectile
    {
        //public override string Texture => "Regressus/Items/Dev/PokerfaceItem";
        private const int NumAnimationFrames = 5;


        public const int NumBeams = 5;


        public const float MaxCharge = 180f;


        public const float DamageStart = 30f;



        private const float AimResponsiveness = 0.1f;


        private const int SoundInterval = 20;





        private const float MaxManaConsumptionDelay = 15f;
        private const float MinManaConsumptionDelay = 5f;


        private float FrameCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }


        private float NextManaFrame
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        private float ManaConsumptionRate
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celeste Prism");



            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {

            Projectile.CloneDefaults(ProjectileID.LastPrism);
            Projectile.width = Projectile.height = 114;
        }
        int timer;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            FrameCounter += 1f;


            UpdateAnimation();
            PlaySounds();


            UpdatePlayerVisuals(player, rrp);


            if (Projectile.owner == Main.myPlayer)
            {

                UpdateAim(rrp, player.HeldItem.shootSpeed);



                bool manaIsAvailable = !ShouldConsumeMana() || player.CheckMana(player.HeldItem.mana, true, false);



                bool stillInUse = player.channel && manaIsAvailable && !player.noItems && !player.CCed;

                if (stillInUse)
                {
                    timer++;
                    if (timer > 25)
                    {
                        for (int i = -3; i < 4; i++)
                        {
                            if (i == 0)
                                continue;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/Vector2.Normalize(Projectile.velocity) * 12.5f, ModContent.ProjectileType<PokerfaceP2>(), Projectile.damage, Projectile.knockBack, player.whoAmI, i);
                        }
                        timer = 0;
                    }
                }
                if (stillInUse && FrameCounter == 1f)
                {
                    FireBeams();
                }


                else if (!stillInUse)
                {
                    Projectile.Kill();
                }
            }


            Projectile.timeLeft = 2;
        }


        private void UpdateAnimation()
        {
        }

        private void PlaySounds()
        {

            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = SoundInterval;


                if (FrameCounter > 1f)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                }
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {

            Projectile.Center = playerHandPos;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;
            Projectile.spriteDirection = Projectile.direction;



            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - MathHelper.PiOver2);
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;


            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        private bool ShouldConsumeMana()
        {

            if (ManaConsumptionRate == 0f)
            {
                NextManaFrame = ManaConsumptionRate = MaxManaConsumptionDelay;
                return true;
            }


            bool consume = FrameCounter == NextManaFrame;


            if (consume)
            {

                ManaConsumptionRate = MathHelper.Clamp(ManaConsumptionRate - 1f, MinManaConsumptionDelay, MaxManaConsumptionDelay);
                NextManaFrame += ManaConsumptionRate;
            }
            return consume;
        }

        private void UpdateAim(Vector2 source, float speed)
        {

            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }


            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        private void FireBeams()
        {

            Vector2 beamVelocity = Vector2.Normalize(Projectile.velocity);
            if (beamVelocity.HasNaNs())
            {
                beamVelocity = -Vector2.UnitY;
            }


            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);

            int damage = Projectile.damage;
            float knockback = Projectile.knockBack;
            for (int b = 0; b < NumBeams; ++b)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, beamVelocity, ModContent.ProjectileType<PokerfacePBeam>(), damage, knockback, Projectile.owner, b, uuid);
            }


            Projectile.netUpdate = true;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 sheetInsertPosition = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
            Color drawColor = Color.White;
            Main.spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawColor, Projectile.rotation/* + (Projectile.spriteDirection == -1 ? 0 : MathHelper.PiOver2 * 3)*/, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, effects, 0f);
            return false;
        }
    }
}