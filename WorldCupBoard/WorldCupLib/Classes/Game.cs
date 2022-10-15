using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupLib.Classes
{
    /// <summary>
    /// This class holds a single game
    /// </summary>
    public class Game
    {
        #region GameId
        private int _gameId;
        public int GameId
        {
            get { return _gameId; }
        }
        #endregion

        #region TEAMS variables and Properties
        private Team _awayTeam;
        private Team _homeTeam;

        public Team AwayTeam 
        {
            get { return _awayTeam; }
        }
        public Team HomeTeam 
        { 
            get { return _homeTeam; }
        }
        #endregion

        #region SCORE private variables
        private int _awayTeamScore;
        private int _homeTeamScore;


        #endregion

        #region DATETIMES variables and Properties
        private DateTime _startTime;
        private DateTime _endTime;

        public DateTime StartTime 
        { 
            get { return _startTime; }
        }
        #endregion


        

        private Game(Team home, Team away, int gameId)
        {
            _awayTeam = away;
            _homeTeam = home;
            _awayTeamScore = 0;
            _homeTeamScore = 0;
            _gameId = gameId;
            _startTime = DateTime.Now;
        }

        


        public static Game Start(Team home, Team away,int gameId)
        {

            Game g = new Game(home, away,gameId);
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

        public void UpdateScore(int homeScore, int awayScore)
        {
            _awayTeamScore = awayScore;
            _homeTeamScore = homeScore;
        }

        public Tuple<int, int> GetScore()
        {
            return new Tuple<int, int>(_homeTeamScore, _awayTeamScore);
        }
    }
}
