using System;
using System.Collections.Generic;

namespace M05_UF3_P3_Frogger
{
    class Program
    {
        static bool run = true;
        static bool win = false;
        static Player player;
        static List<Lane> lanes;

        static void Main(string[] args)
        {
            Setup();
            while (run)
            {
                Input();
                Logic();
                Draw();
                TimeManager.NextFrame();
                if (player.pos.y == 0)
                {
                    run = false; //para que pare todo y salga del bucle
                    win = true;
                }
            }
            if (win)
            {
                Console.Clear();
                Console.WriteLine("Has GANADO");
            }
            else //solo puede ser run = false
            {
                Console.Clear();
                Console.WriteLine("Has MUERTO");
            }
        }

        static void Setup()
        {
            Console.CursorVisible = false;

            //MAPA
            lanes = new List<Lane>();
            List<ConsoleColor> colorsCars = new List<ConsoleColor>(Utils.colorsCars);
            List<ConsoleColor> colorsLogs = new List<ConsoleColor>(Utils.colorsLogs);

            lanes.Add(new Lane(12, false, ConsoleColor.Green, false, false, 0, ' ', null));
            lanes.Add(new Lane(11, false, ConsoleColor.Black, true, false, 0.1f, Utils.charCars, colorsCars));
            lanes.Add(new Lane(10, false, ConsoleColor.Black, true, false, 0.1f, Utils.charCars, colorsCars));
            lanes.Add(new Lane(9, false, ConsoleColor.Black, true, false, 0.1f, Utils.charCars, colorsCars));
            lanes.Add(new Lane(8, false, ConsoleColor.Black, true, false, 0.1f, Utils.charCars, colorsCars));
            lanes.Add(new Lane(7, false, ConsoleColor.Black, true, false, 0.1f, Utils.charCars, colorsCars));
            lanes.Add(new Lane(6, false, ConsoleColor.Green, false, false, 0, ' ', null));
            lanes.Add(new Lane(5, true, ConsoleColor.Blue, false, true, 0.8f, Utils.charLogs, colorsLogs));
            lanes.Add(new Lane(4, true, ConsoleColor.Blue, false, true, 0.8f, Utils.charLogs, colorsLogs));
            lanes.Add(new Lane(3, true, ConsoleColor.Blue, false, true, 0.8f, Utils.charLogs, colorsLogs));
            lanes.Add(new Lane(2, true, ConsoleColor.Blue, false, true, 0.8f, Utils.charLogs, colorsLogs));
            lanes.Add(new Lane(1, true, ConsoleColor.Blue, false, true, 0.8f, Utils.charLogs, colorsLogs));
            lanes.Add(new Lane(0, false, ConsoleColor.Green, false, false, 0, ' ', null));

            player = new Player();
        }

        static void Input()
        {
            Vector2Int dir = Utils.Input();
            if (dir != Vector2Int.zero) //si no es zero, el jugador se esta moviendo
            {
                Utils.GAME_STATE state = player.Update(dir, lanes);
                if (state != Utils.GAME_STATE.RUNNING && state != Utils.GAME_STATE.WIN) // si no se cumple es que ha muerto
                {
                    run = false;
                }
            }
        }

        static void Logic()
        {
            foreach (Lane lane in lanes)//recorre cada lane para saber si hay colision entre coche y personaje
            {
                lane.Update();
                if (lane.posY == player.pos.y)
                {
                    DynamicElement element = lane.ElementAtPosition(player.pos);
                    if (element != null && element.character == Utils.charCars)
                    {
                        run = false;
                    }
                }
            }

            foreach (Lane lane in lanes)//recorre cada lane
            {

                if (lane.posY == player.pos.y)
                {
                    DynamicElement element = lane.ElementAtPosition(player.pos);
                    if (element != null && element.character == Utils.charLogs) //en caso de cumplirse no pasa nada, (esta subido a un log)
                    {
                        player.Update(element.speed);
                        break;
                    }
                }
            }
        }

        static void Draw()
        {
            Console.Clear();
            //recorrer la lista y dibujar todas las lanes
            foreach (Lane lane in lanes)
            {
                lane.Draw();
            }
            player.Draw(lanes);

            Console.ResetColor();
        }
    }
}

