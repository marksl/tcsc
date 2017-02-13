using System;
using System.Collections.Immutable;

namespace Thriftier.Schema.Parser
{
    public interface UserElement
    {
        /**
 * Gets the name of the element.
 *
 * @return the name of this element.
 */
        string Name { get; }

        /**
         * Gets the {@link Location} where the element is defined.
         *
         * @return the Location where this element is defined.
         */
        Location Location { get; }

        /**
         * Gets the documentation comments of the element, or an empty string.
         *
         * @return the documentation present on this element, or an empty string.
         */
        string Documentation { get; }

        /**
         * Gets an immutable map containing any annotations present on the element.
         *
         * @return all annotations present on this element.
         */
        ImmutableDictionary<string, string> Annotations { get; }

        /**
         * Gets a value indicating whether the element contains non-empty Javadoc.
         *
         * @return true if this element contains non-empty Javadoc.
         */
        bool HasJavaDoc { get; }

        /**
         * Gets a value indicating whether the element has been marked as
         * deprecated; this may or may not be meaningful, depending on the
         * particular type of element.
         *
         * @return true if this element has been marked as deprecated.
         */
        bool IsDeprecated { get; }

    }
}