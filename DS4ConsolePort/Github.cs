using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DS4ConsolePort
{
    public class Github
    {
        // Check if there is a newer release on Github than current
        public static async Task<GithubRelease> CheckForUpdate(string user, string repo, Version version)
        {
            try
            {
                var latest = await GetLatestRelease(user, repo);

                // tag needs to be in strict version format: e.g. 0.0.0
                Version v = new Version(latest.tag_name);

                // check if latest is newer than current
                if (v.CompareTo(version) > 0)
                {
                    return latest;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update check failed: " + ex.Message, "Github");
            }
            return null;
        }

        // Use the Github API to get the latest release for a repo
        public static async Task<GithubRelease> GetLatestRelease(string user, string repo)
        {
            var url = string.Format("https://api.github.com/repos/{0}/{1}/releases?client_id=3e70d5647db6a4907fda&client_secret=0b36856d59d15eed5fe07159b4284e77bd1c00f6", user, repo);
            try
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    // API requires user-agent string, user name or app name preferred
                    wc.Headers.Add(HttpRequestHeader.UserAgent, user);
                    json = await wc.DownloadStringTaskAsync(url);
                }
                var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(json);

                return releases.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GithubRelease GetLatestRelease2(string user, string repo)
        {
            var url = string.Format("https://api.github.com/repos/{0}/{1}/releases?client_id=3e70d5647db6a4907fda&client_secret=0b36856d59d15eed5fe07159b4284e77bd1c00f6", user, repo);
            try
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    // API requires user-agent string, user name or app name preferred
                    wc.Headers.Add(HttpRequestHeader.UserAgent, user);
                    json = wc.DownloadString(url);
                }
                var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(json);

                return releases.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Use the Github API to get the latest release for a repo
        public static async Task<GithubCommit> GetLatestCommit(string user, string repo)
        {
            var url = string.Format("https://api.github.com/repos/{0}/{1}/commits", user, repo);
            try
            {
                var json = "";
                using (WebClient wc = new WebClient())
                {
                    // API requires user-agent string, user name or app name preferred
                    wc.Headers.Add(HttpRequestHeader.UserAgent, user);
                    json = await wc.DownloadStringTaskAsync(url);
                }
                var commits = JsonConvert.DeserializeObject<List<GithubCommit>>(json);

                return commits.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Basic release info for JSON deserialization
        public class GithubRelease
        {
            public string html_url { get; set; }
            public string tag_name { get; set; }
            public string prerelease { get; set; }
            public string published_at { get; set; }
            public List<GithubAsset> assets { get; set; }
        }

        public class GithubCommit
        {
            public GithubCommitData commit { get; set; }
        }

        public class GithubCommitData
        {
            public string message { get; set; }
        }

        public class GithubAsset
        {
            public string name { get; set; }
            public string browser_download_url { get; set; }
        }

    }
}