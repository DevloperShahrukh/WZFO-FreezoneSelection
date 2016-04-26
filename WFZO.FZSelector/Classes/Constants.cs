using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFZO.FZSelector
{
    public static class Constants
    {
        public static class ContentType
        {
            public static class News
            {
                public const string Comments = "Comments";
                public const string Date = "ArticleStartDate";
            }
        }

        public static class Subsite
        {
            public const string News = "/News";
        }
        public static class Modules
        {
            public const string Benchmarking = "Benchmarking";
            public const string Weighted = "Weighted";
            public const string Profile = "Profile";

        }

        public static class ReportTypes
        {
            public const string Trend = "Trend";
            public const string Normal = "Normal";
        }
        public static class List
        {
            public static class BaseColumns
            {
                public const string Title = "Title";
                public const string IsActive = "IsActive";
                public const string Created = "Created";
            }
            public static class Configuration
            {
                public const string Name = "Configuration";
                public static class Fields
                {
                    public const string Value = "Value";
                }
            }

            public static class Pages
            {
                public const string Name = "Pages";
                public static class Fields
                {

                    public const string ContentType = "ContentType";
                }
            }
            public static class Footer
            {
                public const string Name = "Footer";
                public static class Fields
                {
                    public const string Sequence = "Sequence";
                    public const string InWindow = "InWindow";
                }
            }
            public static class Reports
            {
                public const string Name = "Reports";
                public static class Fields
                {
                    public const string Name = "Name";
                    public const string Module = "Module";
                }
            }
            public static class FAQ
            {
                public const string Name = "FAQ";
                public static class Fields
                {
                    public const string Answer = "Answer";
                }
            }
            public static class BottomSlider
            {
                public const string Name = "BottomSlider";
                public static class Fields
                {
                    public const string ImageColumn = "ImageColumn";
                }
            }

            public static class TopSlider
            {
                public const string Name = "TopSlider";
                public static class Fields
                {
                    public const string ImageColumn = "ImageColumn";
                }
            }
        }
    }
}
