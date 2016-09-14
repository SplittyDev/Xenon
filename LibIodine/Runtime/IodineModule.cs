using System;
using System.Collections.Generic;

namespace IodineCore {

    /// <summary>
    /// Xenon module.
    /// </summary>
    public class IodineModule {

        /// <summary>
        /// The constants.
        /// </summary>
        public readonly List<IodineObject> Constants;

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
        public IodineModule () {
            Constants = new List<IodineObject> ();
            Initializer = new Emitter ();
        }

        /// <summary>
        /// Defines a constant.
        /// </summary>
        /// <returns>The constant id.</returns>
        /// <param name="obj">The object.</param>
        public int DefineConstant (IodineObject obj) {

            // Return the index if the object is already defined
            if (Constants.Contains (obj))
                return Constants.IndexOf (obj);

            // Otherwise add the object and return its index
            Constants.Add (obj);
            return Constants.Count - 1;
        }
    }
}

