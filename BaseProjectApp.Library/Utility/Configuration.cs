using BaseProjectApp.Library.Templates;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace BaseProjectApp.Library.Utility
{
    public class Configuration
    {
        public static string GetConfigurationString()
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(path)
                                                                         .AddJsonFile("appsettings.json")
                                                                         .Build();

            string connectionString = configuration.GetConnectionString("ContextConnection");

            return connectionString;
        }
        public static string? GetConfigURL()
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(path)
                                                                         .AddJsonFile("appsettings.json")
                                                                         .Build();

            string? url = configuration?.GetSection("ConfigUrl")?.Value;

            return url;
        }
        public static string? GenerateURL(string? URL, string? ConfigURL)
        {
            if (string.IsNullOrWhiteSpace(URL) || URL.Contains("http"))
                return URL;

            return ConfigURL + URL;
        }
        public static SaveFileResponse SaveFile(string File = "", string dataUrl = "", bool IsImage = false, string BasePath = "", byte[] Data = null, bool Reports = false)
        {
            if (dataUrl == "$")
                return null;

            if ((string.IsNullOrEmpty(dataUrl) || string.IsNullOrEmpty(File)) && Data == null)
                return null;

            try
            {
                string dateString = DateTime.Now.Ticks.ToString();
                string[] extension = File.Split('.');
                var DocPath = Reports ? "/Media/Default/Reports/" + extension[0] + "_" + dateString + "." + extension[1] : "/Media/Default/" + extension[0] + "_" + dateString + "." + extension[1];
                string path = Path.Combine(DocPath);

                string filename = extension[0] + "_" + dateString + "." + extension[1];
                string caption = extension[0];
                string filepath = Reports ? Path.Combine(BasePath, "Media", "Default", "Reports") + $@"\{filename}" : Path.Combine(BasePath, "Media", "Default") + $@"\{filename}";

                string cleandata = Data == null ? Regex.Replace(dataUrl, @"^data:image\/[a-zA-Z]+;base64,", string.Empty) : "";
                byte[] data = Data == null ? System.Convert.FromBase64String(cleandata) : Data;
                MemoryStream ms = new MemoryStream(data);

                //dataUrl = HttpUtility.UrlDecode(dataUrl); 
                //var completePath = @"~\user-images\file.svg";
                //completePath = HttpContext.Current.Server.MapPath(completePath);

                //File.WriteAllText(completePath, dataUrl);

                if (IsImage)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    img.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    using (FileStream file = new FileStream(filepath, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }

                return new SaveFileResponse { FileName = filename, FilePath = path, Caption = caption };
            }

            catch (System.Exception ex)
            {
                string Error = ex.Message;
                return null;
            }
        } 
        public static DateTime? ConvertToDate(string? _Date, string Format = "dd-MM-yyyy")
        {
            if (string.IsNullOrWhiteSpace(_Date))
                return null;

            DateTime DateVal;

            if (DateTime.TryParseExact(_Date, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateVal))
            {
                return DateVal;
            }

            return null; 
        }

        public static string ConvertDateToSTR(DateTime? _Date, string Format = "dd/MM/yyyy")
        {
            if (_Date == null)
                return "";
             

            return ((DateTime)_Date).ToString(Format);
        }


        public static string GetPackageFullText(int? PackageFreeAds , int? PackageFeaturedAds , int? PackagePremiumAds  , string Lang)
        {
            string Text = "";

            Text =  Lang == "en" ?
                (PackageFreeAds <= 0 ? "Free Ads 0, " : $"Free Ads {PackageFreeAds}, ") +
                (PackageFeaturedAds <= 0 ? "Featured Ads 0, " : $"Featured Ads {PackageFeaturedAds}, ") +
                (PackagePremiumAds <= 0 ? "Premium Ads: 0, " : $"Premium Ads {PackagePremiumAds}")
                :
                (PackageFreeAds <= 0 ? "إعلانات مجانية:0 ," : $"إعلانات مجانية {PackageFreeAds}, ") +
                (PackageFeaturedAds <= 0 ? "إعلانات مميزة: 0 ," : $"إعلانات مميزة {PackageFeaturedAds}, ") +
                (PackagePremiumAds <= 0 ? "إعلانات مثبتة: 0 ," : $"إعلانات مثبتة {PackagePremiumAds}")
                ; 

            return Text;
        }


        public static string GenerateCode()
        {
            var random = new Random();

            var tem = random.Next(2, 9);

            var code = random.Next(1001, 9999).ToString();

            return code;
        }
        public static bool IsNewDate(DateTime? _Date)
        {

            if (_Date == null || _Date > DateTime.Now)
                return false;

            return _Date < DateTime.Now && _Date > DateTime.Now.AddDays(-10);

        }
    }
}
