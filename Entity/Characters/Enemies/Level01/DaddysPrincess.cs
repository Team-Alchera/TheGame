﻿using System.Collections.Generic;
using Baligo.Console;
using Baligo.ConsoleDebugStats;
using Baligo.Entity.Items.Weapons;
using Baligo.Graphics;
using Baligo.Main;
using Baligo.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baligo.Entity.Characters.Enemies.Level01
{
    public class DaddysPrincess : Enemy
    {
        // Constructor
        public DaddysPrincess(Vector2 position) : base(position)
        {
            // Set Parameters
            EnemyTexture = Assets.Enemy.Texture;
            Health = 100;
            Armor = 100;
            Damage = 10;
            IsAlive = true;
            Speed = 5;
            Story = "Daddy's princess looks cute, but she is very dangerous, when not satisfied. Her iPigeon" +
                    "communication device broke and she is unable to talk with her best friend the duchess" +
                    "Mutzka of Kifland. Give her something shiny or try to fight her anger off";


            // position
            ((Character) this).position = position;

            // Animation
            ShootArrowLeft = new Animation(SpeedOfAnimations, 17, 12);
            ShootArrowRight = new Animation(SpeedOfAnimations, 19, 12);
            ShootArrowUp = new Animation(SpeedOfAnimations, 16, 12);
            ShootArrowDown = new Animation(SpeedOfAnimations, 18, 12);

            // Orientation
            Orientation = new Rectangle(0, 64 * 11, 64, 64);

            // Collision
            CollisionBox = new Rectangle((int)((Character) this).position.X, (int)((Character) this).position.Y, 44, 54);

            // Set default arrow list
            Arrows = new List<Arrow>();
        }
        public string Story { get; set; }
        public string GetStory()
        {
            return this.Story;
        }
        public override void Init()
        {
        }

        private int countDown = 120;
        public override void Update(GameTime gmaTime)
        {
            // Update the collision box
            CollisionBox.X = (int)position.X + 10;
            CollisionBox.Y = (int)position.Y + 10;

            if (countDown == 0)
            {
                Vector2 PlayerPosition = MainGame.Player.CurrentPlayerClass.position;
                PlayerPosition.Y += 32;
                PlayerPosition.X += 16;

                Arrows.Add(new Arrow(position, PlayerPosition, Arrows.Count - 1, true));
                countDown = 120;
                BaligoConsole.WriteLine("Enemy arrow Spawned ID: " + Arrows.Count, Color.Tomato);
                Statistics.TotalArrowsFired++;
            }

            // Update each arrow
            for (int arrowId = 0; arrowId < Arrows.Count; arrowId++)
            {
                Rectangle playerCollision = MainGame.Player.CurrentPlayerClass.CollisionBox;

                if (Arrows[arrowId].Timer > 0)
                {
                    Arrows[arrowId].Update();
                    if (Arrows[arrowId].CollisionBox.Intersects(playerCollision) && Arrows[arrowId].IsActive)
                    {
                        MainGame.Player.CurrentPlayerClass.Health -= Damage;
                        BaligoConsole.WriteLine("Damage Taken 10 | Current Health: " +
                            MainGame.Player.CurrentPlayerClass.Health, Color.Thistle);

                        BaligoConsole.WriteLine("Arrow Removed ID: " + Arrows[arrowId].Id + " Player Collision!", Color.Tomato);
                        Arrows.RemoveAt(arrowId);
                    }
                }
                else
                {
                    BaligoConsole.WriteLine("Enemy arrow Removed ID: " + Arrows[arrowId].Id, Color.Tomato);
                    Arrows.RemoveAt(arrowId);
                }
            }

            // Check for collision
            for (int i = 0; i < MainGame.Player.HunterClass.Arrows.Count; i++)
            {
                var arrow = MainGame.Player.HunterClass.Arrows[i];

                if (arrow.CollisionBox.Intersects(CollisionBox))
                {
                    Health -= 25;
                    BaligoConsole.WriteLine("Enemy hit: 25 | Health: " + Health, Color.Cyan);

                    BaligoConsole.WriteLine("Player arrow Removed ID: " + i, Color.Red);
                    MainGame.Player.HunterClass.Arrows.RemoveAt(i);
                }
            }

            if (countDown - 1 >= 0)
            {
                countDown--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EnemyTexture, position, Orientation, Color.White);

            foreach (var arrow in Arrows)
            {
                arrow.Draw(spriteBatch);
            }

            if (BaligoEngine.IsDebugModeActive)
                spriteBatch.Draw(Assets.RedRectangle1.Texture, new Vector2(CollisionBox.X, CollisionBox.Y), CollisionBox, Color.White);
        }
    }
}
