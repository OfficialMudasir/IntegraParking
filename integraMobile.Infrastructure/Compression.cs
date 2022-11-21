using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace integraMobile.Infrastructure
{
    public class Compression
    {

        public static bool Compress(List<string> files, string zippath)
        {
            bool bRes = false;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in files)
                        {
                            var demoFile = archive.CreateEntry(file);
                            byte[] bFileContent = File.ReadAllBytes(file);

                            using (var entryStream = demoFile.Open())
                            using (var b = new BinaryWriter(entryStream))
                            {
                                b.Write(bFileContent);
                            }
                        }
                    }

                    using (var fileStream = new FileStream(zippath, FileMode.Create))
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.CopyTo(fileStream);
                    }
                }
                bRes = true;
            }
            catch
            {
                bRes = false;
            }
            return bRes;

        }


    }
}
