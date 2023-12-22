using System.Drawing;

namespace BrickBreaker
{
    public class SessionPoints 
    {
        public int Points { get; private set; }

        public void Add(int toAdd)
        {
            Points += toAdd;
        }
    }
}