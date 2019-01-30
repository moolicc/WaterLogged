
using WaterLogged.Templating;

namespace WaterLogged.Output
{
    /// <summary>
    /// Implements a listener which appends output to a file.
    /// </summary>
    public class FileOut : ListenerSink
    {
        public FileTypeTransformer FileTypeTransformer { get; set; }

        /// <summary>
        /// The specified file to append log messages to.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Instantiates a new instance of the FileOut listener. By default; appends to "log.txt".
        /// </summary>
        public FileOut()
            : this("log.txt", FileTypeTransformers.TextTransformer)
        {
        }

        /// <summary>
        /// Instantiates a new instance of the FileOut listener. Appends to the file at the specified path.
        /// </summary>
        public FileOut(string filePath, FileTypeTransformer transformer)
        {
            FilePath = filePath;
            FileTypeTransformer = transformer;
        }

        /// <summary>
        /// Outputs a log message to a file.
        /// </summary>
        public override void Write(string value, string tag)
        {
            System.IO.File.AppendAllText(FilePath, FileTypeTransformer(value));
        }

        public override void ProcessMessage(StructuredMessage message, string tag)
        {
            System.IO.File.AppendAllText(FilePath, FileTypeTransformer("", message));
        }
    }
}
