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
            if (await _appDbContext.Set<Game>().AnyAsync(game => game.Result == null && game.PlayerAId == playerAId))
            {
                throw new ForbiddenException("Player cannot create 2nd game at the same time");
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

            if (game.Result != null || game.PlayerAId != playerAId)
            {
                throw new ForbiddenException("Game cannot be closed or is not created by this player");
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

            if (game.PlayerAId == playerBId)
            {
                throw new ForbiddenException("Game was created by this player");
            }

            if (game.PlayerBId != null)
            {
                throw new ForbiddenException("Another player cannot be added to this game");
            }

            game.AddAnotherPlayer(playerBId);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task MakeAPlayerMoveAsync(int id, int playerId, MakeAPlayerMoveRequest request)
        {
            var game = await _appDbContext
                .Set<Game>()
                .SingleOrDefaultAsync(game => game.Id == id);

            game = game ?? throw new BusinessException("There is no game with this id");

            try
            {
                game.UpdateTheField(playerId, request.I, request.J);
            }
            catch (IndexOutOfRangeException)
            {
                throw new BusinessException("Indexes i or j are out of range");
            }

            if (game.IsOver)
            {
                game.SetTheResult();
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}