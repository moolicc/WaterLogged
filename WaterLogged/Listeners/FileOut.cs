
namespace WaterLogged.Listeners
{
    /// <summary>
    /// Implements a listener which appends output to a file.
    /// </summary>
    public class FileOut : Listener
    {
        /// <summary>
        /// The specified file to append log messages to.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Instantiates a new instance of the FileOut listener. By default; appends to "log.txt".
        /// </summary>
        public FileOut()
            : this("log.txt")
        {
        }

        /// <summary>
        /// Instantiates a new instance of the FileOut listener. Appends to the file at the specified path.
        /// </summary>
        public FileOut(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Outputs a log message to a file.
        /// </summary>
        public override void Write(string value, string tag)
        {
            System.IO.File.AppendAllText(FilePath, value);
        }
    }
}
