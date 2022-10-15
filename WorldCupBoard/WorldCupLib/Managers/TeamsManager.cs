using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupLib.Classes;

namespace WorldCupLib.Managers
{
    public sealed class TeamsManager
    {
        private IList<Team> _teams;
        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the `new` operator.
        private TeamsManager() 
        {
            _teams = new List<Team>();
        }

        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multithreaded program.
        private static TeamsManager _instance;

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
        public static TeamsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TeamsManager();
            }
            return _instance;
        }

        private bool ExistTeam(string team)
        {

            if (_teams.Count == 0)
                return false;

            if (_teams.Any(t => t.Name == team))
                return true;

            return false;
        }

        public Team ReturnTeam(string team)
        {

            return _teams.FirstOrDefault(t => t.Name == team);
                
        }

        public Team AddTeam(string team)
        {
            //Double check for sanity, we shouldn´t even doublecheck because previously manager should have checked 
            if (!ExistTeam(team))
            {
                Team t = new Team(team);

                _teams.Add(t);

                return t;
            }

            return null;
        }

        public bool RemoveTeam(string team)
        {
            //Double check for sanity, we shouldn´t even doublecheck because previously manager should have checked 
            if (ExistTeam(team))
            {
                Team t = ReturnTeam(team);

                _teams.Remove(t);

                return true;
            }

            return false;
        }




    }
}
