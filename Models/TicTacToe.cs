using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Correctif_SignalR_TicTacToe.Models
{
    public static class TicTacToe
    {
        private const int GRIDWIDTH = 3;
        private static Random RNG = new Random();
        /// <summary>
        /// Dictionnaire regrouppant les deux joueurs. La clé est l'identifiant de l'utilisateur enregistré en tant que joueur. La valeur est une nouvelle instance du joueur avec en son sein l'instance de l'utilisateur.
        /// </summary>
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
        /// <summary>
        /// Dictionnaire regrouppant l'ensemble des utilisateurs. La clé est l'identifiant de l'utilisateur enregistré en tant que joueur. La valeur est une nouvelle instance de l'utilisateur.
        /// </summary>
        private static Dictionary<string, User> _users = new Dictionary<string, User>();
        internal static char[,] Grid = { { '-', '-', '-' }, { '-', '-', '-' }, { '-', '-', '-' } };

        /// <summary>
        /// Propriété resortant un tableau de joueurs.
        /// </summary>
        public static Player[] Players { get { return _players.Values.ToArray(); } }
        /// <summary>
        /// Propriété resortant un tableau d'utilisateurs.
        /// </summary>
        public static User[] Users { get { return _users.Values.ToArray(); } }
        public static string[] PlayersIds { get { return _players.Keys.ToArray(); } }
        public static string[] UsersIds { get { return _users.Keys.ToArray(); } }
        /// <summary>
        /// Méthode ajoutant un utilisateur, déjà enregistré dans le dictionnaire des utilisateurs, dans le dictionnaire des joueurs.
        /// </summary>
        /// <param name="userId">Identifiant d'un utilisateur, basé sur un Guid.</param>
        internal static void SetAsPlayer(string userId)
        {
            User user = GetUser(userId);
            if (Players.Length >= 2)
            {
                throw new ArgumentOutOfRangeException("To many Players registered...");
            }
            if (_players.ContainsKey(userId))
            {
                throw new ArgumentException("Player all ready registered");
            }
            _players.Add(userId, new Player(user));
        }
        /// <summary>
        /// Méthode enlevant un utilisateur du dictionnaire des joueurs.
        /// </summary>
        /// <param name="userId">Identifiant d'un utilisateur, basé sur un Guid.</param>
        internal static void RemoveFromPlayer(string userId)
        {
            if (Players.Length <= 0)
            {
                throw new IndexOutOfRangeException("No player registered.");
            }
            if (!_players.ContainsKey(userId))
            {
                throw new ArgumentOutOfRangeException("This user is not registered as player.");
            }
            _players.Remove(userId);
        }
        internal static void ClearPlayers()
        {
            _players.Clear();
        }
        /// <summary>
        /// Méthode ajoutant un utilisateur non-enregistré dans le dictionnaire des utilisateurs.
        /// </summary>
        /// <param name="username">Pseudonyme d'un utilisateur.</param>
        internal static User AddUser(string username)
        {
            User user = new User();
            user.Username = username;
            _users.Add(user.UserId, user);
            return user;
        }
        internal static User GetUser(string userId)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(userId, out guid))
            {
                throw new ArgumentException("Guid non valide");
            }
            if (Users.Length == 0 || !_users.ContainsKey(userId))
            {
                throw new ArgumentOutOfRangeException("Guid not registered into the Lobby");
            }
            return _users[userId];
        }
        internal static string StartGame()
        {
            int turnof = RNG.Next() % 2;
            _players[PlayersIds[turnof]].Start();
            return PlayersIds[turnof];
        }
        internal static void TurnEnd(string userId, int row, int col)
        {
            if (_players[userId].IsStarting)
                Grid[row, col] = 'X';
            else
                Grid[row, col] = 'O';
        }
        internal static bool IsFinished(int row, int col)
        {
            if (Grid[row, 1] == Grid[row, 0] && Grid[row, 1] == Grid[row, 2])
                return true;
            if (Grid[1, col] == Grid[0, col] && Grid[1, col] == Grid[2, col])
                return true;
            if ((col + 1) * (row + 1) == 3 || (col == 1 && row == 1))
                if (Grid[1, 1] == Grid[0, 2] && Grid[1, 1] == Grid[2, 0])
                    return true;
            if (col == row)
                if (Grid[1, 1] == Grid[0, 0] && Grid[1, 1] == Grid[2, 2])
                    return true;
            return false;
        }
        internal static string GetOtherPlayerId(string userId)
        {
            return (PlayersIds[0] == userId) ? PlayersIds[1] : PlayersIds[0];
        }
    }
}
