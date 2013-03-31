using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2._5D_FYP
{
    public class Weapon
    {
        float lastBulletFired = 0.0f;
        Bullet bullet;

        bool shootWeapon = false;

        public Weapon()
        {
        }

        public void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lastBulletFired += timeDelta;
        }

        public void CheckWeaponFire(int index, Entity e)
        {
            switch (index)
            {
                case 0:
                    {
                        if (e is Player && Game1.Instance().Player._fireWeapon)
                        {
                            if (!shootWeapon)
                            {
                                FireBullet(e, e._pos, e._look);
                                shootWeapon = true;
                            }
                        }
                        else shootWeapon = false;

                        if (e is Enemy)
                        {
                            if (lastBulletFired >= 1.0f)
                            {
                                lastBulletFired = 0.0f;
                                FireBullet(e, e._pos, e._look);
                            }
                        }
                        
                        break;
                    } 
                case 1:
                    {
                        if (e is Player && Game1.Instance().Player._fireWeapon && lastBulletFired >= 0.1f) //10 milliseconds
                        {
                            //bullet = new Bullet(game.PlayerBulletList);
                            //bullet.getBulletType(e, index);

                            lastBulletFired = 0.0f;
                            FireBullet(e, e._pos, e._look);
                        }
                        if (e is Enemy && lastBulletFired >= 0.1f) //10 milliseconds
                        {
                            //bullet = new Bullet(game.EnemyBulletList);
                            //bullet.getBulletType(e, index);

                            lastBulletFired = 0.0f;
                            FireBullet(e, e._pos, e._look);
                        }
                        break;
                    }
                case 2: 
                    {
                        if (e is Player && Game1.Instance().Player._fireWeapon && lastBulletFired >= 3.0f)
                        {
                            //bullet = new Bullet(game.PlayerBulletList);
                            //bullet.getBulletType(e, index);

                            lastBulletFired = 0.0f;
                            FireBullet(e, e._pos, e._look);
                        }
                        if (e is Enemy && lastBulletFired >= 3.0f)
                        {
                            //bullet = new Bullet(game.EnemyBulletList);
                            //bullet.getBulletType(e, index);

                            lastBulletFired = 0.0f;
                            FireBullet(e, e._pos, e._look);
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
                        if (e is Player && Game1.Instance().Player._fireWeapon)
                        {
                            if (!shootWeapon)
                            {
                                FireBullet(e, e._pos, e._look);
                                shootWeapon = true;
                            }
                        }
                        else shootWeapon = false;
                        break;
                    }
            } //End of switch
        }

        public void FireBullet(Entity e, Vector3 pos, Vector3 look)
        {
            if(e is Player)
                bullet = new Bullet(Game1.Instance().PlayerBulletList);
            if(e is Enemy)
                bullet = new Bullet(Game1.Instance().EnemyBulletList);
            bullet.getBulletType(e, e._weaponIndex);       
            bullet._pos = pos;
            bullet._look = look;
            bullet.LoadContent();
        }
    }
}
