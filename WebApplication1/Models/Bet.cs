using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Bet
    {
        [Key]
        public int BetID { get; set; }
        public int MatchID { get; set; }
        public string Command { get; set; }
        public int BetValue { get; set; }
        public int UserID { get; set; }
    }
}
