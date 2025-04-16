using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sportingh.Classes
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }
        public static Data.User CurrentUser { get; set; }

        public static void GetImageData()
        {
            try
            {
                var list = Data.SportingZlatovRyaskixEntities.GetContext().Fitness.ToList();
                foreach (var item in list)
                {
                    string path = Directory.GetCurrentDirectory() + @"\img\" + item.PhotoName;
                    if (File.Exists(path))
                    {
                        item.Image = File.ReadAllBytes(path);
                    }
                }
                Data.SportingZlatovRyaskixEntities.GetContext().SaveChanges();
            }
            catch (Exception)
            {

            }
            }
        }
    }
