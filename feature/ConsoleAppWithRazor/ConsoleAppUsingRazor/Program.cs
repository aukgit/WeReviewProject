using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateEmailUsingRazor.Model;
using RazorEngine.Templating;

namespace GenerateEmailUsingRazor
{
    class Program
    {
        static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");

        static void Main(string[] args)
        {
            var model = GetUserDetail();


            var emailTemplatePath = Path.Combine(TemplateFolderPath, "InviteEmailTemplate.cshtml");

            var templateService = new TemplateService();
            var emailHtmlBody = templateService.Parse(File.ReadAllText(emailTemplatePath), model, null, null);

            
            Console.WriteLine(emailHtmlBody);

            Console.ReadLine();

        }

        private static UserDetail GetUserDetail()
        {
            var model = new UserDetail()
                            {
                                Id = 1,
                                Name = "Khaled ",
                                Address = "Dhaka"
                            };


            for (int i = 1; i <= 10; i++)
            {
                model.PurchasedItems.Add("Item No " + i);
            }
            return model;
        }
    }
}
