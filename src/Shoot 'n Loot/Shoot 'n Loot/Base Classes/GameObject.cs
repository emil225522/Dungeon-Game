using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class GameObject
    {
        public enum Direction { Up = 0, Down = 1, Left = 2, Right = 3 } //number corresponds to index in array in texturemanager

        Direction direction;

        private float health;
        private float maxHealth;

        public const string TYPE = "GameObject";
        public virtual string Type { get { return TYPE;  } }

        public Vector2 Position { get { return Sprite.Position; } set { Sprite.Position = value; } }
        public Vector2 Size { get { return Sprite.Size; } protected set { Sprite.Size = value; } }
        public Vector2 Center { get { return new Vector2(MapCollider.Center.X, MapCollider.Center.Y); } }
        public virtual Rectangle MapCollider { get { return Sprite.Area; } }
        public virtual Rectangle BulletCollider { get { return Sprite.Area; } }
        
        public bool Dead { get; set; }
        public float Health { get { return health; } set 
        {
            OnTakeDamage(health - value);
            health = value;
            if (health > maxHealth) health = maxHealth; 
            if (health <= 0 && CanDie) 
            { 
                Dead = true; OnDestroy(); 
            } 
        } }
        public float MaxHealth { get { return maxHealth; } set { maxHealth = health = value; } }
        public bool CanDie { get; set; }
        
        public bool ObstructsBullets { get; set; }

        protected Vector2 Velocity { get; set; }
        
        public Sprite Sprite { get; set; }

        protected bool CollidedOnX { get; private set; }
        protected bool CollidedOnY { get; private set; }

        public Direction VelDirection
        {
            get
            {
                //todo: implement some "old direction" and only update if moving
                if (Velocity.LengthSquared() > 0)
                {
                    if (Math.Abs(Velocity.X) > Math.Abs(Velocity.Y))
                    {
                        //left and right movement
                        if (Velocity.X > 0) direction = Direction.Right;
                        else if (Velocity.X < 0) direction = Direction.Left;
                    }
                    else
                    {
                        if (Velocity.Y > 0) direction = Direction.Down;
                        else if (Velocity.Y < 0) direction = Direction.Up;
                    }
                }
                return direction;
            }
        }


        /// <summary>
        /// returns a list of all tiles on the map within 100 pixels.
        /// </summary>
        protected List<Tile> CloseSolidTiles
        {
            get
            {
                List<Tile> solidTiles = new List<Tile>();

                foreach (Chunk c in Map.VisibleChunks)
                {
                    foreach (Tile t in c.NonWalkableTiles())
                    {
                        solidTiles.Add(t);
                    }
                }

                for (int i = solidTiles.Count - 1; i >= 0; i--)
                {
                    if (DistanceSquared(solidTiles[i].Position) > 10000) solidTiles.RemoveAt(i);
                }

                return solidTiles;
            }
        }

        /// <summary>
        /// should be overridden by child object
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// draws the sprite. override and draw sprite manually or call base.Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

            Sprite.LayerDepth = .9999f - ((float)Sprite.Area.Bottom / (Map.height * Chunk.sizePx)); //draws things closer to camera on top
            Sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// draws the sprite. override and draw sprite manually or call base.Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch, bool dontAdjust)
        {
            
            if (!dontAdjust) Sprite.LayerDepth = .9999f - ((float)Sprite.Area.Bottom / (Map.height * Chunk.sizePx)); //draws things closer to camera on top
            Sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// returns the distance squared to the specified point on the map.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float DistanceSquared(Vector2 target)
        {
            return (target - Position).LengthSquared();
        }


        /// <summary>
        /// adds the specified values to this.Position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void Move(float x, float y)
        {
            Position += new Vector2(x, y);
        }

        /// <summary>
        /// move using this.velocity, colliding with tiles if specified. CollidedOnX and CollidedOnY specify if the path was open.
        /// </summary>
        /// <param name="tileColission"></param>
        protected void Move(bool tileColission)
        {
            if (tileColission) MoveWithTileCollision();
            else Move(Velocity.X, Velocity.Y);
        }

        /// <summary>
        /// is called when the object dies.
        /// </summary>
        protected virtual void OnDestroy() { }


        /// <summary>
        /// checks each tile in the specified list and if the this.Hitbox collides with any of them, true is returned, otherwise false.
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        protected bool IsCollidingWithAny(List<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                if (t.Hitbox.Intersects(MapCollider)) return true;
            }
            return false;
        }

        protected virtual void OnTakeDamage(float amount) { }

        /// <summary>
        /// moves the object on the map using this.Velocity, colliding with tiles and setting the velocity to 0 in the necessary axees if it hits something. Uses this.hitbox, which can be overridden.
        /// </summary>
        private void MoveWithTileCollision()
        {
            CollidedOnX = CollidedOnY = false;

            List<Tile> solidTiles = CloseSolidTiles;

            Move(Velocity.X, 0);
            int x = Velocity.X.CompareTo(0);
            int _x = 0;
            while (IsCollidingWithAny(solidTiles) && _x <= Math.Abs(Velocity.X) + 1)
            {
                Move(-x, 0);
                Velocity = new Vector2(0, Velocity.Y);
                CollidedOnX = true;
                //_x++;
            }

            Move(0, Velocity.Y);
            int y = Velocity.Y.CompareTo(0);
            int _y = 0;
            while (IsCollidingWithAny(solidTiles) && _y <= Math.Abs(Velocity.Y) + 1)
            {
                Move(0, -y);
                Velocity = new Vector2(Velocity.X, 0);
                CollidedOnY = true;
                //_y++;
            }
        }
    }
}
