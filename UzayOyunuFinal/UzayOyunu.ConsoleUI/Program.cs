using UzayOyunu.Business;
using UzayOyunu.DataAccess;

namespace UzayOyunu.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Uzay Oyunu";

            const int Genislik = 58;
            const int Yukseklik = 22;

            var arsiv = new ScoreArsivi();
            var renderer = new OyunOlusturma(Genislik, Yukseklik);

            bool running = true;
            while (running)
            {
                renderer.RenderTitle();

                ConsoleKey key = WaitForKey();
                if (key == ConsoleKey.Q) break;

                var scoreManager = new ScoreManager(arsiv);
                var oyun = new GameManager(Genislik, Yukseklik, scoreManager);
                Console.Clear();
                var gecikmeSuresi = DateTime.UtcNow;

                while (!oyun.IsGameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        var basili = Console.ReadKey(true).Key;
                        switch (basili)
                        {
                            case ConsoleKey.LeftArrow:  oyun.OyuncuHareketSol(); break;
                            case ConsoleKey.RightArrow: oyun.OyuncuHareketSag(); break;
                            case ConsoleKey.Spacebar:   oyun.LazeriAtesle();     break;
                            case ConsoleKey.Q:
                                oyun.Quit();
                                running = false;
                                break;
                        }
                    }

                    if ((DateTime.UtcNow - gecikmeSuresi).TotalMilliseconds >= 120)
                    {
                        oyun.Zaman();
                        renderer.Render(oyun);
                        gecikmeSuresi = DateTime.UtcNow;
                    }

                    Thread.Sleep(10);
                }

                if (!running) break;

                renderer.RenderGameOver(scoreManager.MevcutSkor, scoreManager.EnYuksekSkor);

                ConsoleKey endKey = WaitForKey();
                if (endKey == ConsoleKey.Q) running = false;
            }

            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Oyun kapatıldı. Görüşürüz!");
        }

        static ConsoleKey WaitForKey()
        {
            while (Console.KeyAvailable) Console.ReadKey(true);
            return Console.ReadKey(true).Key;
        }
    }
}
