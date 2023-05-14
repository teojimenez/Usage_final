using System;
using System.Collections.Generic;

namespace M05_UF3_P3_Frogger
{
    public abstract class Element
    {
        public Vector2Int pos { get; protected set; }
        public char character { get; protected set; }
        public readonly ConsoleColor foreground;

        public Element(Vector2Int pos, char character = ' ', ConsoleColor foreground = ConsoleColor.White)
        {
            this.pos = pos;
            this.character = character;
            this.foreground = foreground;
        }

        public virtual void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = foreground;
            Console.Write(character);
        }
        public virtual void Draw(ConsoleColor background)
        {
            Console.BackgroundColor = background;
            Draw();
        }
        public abstract void Update();
    }

    public class DynamicElement : Element
    {

        public Vector2Int speed { get; protected set; }
        public DynamicElement(Vector2Int speed, Vector2Int pos, char character = ' ', ConsoleColor foreground = ConsoleColor.White) : base(pos, character, foreground)
        {
            this.speed = speed;
        }

        public override void Update()
        {
            pos += speed;
            if(pos.x >= Utils.MAP_WIDTH)
            {
                pos.x = 0;
            }
            else if (pos.x < 0)
            {
                pos.x = Utils.MAP_WIDTH - 1;
            }
            if(pos.y >= Utils.MAP_HEIGHT)
            {
                pos.y = 0;
            }
            else if (pos.y < 0)
            {
                pos.y = Utils.MAP_HEIGHT - 1;
            }
        }
        public virtual void Update(Vector2Int dir)
        {
            speed = dir;
            Update();
        }
    }

    public class Player : DynamicElement
    {
        public const char characterForward = '╧';
        public const char characterBackwards = '╤';
        public const char characterLeft = '╢';
        public const char characterRight = '╟';

        public Player() : base(Vector2Int.zero, new Vector2Int(Utils.MAP_WIDTH / 2, Utils.MAP_HEIGHT - 1), characterForward, ConsoleColor.Green)
        {
        }

        public Utils.GAME_STATE Update(Vector2Int dir, List<Lane> lanes)
        {
            speed = dir;

            if(dir.y < 0)
            { character = characterForward; }
            else if (dir.y > 0)
            { character = characterBackwards;}
            else if (dir.x > 0)
            { character = characterRight; }
            else if (dir.x < 0)
            { character = characterLeft; }

            pos += speed;
            if (pos.y <= 0)
            {
                return Utils.GAME_STATE.WIN;
            }
            else if (pos.y >= Utils.MAP_HEIGHT)
            {
                pos.y = Utils.MAP_HEIGHT - 1;
            }
            foreach (Lane lane in lanes)
            {
                if (lane.posY == pos.y)
                {
                    if (lane.speedPlayer) {
                        pos.x += lane.speedElements;
                    }
                    if (pos.x >= Utils.MAP_WIDTH)
                    {
                        pos.x = 0;
                    }
                    else if (pos.x < 0)
                    {
                        pos.x = Utils.MAP_WIDTH - 1;
                    }
                    if (lane.ElementAtPosition(pos) == null)
                    {
                        if (lane.damageBackground)
                        {
                            return Utils.GAME_STATE.LOOSE;
                        }
                        else
                        {
                            return Utils.GAME_STATE.RUNNING;
                        }
                    }
                    else
                    {
                        if (lane.damageElements)
                        {
                            return Utils.GAME_STATE.LOOSE;
                        }
                        else
                        {
                            return Utils.GAME_STATE.RUNNING;
                        }
                    }
                }
            }
            return Utils.GAME_STATE.RUNNING;
        }

        public void Draw(List<Lane> lanes)
        {
            foreach (Lane lane in lanes)
            {
                if (lane.posY == pos.y)
                {
                    Console.BackgroundColor = lane.background;
                }
            }
            base.Draw();
        }
    }
}
