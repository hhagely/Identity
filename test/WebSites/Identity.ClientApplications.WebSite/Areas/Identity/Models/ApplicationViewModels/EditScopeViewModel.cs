﻿namespace Identity.ClientApplications.WebSite
{
    public class EditScopeViewModel
    {
        public EditScopeViewModel()
        {
        }

        public EditScopeViewModel(string applicationName, string scope)
        {
            Name = applicationName;
            Scope = scope;
        }

        public string Name { get; }
        public string Scope { get; set; }
    }
}