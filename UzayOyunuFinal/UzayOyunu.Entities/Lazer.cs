using UzayOyunu.Core;

namespace UzayOyunu.Entities
{
    public class Lazer : IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsUzayli { get; set; } = true;
        public char Sembol => '|';

        public Lazer(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Yukari() => Y--;
    }
}
