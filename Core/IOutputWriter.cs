using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Allows writing things out to whatever display or communication method.
    /// </summary>
    public interface IOutputWriter
    {
        public Task WriteAsync(string s);
    }
}