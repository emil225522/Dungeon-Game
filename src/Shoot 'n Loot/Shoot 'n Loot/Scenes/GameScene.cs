using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Enemies;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class GameScene : Scene
    {
        public  Player player;

        public const float MINSPAWNDIST = 500, MAXSPAWNDIST = 1000;
        
        public GameScene()
        {
            base.Initialize();

            player = new Player();
            //Enemy enemy = new Onelegged(Vector2.Zero);
            //objects.Add(enemy);
            //objects.Add(new Enemy(new Vector2(1000, 1000), Enemy.EnemyType.enemy2));
            objects.Add(player);
            //objects.Add(new Item(1, 1, 1, new Sprite(TextureManager.enemy1, new Vector2(1000), new Vector2(40))));
            //Map.Initialize();
        }

        public override void OnResume()
        {
            Camera.Position = player.Center;
        }

        public override void Update()
        {
            Camera.Follow(player.Position);

            if (player.Dead) { SceneManager.CurrentScene = SceneManager.gameOverScene; SoundManager.playerDie.Play(); }

            if (Input.KeyWasJustPressed(Microsoft.Xna.Framework.Input.Keys.Escape)) SceneManager.CurrentScene = SceneManager.pauseScene;

            foreach (Chunk c in Map.chunks)
            {
                if (player.DistanceSquared(c.Center) < Math.Pow(MAXSPAWNDIST, 2) && player.DistanceSquared(c.Center) > Math.Pow(MINSPAWNDIST, 2)) c.SpawnZombie(objects);
            }
         
            base.Update();
        }

        public int NoOfZombies()
        {
            return objects.Where(i => i is Enemy).Count();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
