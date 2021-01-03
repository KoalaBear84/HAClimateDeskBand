# HA Climate Deskband

Control a thermostat from you taskbar!

I wanted to have a quicker way to control the thermostat than open the webpage or the app, but have it always available at my fingertips.

## Home Assistant

Home Assistant is a free and open-source home automation software designed to be the central home automation control system for controlling smart home technology.

## Screenshots

Off, On (Heating) and Pause (temperature OK) state:

![](https://user-images.githubusercontent.com/5886900/103456043-b35dd600-4cf2-11eb-814c-949253dd4bb3.png) ![](https://user-images.githubusercontent.com/5886900/103456050-c53f7900-4cf2-11eb-90a5-3d287ee338e4.png) ![](https://user-images.githubusercontent.com/5886900/103456056-d8524900-4cf2-11eb-9df0-8a4c9cbb8485.png)

At the bottom left you see the history of the last 3 hours.

## My setup

Working from Home since March 2020, and now the winter has arrived, it was getting cold (17°C) in the garage to work all day, sitting still. The wall radiator only provides some warmth when the underfloor heater is asking heat. So I needed some extra heating.

I have a smart power plug connected to Home Assistant which has a electric heater on it. Besides that I have the [Xiaomi LYWSD03MMC](https://a.aliexpress.com/_BSEyaG) Hygrometer, read through an [ESP32](https://a.aliexpress.com/_B1IE4Y) with an [antenna](https://a.aliexpress.com/_B1fp7a). The Xiaomi LYWSD03MMC is flashed with [custom firmware](https://github.com/atc1441/ATC_MiThermometer) made by ATC1441, thanks @ATC1441! This provides faster/better temperature changes and also shows battery.

# Installation

You need Administrator rights!

You need a Long Lived Access Token for the [Home Assistant REST API](https://developers.home-assistant.io/docs/api/rest/).

1.  Download the latest release
2.  Place it on a fixed place, like C:\\Program Files\\HAClimateDeskBand\\
3.  Right click DeskBand\_Install.bat and choose "Run as Administrator"
4.  Right click the taskbar, go to Toolbars, check "HA Climate DeskBand". You might have to click twice to make it appear.
5.  Click on the label appearing to open the Settings

# Manual

To open Settings click on the first label on the control.

![](https://user-images.githubusercontent.com/5886900/103456844-4e0de300-4cfa-11eb-85a8-b78b0b07ee31.png)

# Support

I do NOT provide support for setting up Home Assistant, ESP32, Xiaomi. If you have an issue with the DeskBand itself, create an issue.

If you have any other issue, see the appropriate reference below.

# Ideas

*   Add context menu
*   Hover over it to see a 'popup' with a bigger graph.
*   Replace Newtonsoft.Json with System.Text.Json (upgrade to .NET 5)
*   Installer

# References

[Home Assistant](https://www.home-assistant.io/)

[HA Generic Thermostat](https://www.home-assistant.io/integrations/generic_thermostat/)

[Home Assistant REST API](https://developers.home-assistant.io/docs/api/rest/)

[Xiaomi LYWSD03MMC](https://a.aliexpress.com/_BSEyaG)

[ATC1441 firmware](https://github.com/atc1441/ATC_MiThermometer)

[ESP32](https://a.aliexpress.com/_B1IE4Y)

[CSDeskBand](https://github.com/dsafa/CSDeskBand)

[HA Community Xiaomi LYWSD03MMC](https://community.home-assistant.io/search?q=Xiaomi%20LYWSD03MMC)
