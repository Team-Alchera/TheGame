﻿using System;
using Baligo.Console;
using Baligo.ConsoleDebugStats;
using Baligo.Graphics;
using Baligo.Main;
using Baligo.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baligo.Entity.Items.Weapons
{
    public class Arrow : Weapon
    {
        public const int Damage = 10;
        public const int Speed = 4;
        public int Id;

        public Rectangle CollisionBox;

        public Arrow(Vector2 position, Vector2 direction, int id, bool isEnemyArrow = false)
        {
            this.Id = id;
            // Set Parameters
            this.Position = position;
            this.Position.X += 16;
            this.Position.Y += 32;
            this.Direction = direction;

            // Calculate Velocity
            this.Velocity = CalculateVelocity();

            // Calculate Angle
            this.Angle = CalculateAngle(direction);

            // Create Collision
            this.CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 10, 10);

            // Set default active state and timer
            this.IsActive = true;
            this.Timer = 900;

            this.IsEnemyArrow = isEnemyArrow;
        }

        private float CalculateAngle(Vector2 direction)
        {
            var toCalcAngle = this.Position - direction;
            this.Angle = (float) Math.Atan2(toCalcAngle.Y, toCalcAngle.X);

            return this.Angle;
        }

        private Vector2 CalculateVelocity()
        {
            this.Velocity = -(this.Position - this.Direction);
            this.Velocity.Normalize();

            return this.Velocity;
        }

        public override void Update()
        {
            if (IsActive)
            {
                // Update position
                this.Position.X += this.Velocity.X / 100 * Speed;
                this.Position.Y += this.Velocity.Y / 100 * Speed;

                // Update Collision
<<<<<<< HEAD
                // CollisionBox.X = (int)Position.X;
                // CollisionBox.Y = (int)Position.Y - 5;
=======

                this.CollisionBox.X = (int)this.Position.X;
                this.CollisionBox.Y = (int)this.Position.Y - 5;
>>>>>>> b4d4a473237b70d6c9ab4334809a61b7feeb972a

                // Check Collision
                for (int row = 0; row < 24; row++)
                {
                    for (int col = 0; col < 43; col++)
                    {
                        Tile currentTile = WorldManager.GetCurrentWorld().WorldData[row, col];

                        if (currentTile.CollisionBox.Intersects(this.CollisionBox) && currentTile.IsSolid)
                        {
                            this.IsActive = false;
                            Statistics.TotalArrowsMissed++;
                        }
                    }
                }
            }
            else
            {
                if (this.Timer - 1 >= 0)
                    this.Timer--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw 
            if (IsEnemyArrow)
            {
                Assets.PlayerHunterArrow.DrawWithRotation(spriteBatch, (int)this.Position.X, (int)this.Position.Y, this.Angle, true);
            }
            else
            {
                Assets.PlayerHunterArrow.DrawWithRotation(spriteBatch, (int)this.Position.X, (int)this.Position.Y, this.Angle);
            }

           // Assets.Enemy.Draw(spriteBatch,(int)this.Position.X,(int)this.Position.Y);

            // Draw Arrow Collision if debug is active
            if (BaligoEngine.IsDebugModeActive)
            {
                spriteBatch.Draw(Assets.RedRectangle2.Texture, new Vector2(this.CollisionBox.X, this.CollisionBox.Y),
                    CollisionBox,
                    Color.White);
            }
        }
    }
}
