﻿using System.Net;
using Newtonsoft.Json.Linq;

Console.WriteLine("Enter the name of the city:");
string city = Console.ReadLine();

string apiKey = "ce366f564975b5dd85846495183bc5a7";

string urlForecast = string.Format($"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric&lang=en");
using (WebClient web = new WebClient())
{
    var jsonForecast = web.DownloadString(urlForecast);
    var dataForecast = JObject.Parse(jsonForecast);

    Console.WriteLine($"Weather forecast for the next 7 days in the city {city}:");
    foreach (var item in dataForecast["list"])
    {
        var date = DateTime.Parse(item["dt_txt"].ToString());
        var localTime = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        var temperature = item["main"]["temp"];
        var weatherDescription = item["weather"][0]["description"];

        Console.WriteLine($"Date: {localTime.ToShortDateString()}\tTime: {localTime.ToShortTimeString()}\tTemperature: {temperature}°C\tWeather description: {weatherDescription}");
    }
}