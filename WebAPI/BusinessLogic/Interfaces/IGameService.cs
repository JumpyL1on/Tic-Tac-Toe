using Common.Requests;

namespace BusinessLogic.Interfaces
{
    public interface IGameService
    {
        public Task CreateGameAsync(int playerAId);
        public Task CloseGameAsync(int id, int playerAId);
        public Task AddAnotherPlayerToTheGameAsync(int id, int playerBId);
        public Task MakeAPlayerMoveAsync(int id, int playerId, MakeAPlayerMoveRequest request);
    }
}