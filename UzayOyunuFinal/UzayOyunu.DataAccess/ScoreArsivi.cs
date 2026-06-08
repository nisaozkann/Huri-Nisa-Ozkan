using UzayOyunu.Core;

namespace UzayOyunu.DataAccess
{
    public class ScoreArsivi : IScoreArsivi
    {
        private const string FilePath = "highscore.txt";

        public int EnYuksekSkoruGetir()
        {
           
            if (!File.Exists(FilePath)) return 0;
            return int.TryParse(File.ReadAllText(FilePath).Trim(), out int skor) ? skor : 0;
        }

        public void EnYuksekSkoruSakla(int skor)
        {
            File.WriteAllText(FilePath, skor.ToString());
        }
    }
}
