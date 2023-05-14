using System;
using System.Collections.Generic;
using System.Text;

namespace M05_UF3_P3_Frogger
{
    public class Lane
    {
        public readonly int posY;
        public readonly int speedElements;
        public readonly bool speedPlayer;
        public readonly ConsoleColor background;
        public readonly bool damageElements;
        public readonly bool damageBackground;
        public List<DynamicElement> elements { get; protected set; } = new List<DynamicElement>();


        public Lane(int posY, bool speedPlayer, ConsoleColor background, bool damageElements, bool damageBackground, float elementsPercent, char elementsChar, List<ConsoleColor> colorsElements)
        {
            this.posY = posY;
            this.speedElements = Utils.rnd.Next(-10, 10) < 0 ? 1 : -1;
            this.speedPlayer = speedPlayer;
            this.background = background;
            this.damageElements = damageElements;
            this.damageBackground = damageBackground;

            for (int i = 0; i < Utils.MAP_WIDTH; i++)
            {
                if(Utils.rnd.NextDouble() < elementsPercent)
                {
                    this.elements.Add(
                        new DynamicElement(
                            new Vector2Int(speedElements, 0), 
                            new Vector2Int(i, posY),
                            elementsChar, 
                            colorsElements[Utils.rnd.Next(colorsElements.Count)]
                            ));
                }
            }
            this.elements.TrimExcess();
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, posY);
            Console.BackgroundColor = background;
            for (int i = 0; i < Utils.MAP_WIDTH; i++)
            {
                DynamicElement element = ElementAtPosition(new Vector2Int(i, posY));
                if (element == null)
                {
                    Console.Write(' ');
                }
                else
                {
                    Console.ForegroundColor = element.foreground;
                    Console.Write(element.character);
                }
            }
        }
        public void Update()
        {
            foreach (DynamicElement element in elements)
            {
                element.Update();
            }
        }

        public DynamicElement ElementAtPosition(Vector2Int position)
        {
            foreach (DynamicElement element in elements)
            {
                if(element.pos == position)
                    return element;
            }
            return null;
        }
    }
}
