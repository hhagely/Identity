﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Identity.UI
{
    internal class IdentityDefaultUIConfigureOptions :
        IConfigureOptions<RazorPagesOptions>,
        IConfigureOptions<RazorViewEngineOptions>,
        IConfigureOptions<StaticFileOptions>,
        IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        public IdentityDefaultUIConfigureOptions(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public IHostingEnvironment Environment { get; }

        public void Configure(RazorPagesOptions options) => options.EnableAreas = true;

        public void Configure(RazorViewEngineOptions options) => 
            options.FileProviders.Add(new ManifestEmbeddedFileProvider(GetType().Assembly));

        public void Configure(StaticFileOptions options)
        {
            // Basic initialization in case the options weren't initialized by any other component
            options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            if (options.FileProvider == null && Environment.WebRootFileProvider == null)
            {
                throw new InvalidOperationException("Missing FileProvider.");
            }

            options.FileProvider = Environment.WebRootFileProvider;

            // Add our provider
            var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "wwwroot");
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.Equals(IdentityConstants.ApplicationScheme, name, StringComparison.Ordinal))
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            }
        }

        public void Configure(CookieAuthenticationOptions options) => Configure(Options.DefaultName, options);
    }
}