using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using WorldCupLib.Classes;

namespace WorldCupLibTesting
{
    public class GameUnitTest
    {
        //THIS TEST IS FIRST REQUIREMENT
        [Fact]
        public void InitialScore()
        {
            Team away = new Team("AwayTeam");
            Team home = new Team("HomeTeam");

            Game g = Game.Start(away, home,0);

            Tuple<int, int> score = g.GetScore();

            Assert.True(score.Item1 == 0);
            Assert.True(score.Item2 == 0);

        }

        [Fact]
        public void StartTimePopulated_Nominal()
        {
            Team away = new Team("AwayTeam");
            Team home = new Team("HomeTeam");

            Game g = Game.Start(away, home,0);

            Assert.NotNull(g.StartTime);
           
        }


        //THIS TEST IS 3rd REQUIREMENT
        [Fact]
        public void UpdateScore_Nominal()
        {
            Team away = new Team("AwayTeam");
            Team home = new Team("HomeTeam");

            Game g = Game.Start(away, home,0);

            g.UpdateScore(2, 1);

            Tuple<int, int> score = g.GetScore();

            Assert.True(score.Item1 == 2);
            Assert.True(score.Item2 == 1);

        }
    }
}
