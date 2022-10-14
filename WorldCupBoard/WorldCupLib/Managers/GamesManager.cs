using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupLib.Classes;

namespace WorldCupLib.Managers
{
    public sealed class GamesManager
    {

        private IList<Game> _games;
        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the `new` operator.
        private GamesManager() 
        {
            _games = new List<Game>();
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

        //TO-DO test if same team if in different games.
        public bool IsGameInProgress(Team away, Team home)
        {
           return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).Any();
        }

        public Game GetGameInProgress(Team away, Team home)
        {
            return _games.Where(g => g.AwayTeam == away && g.HomeTeam == home && g.StartTime != null).FirstOrDefault<Game>();
        }

        public Game StartGame(Team away, Team home)
        {
            //Don´t start another game with same teams if in progress
            if (!IsGameInProgress(away, home))
            {
                Game g= Game.Start(away, home);
                _games.Add(g);
                return g;
            }

            return null;
        }

        public bool FinishGame(Team away, Team home)
        {
            if (IsGameInProgress(away, home))
            {
                Game g = GetGameInProgress(away, home);
                _games.Remove(g);
                return true;
            }

            return false;
        }
    }
}
