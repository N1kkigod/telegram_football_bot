using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Match
    {
        [Key]
        public int MatchID { get; set; }
        public string Date { get; set; }
        public string TournamentName { get; set; }
        public string Command1 { get; set; }
        public string Command2 { get; set; }
        public int Command1Score { get; set; }
        public int Command2Score { get; set; }
        public string MatchStatus { get; set; }
        public override string ToString()
        {
            return TournamentName + '\n' + Command1 + " " + Command1Score  + "-" + Command2Score + " " + Command2 + '\n' + MatchStatus;
        }
    }
}
