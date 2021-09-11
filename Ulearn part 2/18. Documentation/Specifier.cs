using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            ApiDescriptionAttribute attr = (ApiDescriptionAttribute)Attribute.
                GetCustomAttribute(typeof(T), typeof(ApiDescriptionAttribute));
            return attr == null ? null : attr.Description;
        }

        public string[] GetApiMethodNames()
        {
            var type = typeof(T);
            var methods = type.GetMethods();
            var result = new List<string>();
            foreach (var method in methods)
            {
                if (method.GetCustomAttributes(typeof(ApiMethodAttribute), false).Any())
                {
                    result.Add(method.Name);
                }
            }
            return result.ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method == null)
                return null;
            var descrAttr = (ApiDescriptionAttribute)method.GetCustomAttribute(typeof(ApiDescriptionAttribute), false);
            return descrAttr == null ? null : descrAttr.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var result = new List<string>();
            var method = typeof(T).GetMethod(methodName);
            if (method == null)
                return null;
            var parameters = method.GetParameters();
            if (parameters == null)
                return null;
            return parameters.Select(par => par.Name).ToArray();
            //foreach (var p in parameters)
            //    if (p.GetCustomAttributes(typeof(ApiRequiredAttribute), false).Any() ||
            //        p.GetCustomAttributes(typeof(ApiIntValidationAttribute), false).Any())
            //        result.Add(p.Name);
            //return result.ToArray(); 
            // Этот код возвращал все параметры помеченные атрибутами 
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method == null)
                return null;
            var parameters = method.GetParameters();
            if (parameters == null)
                return null;
            ApiDescriptionAttribute descrAttr = null;
            foreach(var p in parameters)
            {
                if (p.Name == paramName)
                    descrAttr = (ApiDescriptionAttribute)p.GetCustomAttribute(typeof(ApiDescriptionAttribute), false);
            }
            return descrAttr == null ? null : descrAttr.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method == null)
                return new ApiParamDescription
                {
                    MinValue = null,
                    MaxValue = null,
                    Required = false,
                    ParamDescription = new CommonDescription(paramName)
                };
            var parameters = method.GetParameters();
            if (parameters == null)
                return null;
            ApiDescriptionAttribute descrAttr = null;
            ApiRequiredAttribute reqAttr = null;
            ApiIntValidationAttribute intValAttr = null;
            foreach(var p in parameters)
            {
                if (p.Name == paramName)
                {
                    descrAttr = (ApiDescriptionAttribute)p.GetCustomAttribute(typeof(ApiDescriptionAttribute), false);
                    reqAttr = (ApiRequiredAttribute)p.GetCustomAttribute(typeof(ApiRequiredAttribute), false);
                    intValAttr = (ApiIntValidationAttribute)p
                        .GetCustomAttribute(typeof(ApiIntValidationAttribute), false);
                }
            }
            var minValue = intValAttr == null ? null : intValAttr.MinValue;
            var maxValue = intValAttr == null ? null : intValAttr.MaxValue;
            CommonDescription paramDescription = descrAttr == null ? new CommonDescription(paramName) :
                new CommonDescription(paramName, descrAttr.Description);
            bool required = reqAttr == null ? false : reqAttr.Required;
            return new ApiParamDescription
            {
                MinValue = minValue,
                MaxValue = maxValue,
                ParamDescription = paramDescription,
                Required = required
            };
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var paramDescriptions = new List<ApiParamDescription>();
            var method = typeof(T).GetMethod(methodName);
            if (method == null)
                return null;
            var description = GetApiMethodDescription(methodName);
            if (description == null || !GetApiMethodNames().Contains(methodName))
                return null;
            var parameters = method.GetParameters();
            if (parameters == null)
                return null;
            foreach (var p in parameters)
                paramDescriptions.Add(GetApiMethodParamFullDescription(methodName, p.Name));
            var returnParam = method.ReturnParameter;
            var returnParamDescrAttr = (ApiDescriptionAttribute)returnParam.
                GetCustomAttribute(typeof(ApiDescriptionAttribute), false);
            var returnParamReqAttr = (ApiRequiredAttribute)returnParam.
                GetCustomAttribute(typeof(ApiRequiredAttribute), false);
            var returnParamIntValAttr = (ApiIntValidationAttribute)returnParam
                        .GetCustomAttribute(typeof(ApiIntValidationAttribute), false);
            var returnMinValue = returnParamIntValAttr == null ? null : returnParamIntValAttr.MinValue;
            var returnMaxValue = returnParamIntValAttr == null ? null : returnParamIntValAttr.MaxValue;
            CommonDescription returnParamDescription = returnParamDescrAttr == null ? 
                new CommonDescription(null) :
                new CommonDescription(null, description);
            bool returnRequired = returnParamReqAttr == null ? false : returnParamReqAttr.Required;
            var returnDescription = method.ReturnType == typeof(void) ? null 
                : new ApiParamDescription
            {
                MaxValue = returnMaxValue,
                MinValue = returnMinValue,
                ParamDescription = returnParamDescription,
                Required = returnRequired
            };
            return new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(methodName, description),
                ParamDescriptions = paramDescriptions.ToArray(),
                ReturnDescription = returnDescription
            };
        }
    }
}