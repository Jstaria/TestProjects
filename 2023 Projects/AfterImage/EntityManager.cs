using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AfterImage
{
    internal class EntityManager
    {
        private KeyboardState prevKBState;
        private MouseState prevMouseState;

        private Vector2 mousePos;
        private Polygon attackBox;

        private float weaponAngle;

        private bool attackFlag;
        private float lastAttackTime;

        private GraphicsDevice graphics;

        private Player player;
        private List<Enemy> enemies;
        private ParticleSystem system;
        private CollisionCheck collisionCheck;
        private CollisionCheck attackCheck;

        public EntityManager(Player player, List<Enemy> enemies, GraphicsDevice graphics)
        {
            this.collisionCheck = new CollisionCheck();
            this.player = player;
            this.enemies = enemies;
            this.system = new ParticleSystem(player.Asset, Point.Zero, 0, 100, 6, 0, player);

            UpdateEntityCount();

            this.graphics = graphics;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            KeyboardState kbState = Keyboard.GetState();

            mousePos = new Vector2(mouseState.X, mouseState.Y);

            player.Update(gameTime);
            system.speed = 5;

            Attack(mouseState, gameTime);

            system.ParticleUpdate(gameTime);

            prevKBState = kbState;
            prevMouseState = mouseState;
        }

        public bool Attack(MouseState mouseState, GameTime gt)
        {
            
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                attackCheck = new CollisionCheck();
                attackBox = new Polygon();

                lastAttackTime = (float)gt.TotalGameTime.TotalSeconds;

                Vector2 direction = mousePos - player.Origin;
                direction.Normalize();

                attackBox.angle = MathHelper.ToRadians(180) + (float)Math.Atan2(direction.Y, direction.X);
                attackBox.origin = new Vector2(player.Position.X + player.Asset.Width / 4, player.Position.Y + player.Asset.Height / 4);
                attackBox.points = new List<Vector2>();
                attackBox.model = new List<Vector2>();

                attackBox.points.Add(new Vector2(+10, 0));
                attackBox.points.Add(new Vector2(-10, 0));
                attackBox.points.Add(new Vector2(-10, 150));
                attackBox.points.Add(new Vector2(+10, 150));
                
                attackBox.model.Add(new Vector2(+10, 0));
                attackBox.model.Add(new Vector2(-10, 0));
                attackBox.model.Add(new Vector2(-10, 150));
                attackBox.model.Add(new Vector2(+10, 150));

                attackCheck.AddPolygon = attackBox;
                attackCheck.UpdateShapes();
                attackFlag = true;
            }    

            float timeSinceLastAttack = (float)gt.TotalGameTime.TotalSeconds - lastAttackTime;

            if (attackFlag)
            {
                Polygon attack = attackCheck.Polygons[0];
                attack.angle += MathHelper.ToRadians(25);
                weaponAngle = attack.angle;
                attack.origin = new Vector2(player.Position.X + player.Asset.Width / 4, player.Position.Y + player.Asset.Height / 4);
                attackCheck.Polygons[0] = attack;
                attackCheck.UpdateShapes();
                collisionCheck.UpdateShapes();

                for (int i = 0; i < collisionCheck.Polygons.Count; i++)
                {    
                    Polygon shape = collisionCheck.Polygons[i];
                    shape.overlap |= collisionCheck.Collide(attackCheck.Polygons[0], collisionCheck.Polygons[i]);
                    collisionCheck.Polygons[i] = shape;   
                }

                
                //System.Diagnostics.Debug.WriteLine("Attacking:" + attackFlag);

                
            }

            if (timeSinceLastAttack > .1 && attackFlag)
            {
                attackFlag = false;
                attackCheck.UpdateShapes();
                collisionCheck.UpdateShapes();
                attackCheck.Polygons.Clear();
            }

            
            
            return attackBox.overlap;
        }

        public void UpdateEntityCount()
        {
            collisionCheck.Polygons.Clear();

            foreach (Enemy enemy in enemies)
            {
                Polygon enemyPoly = new Polygon();

                enemyPoly.points = new List<Vector2>();
                enemyPoly.model = new List<Vector2>();

                enemyPoly.origin = enemy.Origin;
                enemyPoly.points = enemy.Points;

                List<Vector2> points = new List<Vector2>();

                for (int i = 0; i < enemy.Points.Count; i++)
                {
                    points.Add(new Vector2(enemy.Points[i].X, enemy.Points[i].Y));
                }

                enemyPoly.model = points;
                enemyPoly.angle = enemy.Angle;
                enemyPoly.overlap = false;

                collisionCheck.Polygons.Add(enemyPoly);
                collisionCheck.UpdateShapes();
            }

            
        }

        public void Draw(SpriteBatch sb)
        {
            Color color = Color.Red;

            system.Draw(sb);
            for (int i = 0; i < collisionCheck.Polygons.Count; i++)
            {
                if (collisionCheck.Polygons[0].overlap)
                {
                    color = Color.Black;
                }
                System.Diagnostics.Debug.WriteLine("colliding?:" + collisionCheck.Polygons[0].overlap);
                enemies[i].Draw(sb, color);
            }
            player.Draw(sb);

            if (attackFlag)
            {
                sb.Draw(player.Asset, new Rectangle((int)player.Origin.X, (int)player.Origin.Y, 20, 150), null, Color.Gray, weaponAngle, new Vector2(20, 0), SpriteEffects.None, 0);
            }
        }
    }
}
