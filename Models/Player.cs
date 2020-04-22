using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Correctif_SignalR_TicTacToe.Models
{
    public class Player
    {
        private int _nbTurn;
        private bool _isStarting;
        public User User { get; set; }
        public int NbTurn { get { return this._nbTurn; } }
        public bool IsStarting { get { return this._isStarting; } }
        public Player(User user)
        {
            this.User = user;
            this._isStarting = false;
            this._nbTurn = 0;
        }
        public void Start()
        {
            this._isStarting = true;
        }
        public void Played()
        {
            this._nbTurn++;
        }
    }
}
