using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// Xenon object.
    /// </summary>
    public class XenonObject {

        /// <summary>
        /// The type of the object.
        /// </summary>
        public readonly XenonType BaseType;

        /// <summary>
        /// The attributes.
        /// </summary>
        public readonly List<XenonObject> Attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonObject"/> class.
        /// </summary>
        /// <param name="baseType">Type.</param>
        public XenonObject (XenonType baseType) {
            Attributes = new List<XenonObject> ();
            BaseType = baseType;
        }
    }
}

