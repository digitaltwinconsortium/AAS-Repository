//-----------------------------------------------------------------------------
//
// <copyright file="PartManifestEntry.cs" company="Microsoft">
//    Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// Description:
//  Simple struct for maintaining information of a Part found in an Xml signature manifest
//
// History:
//  02/07/2005: BruceMac: Initial Implementation
//  11/30/2005: BruceMac: Security Mitigations
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.Net.Mime;

namespace MS.Internal.IO.Packaging.AdminShell
{
    /// <summary>
    /// Represents parsed value for a single Part/Relationship entry in the Manifest
    /// </summary>
    internal struct PartManifestEntry
    {
        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------
        // is this a relationship entry?
        internal bool IsRelationshipEntry { get { return _relationshipSelectors != null; } }

        internal Uri Uri { get { return _uri; } }
        internal ContentType ContentType { get { return _contentType; } }
        internal String HashAlgorithm { get { return _hashAlgorithm; } }
        internal String HashValue { get { return _hashValue; } }
        internal List<String> Transforms { get { return _transforms; } }
        internal List<PackageRelationshipSelector> RelationshipSelectors { get { return _relationshipSelectors; } }      // null if Part entry
        internal Uri OwningPartUri // only valid if IsRelationshipEntry
        {
            get
            {
                Debug.Assert(_owningPartUri != null, "Logic error: OwningPart is null on a non-Relationship entry");
                return _owningPartUri;
            }
        }

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">part uri of part in question</param>
        /// <param name="contentType">type of part</param>
        /// <param name="hashAlgorithm">digest method</param>
        /// <param name="hashValue">value of the hash calculation extracted from the signature Xml</param>
        /// <param name="transforms">ordered transform list - may be null</param>
        /// <param name="relationshipSelectors">may be null but can never be empty</param>
        internal PartManifestEntry(Uri uri, ContentType contentType, String hashAlgorithm,
            String hashValue, List<String> transforms, List<PackageRelationshipSelector> relationshipSelectors)
        {
            Debug.Assert(uri != null);
            Debug.Assert(contentType != null);
            Debug.Assert(hashAlgorithm != null);

            _uri = uri;
            _contentType = contentType;
            _hashAlgorithm = hashAlgorithm;
            _hashValue = hashValue;
            _transforms = transforms;
            _relationshipSelectors = relationshipSelectors;
            _owningPartUri = null;

            if (_relationshipSelectors != null)
            {
                Debug.Assert(relationshipSelectors.Count > 0);
#if DEBUG
                Debug.Assert(DoAllSelectorsHaveSameOwningPart(relationshipSelectors), "All relationship selectors should have same owningPart for a given part manifest");
#endif
                //Get owning Part uri from one of the relationship selectors
                _owningPartUri = relationshipSelectors[0].SourceUri;
            }
        }


#if DEBUG

        private bool DoAllSelectorsHaveSameOwningPart(IEnumerable<PackageRelationshipSelector> relationshipSelectors)
        {
            Uri owningPartUri = null;
            foreach (PackageRelationshipSelector selector in relationshipSelectors)
            {
                if (owningPartUri == null)
                {
                    owningPartUri = selector.SourceUri;
                }
                else
                    if (Uri.Compare(owningPartUri, selector.SourceUri, UriComponents.SerializationInfoString, UriFormat.UriEscaped, StringComparison.Ordinal) != 0)
                    return false;
            }
            return true;
        }

#endif

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
        private Uri _owningPartUri;             // owing part if this is a Relationship Uri
        private Uri _uri;
        private ContentType _contentType;
        private String _hashAlgorithm;
        private String _hashValue;
        private List<String> _transforms;
        private List<PackageRelationshipSelector> _relationshipSelectors;    // null if this is a Part
    }
}
