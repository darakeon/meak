using System;

namespace Presentation.Helpers
{
    public class SeasonEpisode //: IDisposable
    {
        public String Season { get; set; }
        public String Episode { get; set; }

        //public void Dispose() { }

        public void Next(String currentSeason, String currentEpisode)
        {
            if (currentEpisode == null || currentSeason == null)
            {
                Season = "A";
                Episode = "01";
            }
            else if (currentEpisode == "20")
            {
                NextSeason(currentSeason);
                Episode = "01";
            }
            else
            {
                Season = currentSeason;
                NextEpisode(currentEpisode);
            }
        }

        private void NextSeason(String currentSeason)
        {
            var currentLetter = currentSeason[0];
            var currentAscii = Convert.ToInt32(currentLetter);
            var nextAscii = currentAscii + 1;
            var nextLetter = Convert.ToChar(nextAscii);

            Season = nextLetter.ToString();
        }

        private void NextEpisode(String currentEpisode)
        {
            var currentNumber = Convert.ToInt32(currentEpisode);
            var nextNumber = currentNumber + 1;
            var format = nextNumber < 10 ? "0{0}" : "{0}";

            Episode = String.Format(format, nextNumber); ;
        }

        public bool IsValid()
        {
            try
            {
                var wrongSeason = Season.Length != 1
                                  || Season[0] > 'Z'
                                  || Season[0] < 'A';

                var wrongEpisode = Episode.Length != 2
                                   || Convert.ToInt32(Episode) > 20
                                   || Convert.ToInt32(Episode) < 0;

                return !wrongSeason && !wrongEpisode;
            }
            catch
            {
                return false;
            }
        }
    }
}