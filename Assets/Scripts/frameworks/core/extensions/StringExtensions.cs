using System.Text;

namespace Sakura
{
    public static class StringExtensions
    {
        public static void Clear(this StringBuilder self)
        {
            self.Length = 0;
        }
    }
}