using System;
using System.Reflection;

namespace FUEL_DISPATCH_API.CrystalRpt.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}