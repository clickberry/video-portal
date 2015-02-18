// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;
using Portal.DTO.Watch;

namespace Portal.BLL.Concrete.Services
{
    public class LastProjectService : ILastProjectService
    {
        private const int ProjectCount = 2;

        private readonly ITableRepository<ProjectEntity> _projectRepository;

        public LastProjectService(IRepositoryFactory repositoryFactory)
        {
            _projectRepository = repositoryFactory.Create<ProjectEntity>();
        }


        public Task<List<Watch>> GetLastForTagger(int count)
        {
            List<ProjectEntity> lastProjects = _projectRepository.Context
                .Where(p => (p.ProductId == (int)ProductType.TaggerAndroid ||
                             p.ProductId == (int)ProductType.TaggerIPhone) && p.ScreenshotFileId != null)
                .OrderByDescending(p => p.Created)
                .Take(count)
                .ToList();

            List<Watch> list = GetWatches(lastProjects);

            return Task.FromResult(list);
        }

        public Task<List<Watch>> GetLastForExtension(int count)
        {
            List<ProjectEntity> lastProjects = _projectRepository.Context
                .Where(p => (p.ProductId == (int)ProductType.DailyMotion ||
                             p.ProductId == (int)ProductType.ImageShack ||
                             p.ProductId == (int)ProductType.Standalone ||
                             p.ProductId == (int)ProductType.YoutubePlayer) && p.ScreenshotFileId != null)
                .OrderByDescending(p => p.Created)
                .Take(count)
                .ToList();

            List<Watch> list = GetWatches(lastProjects);

            return Task.FromResult(list);
        }

        private static List<Watch> GetWatches(IEnumerable<ProjectEntity> lastProjects)
        {
            var list = new List<Watch>();

            foreach (ProjectEntity project in lastProjects)
            {
                if (list.Count == ProjectCount)
                {
                    break;
                }

                if (string.IsNullOrEmpty(project.ScreenshotFileId))
                {
                    continue;
                }

                var watch = new Watch
                {
                    ScreenshotUrl = project.ScreenshotFileId,
                    Generator = project.ProductId,
                    Id = project.Id,
                    Name = project.Name
                };

                list.Add(watch);
            }

            return list;
        }
    }
}