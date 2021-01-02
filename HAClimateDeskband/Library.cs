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

        public static HAClimateDeskBandSettings LoadSettings()
        {
            if (File.Exists(SettingsFileName))
            {
                try
                {
                    return JsonConvert.DeserializeObject<HAClimateDeskBandSettings>(File.ReadAllText(SettingsFileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading settings: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return new HAClimateDeskBandSettings();
        }

        public static void SaveSettings(HAClimateDeskBandSettings settings)
        {
            try
            {
                File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(settings));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
