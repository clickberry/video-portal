// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData.Query;
using AutoMapper;
using Microsoft.Data.OData.Query;
using Microsoft.Data.OData.Query.SemanticAst;
using Portal.Domain;
using Portal.Domain.Admin;

namespace Portal.Mappers.TypeConverters
{
    public sealed class ODataQueryOptionsToDataQueryOptions : ITypeConverter<ODataQueryOptions, DataQueryOptions>
    {
        private const int DefaultTopValue = 20;

        public DataQueryOptions Convert(ResolutionContext context)
        {
            var options = (ODataQueryOptions)context.SourceValue;

            var filter = new DataQueryOptions
            {
                Take = options.Top != null ? options.Top.Value : DefaultTopValue,
                Skip = options.Skip != null ? options.Skip.Value : (int?)null,
                Count = options.InlineCount != null && options.InlineCount.Value == InlineCountValue.AllPages
            };

            // parsing order by
            if (options.OrderBy != null)
            {
                var firstOrderByNode = options.OrderBy.OrderByNodes.FirstOrDefault() as OrderByPropertyNode;
                if (firstOrderByNode != null)
                {
                    filter.OrderBy = firstOrderByNode.Property.Name;
                    filter.OrderByDirection = firstOrderByNode.Direction == OrderByDirection.Descending
                        ? OrderByDirections.Desc
                        : OrderByDirections.Asc;
                }
            }

            // parsing filter
            if (options.Filter != null)
            {
                filter.Filters = ParseNode(options.Filter.FilterClause.Expression);
            }

            return filter;
        }


        /// <summary>
        ///     Original from
        ///     http://aspnet.codeplex.com/SourceControl/changeset/view/72014f4c779e#Samples/WebApi/NHibernateQueryableSample/System.Web.Http.OData.NHibernate/NHibernateFilterBinder.cs
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static List<DataFilterRule> ParseNode(QueryNode node)
        {
            var singleValueNode = node as SingleValueNode;
            if (singleValueNode != null)
            {
                if (node.Kind == QueryNodeKind.BinaryOperator)
                {
                    return ParseBinaryOperatorNode(node as BinaryOperatorNode);
                }
            }

            var binaryValueNode = node as BinaryOperatorNode;
            if (binaryValueNode != null)
            {
                return ParseBinaryOperatorNode(binaryValueNode);
            }

            var convertNode = node as ConvertNode;
            if (convertNode != null)
            {
                return ParseNode(convertNode.Source);
            }

            throw new NotSupportedException(String.Format("Nodes of type {0} are not supported", node.Kind));
        }

        private static List<DataFilterRule> ParseBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode)
        {
            if (binaryOperatorNode.OperatorKind == BinaryOperatorKind.And)
            {
                List<DataFilterRule> leftRules = ParseNode(binaryOperatorNode.Left);
                List<DataFilterRule> rightRules = ParseNode(binaryOperatorNode.Right);

                var result = new List<DataFilterRule>(leftRules);
                result.AddRange(rightRules);

                return result;
            }


            var filterRule = new DataFilterRule
            {
                Name = ParseValue(binaryOperatorNode.Left).ToString(),
                Value = ParseValue(binaryOperatorNode.Right)
            };

            switch (binaryOperatorNode.OperatorKind)
            {
                case BinaryOperatorKind.Equal:
                    filterRule.Type = DataFilterTypes.Equal;
                    break;
                case BinaryOperatorKind.LessThan:
                    filterRule.Type = DataFilterTypes.LessThan;
                    break;
                case BinaryOperatorKind.LessThanOrEqual:
                    filterRule.Type = DataFilterTypes.LessThanOrEqual;
                    break;
                case BinaryOperatorKind.GreaterThan:
                    filterRule.Type = DataFilterTypes.GreaterThan;
                    break;
                case BinaryOperatorKind.GreaterThanOrEqual:
                    filterRule.Type = DataFilterTypes.GreaterThanOrEqual;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Binary operand {0} is not supported",
                        binaryOperatorNode.OperatorKind));
            }

            return new List<DataFilterRule> { filterRule };
        }

        private static object ParseValue(QueryNode node)
        {
            while (true)
            {
                var singleValueNode = node as SingleValueNode;
                if (singleValueNode != null)
                {
                    switch (node.Kind)
                    {
                        case QueryNodeKind.SingleValuePropertyAccess:
                            var singlePropertyAccessNode = (SingleValuePropertyAccessNode)node;
                            return singlePropertyAccessNode.Property.Name;

                        case QueryNodeKind.Constant:
                            var constantNode = (ConstantNode)node;
                            return constantNode.Value;

                        case QueryNodeKind.Convert:
                            var convertNode = (ConvertNode)node;
                            node = convertNode.Source;
                            continue;
                    }
                }

                throw new NotSupportedException(String.Format("Nodes of type {0} are not supported", node.Kind));
            }
        }
    }
}