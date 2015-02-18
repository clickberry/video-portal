// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.Domain.ProjectContext;
using Portal.Web.Constants;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectUriProvider _projectUriProvider;

        public SitemapController(IProjectService projectService, IProjectUriProvider projectUriProviders)
        {
            _projectService = projectService;
            _projectUriProvider = projectUriProviders;
        }


        //
        // GET: /sitemap_index.xml

        [Route("sitemap_index.xml")]
        public async Task<ActionResult> SitemapIndex()
        {
            // Get projects
            int pageCount = await _projectService.GetSitemapPageCountAsync();

            // Build sitemap index
            var sitemapIndex = new SitemapIndexModel();
            for (int i = 0; i < pageCount; ++i)
            {
                sitemapIndex.Sitemaps.Add(new SitemapIndexItem
                {
                    Location = Url.RouteUrl(RouteNames.Sitemap, new { page = i }, Request.Url != null ? Request.Url.Scheme : "http")
                });
            }

            // Serialize
            var serializer = new XmlSerializer(typeof (SitemapIndexModel));

            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

            var xmlStream = new MemoryStream();

            serializer.Serialize(xmlStream, sitemapIndex, xmlns);
            xmlStream.Seek(0, SeekOrigin.Begin);

            return File(xmlStream, "text/xml");
        }


        //
        // GET: /sitemap{page}.xml

        [Route("sitemap{page:int}.xml", Name = RouteNames.Sitemap)]
        public async Task<ActionResult> Sitemap(int page)
        {
            // Get projects
            List<DomainProject> projectsPage = await _projectService.GetSitemapPageAsync(page);

            // Build sitemap
            var sitemap = new SitemapModel();
            foreach (DomainProject project in projectsPage)
            {
                sitemap.Urls.Add(new SitemapItem
                {
                    Location = _projectUriProvider.GetUri(project.Id),
                    ChangeFrequency = SitemapUrlChangeFrequency.Monthly,
                    LastModified = project.Modified
                });
            }

            // Serialize
            var serializer = new XmlSerializer(typeof (SitemapModel));

            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

            var xmlStream = new MemoryStream();

            serializer.Serialize(xmlStream, sitemap, xmlns);
            xmlStream.Seek(0, SeekOrigin.Begin);

            return File(xmlStream, "text/xml");
        }
    }
}