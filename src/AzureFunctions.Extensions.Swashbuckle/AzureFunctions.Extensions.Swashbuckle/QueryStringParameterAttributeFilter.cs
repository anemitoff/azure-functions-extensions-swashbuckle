﻿using System.Linq;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AzureFunctions.Extensions.Swashbuckle
{
    internal class QueryStringParameterAttributeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType != null)
            {
                var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<QueryStringParameterAttribute>();

                foreach (var attribute in attributes)
                {
                    var attributeTypeName = "string";
                    if (attribute.DataType != null)
                    {
                        attributeTypeName = attribute.DataType.ToString();
                    }

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = attribute.Name,
                        Description = attribute.Description,
                        In = ParameterLocation.Query,
                        Required = attribute.Required,
                        Schema = new OpenApiSchema {Type = attributeTypeName}
                    });
                }
            }
        }
    }
}