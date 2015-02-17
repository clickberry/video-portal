// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class TinyUrl
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly char[] _alphabet;
        private readonly uint _alphabetBase;

        public TinyUrl()
        {
            if (string.IsNullOrEmpty(Alphabet))
            {
                throw new ArgumentNullException();
            }

            _alphabet = Alphabet.ToCharArray();
            _alphabetBase = (uint)_alphabet.Length;
        }

        public string Compress(Guid guid)
        {
            byte[] guidBytes = guid.ToByteArray();
            string result = new[]
            {
                BitConverter.ToUInt64(guidBytes, 0),
                BitConverter.ToUInt64(guidBytes, 8)
            }
                .Select(EncodeUlong)
                .Aggregate((p, q) => p + q);

            return result;
        }

        public Guid Decompress(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            return new Guid(new[] { value.Substring(0, 11), value.Substring(11) }.Select(DecodeString).ToArray().SelectMany(BitConverter.GetBytes).ToArray());
        }

        private string EncodeUlong(ulong value)
        {
            if (value == 0)
            {
                return new string(_alphabet[0], 0);
            }

            string s = string.Empty;

            while (value > 0)
            {
                s += _alphabet[value%_alphabetBase];
                value = value/_alphabetBase;
            }

            char[] array = s.ToCharArray();
            Array.Reverse(array);

            return new string(array);
        }

        private ulong DecodeString(string value)
        {
            return value.Aggregate<char, ulong>(0, (current, c) =>
            {
                int inx = Array.IndexOf(_alphabet, c);
                if (inx < 0)
                {
                    throw new ArgumentException();
                }

                return current*_alphabetBase + (uint)inx;
            });
        }
    }
}