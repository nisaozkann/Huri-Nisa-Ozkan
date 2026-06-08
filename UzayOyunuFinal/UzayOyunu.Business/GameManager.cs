using UzayOyunu.Entities;

namespace UzayOyunu.Business
{
    public class GameManager
    {
        public UzayGemisi Oyuncu { get; private set; }
        public List<Uzayli> Uzaylilar { get; private set; } = new();
        public List<Lazer> Lazerler { get; private set; } = new();
        public List<Meteor> Meteorlar { get; private set; } = new();

        public ScoreManager ScoreManager { get; }
        public bool IsGameOver { get; private set; } = false;
        public int Dalga { get; private set; } = 1;

        private readonly int _genislik;
        private readonly int _yukseklik;
        private int _uzayliYonu = 1;
        private int _zamanSayaci = 0;
        private int _atesEtmeSuresi = 0;
        private readonly Random _rng = new();

        private int UzayliHiz => Math.Max(4, 18 - Dalga * 2);
        private int MeteorSikligi => Math.Max(40, 90 - Dalga * 10);

        public GameManager(int genislik, int yukseklik, ScoreManager scoreManager)
        {
            _genislik = genislik;
            _yukseklik = yukseklik;
            ScoreManager = scoreManager;
            Oyuncu = new UzayGemisi(genislik / 2, yukseklik - 2);
            UzayliOlusturma();
        }

        private void UzayliOlusturma()
        {
            Uzaylilar.Clear();
            int satirlar = Math.Min(2 + Dalga, 5);
            int sutunlar = 10;
            int boslukX = (_genislik - 8) / sutunlar;

            for (int r = 0; r < satirlar; r++)
            {
                UzayliSeviyesi seviyesi = r == 0 ? UzayliSeviyesi.Guclu
                                        : r == 1 ? UzayliSeviyesi.Orta
                                        : UzayliSeviyesi.Hafif;

                
                for (int c = 0; c < sutunlar; c++)
                    Uzaylilar.Add(new Uzayli(4 + c * boslukX, 3 + r * 3, seviyesi));
            }
        }

        public void OyuncuHareketSol() => Oyuncu.Sola();
        public void OyuncuHareketSag() => Oyuncu.Saga(_genislik);

        public void LazeriAtesle()
        {
            if (_atesEtmeSuresi > 0) return;
            Lazerler.Add(new Lazer(Oyuncu.X, Oyuncu.Y - 1));
            _atesEtmeSuresi = 5;
        }

        public void Quit() => IsGameOver = true;

        public void Zaman()
        {
            if (IsGameOver) return;
            _zamanSayaci++;
            if (_atesEtmeSuresi > 0) _atesEtmeSuresi--;

            LazerleriHareketEttir();
            UzaylilariHareketEttir();
            MeteorlariOlusturveHareketEttir();
            LazerUzayliCarpmasiniKontrolEt();
            MeteorOyuncuCarpmasiniKontrolEt();
            UzayliOyuncuyaUlastimiKontrolEt();
            Temizle();
            DalganinBittiginiKontrolEt();
        }

        private void LazerleriHareketEttir()
        {
            foreach (var l in Lazerler) l.Yukari();
        }

        private void UzaylilariHareketEttir()
        {
            if (_zamanSayaci % UzayliHiz != 0) return;

            var canli = Uzaylilar.Where(a => a.IsUzayli).ToList();
            if (!canli.Any()) return;

            bool sagaCarpti = canli.Any(a => a.X >= _genislik - 2);
            bool solaCarpti = canli.Any(a => a.X <= 1);

            if ((sagaCarpti && _uzayliYonu > 0) || (solaCarpti && _uzayliYonu < 0))
            {
                foreach (var a in canli) a.Y++;
                _uzayliYonu *= -1;
            }
            else
            {
                foreach (var a in canli) a.X += _uzayliYonu;
            }
        }

        private void MeteorlariOlusturveHareketEttir()
        {
            if (_zamanSayaci % MeteorSikligi == 0)
                Meteorlar.Add(new Meteor(_rng.Next(1, _genislik - 1), 1));

            foreach (var m in Meteorlar) m.Asagi();
        }

        private void LazerUzayliCarpmasiniKontrolEt()
        {
            foreach (var lazer in Lazerler.Where(l => l.IsUzayli))
            {
                var carpma = Uzaylilar.FirstOrDefault(a =>
                    a.IsUzayli && a.X == lazer.X && a.Y == lazer.Y);

                if (carpma is null) continue;
                carpma.IsUzayli = false;
                lazer.IsUzayli = false;
                ScoreManager.Add(carpma.Puan);
            }
        }

        private void MeteorOyuncuCarpmasiniKontrolEt()
        {
            foreach (var m in Meteorlar.Where(m => m.IsUzayli))
            {
                if (m.X != Oyuncu.X || m.Y != Oyuncu.Y) continue;
                m.IsUzayli = false;
                Oyuncu.Can--;

                if (Oyuncu.Can <= 0)
                {
                    Oyuncu.IsUzayli = false;
                    IsGameOver = true;
                }
            }
        }

        private void UzayliOyuncuyaUlastimiKontrolEt()
        {
            if (Uzaylilar.Any(a => a.IsUzayli && a.Y >= Oyuncu.Y))
                IsGameOver = true;
        }

        private void DalganinBittiginiKontrolEt()
        {
            if (!Uzaylilar.Any(a => a.IsUzayli))
            {
                Dalga++;
                UzayliOlusturma();
            }
        }

        private void Temizle()
        {
            Lazerler.RemoveAll(l => !l.IsUzayli || l.Y < 0);
            Meteorlar.RemoveAll(m => !m.IsUzayli || m.Y >= _yukseklik);
            Uzaylilar.RemoveAll(a => !a.IsUzayli);
        }
    }
}
