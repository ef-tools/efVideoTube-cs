using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VikingErik.Mvc.ResumingActionResults
{
    public class ResumingFileStreamResult : ResumingActionResultBase
    {
        public ResumingFileStreamResult(Stream fileStream, string contentType, string fileDownloadName = null)
            : base(contentType)
        {
            if (fileStream == null)
                throw new ArgumentNullException("fileStream");

            FileContents = fileStream;
            FileName = fileDownloadName;
        }
    }
}
