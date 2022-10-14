using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupLib.Classes
{
    public class Game
    {
        private int GameID;

        #region TEAMS
        private Team _awayTeam;
        private Team _homeTeam;

        public Team AwayTeam { get; }
        public Team HomeTeam { get; }
        #endregion

        #region SCORE
        private int _awayTeamScore;
        private int _homeTeamScore;
        #endregion

        #region DATETIMES
        private DateTime _startTime;
        private DateTime _endTime;

        public DateTime StartTime { get; }
        #endregion



        private Game(Team away, Team home)
        {
            _awayTeam = away;
            _homeTeam = home;
            _awayTeamScore = 0;
            _homeTeamScore = 0;
        }

        public static Game Start(Team away, Team home)
        {

            Game g = new Game(away, home);
            g._startTime = DateTime.Now;
            return g;
        
        }

        public bool Finish()
        {
            if (_startTime != null)
            {
                _endTime = DateTime.Now;
                return true;
            }
            return false;
        }

        public void UpdateScore(int awayScore,int homeScore)
        {
            _awayTeamScore = awayScore;
            _homeTeamScore = homeScore;
        }

        public Tuple<int, int> GetScore()
        {
            return new Tuple<int, int>(_awayTeamScore, _homeTeamScore);
        }
    }
}
