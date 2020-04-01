using System.Collections.Generic;

namespace Application.Common.AppSettingHelpers.Main
{
    public class PhotoSettings
    {
        public List<string> MimeContentTypes { get; set; }

        public int MaxLengthBytes { get; set; }

        public int FileNameMaxLength { get; set; }
    }
}