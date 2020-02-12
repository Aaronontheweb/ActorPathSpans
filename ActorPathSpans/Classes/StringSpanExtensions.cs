// -----------------------------------------------------------------------
// <copyright file="StringSpanExtensions.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2020 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Akka.Actor
{
    public static class StringSpanExtensions
    {
        public static ReadOnlyMemory<char>[] SplitWithSpan(this string str, char separator)
        {
            int[] positions = new int[str.Length];

            var found = InternalCountPositions(separator, str.AsSpan(), ref positions);
            
            // handle special case (none found)
            if (found == 0)
                return new[] {str.AsMemory()};

            // Allocate array based on number of separators - although we'll discard empty ones
            var splits = new ReadOnlyMemory<char>[found];
            var mem = str.AsMemory();
            var current = 0;
            var outputIndex = 0;
            for (var i = 0; i < found && current < str.Length; i++)
            {
                if (positions[i] - current > 0)
                    splits[outputIndex++] = mem.Slice(current, positions[i] - current);
                current += positions[i] + 1; // add 1 for the length of the separator
            }

            if (current < str.Length) // dangling chunk at the end
            {
                splits[outputIndex++] = mem.Slice(current);
            }

            ReadOnlyMemory<char>[] finalArray = splits;
            if (outputIndex != found)
            {
                finalArray = new ReadOnlyMemory<char>[outputIndex];
                for (var j = 0; j < outputIndex; j++)
                {
                    finalArray[j] = splits[j];
                }
            }

            return finalArray;

        }

        private static unsafe int InternalCountPositions(char separator, ReadOnlySpan<char> str, ref int[] positions)
        {
            var found = 0;
            var length = positions.Length;
            fixed (char* chars = &MemoryMarshal.GetReference(str))
            {
                for (var i = 0; i < length; i++)
                {
                    if (chars[i] == separator)
                    {
                        positions[found++] = i;
                    }
                }
            }

            return found;
        }
    }
}