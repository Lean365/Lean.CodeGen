// 系统命名空间
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;

// Microsoft 扩展
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using NLog;

// 第三方库
global using Autofac;
global using Mapster;
global using Swashbuckle.AspNetCore.Annotations;
global using Lean.CodeGen.Common.Attributes;
// 项目命名空间
//global using Lean.CodeGen.Common.Enums;
//global using Lean.CodeGen.Common.Extensions;
//global using Lean.CodeGen.Common.Helpers;
//global using Lean.CodeGen.Application.Services;
//global using Lean.CodeGen.Domain.Entities;
global using Lean.CodeGen.Common.Localization;
global using Lean.CodeGen.Domain.Context;

global using Lean.CodeGen.Application.Dtos.Identity;
global using Lean.CodeGen.Application.Services.Identity;
global using Lean.CodeGen.Common.Models;
global using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Application.Dtos.Captcha;