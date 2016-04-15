using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFZO.FZSelector
{
    public static class Constants
    {
        public static class Subsite
        {
            public const string News = "/News";
        }
         public static class List
         {
             public static class BaseColumns
             {
                 public const string Title = "Title";
                 public const string IsActive = "IsActive";
                 public const string Created = "Created";
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
