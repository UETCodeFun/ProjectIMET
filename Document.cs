using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPad_SonPX
{
    class Document
    {
        public bool IsEdit { get; set; } = false;
        public string PathDocument { get; set; } = null;
        public string getNameDoc()
        {
            if(PathDocument != String.Empty)
            {
                return Path.GetFileNameWithoutExtension(PathDocument);
            }
            return String.Empty;
        }
    }
}
