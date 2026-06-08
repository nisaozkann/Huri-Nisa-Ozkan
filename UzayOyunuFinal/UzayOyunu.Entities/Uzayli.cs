using UzayOyunu.Core;

namespace UzayOyunu.Entities
{
    public enum UzayliSeviyesi { Guclu, Orta, Hafif }

    public class Uzayli : IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsUzayli { get; set; } = true;
        public UzayliSeviyesi Seviyesi { get; }
        public int Puan { get; }

        public char Sembol => Seviyesi switch
        {
            UzayliSeviyesi.Guclu => 'W',
            UzayliSeviyesi.Orta  => 'V',
            UzayliSeviyesi.Hafif => 'v',
            _                    => '?'
        };

        public Uzayli(int x, int y, UzayliSeviyesi seviyesi)
        {
            X = x;
            Y = y;
            Seviyesi = seviyesi;
            Puan = seviyesi switch
            {
                UzayliSeviyesi.Guclu => 30,
                UzayliSeviyesi.Orta  => 20,
                UzayliSeviyesi.Hafif => 10,
                _                    => 10
            };
        }
    }
}
