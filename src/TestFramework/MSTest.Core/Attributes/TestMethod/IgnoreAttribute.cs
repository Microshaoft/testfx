﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;

    /// <summary>
    /// The ignore attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreAttribute"/> class.
        /// </summary>
        public IgnoreAttribute()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreAttribute"/> class.
        /// </summary>
        /// <param name="message">
        /// Message specifies reason for ignoring.
        /// </param>
        public IgnoreAttribute(string message)
        {
            this.IgnoreMessage = message;
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        public string IgnoreMessage { get; private set; }
    }
}
