using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// Xenon module.
    /// </summary>
    public class XenonModule {

        /// <summary>
        /// The constants.
        /// </summary>
        public readonly List<XenonObject> Constants;

        /// <summary>
        /// Gets the initializer.
        /// </summary>
        /// <value>The initializer.</value>
        public Emitter Initializer {
            get;
            internal set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonModule"/> class.
        /// </summary>
        public XenonModule () {
            Constants = new List<XenonObject> ();
            Initializer = new Emitter ();
        }

        /// <summary>
        /// Defines a constant.
        /// </summary>
        /// <returns>The constant id.</returns>
        /// <param name="obj">The object.</param>
        public int DefineConstant (XenonObject obj) {

            // Return the index if the object is already defined
            if (Constants.Contains (obj))
                return Constants.IndexOf (obj);

            // Otherwise add the object and return its index
            Constants.Add (obj);
            return Constants.Count - 1;
        }
    }
}

