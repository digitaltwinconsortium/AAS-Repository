/********************************************************************************
* Copyright (c) {2019 - 2024} Contributors to the Eclipse Foundation
*
* See the NOTICE file(s) distributed with this work for additional
* information regarding copyright ownership.
*
* This program and the accompanying materials are made available under the
* terms of the Apache License Version 2.0 which is available at
* https://www.apache.org/licenses/LICENSE-2.0
*
* SPDX-License-Identifier: Apache-2.0
********************************************************************************/

using IO.Swagger.Models;
using System.Collections.Generic;

namespace AdminShell
{
    public class LevelExtentTransformer
    {
        public static Submodel TransformSubmodel(Submodel that, LevelExtentModifierContext context)
        {
            Submodel output = new(that);
            if (output != null)
            {
                context.IsRoot = false;
                if (context.Level == LevelEnum.Core)
                {
                    context.IncludeChildren = false;
                }

                if (that.SubmodelElements != null)
                {
                    output.SubmodelElements = new List<SubmodelElement>();
                    foreach (var child in that.SubmodelElements)
                    {
                        context.IncludeChildren = false;
                        output.SubmodelElements.Add(TransformSubmodelElement(child, context));
                    }
                }
            }

            return output;
        }

        public static SubmodelElement TransformSubmodelElement(SubmodelElement that, LevelExtentModifierContext context)
        {
            SubmodelElement output = new SubmodelElement(that);
            if (output != null)
            {
                context.IsRoot = false;
                if (context.Level != LevelEnum.Core)
                {
                    if (that is SubmodelElementList)
                    {
                        output = new SubmodelElementList((SubmodelElementList)that);
                        foreach (var child in ((SubmodelElementList)that).Value)
                        {
                            ((SubmodelElementList)output).Value.Add(TransformSubmodelElement(child, context));
                        }
                    }
                }
            }

            return output;
        }

        public static SubmodelElementList TransformSubmodelElementList(SubmodelElementList that, LevelExtentModifierContext context)
        {
            SubmodelElementList output = new(that);
            if (output != null)
            {
                context.IsRoot = false;
                if (context.IncludeChildren)
                {
                    if (context.Level == LevelEnum.Core)
                    {
                        context.IncludeChildren = false;
                    }

                    if (that.Value != null)
                    {
                        output.Value = new List<SubmodelElement>();
                        foreach (SubmodelElement child in that.Value)
                        {
                            output.Value.Add(TransformSubmodelElement(child, context));
                        }
                    }
                }
                else
                {
                    output.Value = null;
                }
            }

            return output;
        }
    }
}
