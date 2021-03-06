﻿using System.Collections.Generic;

namespace csharp_to_json_converter.model
{
    public class MethodModel: AccessModifierModel
    {
        public string Name { get; set; }
        public string Fqn { get; set; }
        public bool Abstract { get; set; }
        public string ReturnType { get; set; }
        public int CyclomaticComplexity { get; set; }
        public List<InvokesModel> Invocations { get; set; }
        public List<ParameterModel> Parameters { get; set; }

        public MethodModel()
        {
            Invocations = new List<InvokesModel>();
            Parameters = new List<ParameterModel>();
        }
    }
}