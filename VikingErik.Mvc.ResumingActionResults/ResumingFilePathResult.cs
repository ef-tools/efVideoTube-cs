using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VikingErik.Mvc.ResumingActionResults
{
    public class ResumingFilePathResult : ResumingActionResultBase
    {
        private FileInfo MediaFile { get; set; }

        public ResumingFilePathResult(string fileName, string contentType, string fileDownloadName = null)
            : base(contentType)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException();

            MediaFile = new FileInfo(fileName);
            LastModified = MediaFile.LastWriteTime;
            FileContents = MediaFile.OpenRead();
            FileName = fileDownloadName;
        }
    }
}
