﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Protsyk.PMS.FullText.Core
{
    public struct Occurrence : IEquatable<Occurrence>, IComparable<Occurrence>
    {
        #region Fields
        /// <summary>
        /// Not valid Id
        /// </summary>
        public static readonly ulong NoId = 0;

        /// <summary>
        /// Empty occurrence
        /// </summary>
        public static readonly Occurrence Empty = O(NoId, NoId, NoId);

        private static readonly Regex regParse = new Regex("^\\[(?<docId>\\d+),(?<fieldId>\\d+),(?<tokenId>\\d+)\\]$",
                                                           RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Document Id
        /// </summary>
        public readonly ulong DocumentId;

        /// <summary>
        /// Fields Id
        /// </summary>
        public readonly ulong FieldId;

        /// <summary>
        /// Token Id
        /// </summary>
        public readonly ulong TokenId;
        #endregion

        #region Static Methods
        /// <summary>
        /// Construct occurrence
        /// </summary>
        public static Occurrence O(ulong documentId, ulong fieldId, ulong tokenId)
        {
            return new Occurrence(documentId, fieldId, tokenId);
        }

        public static Occurrence Parse(string text)
        {
            var match = regParse.Match(text);
            if (!match.Success)
            {
                throw new InvalidOperationException($"Occurrence text has invalid format: {text}");
            }

            return new Occurrence(
                ulong.Parse(match.Groups["docId"].Value),
                ulong.Parse(match.Groups["fieldId"].Value),
                ulong.Parse(match.Groups["tokenId"].Value));
        }
        #endregion

        #region Methods
        public Occurrence(ulong documentId, ulong fieldId, ulong tokenId)
        {
            this.DocumentId = documentId;
            this.FieldId = fieldId;
            this.TokenId = tokenId;
        }

        public override string ToString()
        {
            return $"[{DocumentId},{FieldId},{TokenId}]";
        }
        #endregion

        #region IEquatable<Occurrence>
        public bool Equals(Occurrence other)
        {
            return DocumentId == other.DocumentId &&
                   FieldId == other.FieldId && 
                   TokenId == other.TokenId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Occurrence && Equals((Occurrence) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DocumentId.GetHashCode();
                hashCode = (hashCode * 397) ^ FieldId.GetHashCode();
                hashCode = (hashCode * 397) ^ TokenId.GetHashCode();
                return hashCode;
            }
        }
        #endregion

        #region IComparable
        public int CompareTo(Occurrence other)
        {
            var documentIdComparison = DocumentId.CompareTo(other.DocumentId);
            if (documentIdComparison != 0)
            {
                return documentIdComparison;
            }

            var fieldIdComparison = FieldId.CompareTo(other.FieldId);
            if (fieldIdComparison != 0)
            {
                return fieldIdComparison;
            }

            return TokenId.CompareTo(other.TokenId);
        }
        #endregion

        #region Operators
        public static bool operator <(Occurrence l, Occurrence r)
        {
            return l.CompareTo(r) < 0;
        }

        public static bool operator >(Occurrence l, Occurrence r)
        {
            return l.CompareTo(r) > 0;
        }

        public static bool operator ==(Occurrence l, Occurrence r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Occurrence l, Occurrence r)
        {
            return !l.Equals(r);
        }
        #endregion
    }
}
