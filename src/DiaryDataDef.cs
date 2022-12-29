/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        //
        // hierarchical organized time stamping and transaction approach, coined "DiaryData"
        //

        public class DiaryDataDef
        {
            public enum TimeStampKind { Create, Update }

            [XmlIgnore]
            [JsonIgnore]
            private DateTime[] _timeStamp = new DateTime[2];

            [XmlIgnore]
            [JsonIgnore]
            public DateTime[] TimeStamp { get { return _timeStamp; } }

            /// <summary>
            /// List of entries, timewise one after each other (entries are timestamped).
            /// Note: Default is <c>Entries = null</c>, as handling of many many AAS elements does not
            /// create additional overhead of creating empty lists. An empty list shall be avoided.
            /// </summary>
            public List<IAasDiaryEntry> Entries = null;

            public static void AddAndSetTimestamps(Referable element, IAasDiaryEntry de, bool isCreate = false)
            {
                // trivial
                if (element == null || de == null || element.DiaryData == null)
                    return;

                // add entry
                if (element.DiaryData.Entries == null)
                    element.DiaryData.Entries = new List<IAasDiaryEntry>();
                element.DiaryData.Entries.Add(de);

                // figure out which timestamp
                var tsk = TimeStampKind.Update;
                if (isCreate)
                {
                    tsk = TimeStampKind.Create;
                }

                // set this timestamp (and for the parents, as well)
                IDiaryData el = element;
                while (el?.DiaryData != null)
                {
                    // itself
                    el.DiaryData.TimeStamp[(int)tsk] = DateTime.UtcNow;

                    // go up
                    el = (el as Referable)?.parent as IDiaryData;
                }
            }
        }

    }

    #endregion
}

