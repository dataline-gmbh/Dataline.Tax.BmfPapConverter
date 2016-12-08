// <copyright file="InvalidPapException.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;

namespace Dataline.Tax.BmfPapConverter
{
    public class InvalidPapException : Exception
    {
        public InvalidPapException(string message) : base($"Der PAP ist ungültig: {message}")
        {
        }
    }
}
