using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingConstructorLib.logic;

namespace ParkingSimulationForms.views.services
{
    public class FormFilesService
    {
        private readonly string workDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private const string FileExtension = ".parking";

        public FormFilesService()
        {
        }

        public bool OpenDialogAndSaveModel<T>(ParkingModel<T> model) where T : class
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "my-parking-model",
                DefaultExt = FileExtension,
                Filter = ' ' + FileExtension + '|' + '*' + FileExtension
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            var fullFilePath = saveFileDialog.FileName;
            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, model);
                }
                catch (Exception e)
                {
                    MessageBox.Show("ОШИБКА: Не удалось сохранить файл!\n" + e.Message, "ОШИБКА: Не удалось сохранить файл!");
                    Console.WriteLine(e);
                    return false;
                }
            }

            return true;
        }

        public ParkingModel<T> OpenDialogAndLoadModel<T>() where T : class
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = FileExtension,
                Filter = ' ' + FileExtension + '|' + '*' + FileExtension
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            var fullFilePath = fileDialog.FileName;

            ParkingModel<T> resultsModel = null;

            if (File.Exists(fullFilePath))
            {
                var formatter = new BinaryFormatter();
                using (var fileStream = new FileStream(fullFilePath, FileMode.Open))
                {
                    try
                    {
                        resultsModel = (ParkingModel<T>) formatter.Deserialize(fileStream);
                    }
                    catch (SerializationException)
                    {
                        MessageBox.Show("ОШИБКА: Загружаемый файл повреждён!");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.StackTrace);
                    }
                }
            }
            else
            {
                MessageBox.Show("ОШИБКА: Выбранный файл не существует");
            }

            return resultsModel;
        }
    }
}