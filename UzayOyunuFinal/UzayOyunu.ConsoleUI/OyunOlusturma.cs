using UzayOyunu.Business;

namespace UzayOyunu.ConsoleUI
{
    public class OyunOlusturma
    {
        private readonly int _genislik;
        private readonly int _yukseklik;
        private readonly char[,] _alan;

        public OyunOlusturma(int genislik, int yukseklik)
        {
            _genislik = genislik;
            _yukseklik = yukseklik;
            _alan = new char[yukseklik, genislik];
        }

        public void Render(GameManager oyun)
        {
            // alanı temizle
            for (int y = 0; y < _yukseklik; y++)
                for (int x = 0; x < _genislik; x++)
                    _alan[y, x] = ' ';

            
            foreach (var a in oyun.Uzaylilar)
                PlaceIfInBounds(a.X, a.Y, a.Sembol);

            foreach (var l in oyun.Lazerler)
                PlaceIfInBounds(l.X, l.Y, l.Sembol);

            foreach (var m in oyun.Meteorlar)
                PlaceIfInBounds(m.X, m.Y, m.Sembol);

            if (oyun.Oyuncu.IsUzayli)
                PlaceIfInBounds(oyun.Oyuncu.X, oyun.Oyuncu.Y, oyun.Oyuncu.Sembol);

            Console.SetCursorPosition(0, 0);

            string can = new string('O', oyun.Oyuncu.Can);
            string ustCizgi = $" @ Uzay Oyunu  Puan:{oyun.ScoreManager.MevcutSkor,-6} En Yüksek:{oyun.ScoreManager.EnYuksekSkor,-6} Can:{can,-3} Dalga:{oyun.Dalga}";
            Console.WriteLine(ustCizgi.PadRight(_genislik + 2));
            Console.WriteLine(new string('═', _genislik + 2));

            for (int y = 0; y < _yukseklik; y++)
            {
                Console.Write('║');
                for (int x = 0; x < _genislik; x++)
                    Console.Write(_alan[y, x]);
                Console.WriteLine('║');
            }

            Console.WriteLine(new string('═', _genislik + 2));
            Console.WriteLine(" ← → Hareket   SPACE Ateş   Q Çıkış");
        }

        public void RenderGameOver(int finalScore, int highScore)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("###################################");
            Console.WriteLine("#           OYUN BİTTİ           #");
           
            Console.WriteLine($"#   Puanın    : {finalScore,-17}#");
            Console.WriteLine($"#   En yüksek : {highScore,-17}#");
            Console.WriteLine("###################################");
            Console.WriteLine("#    Tekrar oynamak için ENTER    #");
            Console.WriteLine("#    Çıkmak için Q'a basınız.     #");
            Console.WriteLine("###################################");
        }

        public void RenderTitle()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("###################################");
            Console.WriteLine("#           UZAY OYUNU           #");
            Console.WriteLine("###################################");
            Console.WriteLine("#   W / V / v  →  Uzaylılar      #");
            Console.WriteLine("#       A      →  Gemimiz        #");
            Console.WriteLine("#       |      →  Lazer          #");
            Console.WriteLine("#       *      →  Meteor         #");
            Console.WriteLine("###################################");
            Console.WriteLine("#         ← →  Hareket           #");
            Console.WriteLine("#          SPACE  Ateş           #");
            Console.WriteLine("#            Q  Çıkış            #");
            Console.WriteLine("###################################");
            Console.WriteLine("#      Başlamak için ENTER        #");
            Console.WriteLine("###################################");
        }

        private void PlaceIfInBounds(int x, int y, char ch)
        {
            if (x >= 0 && x < _genislik && y >= 0 && y < _yukseklik)
                _alan[y, x] = ch;
        }
    }
}
