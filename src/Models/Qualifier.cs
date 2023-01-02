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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace AdminShell
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class Qualifier : HasSemantics, IEquatable<Qualifier>
    {
        /// <summary>
        /// Gets or Sets Value
        /// </summary>

        [DataMember(Name="Value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets ValueId
        /// </summary>

        [DataMember(Name="ValueId")]
        public Reference ValueId { get; set; }

        /// <summary>
        /// Gets or Sets ValueType
        /// </summary>

        [DataMember(Name="valueType")]
        public ValueTypeEnum ValueType { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [Required]

        [DataMember(Name="Type")]
        public string Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Qualifier {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  ValueId: ").Append(ValueId).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Qualifier)obj);
        }

        /// <summary>
        /// Returns true if Qualifier instances are equal
        /// </summary>
        /// <param name="other">Instance of Qualifier to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Qualifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Value == other.Value ||
                    Value != null &&
                    Value.Equals(other.Value)
                ) &&
                (
                    ValueId == other.ValueId ||
                    ValueId != null &&
                    ValueId.Equals(other.ValueId)
                ) &&
                (
                    ValueType == other.ValueType ||
                    ValueType.Equals(other.ValueType)
                ) &&
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
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

                if (Value != null)
                    hashCode = hashCode * 59 + Value.GetHashCode();

                if (ValueId != null)
                    hashCode = hashCode * 59 + ValueId.GetHashCode();

                hashCode = hashCode * 59 + ValueType.GetHashCode();

                if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();

                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Qualifier left, Qualifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Qualifier left, Qualifier right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
