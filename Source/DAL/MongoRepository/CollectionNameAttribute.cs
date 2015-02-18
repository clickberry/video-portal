// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace MongoRepository
{
    /// <summary>
    ///     Attribute used to annotate Enities with to override mongo collection name. By default, when this attribute
    ///     is not specified, the classname will be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the CollectionName class attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the collection.</param>
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Empty collectionname not allowed", "value");
            }

            Name = value;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public string Name { get; private set; }
    }
}