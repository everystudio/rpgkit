using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sequence
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SequencePathAttribute : System.Attribute
    {
        public string Path;
        public string Name;

        public SequencePathAttribute(string path)
        {
            Path = path;
            Name = path.Split('/').Last();
        }

        static public string GetFeedbackDefaultName(System.Type type)
        {
            var attribute = type.GetCustomAttributes(false).OfType<SequencePathAttribute>().FirstOrDefault();
            return attribute != null ? attribute.Name : type.Name;
        }

        static public string GetFeedbackDefaultPath(System.Type type)
        {
            var attribute = type.GetCustomAttributes(false).OfType<SequencePathAttribute>().FirstOrDefault();
            return attribute != null ? attribute.Path : type.Name;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SequenceHelpAttribute : System.Attribute
    {
        public string HelpText;

        public SequenceHelpAttribute(string helpText)
        {
            HelpText = helpText;
        }

        static public string GetSequenceHelpText(System.Type type)
        {
            var attribute = type.GetCustomAttributes(false).OfType<SequenceHelpAttribute>().FirstOrDefault();
            return attribute != null ? attribute.HelpText : "";
        }
    }


}



