using HAClimateDeskband.Models;
using System;
using System.Windows.Forms;

namespace HAClimateDeskband
{
    public partial class FormSettings : Form
    {
        public HAClimateUserControl HAClimateDeskband { get; set; }

        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            SettingsToScreen(Library.LoadSettings());
        }

        public void SettingsToScreen(HAClimateDeskBandSettings settings)
        {
            TxtApiBaseUrl.Text = settings.ApiBaseUrl;
            TxtApiKey.Text = settings.ApiKey;
            TxtEntityIdClimate.Text = settings.ClimateEntityId;
            TxtEntityIdTemperature.Text = settings.TemperatureEntityId;
            TxtEntityIdPowerUsage.Text = settings.PowerUsageEntityId;
            ChkPreferLastChangeAndPowerUsage.Checked = settings.PreferLastChangeAndPowerUsage;
        }

        public HAClimateDeskBandSettings ScreenToSettings()
        {
            return new HAClimateDeskBandSettings()
            {
                ApiBaseUrl = TxtApiBaseUrl.Text,
                ApiKey = TxtApiKey.Text,
                ClimateEntityId = TxtEntityIdClimate.Text,
                TemperatureEntityId = TxtEntityIdTemperature.Text,
                PowerUsageEntityId = TxtEntityIdPowerUsage.Text,
                PreferLastChangeAndPowerUsage = ChkPreferLastChangeAndPowerUsage.Checked
            };
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            Library.SaveSettings(ScreenToSettings());
            HAClimateDeskband.LoadSettings();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Library.SaveSettings(ScreenToSettings());
            HAClimateDeskband.LoadSettings();
            Close();
        }
    }
}
