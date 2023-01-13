//---------------------------------------------------------------------------
//
// <copyright file="PackagingUtilities.cs" company="Microsoft">
//    Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// Description:
//
// History:
//  05/13/2004: Microsoft   Creation
//
//---------------------------------------------------------------------------

using System;
using System.Diagnostics;       // For Debug.Assert
using System.IO;
using System.Net.Mime;
using System.Text;              // For Encoding
using System.Xml;               // For XmlReader

namespace MS.Internal.IO.Packaging.AdminShell
{
    internal static class PackagingUtilities
    {
        //------------------------------------------------------
        //
        //  Internal Fields
        //
        //------------------------------------------------------
        internal static readonly string RelationshipNamespaceUri = "http://schemas.openxmlformats.org/package/2006/relationships";
        internal static readonly ContentType RelationshipPartContentType
            = new ContentType("application/vnd.openxmlformats-package.relationships+xml");

        internal const string ContainerFileExtension = "xps";
        internal const string XamlFileExtension = "xaml";

        #region Internal Methods

        /// <summary>
        /// This method is used to determine if we support a given Encoding as per the
        /// OPC and XPS specs. Currently the only two encodings supported are UTF-8 and
        /// UTF-16 (Little Endian and Big Endian)
        /// </summary>
        /// <param name="reader">XmlTextReader</param>
        /// <returns>throws an exception if the encoding is not UTF-8 or UTF-16</returns>
        internal static void PerformInitailReadAndVerifyEncoding(XmlTextReader reader)
        {
            Debug.Assert(reader != null && reader.ReadState == ReadState.Initial);

            //If the first node is XmlDeclaration we check to see if the encoding attribute is present
            if (reader.Read() && reader.NodeType == XmlNodeType.XmlDeclaration && reader.Depth == 0)
            {
                string encoding;
                encoding = reader.GetAttribute(_encodingAttribute);

                if (encoding != null && encoding.Length > 0)
                {
                    encoding = encoding.ToUpperInvariant();

                    //If a non-empty encoding attribute is present [for example - <?xml version="1.0" encoding="utf-8" ?>]
                    //we check to see if the value is either "utf-8" or utf-16. Only these two values are supported
                    //Note: For Byte order markings that require additional information to be specified in
                    //the encoding attribute in XmlDeclaration have already been ruled out by this check as we allow for
                    //only two valid values.
                    if (String.CompareOrdinal(encoding, _webNameUTF8) == 0
                        || String.CompareOrdinal(encoding, _webNameUnicode) == 0)
                        return;
                    else
                        //if the encoding attribute has any other value we throw an exception
                        throw new FileFormatException("EncodingNotSupported");
                }
            }

            //if the XmlDeclaration is not present, or encoding attribute is not present, we
            //base our decision on byte order marking. reader.Encoding will take that into account
            //and return the correct value.
            //Note: For Byte order markings that require additional information to be specified in
            //the encoding attribute in XmlDeclaration have already been ruled out by the check above.
            //Note: If not encoding attribute is present or no byte order marking is present the
            //encoding default to UTF8
            if (!(reader.Encoding is UnicodeEncoding || reader.Encoding is UTF8Encoding))
                throw new FileFormatException("EncodingNotSupported");
        }

        /// <summary>
        /// Read utility that is guaranteed to return the number of bytes requested
        /// if they are available.
        /// </summary>
        /// <param name="stream">stream to read from</param>
        /// <param name="buffer">buffer to read into</param>
        /// <param name="offset">offset in buffer to write to</param>
        /// <param name="count">bytes to read</param>
        /// <returns>bytes read</returns>
        /// <remarks>Normal Stream.Read does not guarantee how many bytes it will
        /// return.  This one does.</remarks>
        internal static int ReliableRead(Stream stream, byte[] buffer, int offset, int count)
        {
            return ReliableRead(stream, buffer, offset, count, count);
        }

        /// <summary>
        /// Read utility that is guaranteed to return the number of bytes requested
        /// if they are available.
        /// </summary>
        /// <param name="stream">stream to read from</param>
        /// <param name="buffer">buffer to read into</param>
        /// <param name="offset">offset in buffer to write to</param>
        /// <param name="requestedCount">count of bytes that we would like to read (max read size to try)</param>
        /// <param name="requiredCount">minimal count of bytes that we would like to read (min read size to achieve)</param>
        /// <returns>bytes read</returns>
        /// <remarks>Normal Stream.Read does not guarantee how many bytes it will
        /// return.  This one does.</remarks>
        internal static int ReliableRead(Stream stream, byte[] buffer, int offset, int requestedCount, int requiredCount)
        {
            Debug.Assert(stream != null);
            Debug.Assert(buffer != null);
            Debug.Assert(buffer.Length > 0);
            Debug.Assert(offset >= 0);
            Debug.Assert(requestedCount >= 0);
            Debug.Assert(requiredCount >= 0);
            Debug.Assert(checked(offset + requestedCount <= buffer.Length));
            Debug.Assert(requiredCount <= requestedCount);

            // let's read the whole block into our buffer
            int totalBytesRead = 0;
            while (totalBytesRead < requiredCount)
            {
                int bytesRead = stream.Read(buffer,
                                offset + totalBytesRead,
                                requestedCount - totalBytesRead);
                if (bytesRead == 0)
                {
                    break;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }

        /// <summary>
        /// This method returns the count of xml attributes other than:
        /// 1. xmlns="namespace"
        /// 2. xmlns:someprefix="namespace"
        /// Reader should be positioned at the Element whose attributes
        /// are to be counted.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>An integer indicating the number of non-xmlns attributes</returns>
        internal static int GetNonXmlnsAttributeCount(XmlReader reader)
        {
            Debug.Assert(reader != null, "xmlReader should not be null");
            Debug.Assert(reader.NodeType == XmlNodeType.Element, "XmlReader should be positioned at an Element");

            int readerCount = 0;

            //If true, reader moves to the attribute
            //If false, there are no more attributes (or none)
            //and in that case the position of the reader is unchanged.
            //First time through, since the reader will be positioned at an Element,
            //MoveToNextAttribute is the same as MoveToFirstAttribute.
            while (reader.MoveToNextAttribute())
            {
                if (String.CompareOrdinal(reader.Name, XmlNamespace) != 0 &&
                    String.CompareOrdinal(reader.Prefix, XmlNamespace) != 0)
                    readerCount++;
            }

            //re-position the reader to the element
            reader.MoveToElement();

            return readerCount;
        }

        #endregion Internal Methods

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
        /// <summary>
        /// Synchronize access to IsolatedStorage methods that can step on each-other
        /// </summary>
        /// <remarks>See PS 1468964 for details.</remarks>
        private const string XmlNamespace = "xmlns";
        private const string _encodingAttribute = "encoding";
        private static readonly string _webNameUTF8 = Encoding.UTF8.WebName.ToUpperInvariant();
        private static readonly string _webNameUnicode = Encoding.Unicode.WebName.ToUpperInvariant();
    }
}
