using Correctif_SignalR_TicTacToe.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Correctif_SignalR_TicTacToe.Hubs
{
    public class TictactoeHub : Hub
    {

        public async Task NewConnection(User user)
        {
            user = TicTacToe.AddUser(user.Username);
            await Clients.Caller.SendAsync("Registered", user);
            if (TicTacToe.Players.Length < 2)
            {
                TicTacToe.SetAsPlayer(user.UserId);
                int number = TicTacToe.Players.Length;
                //await Groups.AddToGroupAsync(Context.ConnectionId, "Players");
                await Clients.Caller.SendAsync("SetAsPlayer", number);
            }
            else
            {
                //await Groups.AddToGroupAsync(Context.ConnectionId, "Spectators");
                await Clients.Caller.SendAsync("NoMorePlayer", user.Username);
            }
        }

        public async Task CheckPlayers()
        {
            if (TicTacToe.Players.Length == 2)
            {
                string firstPlayerId = TicTacToe.StartGame();
                await Clients.All.SendAsync("StartGame",firstPlayerId); 
            }
        }

        public async Task PlayerPlayed(string userId, string row, string col)
        {
            int r, c;
            if (!int.TryParse(row, out r) || !int.TryParse(col, out c) || TicTacToe.Grid[r, c] != '-')
                await Clients.Caller.SendAsync("BadMove");
            else
            {
                TicTacToe.TurnEnd(userId, r, c);
                userId = TicTacToe.GetOtherPlayerId(userId);
                if (TicTacToe.IsFinished(r, c))
                    await Clients.All.SendAsync("EndGame", userId, r, c);
                else
                    await Clients.All.SendAsync("NewTurn", userId, r, c);
            }
        }
    }
}
