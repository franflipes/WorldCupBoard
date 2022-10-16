using System;
using System.Collections.Generic;
using System.Linq;
using WorldCupLib.Classes;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WorldCupLibTesting")]
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

        #region Internal for testing

        ///<summary>
        ///This method removes al games, that´s why we only make visible to Testing dll
        /// </summary>
        internal void FinishAllGames()
        {
            foreach (Game g in _games.ToList())
            {
                _games.Remove(g);
            }
        }
        #endregion

        /// <summary>
        /// Returns a List of Games ordered  by totalScore and starTime if score is tied - REQUIREMENT 4
        /// </summary>
        /// <returns> List of Games  </returns>
        public List<Game> GetGamesSummary()
        {
            return _games.OrderByDescending(g=>g.StartTime).OrderByDescending(g => g.GetScore().Item1 + g.GetScore().Item2).ToList<Game>();
        }

        #region StartGame, FinishGame and UpdateScore
        /// <summary>
        /// Start a Game if not exists  REQUIREMENT 1
        /// </summary>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns> a Game if started or null if already in progress</returns>
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

        /// <summary>
        /// Finish a Game if exists using 2 teams REQUIREMENT 2
        /// </summary>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns>True or False</returns>
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

        /// <summary>
        /// Finish a Game if exists using a GameId REQUIREMENT 2
        /// </summary>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns>True or False</returns>
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

        /// <summary>
        /// Update a game score if in progress REQUIREMENT 3
        /// </summary>
        /// <param name="score">Tuple of goals, <home,away></param>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns>True or false</returns>
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

        /// <summary>
        /// Get the game score in progress
        /// </summary>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns>Tuple of goals <home,away></returns>
        public Tuple<int, int> GetGameScore( Team home, Team away)
        {
            if (IsGameInProgress(home,away))
            {
                Game g = GetGameInProgress(gameId);
                return g.GetScore();
            }
            return null;
        }
        #endregion


        #region auxiliar methods
        /// <summary>
        /// Check if 2 teams are already playing a game and startTime is not null, therefore in progress
        /// </summary>
        /// <param name="home"></param>
        /// <param name="away"></param>
        /// <returns>True or False</returns>
        //
        public bool IsGameInProgress(Team home, Team away)
        {
            //we could get rid of comparing StartTime as all games set the value when start and actually it doesn´t define whether is in progress or not. That a game exist in the collection is what validates
            // to be in progress
            return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).Any();
        }

        /// <summary>
        /// Check if exist a game with the gameId parameter, therefore in progress
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>True or false</returns>
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

        //Obtain a Game if in progress using 2 teams
        private Game GetGameInProgress(Team home, Team away)
        {
            return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).FirstOrDefault<Game>();
        }

        //Obtain a Game if in progress using a GameId
        private Game GetGameInProgress(int gameId)
        {
            return _games.Where(g => g.GameId==gameId && g.StartTime != null).FirstOrDefault<Game>();
        }

        //Obtain a new game id
        private int GetNewGameID()
        {
            //lock objeect to avoid concurrency or races
            lock (idLock)
            {
                gameId++;
            }
            return gameId;
        }

        /// <summary>
        /// counts the games in progress
        /// </summary>
        /// <returns>Integer of games in progress</returns>
        public int GamesInProgress()
        {
            return _games.Where(g => g.StartTime != null).Count();
        }
        #endregion


    }
}
