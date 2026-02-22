using DevConnect.Application.Attributes;
using DevConnect.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace DevConnect.Application.Configurations;

public class SwaggerCustomOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ApplySuccessBody(operation, context);
        ApplyApiErrors(operation, context);
        Apply422Errors(operation, context);
        Apply401Errors(operation, context);
        Apply500Errors(operation, context);
    }

    private static void ApplySuccessBody(OpenApiOperation operation, OperationFilterContext context)
    {
        var schemaRegistry = context.SchemaGenerator;

        if (context.MethodInfo.GetCustomAttributes(true)
                .SingleOrDefault(attr => attr is SuccessResponseAttribute) is SuccessResponseAttribute responseAttr)
        {

            var type = responseAttr.Type;
            var message = responseAttr.Message;
            var dataResponseType = typeof(DataResponse<>).MakeGenericType(type);
            var dataResponseSchema = schemaRegistry.GenerateSchema(dataResponseType, context.SchemaRepository);

            var content = new OpenApiResponse
            {
                Description = message,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = dataResponseSchema
                    }
                }
            };

            if (operation.Responses.ContainsKey("200"))
            {
                operation.Responses["200"] = content;
            }
            else
            {
                operation.Responses.Add("200", content);
            }
        }
    }

    private static void Apply422Errors(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethodAttr = context.MethodInfo.GetCustomAttributes(true)
           .OfType<HttpMethodAttribute>()
           .FirstOrDefault();

        if (httpMethodAttr != null)
        {
            var httpMethod = httpMethodAttr.HttpMethods.FirstOrDefault();
            operation.OperationId = $"{httpMethod} - {context.MethodInfo.Name}";

            if (string.Equals(httpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {

                var validationErrorSchema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { "code", new OpenApiSchema { Type = "string", Enum = new List<IOpenApiAny> { new OpenApiString(CommonErrorCodes.BadInput) } } },
                        { "message", new OpenApiSchema { Type = "string", Default = new OpenApiString("Validation failed") } },
                        { "errors", new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema
                                {
                                    Type = "object",
                                    Properties = new Dictionary<string, OpenApiSchema>
                                    {
                                        { "path", new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string" } } },
                                        { "code", new OpenApiSchema { Type = "string" } },
                                        { "message", new OpenApiSchema { Type = "string" } }
                                    }
                                }
                            }
                        }
                    }
                };

                ApplySchema(operation, validationErrorSchema, "422", "Validation Failed");
            }
        }
    }

    private static void Apply401Errors(OpenApiOperation operation, OperationFilterContext context)
    {
        var allowAnonymousAttribute = context.MethodInfo.GetCustomAttributes(true)
          .OfType<AllowAnonymousAttribute>()
          .FirstOrDefault();

        if (allowAnonymousAttribute == null)
        {
            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "code", new OpenApiSchema { Type = "string", Enum = new List<IOpenApiAny> { new OpenApiString(CommonErrorCodes.Unauthorized) } } },
                    { "message", new OpenApiSchema { Type = "string", Default = new OpenApiString("Unauthorized") } }
                }
            };

            ApplySchema(operation, schema, "401", "Unauthorized");
        }
    }

    private static void Apply500Errors(OpenApiOperation operation, OperationFilterContext context)
    {
        var unauthorizedErrorSchema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
        {
            { "code", new OpenApiSchema { Type = "string", Enum = new List<IOpenApiAny> { new OpenApiString(CommonErrorCodes.ServiceError) } } },
            { "message", new OpenApiSchema { Type = "string", Default = new OpenApiString("Internal Server Error") } }
            }
        };

        ApplySchema(operation, unauthorizedErrorSchema, "500", "Internal Server Error");
    }

    private static void ApplyApiErrors(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.MethodInfo.GetCustomAttributes(true)
            .OfType<ProducesApiErrorAttribute>()
            .ToList();

        if (attributes.Count != 0)
        {
            var errorCodes = new List<IOpenApiAny>();

            var description = new StringBuilder("<strong>Possible Errors</strong> <br><br>");

            foreach (var attribute in attributes)
            {
                errorCodes.Add(new OpenApiString(attribute.Code));
                description.Append($"<strong>{attribute.Code}</strong> : {attribute.Description} <br>");
            }

            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
            {
                {
                    "code",
                    new OpenApiSchema
                    {
                        Type = "string",
                        Enum = errorCodes
                    }
                },
                {
                    "message",
                    new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("Validation failed")
                    }
                }
            }
            };

            ApplySchema(operation, schema, "400", description.ToString());
        }
    }

    private static void ApplySchema(OpenApiOperation operation, OpenApiSchema schema, string code, string description)
    {
        //TODO check if code already added or not
        operation.Responses.Add(code, new OpenApiResponse
        {
            Description = description.ToString(),
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        });
    }
}