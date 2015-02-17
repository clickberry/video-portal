// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration333_FixYoutubeVideoUrls : Migration, IPortalMigration
    {
        private static readonly Regex YoutubeVideo = new Regex(@".*https?://(www\.)?(youtube|youtube-nocookie).com/(watch\?.*v=|v/|embed/)([^&\?#]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Migration333_FixYoutubeVideoUrls()
            : base("3.3.3")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            Parallel.ForEach(projectCollection.FindAll(), project =>
            {
                BsonValue source = project["VideoSource"];
                if (source == BsonNull.Value || string.IsNullOrEmpty(source.AsString))
                {
                    return;
                }

                Match match = YoutubeVideo.Match(source.AsString);
                if (!match.Success)
                {
                    return;
                }

                // replace video URL
                string videoUrl = string.Format("https://www.youtube.com/watch?v={0}", match.Groups[4].Value);
                if (source.AsString == videoUrl)
                {
                    return;
                }

                project["VideoSource"] = BsonValue.Create(videoUrl);
                projectCollection.Save(project);
            });
        }
    }
}