﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;

namespace Mod_Manager_V2.Resources
{
    public class ModPage
    {
        public int Id { get; set; }
        public bool IsCRSRequired { get; set; }
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Creator { get; set; }
        public string? Description { get; set; }
        public string? ImageLink { get; set; }
        public string? FileLink { get; set; }
        public string? UploadDate { get; set; }
        public string? Category { get; set; }
        public string? FilePath { get; set; } // this isnt actually necessary because used only if category is else
    }

    public class ModPageParser
    {
        private const string rawUrl = "https://raw.githubusercontent.com/ForzaMods/ModManager/V2/Mods.json"; 

        public async Task<List<ModPage>> ParseModPagesFromGitHub()
        {
            string json = await DownloadJsonFromUrl(rawUrl);
            JObject jsonObject = JObject.Parse(json);
            JArray modsArray = (JArray)jsonObject["mods"];
            List<ModPage> modPages = modsArray.ToObject<List<ModPage>>();
            return modPages;
        }

        public async Task<List<ModPage>> ParseModPagesFromLocalJson()
        {
            JObject jsonObject = JObject.Parse(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Forza Mod Manager\DownloadedMods.json");
            JArray modsArray = (JArray)jsonObject["DownloadedMods"];
            List<ModPage> DownloadedModPages = modsArray.ToObject<List<ModPage>>();
            return DownloadedModPages;
        }

        private async Task<string> DownloadJsonFromUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}