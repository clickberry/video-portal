// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using MongoMigrations;
using Portal.DAL.Migrations;
using Xunit;

namespace Portal.Tests
{
    public sealed class MigrationTests
    {
        public MigrationTests()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
        }

        [Fact]
        public void TestMigration()
        {
            var runner = new MigrationRunner("mongodb://admin:5esWedRa@clickmongoqa.cloudapp.net:13878/clickberry");

            // migrations are ordered by version
            runner.MigrationLocator.LookForMigrationsInAssemblyOfType<IPortalMigration>();

            // updating database
            runner.UpdateToLatest();

            runner.DatabaseStatus.ThrowIfNotLatestVersion();
        }

        [Fact]
        public void TestYoutubeVideoUrlExtractor()
        {
            var youtubeVideo = new Regex(@".*https?://(www\.)?(youtube|youtube-nocookie).com/(watch\?.*v=|v/|embed/)([^&\?#]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var testUrls = new List<string>
            {
                "http://www.youtube.com/watch?v=Tqymo0sVUwE",
                "https://www.youtube.com/watch?v=lW_D5rj_0_4",
                "https://www.youtube.com/watch?v=FHdBIjXmXXM&feature=youtu.be",
                "https://www.youtube.com/watch?feature=player_embedded&v=eP1mmy5WJNA#t=33",
                "https://www.youtube.com/watch?v=WG2VKIX72AI&list=PL4BhKLtOyMbqZihadPGd2udYXCBslZ9YM&index=6",
                "http://www.youtube.com/v/8Y-NqShTj5w?fs=1",
                "http://www.youtube.com/v/IZbKVvoXbrI?version=3&hl=en_US",
                "http://www.youtube.com/embed/UqlsVZ1zxMk?enablejsapi=1&origin=http%3A%2F%2Fwww.songcrate.com&wmode=opaque&autoplay=1&cc_load_policy=0&iv_load_policy=3&rel=0&modestbranding=1&theme=dark",
                "http://www.youtube.com/embed/nzUFBkMHjQg?rel=0&autoplay=1",
                "http://www.youtube-nocookie.com/embed/68RXKmkMaR0?autohide=1&theme=light&hd=1&modestbranding=1&rel=0&showinfo=0&showsearch=0&wmode=transparent&autoplay=1"
            };

            foreach (var url in testUrls)
            {
                Assert.True(youtubeVideo.IsMatch(url));
            }
        }
    }
}