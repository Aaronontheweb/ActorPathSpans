// -----------------------------------------------------------------------
// <copyright file="StringSpanExtensions.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2020 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Akka.Actor
{
    public static class StringSpanExtensions
    {
        public static IEnumerable<ReadOnlyMemory<char>> SplitWithSpan(this string str, char separator)
        {
            var lastFound = 0;
            var lastSpan = str;
            var strMemory = str.AsMemory();
            while (lastFound < str.Length)
            {
                var index = lastSpan.IndexOf(separator, lastFound);
               
                if (index == lastFound) // don't return a string - had a trailing or leading separator
                {

                }
                else if (index > 0)
                    yield return strMemory.Slice(lastFound, index - lastFound); // return the rest of the string
                else
                {
                    yield return strMemory.Slice(lastFound, str.Length - lastFound); // return the rest of the string
                    break;
                }
                   
                lastFound = index + 1; // need to skip over separator characters
            }

        }
    }
}