using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Correctif_SignalR_TicTacToe.Models
{
    public class User
    {
        private Guid _userId;
        public string UserId { get { return this._userId.ToString(); } }
        public string Username { get; set; }

        public User()
        {
            this._userId = Guid.NewGuid();
        }
    }
}
