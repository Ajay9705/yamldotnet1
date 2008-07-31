using System;
using System.Collections.Generic;
using System.Globalization;

namespace YamlDotNet.RepresentationModel
{
    /// <summary>
    /// Manages the state of a <see cref="YamlDocument" /> while it is loading.
    /// </summary>
    internal class DocumentState
    {
        private IDictionary<string, YamlNode> anchors = new Dictionary<string, YamlNode>();

        /// <summary>
        /// Adds the specified node to the anchor list.
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddAnchor(YamlNode node)
        {
            if (node.Anchor == null)
            {
                throw new ArgumentException("The specified node does not have an anchor");
            }

            if (anchors.ContainsKey(node.Anchor))
            {
                throw new DuplicateAnchorException(string.Format(CultureInfo.InvariantCulture, "The anchor '{0}' already exists", node.Anchor));
            }

            anchors.Add(node.Anchor, node);
        }

        /// <summary>
        /// Gets the node with the specified anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <param name="throwException">if set to <c>true</c>, the method should throw an exception if there is no node with that anchor.</param>
        /// <returns></returns>
        public YamlNode GetNode(string anchor, bool throwException)
        {
            YamlNode target;
            if (anchors.TryGetValue(anchor, out target))
            {
                return target;
            }
            else if (throwException)
            {
                throw new AnchorNotFoundException(string.Format(CultureInfo.InvariantCulture, "The anchor '{0}' does not exists", anchor));
            }
            else
            {
                return null;
            }
        }

        private bool hasUnresolvedAliases;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has unresolved aliases.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has unresolved aliases; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnresolvedAliases
        {
            get
            {
                return hasUnresolvedAliases;
            }
            set
            {
                hasUnresolvedAliases = value;
            }
        }
    }
}