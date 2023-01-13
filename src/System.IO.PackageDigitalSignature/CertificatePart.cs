//-----------------------------------------------------------------------------
//
// <copyright file="Certificate.cs" company="Microsoft">
//    Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// Description:
//  Handles serialization to/from X509 Certificate part (X509v3 = ASN.1 DER format)
//
// History:
//  03/22/2004: BruceMac: Initial Implementation
//
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;                                        // for Stream
using System.IO.Packaging;
using System.IO.Packaging.AdminShell;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using static System.IO.Packaging.AdminShell.PackUriHelper;

namespace MS.Internal.IO.Packaging.AdminShell
{
    /// <summary>
    /// CertificatePart
    /// </summary>
    internal class CertificatePart
    {
        #region Internal Members
        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------
        /// <summary>
        /// Type of relationship to a Certificate Part
        /// </summary>
        static internal string RelationshipType
        {
            get
            {
                return _certificatePartRelationshipType;
            }
        }

        /// <summary>
        /// Prefix of auto-generated Certificate Part names
        /// </summary>
        static internal string PartNamePrefix
        {
            get
            {
                return _certificatePartNamePrefix;
            }
        }

        /// <summary>
        /// Extension of Certificate Part file names
        /// </summary>
        static internal string PartNameExtension
        {
            get
            {
                return _certificatePartNameExtension;
            }
        }

        /// <summary>
        /// ContentType of Certificate Parts
        /// </summary>
        static internal ContentType ContentType
        {
            get
            {
                return _certificatePartContentType;
            }
        }

        internal Uri Uri
        {
            get
            {
                return _part.Uri;
            }
        }

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------
        /// <summary>
        /// Certificate to associate with this Certificate Part
        /// </summary>
        /// <value></value>
        /// <exception cref="FileFormatException">stream is too large</exception>
        internal X509Certificate2 GetCertificate()
        {
            // lazy init
            if (_certificate == null)
            {
                // obtain from the part
                using (Stream s = _part.GetStream())
                {
                    if (s.Length > 0)
                    {
                        // throw if stream is beyond any reasonable length
                        if (s.Length > _maximumCertificateStreamLength)
                            throw new FileFormatException("CorruptedData");

                        // X509Certificate constructor desires a byte array
                        Byte[] byteArray = new Byte[s.Length];
                        PackagingUtilities.ReliableRead(s, byteArray, 0, (int)s.Length);
                        _certificate = new X509Certificate2(byteArray);
                    }
                }
            }
            return _certificate;
        }

        internal void SetCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException("certificate");

            _certificate = certificate;

            // persist to the part
            Byte[] byteArray = _certificate.GetRawCertData();

            // FileMode.Create will ensure that the stream will shrink if overwritten
            using (Stream s = _part.GetStream(FileMode.Create, FileAccess.Write))
            {
                s.Write(byteArray, 0, byteArray.Length);
            }
        }

        /// <summary>
        /// CertificatePart constructor
        /// </summary>
        internal CertificatePart(Package container, Uri partName)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            if (partName == null)
                throw new ArgumentNullException("partName");

            partName = ValidatePartUri(partName);

            // create if not found
            if (container.PartExists(partName))
            {
                // open the part
                _part = container.GetPart(partName);

                // ensure the part is of the expected type
                if (PackageDigitalSignatureManager.AreTypeAndSubTypeEqual(new ContentType(_part.ContentType), _certificatePartContentType) == false)
                    throw new FileFormatException("CertificatePartContentTypeMismatch");
            }
            else
            {
                // create the part
                _part = container.CreatePart(partName, _certificatePartContentType.ToString());
            }
        }

        /// <summary>
        /// This method is used to validate a part Uri
        /// This method does not perform a case sensitive check of the Uri
        /// </summary>
        /// <param name="partUri">The string that represents the part within a package</param>
        /// <returns>Returns the part uri if it is valid</returns>
        /// <exception cref="ArgumentNullException">If partUri parameter is null</exception>
        /// <exception cref="ArgumentException">If partUri parameter is an absolute Uri</exception>
        /// <exception cref="ArgumentException">If partUri parameter is empty</exception>
        /// <exception cref="ArgumentException">If partUri parameter does not start with a "/"</exception>
        /// <exception cref="ArgumentException">If partUri parameter starts with two "/"</exception>
        /// <exception cref="ArgumentException">If partUri parameter ends with a "/"</exception>
        /// <exception cref="ArgumentException">If partUri parameter has a fragment</exception>
        /// <exception cref="ArgumentException">If partUri parameter has some escaped characters that should not be escaped
        /// or some characters that should be escaped are not escaped.</exception>
        internal static ValidatedPartUri ValidatePartUri(Uri partUri)
        {
            if (partUri is ValidatedPartUri)
                return (ValidatedPartUri)partUri;

            string partUriString;
            Exception exception = GetExceptionIfPartUriInvalid(partUri, out partUriString);
            if (exception != null)
            {
                Debug.Assert(partUriString != null && partUriString.Length == 0);
                throw exception;
            }
            else
            {
                Debug.Assert(partUriString != null && partUriString.Length > 0);
                return new ValidatedPartUri(partUriString);
            }
        }

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
        private PackagePart _part;          // part that houses the certificate
        private X509Certificate2 _certificate;   // certificate itself

        // certificate part constants
        private static readonly ContentType _certificatePartContentType =
            new ContentType("application/vnd.openxmlformats-package.digital-signature-certificate");
        private static readonly string _certificatePartNamePrefix = "/package/services/digital-signature/certificate/";
        private static readonly string _certificatePartNameExtension = ".cer";
        private static readonly string _certificatePartRelationshipType = "http://schemas.openxmlformats.org/package/2006/relationships/digital-signature/certificate";
        private static long _maximumCertificateStreamLength = 0x40000;   // 4MB
        #endregion Private Members
    }
}
