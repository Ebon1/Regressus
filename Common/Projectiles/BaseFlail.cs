using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;

namespace Regressus.Common.Projectiles
{
    //Abstract classes are useful when you're creating a lot of classes that are very similar, such as a type of projectile or NPC
    //You can't directly refer to them when using the projectile elsewhere, so you need to create other classes that inherit some properties while setting others.
    public abstract class FlailProj : ModProjectile
    {
        //These variables are only referred to in this class, so they shouldn't be overridden.
        private readonly string tex;
        private readonly int hitbox;
        private readonly int throwrange;
        private readonly float throwspeed;
        private readonly float retractdist;
        private readonly float retractspeed;
        private readonly int heldbonus;
        //This line is the main thing that will be overridden in other classes. You can also set default values for variables that don't always need to be changed
        public FlailProj(string texture, int size, int range, float speed, float returnrange = 800f, float returnspeed = 3f, int heldlengthbonus = 5)
        {
            tex = texture;                  //What's the path of the flail's texture?
            hitbox = size;                  //How big is the projectile?
            throwrange = range;             //How far can the flail extend?
            throwspeed = speed;             //How fast is it thrown?
            retractdist = returnrange;      //When is it forced to retract?
            retractspeed = returnspeed;     //How fast does it retract?
            heldbonus = heldlengthbonus;    //How much further can the flail travel when being held out by the player?
        }
        //Set the texture to our asset path plus the given texture path. Also take note of the sealed modifier, meaning our other classes cannot override this.
        public sealed override string Texture => "Regressus/Projectiles/Melee/" + tex;
        public sealed override void SetDefaults()
        {
            Projectile.width = hitbox;
            Projectile.height = hitbox;
            Projectile.friendly = true;
            //We don't want our flail to break on the first enemy, so we set it to pierce indefinetly.
            Projectile.penetrate = -1;
            ExtraDefaults();
        }
        public virtual void ExtraDefaults() { }
        /// <summary>
        /// The sound the flail will make upon impact
        /// </summary>
        public virtual SoundStyle FlailSound => SoundID.NPCHit1;
        public sealed override void AI()
        {
            ExtraAI();
            //These variables are VERY important.
            Player player = Main.player[Projectile.owner];
            Vector2 mountedCenter = player.MountedCenter;

            //This section kills the projectile if the player is dead, can't use it or is in the map.
            if (!player.active || player.dead || player.noItems || player.CCed)
            {
                Projectile.Kill();
                return;
            }
            if (Main.myPlayer == Projectile.owner && Main.mapFullscreen)
            {
                Projectile.Kill();
                return;
            }

            //This specifies if this flail rotates when it's thrown out and has enough velocity
            bool angularvelocity = true;
            //This tell the flail when to check if there's a wall between the player and the target.
            bool hitcheck = false;
            //This is the range where the projectile gets killed after the initial throw. This and the following variables generally shouldn't change from flail to flail.
            float retractkillrange = 16f;
            //This is the forced retract speed, for when the flail gets stuck or out of range.
            float forceretract = retractspeed * 2;
            //This is the range where the projectile gets killed after being forced to retract.
            float forceretractkillrange = 48f;
            //This is how long the chain is allowed to be when the flail is being held
            int heldlength = throwrange + heldbonus;

            //This section alters things like speed and range based on the player's melee speed
            float speedmultiplier = 1f / player.GetAttackSpeed(DamageClass.Melee);
            //Since our throwspeed variable is a readonly, we have to set another variable equal to it if we want to modify it.
            float firespeed = throwspeed;
            firespeed *= speedmultiplier;
            //Ditto
            float pullbackspeed = retractspeed;
            pullbackspeed *= speedmultiplier;
            retractkillrange *= speedmultiplier;
            forceretract *= speedmultiplier;
            forceretractkillrange *= speedmultiplier;
            //This defines when the flail retracts while it's being held out
            float heldretractrange = (firespeed * throwrange) + 160f;
            Projectile.localNPCHitCooldown = 10;

            //This section is where the flail decides what it's behavior is between spinning, being thrown, retracting, being held out, and forcibly retracting.
            //Switch blocks are useful for this sort of multi-behavior.
            switch ((int)Projectile.ai[0])
            {
                //The flail is spinning.
                case 0:
                    {
                        //Adding in voids like this helps declutter your code, and allows for differing effects when using abstract classes.
                        //For example, if I wanted my flail to fire projectiles while it was spinning (akin to the Flower Pow), I would override the OnSpinning() void and add that behavior in.
                        OnSpinning();
                        //We don't want the flail attacking through walls, at least not in this example.
                        hitcheck = true;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Vector2 origin = mountedCenter;

                            //While the player is spinning the flail, they should face the mouse.
                            Vector2 mouseWorld = Main.MouseWorld;
                            Vector2 throwdir = origin.DirectionTo(mouseWorld).SafeNormalize(Vector2.UnitX * player.direction);
                            player.ChangeDir((throwdir.X > 0f) ? 1 : (-1));

                            //If the player stops channeling, throw the flail.
                            if (!player.channel)
                            {
                                Projectile.ai[0] = 1f;
                                Projectile.ai[1] = 0f;
                                Projectile.velocity = throwdir * throwspeed + player.velocity;
                                Projectile.Center = mountedCenter;
                                Projectile.netUpdate = true;
                                Projectile.localNPCHitCooldown = 10;
                                break;
                            }
                        }

                        //This part creates the spinning motion.
                        Projectile.localAI[1] += 1f;
                        Vector2 spinpath = new Vector2(player.direction).RotatedBy((float)Math.PI * 10f * (Projectile.localAI[1] / 60f) * player.direction);
                        //Flails spin in an oval, not a circle.
                        spinpath.Y *= 0.8f;
                        //Flip with gravity.
                        if (spinpath.Y * player.gravDir > 0f)
                        {
                            spinpath.Y *= 0.5f;
                        }
                        Projectile.Center = mountedCenter + spinpath * 30f;
                        Projectile.velocity = Vector2.Zero;
                        //Vanilla flails hit less often when they're spinning.
                        Projectile.localNPCHitCooldown = 20;
                        break;
                    }
                //The flail is extending outward.
                case 1:
                    {
                        OnExtending();
                        //This part makes the flail return to the player if it reaches it's maximum length.
                        bool peak = Projectile.ai[1]++ >= throwrange;
                        peak |= Projectile.Distance(mountedCenter) >= retractdist;
                        if (peak)
                        {
                            AtPeak();
                            Projectile.ai[0] = 2f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.3f;
                        }
                        //Vanilla flails allow the player to drop the flail to the ground after they're thrown, so we do that here.
                        if (player.controlUseItem)
                        {
                            Projectile.ai[0] = 5f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.2f;
                            break;
                        }
                        //Now the player turns to face the flail rather than the mouse.
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        //We can set the hit rate back to normal.
                        Projectile.localNPCHitCooldown = 10;
                        break;
                    }
                //The flail is retracting normally, the player can interupt to drop the flail.
                case 2:
                    {
                        OnRetracting();
                        //Point the flail back towards the player to retract.
                        Vector2 dirtoplayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        //If the distance between the player and the flail gets too great, kill the flail.
                        if (Projectile.Distance(mountedCenter) <= retractkillrange)
                        {
                            Projectile.Kill();
                            return;
                        }
                        //The player can still drop the flail while it's retracting.
                        if (player.controlUseItem)
                        {
                            Projectile.ai[0] = 5f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                            Projectile.velocity *= 0.2f;
                        }
                        //However, if they don't interupt then the projectile will continue retracting toward the player.
                        else
                        {
                            Projectile.velocity *= 0.98f;
                            Projectile.velocity = Projectile.velocity.MoveTowards(dirtoplayer * retractkillrange, pullbackspeed);
                            player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        }
                        break;
                    }
                //The flail is being forced to retract, the player can`t interupt.
                case 3:
                    {
                        //This section is mostly similar to above, without the player interuption nor tile collision
                        OnRetracting();
                        Projectile.tileCollide = false;
                        Vector2 dirtoplayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mountedCenter) <= forceretractkillrange)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.velocity *= 0.98f;
                        Projectile.velocity = Projectile.velocity.MoveTowards(dirtoplayer * forceretractkillrange, forceretract);
                        Vector2 target = Projectile.Center + Projectile.velocity;
                        Vector2 value = mountedCenter.DirectionFrom(target).SafeNormalize(Vector2.Zero);
                        if (Vector2.Dot(dirtoplayer, value) < 0f)
                        {
                            Projectile.Kill();
                            return;
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        break;
                    }
                //The flail is floating, right after hitting a tile.
                case 4:
                    {
                        if (Projectile.ai[1]++ >= heldlength)
                        {
                            Projectile.ai[0] = 5f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            Projectile.localNPCHitCooldown = 10;
                            Projectile.velocity.Y += 0.6f;
                            Projectile.velocity.X *= 0.95f;
                            player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        }
                        break;
                    }
                //The flail is being held on the ground by the player.
                case 5:
                    {
                        OnGround();
                        //If the player stops channeling the item or the distance between them and the flail gets too great, the flail forcibly retracts.
                        if (!player.controlUseItem || Projectile.Distance(mountedCenter) > heldretractrange)
                        {
                            Projectile.ai[0] = 3f;
                            Projectile.ai[1] = 0f;
                            Projectile.netUpdate = true;
                        }
                        //Otherwise, it is continually affected by heavy gravity and points the player toward itself.
                        else
                        {
                            Projectile.velocity.Y += 0.8f;
                            Projectile.velocity.X *= 0.95f;
                            player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
                        }
                        break;
                    }
            }

            //These make sure our knockback doesn't knock an enemy right into the player
            Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
            Projectile.spriteDirection = Projectile.direction;

            //The flail only checks for barriers if it is spinning.
            Projectile.ownerHitCheck = hitcheck;

            //Spin the flail if it's traveling out fast enough.
            if (angularvelocity)
            {
                if (Projectile.velocity.Length() > 1f)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + Projectile.velocity.X * 0.1f;
                }
                else
                {
                    Projectile.rotation += Projectile.velocity.X * 0.1f;
                }
            }

            //Normal channeled item things.
            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            player.itemRotation = Projectile.DirectionFrom(mountedCenter).ToRotation();

            //This creates the swinging animation on the player while it is spinning
            if (Projectile.Center.X < mountedCenter.X)
            {
                player.itemRotation += (float)Math.PI;
            }
            player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
        }
        //Regions are useful for organizing your code, and condensing parts of it when you don't need to see it at the moment.
        #region Overrides
        //All of these are spots for our other classes to add in code.
        //Summary:
        /// <summary>
        /// This will always happen while the flail is active. Refer to BaseFlail for more info about this flail AI.
        /// </summary>
        public virtual void ExtraAI() { }
        //Summary:
        /// <summary>
        /// This will trigger while the flail is being initially spun. Could be used to spawning projectiles, like the Flower Pow.
        /// </summary>
        public virtual void OnSpinning() { }
        //Summary:
        /// <summary>
        /// This will trigger while the flail is extending outward from the initial throw. Could be used for spawning projectiles, like the Flailron.
        /// </summary>
        public virtual void OnExtending() { }
        //Summary:
        /// <summary>
        /// This will trigger when the flail reaches the peak of it's throw initial throw. Could be used for spawning projectiles, like the Drippler Crippler.
        /// </summary>
        public virtual void AtPeak() { }
        //Summary:
        /// <summary>
        /// This will trigger while the flail is being retracted, either from the initial throw or while being forced back.
        /// </summary>
        public virtual void OnRetracting() { }
        //Summary:
        /// <summary>
        /// This will trigger while the flail is being held out on the ground by the player.
        /// </summary>
        public virtual void OnGround() { }
        //Summary:
        /// <summary>
        /// Note that this only triggers if when the flail is initially thrown out. Override OnGround() if you want the flail to do something while it's on the ground
        /// </summary>
        public virtual void ExtraTileCollide(Vector2 oldvel) { }
        #endregion
        public sealed override bool OnTileCollide(Vector2 oldVelocity)
        {

            int impactspeed = 0;
            Vector2 velocity = Projectile.velocity;
            float slowdown = 0.2f;
            //Slow down more if the flail is being thrown or is floating
            if (Projectile.ai[0] == 1f || Projectile.ai[0] == 4f)
            {
                slowdown = 0.4f;
            }

            //Don't slow down if the flail is on the ground, we have other code for that.
            if (Projectile.ai[0] == 5f)
            {
                slowdown = 0f;
            }

            //Both of these handle the rebounding effect that flails have when hitting walls.
            if (oldVelocity.X != Projectile.velocity.X)
            {
                if (Math.Abs(oldVelocity.X) > 4f)
                {
                    impactspeed = 1;
                }
                Projectile.velocity.X = (0f - oldVelocity.X) * slowdown;
                Projectile.localAI[0] += 1f;
            }
            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                if (Math.Abs(oldVelocity.Y) > 4f)
                {
                    impactspeed = 1;
                }
                Projectile.velocity.Y = (0f - oldVelocity.Y) * slowdown;
                Projectile.localAI[0] += 1f;
            }

            //Forcibly retract the flail if it hits a tile on the initial throw.
            if (Projectile.ai[0] == 1f)
            {
                ExtraTileCollide(oldVelocity);
                Projectile.ai[0] = 4f;
                Projectile.localNPCHitCooldown = 10;
                Projectile.netUpdate = true;
                impactspeed = 2;
                Projectile.position -= velocity;
            }

            //If the flail hits a tile with enough force, update it and play a sound.
            if (impactspeed > 0)
            {
                Projectile.netUpdate = true;
                for (int i = 0; i < impactspeed; i++)
                {
                    Collision.HitTiles(Projectile.position, velocity, Projectile.width, Projectile.height);
                }
                SoundEngine.PlaySound(FlailSound, Projectile.position);
            }

            //If the flail isn't already being forcibly retracted or being held, pull it in.
            if (Projectile.ai[0] != 3f && Projectile.ai[0] != 0f && Projectile.ai[0] != 5f && Projectile.ai[0] != 6f && Projectile.localAI[0] >= 10f)
            {
                Projectile.ai[0] = 3f;
                Projectile.netUpdate = true;
            }

            //Vanilla tile collide code would kill the flail when it touches a tile, so return false.
            return false;
        }
        #region Drawcode
        private static Asset<Texture2D> chainTexture;
        public sealed override void Load()
        {
            //It's a good habit to not name related textures comepletely different things, so we will automatically detect a texture with the same path as our main flail, with Chain added at the end.
            chainTexture = ModContent.Request<Texture2D>(Texture + "Chain");
            //For any extra assets related to this flail.
            ExtraLoads();
        }
        public virtual void ExtraLoads() { }
        //Unloading our textures when the mod unloaded ensures errors won't occur and ensures our mod doesn't take up too much memory even when unloaded.
        public sealed override void Unload()
        {
            chainTexture = null;
            ExtraUnloads();
        }
        public virtual void ExtraUnloads() { }
        //Chain drawing is a complex process. Content/Items/Tools/ExampleHook has some more explanation.
        public sealed override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
            float distanceToPlayer = directionToPlayer.Length();
            while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer))
            {
                directionToPlayer /= distanceToPlayer;
                directionToPlayer *= chainTexture.Height();
                center += directionToPlayer;
                directionToPlayer = playerCenter - center;
                distanceToPlayer = directionToPlayer.Length();
                Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));
                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition, chainTexture.Value.Bounds, drawColor, chainRotation, chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            ExtraExtraPreDraws();
            return false;
        }
        public virtual void ExtraExtraPreDraws() { }
        #endregion
    }
}