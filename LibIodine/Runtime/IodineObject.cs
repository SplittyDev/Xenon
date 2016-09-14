using System;
using System.Collections.Generic;

namespace IodineCore {

    /// <summary>
    /// Xenon object.
    /// </summary>
    public class IodineObject {

        /// <summary>
        /// The type of the object.
        /// </summary>
        public readonly IodineType BaseType;

        /// <summary>
        /// The attributes.
        /// </summary>
        public readonly List<IodineObject> Attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonObject"/> class.
        /// </summary>
        /// <param name="baseType">Type.</param>
        public IodineObject (IodineType baseType) {
            Attributes = new List<IodineObject> ();
            BaseType = baseType;
        }
    }
}

