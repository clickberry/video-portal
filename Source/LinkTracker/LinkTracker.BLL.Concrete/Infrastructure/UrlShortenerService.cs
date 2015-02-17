// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using LinkTracker.BLL.Infrastructure;

namespace LinkTracker.BLL.Concrete.Infrastructure
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private const String Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string Encode(long number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            var sb = new StringBuilder();
            if (number == 0)
            {
                sb.Append(Alphabet[(int)number]);
            }
            else
            {
                int alphabetLength = Alphabet.Length;
                while (number > 0)
                {
                    long modulo = number%alphabetLength;
                    sb.Append(Alphabet[(int)modulo]);
                    number /= alphabetLength;
                }
            }

            var builder = new StringBuilder();
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                builder.Append(sb[i]);
            }

            return builder.ToString();
        }

        public long Decode(string str)
        {
            long number = 0;
            int alphabetLength = Alphabet.Length;
            for (int i = 0; i < str.Length; i++)
            {
                number = number*alphabetLength + Alphabet.IndexOf(str[i]);
            }

            return number;
        }
    }
}