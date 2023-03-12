using Common.Enums;

namespace DataAccess.Entities
{
    public class Field
    {
        public int Id { get; }
        public Mark? Mark11 { get; private set; }
        public Mark? Mark12 { get; private set; }
        public Mark? Mark13 { get; private set; }
        public Mark? Mark21 { get; private set; }
        public Mark? Mark22 { get; private set; }
        public Mark? Mark23 { get; private set; }
        public Mark? Mark31 { get; private set; }
        public Mark? Mark32 { get; private set; }
        public Mark? Mark33 { get; private set; }
        public Game Game { get; private set; }

        public Mark? this[int i, int j]
        {
            get
            {
                if (i == 1 && j == 1)
                {
                    return Mark11;
                }
                else if (i == 1 && j == 2)
                {
                    return Mark12;
                }
                else if (i == 1 && j == 3)
                {
                    return Mark13;
                }
                else if (i == 2 && j == 1)
                {
                    return Mark21;
                }
                else if (i == 2 && j == 2)
                {
                    return Mark22;
                }
                else if (i == 2 && j == 3)
                {
                    return Mark23;
                }
                else if (i == 3 && j == 1)
                {
                    return Mark31;
                }
                else if (i == 3 && j == 2)
                {
                    return Mark32;
                }
                else if (i == 3 && j == 3)
                {
                    return Mark33;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (i == 1 && j == 1)
                {
                    Mark11 = value;
                }
                else if (i == 1 && j == 2)
                {
                    Mark12 = value;
                }
                else if (i == 1 && j == 3)
                {
                    Mark13 = value;
                }
                else if (i == 2 && j == 1)
                {
                    Mark21 = value;
                }
                else if (i == 2 && j == 2)
                {
                    Mark22 = value;
                }
                else if (i == 2 && j == 3)
                {
                    Mark23 = value;
                }
                else if (i == 3 && j == 1)
                {
                    Mark31 = value;
                }
                else if (i == 3 && j == 2)
                {
                    Mark32 = value;
                }
                else if (i == 3 && j == 3)
                {
                    Mark33 = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}