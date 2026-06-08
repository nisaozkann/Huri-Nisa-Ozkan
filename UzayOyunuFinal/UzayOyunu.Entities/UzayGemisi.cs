using UzayOyunu.Core;

namespace UzayOyunu.Entities
{
    public class UzayGemisi : IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsUzayli { get; set; } = true;
        public char Sembol => 'A';
        public int Can { get; set; } = 3;

        public UzayGemisi(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        public void Sola(int minX = 1)
        {
            if (X > minX) X--;
        }

        public void Saga(int maxX)
        {
            if (X < maxX - 2) X++;
        }
    }
}
