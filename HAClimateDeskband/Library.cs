using HAClimateDeskband.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    public class Library
    {
        const string SettingsFileName = "HAClimateDeskBand.json";

        public static string GetSettingsPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAClimateDeskband");
        public static string GetSettingsFileName() => Path.Combine(GetSettingsPath(), SettingsFileName);

        public static HAClimateDeskBandSettings LoadSettings()
        {
            try
            {
                if (!Directory.Exists(GetSettingsPath()))
                {
                    Directory.CreateDirectory(GetSettingsPath());
                }

                if (File.Exists(GetSettingsFileName()))
                {
                    return JsonConvert.DeserializeObject<HAClimateDeskBandSettings>(File.ReadAllText(GetSettingsFileName()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new HAClimateDeskBandSettings();
        }

        public static void SaveSettings(HAClimateDeskBandSettings settings)
        {
            try
            {
                if (!Directory.Exists(GetSettingsPath()))
                {
                    Directory.CreateDirectory(GetSettingsPath());
                }

                File.WriteAllText(GetSettingsFileName(), JsonConvert.SerializeObject(settings));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
