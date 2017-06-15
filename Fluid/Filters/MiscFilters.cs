﻿using System.Collections.Generic;
using System.Net;
using Fluid.Values;

namespace Fluid.Filters
{
    public static class MiscFilters
    {
        public static FilterCollection WithMiscFilters(this FilterCollection filters)
        {
            filters.AddFilter("default", Default);
            filters.AddFilter("raw", Raw);
            filters.AddFilter("compat", Compact);
            filters.AddFilter("url_encode", UrlEncode);
            filters.AddFilter("url_decode", UrlDecode);

            return filters;
        }

        public static FluidValue Default(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return input.Or(arguments.At(0));
        }

        public static FluidValue Raw(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var stringValue = new StringValue(input.ToStringValue());
            stringValue.Encode = false;

            return stringValue;
        }

        public static FluidValue Compact(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var compacted = new List<FluidValue>();
            foreach(var value in input.Enumerate()) 
            {
                if (!value.IsNil())
                {
                    compacted.Add(value);
                }
            } 

            return new ArrayValue(compacted);
        }

        public static FluidValue UrlEncode(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return new StringValue(WebUtility.UrlEncode(input.ToStringValue()));
        }

        public static FluidValue UrlDecode(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return new StringValue(WebUtility.UrlDecode(input.ToStringValue()));
        }
    }
}
