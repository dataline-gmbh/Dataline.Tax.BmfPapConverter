// <copyright file="XmlExtensions.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

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
