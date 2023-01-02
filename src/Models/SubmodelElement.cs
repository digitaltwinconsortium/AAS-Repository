/*
 * DotAAS Part 2 | HTTP/REST | Entire Interface Collection
 *
 * The entire interface collection as part of Details of the Asset Administration Shell Part 2
 *
 * OpenAPI spec Version: Final-Draft
 *
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AdminShell
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class SubmodelElement : Referable, IEquatable<SubmodelElement>
    {
        /// <summary>
        /// Gets or Sets EmbeddedDataSpecifications
        /// </summary>

        [DataMember(Name="embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        /// <summary>
        /// Gets or Sets SemanticId
        /// </summary>

        [DataMember(Name="semanticId")]
        public Reference SemanticId { get; set; }

        /// <summary>
        /// Gets or Sets Qualifiers
        /// </summary>

        [DataMember(Name="qualifiers")]
        public List<Constraint> Qualifiers { get; set; }

        /// <summary>
        /// Gets or Sets Kind
        /// </summary>

        [DataMember(Name="kind")]
        public ModelingKind Kind { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SubmodelElement {\n");
            sb.Append("  EmbeddedDataSpecifications: ").Append(EmbeddedDataSpecifications).Append("\n");
            sb.Append("  SemanticId: ").Append(SemanticId).Append("\n");
            sb.Append("  Qualifiers: ").Append(Qualifiers).Append("\n");
            sb.Append("  Kind: ").Append(Kind).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SubmodelElement)obj);
        }

        /// <summary>
        /// Returns true if SubmodelElement instances are equal
        /// </summary>
        /// <param name="other">Instance of SubmodelElement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SubmodelElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    EmbeddedDataSpecifications == other.EmbeddedDataSpecifications ||
                    EmbeddedDataSpecifications != null &&
                    EmbeddedDataSpecifications.SequenceEqual(other.EmbeddedDataSpecifications)
                ) &&
                (
                    SemanticId == other.SemanticId ||
                    SemanticId != null &&
                    SemanticId.Equals(other.SemanticId)
                ) &&
                (
                    Qualifiers == other.Qualifiers ||
                    Qualifiers != null &&
                    Qualifiers.SequenceEqual(other.Qualifiers)
                ) &&
                (
                    Kind == other.Kind ||
                    Kind.Equals(other.Kind)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;

                if (EmbeddedDataSpecifications != null)
                    hashCode = hashCode * 59 + EmbeddedDataSpecifications.GetHashCode();

                if (SemanticId != null)
                    hashCode = hashCode * 59 + SemanticId.GetHashCode();

                if (Qualifiers != null)
                    hashCode = hashCode * 59 + Qualifiers.GetHashCode();

                hashCode = hashCode * 59 + Kind.GetHashCode();

                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SubmodelElement left, SubmodelElement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SubmodelElement left, SubmodelElement right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
