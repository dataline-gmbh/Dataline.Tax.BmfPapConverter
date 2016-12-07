using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter
{
    public static class XmlExtensions
    {
        public static string GetPreviousComment(this XElement element)
        {
            var comment = element.PreviousNode as XComment;

            return comment?.Value;
        }
    }
}
