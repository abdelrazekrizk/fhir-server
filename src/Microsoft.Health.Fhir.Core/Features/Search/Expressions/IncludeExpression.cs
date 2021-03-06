﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using EnsureThat;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Features.Search.Expressions
{
    /// <summary>
    /// Represents an include expression (where an additional resource is included based on a reference)
    /// </summary>
    public class IncludeExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncludeExpression"/> class.
        /// </summary>
        /// <param name="resourceType">The resource that supports the reference.</param>
        /// <param name="referenceSearchParameter">THe search parameter that establishes the reference relationship.</param>
        /// <param name="sourceResourceType">The source type of the reference.</param>
        /// <param name="targetResourceType">The target type of the reference.</param>
        /// <param name="wildCard">If this is a wildcard reference include (include all referenced resources).</param>
        /// <param name="reversed">If this is a reversed include (revinclude) expression.</param>
        public IncludeExpression(string resourceType, SearchParameterInfo referenceSearchParameter, string sourceResourceType, string targetResourceType, bool wildCard, bool reversed)
        {
            EnsureArg.IsNotNullOrWhiteSpace(resourceType, nameof(resourceType));

            if (!wildCard)
            {
                EnsureArg.IsNotNull(referenceSearchParameter, nameof(referenceSearchParameter));
            }

            if (reversed)
            {
                EnsureArg.IsNotNull(sourceResourceType, nameof(sourceResourceType));
            }

            ResourceType = resourceType;
            ReferenceSearchParameter = referenceSearchParameter;
            TargetResourceType = targetResourceType;
            WildCard = wildCard;
            Reversed = reversed;
            SourceResourceType = sourceResourceType;
        }

        /// <summary>
        /// Gets the resource type which is being searched.
        /// </summary>
        public string ResourceType { get; }

        /// <summary>
        /// Gets the reference search parameter for the relationship.
        /// </summary>
        public SearchParameterInfo ReferenceSearchParameter { get; }

        /// <summary>
        /// Gets the target resource type. Value will be null if none are specified.
        /// </summary>
        public string TargetResourceType { get; }

        /// <summary>
        /// Gets the source resource type. Value will be null if none are specified.
        /// </summary>
        public string SourceResourceType { get; }

        /// <summary>
        /// Gets if the include is a wildcard include.
        /// </summary>
        public bool WildCard { get; }

        /// <summary>
        /// Get if the expression is reversed.
        /// </summary>
        public bool Reversed { get; }

        public override TOutput AcceptVisitor<TContext, TOutput>(IExpressionVisitor<TContext, TOutput> visitor, TContext context)
        {
            EnsureArg.IsNotNull(visitor, nameof(visitor));

            return visitor.VisitInclude(this, context);
        }

        public override string ToString()
        {
            if (WildCard)
            {
                return "(Include wildcard)";
            }

            var targetType = TargetResourceType != null ? $":{TargetResourceType}" : string.Empty;
            return $"({(Reversed ? "Reverse " : string.Empty)}Include {ReferenceSearchParameter.Name}{targetType})";
        }
    }
}
