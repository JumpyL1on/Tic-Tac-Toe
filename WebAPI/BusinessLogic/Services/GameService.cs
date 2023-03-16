using BusinessLogic.Interfaces;
using Common.Exceptions;
using Common.Requests;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class GameService : IGameService
    {
        private readonly DbContext _appDbContext;

        public GameService(DbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public async Task CreateGameAsync(int playerAId)
        {
            if (await _appDbContext
                .Set<Game>()
                .AnyAsync(game => game.Result == null && (game.PlayerAId == playerAId || game.PlayerBId == playerAId)))
            {
                throw new ForbiddenException("Player cannot create game if he is already in the game");
            }

            var game = new Game(playerAId);

            _appDbContext.Set<Game>().Add(game);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task CloseGameAsync(int id, int playerAId)
        {
            var game = await _appDbContext
                .Set<Game>()
                .SingleOrDefaultAsync(game => game.Id == id);

            game = game ?? throw new BusinessException("There is no game with this id");

            if (game.Result != null)
            {
                throw new BusinessException("Game is already finished");
            }

            if (game.PlayerAId != playerAId || game.PlayerBId != null)
            {
                throw new ForbiddenException("Game is not created by this player or was already started");
            }

            _appDbContext.Set<Game>().Remove(game);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task AddAnotherPlayerToTheGameAsync(int id, int playerBId)
        {
            var game = await _appDbContext
                .Set<Game>()
                .SingleOrDefaultAsync(game => game.Id == id);

            game = game ?? throw new BusinessException("There is no game with this id");

            if (game.Result != null)
            {
                throw new BusinessException("Game is already finished");
            }

            if (game.PlayerAId == playerBId || game.PlayerBId != null)
            {
                throw new ForbiddenException("Game was created by this player or was already started");
            }

            game.AddAnotherPlayer(playerBId);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task MakeAPlayerMoveAsync(int id, int playerId, MakeAPlayerMoveRequest request)
        {
            var game = await _appDbContext
                .Set<Game>()
                .Include(game => game.Field)
                .SingleOrDefaultAsync(game => game.Id == id);

            game = game ?? throw new BusinessException("There is no game with this id");

            if (game.Result != null)
            {
                throw new BusinessException("The game is already finished");
            }

            game.UpdateTheField(playerId, request.I, request.J);

            if (game.IsOver)
            {
                game.SetTheResult();
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}