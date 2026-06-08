using UzayOyunu.Core;

namespace UzayOyunu.Business
{
    public class ScoreManager
    {
        private readonly IScoreArsivi _arsiv;

        public int MevcutSkor { get; private set; } = 0;
        public int EnYuksekSkor { get; private set; } = 0;

        public ScoreManager(IScoreArsivi arsiv)
        {
            _arsiv = arsiv;
            EnYuksekSkor = _arsiv.EnYuksekSkoruGetir();
        }

        public void Add(int puan)
        {
            MevcutSkor += puan;
            if (MevcutSkor > EnYuksekSkor)
            {
                EnYuksekSkor = MevcutSkor;
                _arsiv.EnYuksekSkoruSakla(EnYuksekSkor);
            }
        }

        public void Reset()
        {
            MevcutSkor = 0;
        }
    }
}
