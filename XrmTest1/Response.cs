using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmTest1.Core;

namespace XrmTest1
{
    public class Response
    {
        public DtoMetadata Metadata { get; set; }
        public List<Resume> Resumes { get; set; }
    }
}
