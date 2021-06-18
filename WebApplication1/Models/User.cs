using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//TODO:
//Add Tables in database
//Rethink of logic
//etc.
namespace WebApplication1.Models
{
    
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int UserTelegramID { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
    }
}
