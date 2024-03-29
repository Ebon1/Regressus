﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Regressus.Projectiles.EoC
{
    public class TestCurveProj : ModProjectile
    {

        private List<Vector2> telegraphPositions;
        private Vector2 originalCenter;
        private Vector2 originalVelocity;

        private bool runOnce = true;
        private bool[] ignoreTiles;

        private float Direction1 = 0;
        private float Direction2 = 0;
        private float maxTime = 480;

        public ref float AICounter => ref Projectile.ai[0];
        public override bool ShouldUpdatePosition() => false;
        public override string Texture => "Regressus/Extras/Circle";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test");
        }
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 24;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)maxTime;
        }

        public override void OnSpawn(IEntitySource source)
        {
            telegraphPositions = new List<Vector2>();
            base.OnSpawn(source);
        }

        public override void AI()
        {
            SimulateMovement(4);
            SimulateMovement(-1);

            AICounter++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public void SimulateMovement(int updates = 1)
        {
            if (runOnce)
            {
                Direction1 = Projectile.ai[1];
                Direction2 = Direction1;

                // Initialize the tiles that should be ignored when calculating the movement
                // Set the tile type to false if you want to recalculate the movement if it "hits" a block
                if (ignoreTiles == null)
                {
                    ignoreTiles = new bool[Main.tileSolid.Length];
                    for (int i = 0; i < ignoreTiles.Length; i++)
                    {
                        ignoreTiles[i] = true;
                    }
                }

                originalCenter = Projectile.Center;
                originalVelocity = Projectile.velocity;
                runOnce = false;
            }

            float veloMult = 1.01f;
            float rotateMod = 0.9f * 2;
            for (int i = 0; i < updates; i++)
            {
                if (telegraphPositions.Count >= Projectile.timeLeft) break;

                Vector2 initialVelocity = originalVelocity;
                originalVelocity = Collision.AdvancedTileCollision(ignoreTiles, originalCenter - new Vector2(4, 4), originalVelocity, 8, 8, true, true);
                originalCenter += originalVelocity;

                if (originalVelocity != initialVelocity)
                {
                    originalVelocity = -initialVelocity;
                    Direction1 = -Direction1;
                }

                // Cap the interval for acceleration
                if (AICounter < 420)
                    originalVelocity *= veloMult;

                originalVelocity = originalVelocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction1));
                telegraphPositions.Add(originalCenter);
            }

            // Setting it to -1 moves the actual projectile
            if (updates == -1)
            {
                Vector2 initialVelocity = Projectile.velocity;
                Projectile.velocity = Collision.AdvancedTileCollision(ignoreTiles, Projectile.Center - new Vector2(4, 4), Projectile.velocity, 8, 8, true, true);
                Projectile.Center += Projectile.velocity;

                if (Projectile.velocity != initialVelocity)
                {
                    Projectile.velocity = -initialVelocity;
                    Direction2 = -Direction2;
                }

                Projectile.velocity *= veloMult;
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction2));
            }
            else if (telegraphPositions.Count >= 1)
                telegraphPositions.RemoveAt(0);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (telegraphPositions.Count > 1)
            {
                Vector2 from = telegraphPositions[0];
                for (int i = 1; i < telegraphPositions.Count; i++)
                {
                    float alphaMult = 1.1f - (0.5f * AICounter / maxTime) - ((float)(i - 1) / telegraphPositions.Count);
                    if (alphaMult > 1) alphaMult = 1;

                    Vector2 to = telegraphPositions[i];
                    Vector2 toPos = from - to;
                    float rotation = toPos.ToRotation();
                    Texture2D texture = RegreUtils.GetExtraTexture("Line");
                    Main.EntitySpriteDraw(texture, from - Main.screenPosition, null, Color.Cyan * alphaMult, rotation, new Vector2(1, 1), 1f, SpriteEffects.None, 0);
                    from = to;
                }
            }

            return base.PreDraw(ref lightColor);
        }
    }
}
