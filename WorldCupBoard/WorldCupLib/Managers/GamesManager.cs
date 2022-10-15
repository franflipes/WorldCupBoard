using System;
using System.Collections.Generic;
using System.Linq;
using WorldCupLib.Classes;

namespace WorldCupLib.Managers
{
    public sealed class GamesManager
    {
        //variables to lock the gameId to prevent repeats
        private readonly object idLock = new object();
        private int gameId;

        private IList<Game> _games;
        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the `new` operator.
        private GamesManager() 
        {
            _games = new List<Game>();
            gameId = 0;
        }

        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multithreaded program. 
        private static GamesManager _instance;

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
        public static GamesManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GamesManager();
            }
            return _instance;

        }   

        public List<Game> GetGamesSummary()
        {
            return _games.OrderByDescending(g=>g.StartTime).OrderByDescending(g => g.GetScore().Item1 + g.GetScore().Item2).ToList<Game>();
        }

        #region StartGame, FinishGame and UpdateScore
        //REQUIREMENT 1
        //Returns game if started or null if not
        public Game StartGame(Team home, Team away)
        {
            //Don´t start another game with same teams if in progress
            if (!IsGameInProgress(home, away) && !IsAnyTeamInPlaying(home,away))
            {
                int gameId = GetNewGameID();
                Game g= Game.Start(home, away, gameId);
                _games.Add(g);
                return g;
            }

            return null;
        }

        //REQUIREMENT 2
        //Just remove game from List and return a bool if removal happened
        public bool FinishGame(Team home, Team away)
        {
            if (IsGameInProgress(home, away))
            {
                Game g = GetGameInProgress(home, away);
                _games.Remove(g);
                return true;
            }

            return false;
        }

        //REQUIREMENT 2
        //Just remove game from List and return a bool if removal happened
        public bool FinishGame(int gameId)
        {
            if (IsGameInProgress(gameId))
            {
                Game g = GetGameInProgress(gameId);
                _games.Remove(g);
                return true;
            }

            return false;
        }

        //REQUIREMENT 3
        //Just remove game from List and return a bool if removal happened
        public bool UpdateScore(Tuple<int,int> score, Team home,Team away)
        {
            if (IsGameInProgress(home, away))
            {
                Game g = GetGameInProgress(home, away);
                g.UpdateScore(score.Item1, score.Item2);
                return true;
            }

            return false;
        }

        public Tuple<int, int> GetGameScore( Team home, Team away)
        {
            if (IsGameInProgress(gameId))
            {
                Game g = GetGameInProgress(gameId);
                return g.GetScore();
            }
            return null;
        }
        #endregion


        #region auxiliar methods
        //Check if 2 teams are already playing a game
        public bool IsGameInProgress(Team home, Team away)
        {
            //we could get rid of comparing StartTime as all games set the value when start and actually it doesn´t define whether is in progress or not. That a game exist in the collection is what validates
            // to be in progress
            return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).Any();
        }

        public bool IsGameInProgress(int gameId)
        {
            //we could get rid of comparing StartTime as all games set the value when start and actually it doesn´t define whether is in progress or not. That a game exist in the collection is what validates
            // to be in progress
            return _games.Where(g => g.GameId==gameId && g.StartTime != null).Any();
        }

        //whether away or Home teams are playing, if so, can not start a new game with any involved
        private bool IsAnyTeamInPlaying(Team home, Team away)
        {
            return _games.Where(g => g.AwayTeam == away || g.HomeTeam == home && g.StartTime != null).Any();
        }

        private Game GetGameInProgress(Team home, Team away)
        {
            return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).FirstOrDefault<Game>();
        }

        private Game GetGameInProgress(int gameId)
        {
            return _games.Where(g => g.GameId==gameId && g.StartTime != null).FirstOrDefault<Game>();
        }


        private int GetNewGameID()
        {
            //lock objeect to avoid concurrency or races
            lock (idLock)
            {
                gameId++;
            }
            return gameId;
        }

        public int GamesInProgress()
        {
            return _games.Where(g => g.StartTime != null).Count();
        }
        #endregion


    }
}
