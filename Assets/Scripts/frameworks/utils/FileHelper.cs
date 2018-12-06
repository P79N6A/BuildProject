using System.IO;
using System.Text;

namespace Sakura
{
    public class FileHelper
    {
        public static byte[] GetBytes(string fullPath)
        {
            if (File.Exists(fullPath) == false)
            {
                return null;
            }

            FileStream fs = File.OpenRead(fullPath);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            return bytes;
        }

        public static string GetUTF8Text(string fullPath)
        {
            if (File.Exists(fullPath) == false)
            {
                return "";
            }

            FileStream fs = File.OpenRead(fullPath);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            string v = Encoding.UTF8.GetString(bytes);
            fs.Close();
            return v;
        }

        public static void AutoCreateDirectory(string fullPath)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            DirectoryInfo di = fileInfo.Directory;
            if (di.Exists == false)
            {
                Directory.CreateDirectory(di.FullName);
            }
        }

        public static object GetAMF(string fullPath, bool deflate = true)
        {
            if (File.Exists(fullPath) == false)
            {
                return null;
            }

            byte[] bytes;
            using (FileStream fileStream=File.OpenRead(fullPath))
            {
                bytes=new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
            }

            if (bytes.Length < 1)
            {
                return null;
            }

            object result = null;
            if (bytes != null)
            {
//                ByteArray byteArray
            }

            return null;
        }

    }
}