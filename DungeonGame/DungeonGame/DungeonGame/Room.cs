using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Room
    {
        public enum Doors 
        {
            Left,
            Up,
            Right,
            Down
        }
        public int[] doors;
        Generation generation = new Generation();
        List<GameObject> gameObject = new List<GameObject>();
        List<Tile> tiles = new List<Tile>();
        List<LockedDoor> lockedDoors = new List<LockedDoor>();
        List<Drop> drops = new List<Drop>();
        List<Enemy> enemies = new List<Enemy>();
        List<Ghost> ghosts = new List<Ghost>();
        List<Bomb> bombs = new List<Bomb>();
        Random random = new Random();
        List<Explosion> explosions = new List<Explosion>();
        public List<Projectile> blubaBall = new List<Projectile>();
        Color color = Color.White;
        Random rnd = new Random();
        ContentManager Content;
        Game1 game;
        Vector2 roomPosition;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition, int[] doors, sbyte fromRoom)
        {
            this.game = game;
            this.Content = Content;
            this.roomPosition = roomPosition;
            this.doors = doors;
            int randomValue = random.Next(1, 5);
            if (randomValue == 1)
                color = Color.Red;
            else if (randomValue == 2)
                color = Color.Turquoise;
            else if (randomValue == 3)
                color = Color.Yellow;
            else if (randomValue == 4)
                color = new Color(random.Next(1, 255), random.Next(1, 255), random.Next(1, 255));
            generation.Generate(Content, tiles, "map");
            #region CreateDoorOrWall
            if (doors[0] > 0)
            {
                tiles.Add(new Tile(generation.doorLeft, new Vector2(50, 300), 2));
                if (fromRoom != 2)
                tiles.Add(new LockedDoor(new Vector2(50, 300), Content,5));
            }
            else
                tiles.Add(new Tile(generation.wallLeft, new Vector2(50, 300), 3));

            if (doors[1] > 0)
            {
                tiles.Add(new Tile(generation.doorUp, new Vector2(450, 50), 2));
                if (fromRoom != 1)
                tiles.Add(new LockedDoor(new Vector2(450, 50), Content,6));
            }
            else
                tiles.Add(new Tile(generation.wallUp, new Vector2(450, 50), 3));

            if (doors[2] > 0)
            {
                tiles.Add(new Tile(generation.doorRight, new Vector2(850, 300), 2));
                if (fromRoom != 0)
                tiles.Add(new LockedDoor(new Vector2(850, 300), Content,7));
            }
            else
                tiles.Add(new Tile(generation.wallRight, new Vector2(850, 300), 3));

            if (doors[3] > 0)
            {
                tiles.Add(new Tile(generation.doorDown, new Vector2(450, 550), 2));
                if (fromRoom != 3)
                tiles.Add(new LockedDoor(new Vector2(450, 550), Content,8));
            }
            else
                tiles.Add(new Tile(generation.wallDown, new Vector2(450, 550), 3));
            #endregion

            //for (int i = 0; i < tiles.Count; i++)
            //{
            //    if (rnd.Next(-5, 5) == 2 && tiles[i].type == 1)
            //        tiles.Add(new Tile(Content.Load<Texture2D>("rock"), tiles[i].position, 4));
            //}
            for (int i = 0; i < spawn.Count; i++)
            {
                for (int j = 0; j < spawn[i].Item2; j++)
                    enemies.Add(CreateMob(spawn[i].Item1));
            }
        }

        public void Update(GameTime gameTime,Player player)
        {
            #region doorfunction
            if (player.position.X < 0) 
            {
                ExistOrCreate(Doors.Left);
                player.position.X = 50 * 18;
                explosions.Clear();
                blubaBall.Clear();
            }

            if (player.position.X > (50 * 18))
            {
                ExistOrCreate(Doors.Right);
                player.position.X = 0;
                explosions.Clear();
                blubaBall.Clear();
            }

            if (player.position.Y < -20) 
            {
                ExistOrCreate(Doors.Down);
                player.position.Y = 50 * 11;
                explosions.Clear();
                blubaBall.Clear();
            }

            if (player.position.Y > (50 * 11)) 
            {
                ExistOrCreate(Doors.Up);
                player.position.Y = -20;
                explosions.Clear();
                blubaBall.Clear();
            }
            #endregion
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].isdead)
                {
                    ghosts.Add(new Ghost(new Animation(Content, "ghost", 100, 2, true), enemies[i].position));
                    enemies.RemoveAt(i);
                }
            }
            for (int i = 0; i < bombs.Count; i++)
            {
                if (bombs[i].willExplode)
                {
                    explosions.Add(new Explosion(new Vector2(bombs[i].position.X- 65,bombs[i].position.Y- 65), new Animation(Content, "explosion", 200, 4, false)));
                    bombs.RemoveAt(i);
                }
            }
            for (int i = 0; i < explosions.Count; i++)
            {
                if (explosions[i].Animation.currentFrame == 3)
                    explosions.RemoveAt(i);
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].isDeleted)
                    tiles.RemoveAt(i);
            }
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (explosions[i].HitBox.Intersects(tiles[j].hitBox))
                        if (tiles[j].type == 4)
                            tiles.RemoveAt(j);
                }
            }
            for (int i = 0; i < ghosts.Count; i++)
            {
                if (ghosts[i].isdead)
                    ghosts.RemoveAt(i);
            }
            foreach (Ghost g in ghosts)
            {
                g.Update(gameTime);
            }
            foreach (Bomb b in bombs)
            {
                b.Update(gameTime);
            }
                foreach (Projectile p in blubaBall)
            {
                p.Update();
            }
            foreach (Tile t in tiles)
            {
                t.Update(gameTime,player);
            }
            foreach (Drop d in drops)
            {
                d.Update(gameTime);
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(tiles, gameTime, this,player);
            }
            player.Update(gameTime, tiles, enemies, Content,drops, bombs);
        }

        public void Draw(SpriteBatch spriteBatch,Player player,GameTime gameTime)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch,color);
            }
            foreach (Explosion e in explosions)
                e.Draw(spriteBatch);
            for (int i = 0; i < blubaBall.Count; i++)
            {
                if (blubaBall[i].hitBox.Intersects(player.hitBox))
                {
                    player.health--;
                    blubaBall.RemoveAt(i);
                    player.isHurt = true;
                }
            }
            foreach (Ghost g in ghosts)
            {
                g.Draw(spriteBatch);
            }
            foreach (Drop d in drops)
            {
                d.Draw(spriteBatch);
            }
            foreach (Projectile p in blubaBall)
            {
                p.Draw(spriteBatch);
            }
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }

            foreach (Bomb b in bombs)
            {
                b.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            foreach (LockedDoor l in lockedDoors)
                l.Draw(spriteBatch,color);
        }

        private Enemy CreateMob(String mob)
        {
            switch(mob)
            {
                case "bat":
                    return new Bat(Content, rnd.Next(), new Vector2(rnd.Next(100,700), rnd.Next(100, 450)));
                case "bluba":
                    return new Bluba(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "blubaTower":
                    return new BlubaTower(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "slime":
                    return new Slime(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                default:
                    return null;
            }
        }

        private void ExistOrCreate(Doors side)
        {
            Vector2 nextRoom = new Vector2(roomPosition.X, roomPosition.Y);
            switch (side)
            {
                case Doors.Down:
                    nextRoom.Y--;
                    break;
                case Doors.Up:
                    nextRoom.Y++;
                    break;
                case Doors.Left:
                    nextRoom.X--;
                    break;
                case Doors.Right:
                    nextRoom.X++;
                    break;
            }

            if (!game.RoomExists(nextRoom))
            {
                int[] doors = {0};
                sbyte fromRoom = 0;
                switch (side)
                {
                    case Doors.Left:
                        fromRoom = 0;
                        doors = new int[] { rnd.Next(0, 2), rnd.Next(0, 2), 1, rnd.Next(0, 2) };
                        break;
                    case Doors.Up:
                        fromRoom = 1;
                        doors = new int[] { rnd.Next(0, 2), 1, rnd.Next(0, 2), rnd.Next(0, 2) };
                        break;
                    case Doors.Right:
                        fromRoom = 2;
                        doors = new int[] { 1, rnd.Next(0, 2), rnd.Next(0, 2), rnd.Next(0, 2) };
                        break;
                    case Doors.Down:
                        fromRoom = 3;
                        doors = new int[] { rnd.Next(0, 2), rnd.Next(0, 2), rnd.Next(0, 2), 1 };
                        break;

                }
                game.CreateRoom(nextRoom, doors, fromRoom);
            }

            game.SetCurrentRoom(nextRoom);
        }
    }
}
