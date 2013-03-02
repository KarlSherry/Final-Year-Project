using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2._5D_FYP
{
    public class Weapon : Entity
    {
        int weaponIndex = 0;
        float lastBulletFired = 0.0f;
        public Bullet bullet;
        Player player;

        KeyboardState keyState;
        bool fireWeaponPressed = false;

        public Weapon()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lastBulletFired += timeDelta;

            base.Update(gameTime);
        }

        public void CheckWeaponFire(int index, Entity e)
        {
            player = Game1.Instance().Player;
            bullet = new Bullet();

            switch (index)
            {
                case 0:
                    {
                        bullet._entityModelName = "Models//sphere";

                        if (e is Player && e.keyPressed)
                        {
                            if (!fireWeaponPressed)
                            {
                                FireBullet(e._pos, e._look);
                                fireWeaponPressed = true;
                            }
                        }                            
                        else fireWeaponPressed = false;

                        if (e is Enemy)
                        {
                            if (lastBulletFired >= 1.0f)
                            {
                                lastBulletFired = 0.0f;

                                FireBullet(e._pos, e._look);
                            }
                        }
                        break;
                    }
                    

                case 1:
                    {
                        bullet._entityModelName = "Models//sphere";

                        if (keyState.IsKeyDown(Keys.Space) && lastBulletFired >= 0.1f) //10 milliseconds
                        {
                            lastBulletFired = 0.0f;

                            FireBullet(player._pos, player._look);
                        }
                        break;
                    }

                case 2: 
                    {
                        bullet._entityModelName = "Models//Station";

                        if (keyState.IsKeyDown(Keys.Space) && lastBulletFired >= 3.0f)
                        {
                            lastBulletFired = 0.0f;

                            FireBullet(player._pos, player._look);
                        }
                        break;
                    }

                case 3:
                    {
                        //bullet._entityName = "Models//sphere";
                        break;
                    }

                default: //Assumes case 0
                    {
                        bullet._entityModelName = "Models//sphere";

                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            if (!fireWeaponPressed)
                            {
                                FireBullet(player._pos, player._look);
                                fireWeaponPressed = true;
                            }
                        }
                        else fireWeaponPressed = false;
                        break;
                    }
            } //End of switch
        }

        public void FireBullet(Vector3 pos, Vector3 look)
        {
            bullet.LoadContent();
            bullet._pos = pos;
            bullet._look = look;

            Game1.Instance().Children.Add(bullet);
        }
    }
}
