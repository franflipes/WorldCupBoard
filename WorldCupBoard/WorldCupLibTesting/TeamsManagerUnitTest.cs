﻿using Xunit;
using WorldCupLib.Managers;
using WorldCupLib.Classes;

namespace WorldCupLibTesting
{
    public class TeamsManagerUnitTest
    {
        //Singleton, same manager when run full suite
        TeamsManager tm;

        [Fact]
        public void TeamsManagerSingleton()
        {
            TeamsManager tm1 = TeamsManager.GetInstance();
            TeamsManager tm2 = TeamsManager.GetInstance();

            Assert.True(tm1 == tm2);
        }

        [Fact]
        //Team can not be added, should return an error or message
        //Not-Nominal use case
        public void TeamCanNotBeAddedTwice_NotNominal()
        {
            tm = TeamsManager.GetInstance();

            tm.AddTeam("Test2 Team");
            Team t = tm.AddTeam("Test2 Team");

            Assert.Null(t);  
        }

        [Fact]
        //Team return the same Team when we try to get same  name
        //Nominal use case
        public void TeamReturnTheSame_Nominal()
        {
            tm = TeamsManager.GetInstance();
           
            Team t1 = tm.AddTeam("Test3 Team");
            Team t2 = tm.ReturnTeam("Test3 Team");

            Assert.True(t1==t2);
        }

        [Fact]
        //Team can be remove properly
        //Nominal use case
        public void TeamCanBeRemoved_Nominal()
        {
            tm = TeamsManager.GetInstance();

            tm.AddTeam("Test4 Team");
            tm.RemoveTeam("Test4 Team");
            Team t1 = tm.ReturnTeam("Test4 Team");

            Assert.Null(t1);
        }
    }
}
