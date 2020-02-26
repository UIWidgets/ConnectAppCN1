// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Collections;
using System.Collections.Generic;

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents a combined list and collection of HTML nodes.
    /// </summary>
    public class HtmlAttributeCollection : IList<HtmlAttribute> {
        #region Fields

        internal Dictionary<string, HtmlAttribute> Hashitems = new Dictionary<string, HtmlAttribute>();
        HtmlNode _ownernode;
        List<HtmlAttribute> items = new List<HtmlAttribute>();

        #endregion

        #region Constructors

        internal HtmlAttributeCollection(HtmlNode ownernode) {
            this._ownernode = ownernode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a given attribute from the list using its name.
        /// </summary>
        public HtmlAttribute this[string name] {
            get {
                if (name == null) {
                    throw new ArgumentNullException("name");
                }

                HtmlAttribute value;
                return this.Hashitems.TryGetValue(name.ToLower(), out value) ? value : null;
            }
            set { this.Append(value); }
        }

        #endregion

        #region IList<HtmlAttribute> Members

        /// <summary>
        /// Gets the number of elements actually contained in the list.
        /// </summary>
        public int Count {
            get { return this.items.Count; }
        }

        /// <summary>
        /// Gets readonly status of colelction
        /// </summary>
        public bool IsReadOnly {
            get { return false; }
        }

        /// <summary>
        /// Gets the attribute at the specified index.
        /// </summary>
        public HtmlAttribute this[int index] {
            get { return this.items[index]; }
            set { this.items[index] = value; }
        }

        /// <summary>
        /// Adds supplied item to collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(HtmlAttribute item) {
            this.Append(item);
        }

        /// <summary>
        /// Explicit clear
        /// </summary>
        void ICollection<HtmlAttribute>.Clear() {
            this.items.Clear();
        }

        /// <summary>
        /// Retreives existence of supplied item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(HtmlAttribute item) {
            return this.items.Contains(item);
        }

        /// <summary>
        /// Copies collection to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(HtmlAttribute[] array, int arrayIndex) {
            this.items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get Explicit enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator() {
            return this.items.GetEnumerator();
        }

        /// <summary>
        /// Explicit non-generic enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.items.GetEnumerator();
        }

        /// <summary>
        /// Retrieves the index for the supplied item, -1 if not found
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(HtmlAttribute item) {
            return this.items.IndexOf(item);
        }

        /// <summary>
        /// Inserts given item into collection at supplied index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, HtmlAttribute item) {
            if (item == null) {
                throw new ArgumentNullException("item");
            }

            this.Hashitems[item.Name] = item;
            item._ownernode = this._ownernode;
            this.items.Insert(index, item);

            this._ownernode.SetChanged();
        }

        /// <summary>
        /// Explicit collection remove
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<HtmlAttribute>.Remove(HtmlAttribute item) {
            return this.items.Remove(item);
        }

        /// <summary>
        /// Removes the attribute at the specified index.
        /// </summary>
        /// <param name="index">The index of the attribute to remove.</param>
        public void RemoveAt(int index) {
            HtmlAttribute att = this.items[index];
            this.Hashitems.Remove(att.Name);
            this.items.RemoveAt(index);

            this._ownernode.SetChanged();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new attribute to the collection with the given values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value) {
            this.Append(name, value);
        }

        /// <summary>
        /// Inserts the specified attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="newAttribute">The attribute to insert. May not be null.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(HtmlAttribute newAttribute) {
            if (newAttribute == null) {
                throw new ArgumentNullException("newAttribute");
            }

            this.Hashitems[newAttribute.Name] = newAttribute;
            newAttribute._ownernode = this._ownernode;
            this.items.Add(newAttribute);

            this._ownernode.SetChanged();
            return newAttribute;
        }

        /// <summary>
        /// Creates and inserts a new attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="name">The name of the attribute to insert.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(string name) {
            HtmlAttribute att = this._ownernode._ownerdocument.CreateAttribute(name);
            return this.Append(att);
        }

        /// <summary>
        /// Creates and inserts a new attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="name">The name of the attribute to insert.</param>
        /// <param name="value">The value of the attribute to insert.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(string name, string value) {
            HtmlAttribute att = this._ownernode._ownerdocument.CreateAttribute(name, value);
            return this.Append(att);
        }

        /// <summary>
        /// Checks for existance of attribute with given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) {
            for (int i = 0; i < this.items.Count; i++) {
                if (this.items[i].Name.Equals(name.ToLower())) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Inserts the specified attribute as the first node in the collection.
        /// </summary>
        /// <param name="newAttribute">The attribute to insert. May not be null.</param>
        /// <returns>The prepended attribute.</returns>
        public HtmlAttribute Prepend(HtmlAttribute newAttribute) {
            this.Insert(0, newAttribute);
            return newAttribute;
        }

        /// <summary>
        /// Removes a given attribute from the list.
        /// </summary>
        /// <param name="attribute">The attribute to remove. May not be null.</param>
        public void Remove(HtmlAttribute attribute) {
            if (attribute == null) {
                throw new ArgumentNullException("attribute");
            }

            int index = this.GetAttributeIndex(attribute);
            if (index == -1) {
                throw new IndexOutOfRangeException();
            }

            this.RemoveAt(index);
        }

        /// <summary>
        /// Removes an attribute from the list, using its name. If there are more than one attributes with this name, they will all be removed.
        /// </summary>
        /// <param name="name">The attribute's name. May not be null.</param>
        public void Remove(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            string lname = name.ToLower();
            for (int i = 0; i < this.items.Count; i++) {
                HtmlAttribute att = this.items[i];
                if (att.Name == lname) {
                    this.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Remove all attributes in the list.
        /// </summary>
        public void RemoveAll() {
            this.Hashitems.Clear();
            this.items.Clear();

            this._ownernode.SetChanged();
        }

        #endregion

        #region LINQ Methods

        /// <summary>
        /// Returns all attributes with specified name. Handles case insentivity
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <returns></returns>
        public IEnumerable<HtmlAttribute> AttributesWithName(string attributeName) {
            attributeName = attributeName.ToLower();
            for (int i = 0; i < this.items.Count; i++) {
                if (this.items[i].Name.Equals(attributeName)) {
                    yield return this.items[i];
                }
            }
        }

        /// <summary>
        /// Removes all attributes from the collection
        /// </summary>
        public void Remove() {
            foreach (HtmlAttribute item in this.items) {
                item.Remove();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Clears the attribute collection
        /// </summary>
        internal void Clear() {
            this.Hashitems.Clear();
            this.items.Clear();
        }

        internal int GetAttributeIndex(HtmlAttribute attribute) {
            if (attribute == null) {
                throw new ArgumentNullException("attribute");
            }

            for (int i = 0; i < this.items.Count; i++) {
                if ((this.items[i]) == attribute) {
                    return i;
                }
            }

            return -1;
        }

        internal int GetAttributeIndex(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            string lname = name.ToLower();
            for (int i = 0; i < this.items.Count; i++) {
                if ((this.items[i]).Name == lname) {
                    return i;
                }
            }

            return -1;
        }

        #endregion
    }
}