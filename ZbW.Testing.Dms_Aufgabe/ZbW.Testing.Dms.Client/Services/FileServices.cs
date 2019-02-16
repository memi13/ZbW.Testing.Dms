using System.Diagnostics;
using System.IO;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.Services
{
    public class FileServices
    {
        public void OpenFile(string phate)
        {
            Process.Start(phate);
        }
        public void CopyTo(string from,string to)
        {
            File.Copy(from, to);
        }

        public bool CreatePhatIfNotExist(string phat)
        {
            if (CheckPathExist(phat))
                return true;
            else
            {
                var phatParts = phat.Split('\\');
                var phateComplit = false;
                var counter = 0;
                while (!phateComplit)
                {
                    var newphate = string.Empty;
                    for (int i = 0; i < counter + 1; i++)
                    {
                        newphate = newphate + phatParts[i] + "\\";
                    }

                    if (!CheckPathExist(newphate))
                    {
                        System.IO.Directory.CreateDirectory(newphate);
                    }
                    counter++;

                    phateComplit = CheckPathExist(phat);
                }
                return true;
            }

        }
        public bool CheckPathExist(string phat)
        {
            return Directory.Exists(phat);
        }

        public void GeneratXMl(string phate,MetadataItem daten)
        {
            var xml = MetadataItem.Seralize(daten);
            using (File.Create(phate + "_Metadata.xml")) ;

            using (TextWriter tw = new StreamWriter(phate + "_Metadata.xml"))
            {
                tw.WriteLine(xml);
                tw.Close();
            }
        }

    }
}