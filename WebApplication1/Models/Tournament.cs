using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Tournament
    {
        [Key]
        public int TournamentID { get; set; }
        public string TournamentName { get; set; }
        public int MatchID { get; set; }
    }
}
