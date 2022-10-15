using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupLib.Classes
{
    public class Team: IComparable<Team>
    {
        public String Name { get; set; }

        public Team(string name)
        {
            Name = name;
        }

        public int CompareTo(Team other)
        {
            if (this.Name == other.Name)
                return 1;
            return 0;
        }
    }
}
