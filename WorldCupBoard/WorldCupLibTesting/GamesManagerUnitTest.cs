using System.Threading;

using Xunit;
using WorldCupLib.Managers;
using WorldCupLib.Classes;
using System;

namespace WorldCupLibTesting
{
    public class GamesManagerUnitTest
    {
        private GamesManager gm;
        [Fact]
        public void GamesManagerSingleton()
        {
            GamesManager gm1 = GamesManager.GetInstance();
            GamesManager gm2 = GamesManager.GetInstance();

            Assert.True(gm1 == gm2);

        }

        #region Game In Progress Tests
        //A started game that has not finished, is in progress
        //THIS TEST IS 1ST REQUIREMENT
        [Fact]
        public void StartGame_Nominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            gm.StartGame(home, away);

            Assert.True(gm.IsGameInProgress(home, away));
            gm.FinishGame(home, away);

        }

        
        #endregion

        #region Finish games Test
        //Finish a game using home/away teams
        //Nominal use case
        //THIS TEST IS 2ND REQUIREMENT
        [Fact]
        //A started game that has  finished, is not in progress
        public void FinishGame_Nominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            gm.StartGame(home, away);
            gm.FinishGame(home, away);

            Assert.True(!gm.IsGameInProgress(home, away));

        }

        //Finish a game using a non existing home/away game
        //Not-Nominal use case
        [Fact]
        public void FinishGame_NotNominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            Assert.False(gm.FinishGame(home, away));

        }

        //Finish a game using gameId
        //Nominal use case
        [Fact]
        public void FinishGameByGameId_Nominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            Game g = gm.StartGame(home, away);
            gm.FinishGame(g.GameId);

            Assert.True(gm.GamesInProgress()==0);

        }

        //A finished Game should not be in progress
        //Not-Nominal use case
        [Fact]
        public void FinishGameByGameId_NotNominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            Game g = gm.StartGame(home, away);
            //try finish a game with gameID not existing
            gm.FinishGame(5);

            Assert.True(gm.GamesInProgress() == 1);
            gm.FinishGame(home, away);

        }
        #endregion

        #region Update Score
        //Update a game score using home/away teams
        //Nominal use case
        //THIS TEST IS 3RD REQUIREMENT
        [Fact]
        //A started game that has  finished, is not in progress
        public void UpdateScore_Nominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            gm.StartGame(home, away);
            gm.UpdateScore(new Tuple<int, int>(1, 1), home, away);
            Tuple<int,int> score = gm.GetGameScore(home, away);

            Assert.True(score.Item1==1 && score.Item2==1);
            gm.FinishGame(home, away);

        }

        //Update a game score using a game not existing
        //Nominal use case
        [Fact]
        public void UpdateScore_NotNominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

            //gm.StartGame(home, away);
            gm.UpdateScore(new Tuple<int, int>(1, 1), home, away);
            Tuple<int, int> score = gm.GetGameScore(home, away);

            Assert.Null(score);
           

        }
        #endregion

        #region Team or Game already In Progress
        //Same game can not be started twice
        //Not-Nominal use case
        [Fact]
        public void SameGameCanNotBeStartedTwice_NotNominal()
        {
            gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");

             
            gm.StartGame(home, away);
            //StartGame returns the game created, we try to start two times
            Game g = gm.StartGame(home, away);

            Assert.Null(g);
            gm.FinishGame(home, away);

        }

        //Same team can not be in a second match
        //Not-Nominal use case
        [Fact]
        public void SameTeamCanNotBePlayingTwice_NotNominal()
        {
            GamesManager gm = GamesManager.GetInstance();

            Team away = new Team("Away1 Team");
            Team home = new Team("Home1 Team");
            Team secondHome = new Team("SecondHomeTeam");

            gm.StartGame(home, away);
            //StartGame returns the game created, we try to start two times
            Game g = gm.StartGame(secondHome, away);

            Assert.Null(g);
            gm.FinishGame(home, away);

        }
        #endregion

        //THIS TEST is 4th REQUIREMENT
        [Fact]
        public void GamesSummaryNominal()
        {
            GamesManager gm = GamesManager.GetInstance();

            Team Mexico = new Team("Mexico");
            Team Canada = new Team("Canada");
            Team Spain = new Team("Spain");
            Team Brazil = new Team("Brazil");
            Team Germany = new Team("Germany");
            Team France = new Team("France");
            Team Uruguay = new Team("Uruguay");
            Team Italy = new Team("Italy");
            Team Argentina = new Team("Argentina");
            Team Australia = new Team("Australia");

            //We create the 5 game sin the description, we sleep the thread 1 second to see a change in the startTime 
            Game g1=gm.StartGame(Mexico, Canada);
            g1.UpdateScore(0, 5);
            Thread.Sleep(1000);

            Game g2 = gm.StartGame(Spain, Brazil);
            g2.UpdateScore(10, 2);
            Thread.Sleep(1000);

            Game g3 = gm.StartGame(Germany, France);
            g3.UpdateScore(2, 2);
            Thread.Sleep(1000);

            Game g4 = gm.StartGame(Uruguay, Italy);
            g4.UpdateScore(6, 6);
            Thread.Sleep(1000);

            Game g5 = gm.StartGame(Argentina, Australia);
            g5.UpdateScore(3, 1);
            Thread.Sleep(1000);


            //Method GetGamesSummary sort the list of games by total score and then by start time recently if games' score is tied  
            //After ordering, we assert the new list with the ones matching in the description
            int i = 1;
            foreach (Game g in gm.GetGamesSummary())
            {
                switch (i) 
                {
                    case 1:
                        Assert.True(g == g4);//Uruguay 6 - Italy 6
                        break;
                    case 2:
                        Assert.True(g == g2);//Spain 10 - Brazil 2
                        break;
                    case 3:
                        Assert.True(g == g1);//Mexico 0 - Canada 5
                        break;
                    case 4:
                        Assert.True(g == g5);//Argentina 3 - Australia 1
                        break;
                    case 5:
                        Assert.True(g == g3);//Germany 2 - France 2
                        break;

                }

                i++;
            }

            gm.FinishGame(Mexico, Canada);
            gm.FinishGame(Spain, Brazil);
            gm.FinishGame(Germany, France);
            gm.FinishGame(Uruguay, Italy);
            gm.FinishGame(Argentina, Australia);

        }

    }
}
