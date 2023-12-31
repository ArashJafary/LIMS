﻿using System.Net;
using LIMS.Application.Exceptions.Http.BBB;
using LIMS.Application.Models;
using LIMS.Domain.Exceptions.Database.BBB;

namespace LIMS.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger) => (_next, _logger) = (next, logger);

    public async Task InvokeAsync(HttpContext context, IWebHostEnvironment environment)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, environment);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment environment)
    {
        var response = context.Response;
        HttpStatusCode status;

        _ = exception switch
        {
            NotAnyEntityFoundInDatabaseException => status = HttpStatusCode.NotFound,
            ArgumentNullException => status = HttpStatusCode.BadRequest,
            UserCannotLoginException => status = HttpStatusCode.BadRequest,
            EntityConnotAddInDatabaseException => status = HttpStatusCode.NotFound,
            _ => status = HttpStatusCode.InternalServerError
        };

        string errorDetailsJson;

        if (environment.IsDevelopment())
            errorDetailsJson = new ErrorDetailsModel(exception.Message, exception.StackTrace!).ToString();
        else
            errorDetailsJson = new ErrorDetailsModel(exception.Message).ToString();

        response.ContentType = "application/json";
        response.ContentLength = errorDetailsJson.Length;
        response.StatusCode = (int)status;

        _logger.LogError(exception, exception.Message);

        return response.WriteAsync(errorDetailsJson);
    }
}

public static class GlobalExceptionHandlerMiddelwareExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
