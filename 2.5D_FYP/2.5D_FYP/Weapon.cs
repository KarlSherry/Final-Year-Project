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

            keyState = Keyboard.GetState();

            lastBulletFired += timeDelta;

            base.Update(gameTime);
        }

        public void CheckWeaponFire(int weaponIndex, Entity e)
        {
            player = game.Player;

            switch (weaponIndex)
            {
                case 0:
                    {
                        if (e is Player && player.keyState.IsKeyDown(Keys.Space))
                        {
                            if (!fireWeaponPressed)
                            {
                                FireBullet(e, e._pos, e._look);
                                fireWeaponPressed = true;
                            }
                        }
                        else fireWeaponPressed = false;

                        /*if (e is Enemy)
                        {
                            if (lastBulletFired >= 1.0f)
                            {
                                lastBulletFired = 0.0f;

                                FireBullet(e._pos, e._look);
                            }
                        }*/
                        break;
                    }                    

                case 1:
                    {
                        if (e is Player && player.keyState.IsKeyDown(Keys.Space) && lastBulletFired >= 0.1f) //10 milliseconds
                        {
                            lastBulletFired = 0.0f;

                            //FireBullet(game.PlayerBulletList, e._pos, e._look);
                        }
                        break;
                    }

                case 2: 
                    {
                        if (e is Player && player.keyState.IsKeyDown(Keys.Space) && lastBulletFired >= 3.0f)
                        {
                            lastBulletFired = 0.0f;

                           // FireBullet(game.PlayerBulletList, e._pos, e._look);
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
                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            if (!fireWeaponPressed)
                            {
                                //FireBullet(game.PlayerBulletList, player._pos, player._look);
                                fireWeaponPressed = true;
                            }
                        }
                        else fireWeaponPressed = false;
                        break;
                    }
            } //End of switch
        }

        public void FireBullet(Entity e, Vector3 pos, Vector3 look)
        {
            if(e is Player)
                bullet = new Bullet(game.PlayerBulletList);

            bullet._entityModel = "Models//sphere";
            bullet.LoadContent();
            //if (e is Enemy)
               // bullet = new Bullet(game.EnemyBulletList);
            bullet._pos = pos;
            bullet._look = look;
        }
    }
}
