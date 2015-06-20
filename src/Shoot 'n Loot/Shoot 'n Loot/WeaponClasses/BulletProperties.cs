using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class BulletProperties
    {
        public float Damage { get; private set; }
        public float Speed { get; private set; }
        public int MaxRange { get; private set; }

        public BulletProperties(float damage, float speed, int maxRange)
        {
            this.Damage = damage;
            this.Speed = speed;
            this.MaxRange = maxRange;
        }

        public Bullet GetBullet(Vector2 position, float angle)
        {
            return new Bullet(angle, position, this);
        }
    }
}
