using Habr.Common.Exceptions;
using WebAPI.Enums;

namespace WebAPI.Entities
{
    public class Game
    {
        public int Id { get; }
        public Field Field { get; private set; }
        public WhoseMove WhoseMove { get; private set; }
        public Result? Result { get; private set; }
        public Player PlayerA { get; private set; }
        public int PlayerAId { get; private set; }
        public Player PlayerB { get; private set; }
        public int? PlayerBId { get; private set; }
        public bool IsOver
        {
            get
            {
                if (IsOneOfTheRowsFilled)
                {
                    return true;
                }
                else if (IsOneOfTheColumnsFilled)
                {
                    return true;
                }
                else if (IsOneOfTheDiagonalsFilled)
                {
                    return true;
                }

                return false;
            }
        }

        private bool IsOneOfTheRowsFilled
        {
            get
            {
                for (var i = 0; i < 3; i++)
                {
                    if (IsCombinationMade(Field[i, 0], Field[i, 1], Field[i, 2]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsOneOfTheColumnsFilled
        {
            get
            {
                for (var j = 0; j < 3; j++)
                {
                    if (IsCombinationMade(Field[0, j], Field[1, j], Field[2, j]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsOneOfTheDiagonalsFilled
        {
            get
            {
                if (IsCombinationMade(Field[0, 0], Field[1, 1], Field[2, 2]))
                {
                    return true;
                }
                else if (IsCombinationMade(Field[2, 0], Field[1, 1], Field[0, 2]))
                {
                    return true;
                }

                return false;
            }
        }

        private bool IsCombinationMade(Mark? mark1, Mark? mark2, Mark? mark3) =>
            mark1.HasValue && mark2.HasValue && mark3.HasValue && mark1.Value == mark2.Value && mark2.Value == mark3.Value;

        public Game(int playerAId)
        {
            WhoseMove = WhoseMove.PlayerA;
            Result = null;
            Field = new Field();
            PlayerAId = playerAId;
        }

        public void AddAnotherPlayer(int playerBId)
        {
            PlayerBId = playerBId;
        }

        public void UpdateTheField(int playerId, int i, int j)
        {
            if (WhoseMove == WhoseMove.PlayerA && PlayerAId == playerId)
            {
                Field[i, j] = Mark.Cross;
                WhoseMove = WhoseMove.PlayerB;
            }
            else if (WhoseMove == WhoseMove.PlayerB && PlayerBId == playerId)
            {
                Field[i, j] = Mark.Nought;
                WhoseMove = WhoseMove.PlayerA;
            }
            else
            {
                throw new ForbiddenException("Player cannot make a move");
            }
        }

        public void SetTheResult()
        {
            Result = WhoseMove == WhoseMove.PlayerA ? Enums.Result.PlayerBWon : Enums.Result.PlayerAWon;
        }
    }
}