global using Microsoft.AspNetCore.Http;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using MediatR;
global using FluentValidation;

global using Together.Application.WebSockets;
global using Together.Persistence;
global using Together.Shared.Constants;
global using Together.Shared.Mediator;
global using Together.Shared.ValueObjects;
global using Together.Shared.Exceptions;
global using Together.Shared.Extensions;
global using Together.Shared.Localization;
global using Together.Shared.Redis;
global using Together.Shared.WebSockets;
global using Together.Shared.Helpers;
global using Together.Domain.Abstractions;
global using Together.Domain.Enums;

global using IBaseRequest = Together.Shared.Mediator.IBaseRequest;