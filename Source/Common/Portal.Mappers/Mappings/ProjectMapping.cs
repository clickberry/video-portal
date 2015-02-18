// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProjectContext;

namespace Portal.Mappers.Mappings
{
    public sealed class ProjectMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<ProjectEntity, DomainProject>().ConvertUsing(
                entity => new DomainProject
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    Created = entity.Created,
                    Modified = entity.Modified,
                    Name = entity.Name,
                    Description = entity.Description,
                    Access = (ProjectAccess)entity.Access,
                    ProjectType = (ProjectType)entity.ProjectType,
                    ProjectSubtype = (ProjectSubtype)entity.ProjectSubtype,
                    ScreenshotFileId = entity.ScreenshotFileId,
                    ProductType = (ProductType)entity.ProductId,
                    VideoType = (VideoType)entity.VideoType,
                    VideoSource = entity.VideoSource,
                    VideoSourceProductName = entity.VideoSourceProductName,
                    OriginalVideoFileId = entity.OriginalVideoFileId,
                    AvsxFileId = entity.AvsxFileId,
                    EncodedVideos = entity.EncodedVideos == null
                        ? new List<DomainProject.EncodedVideo>()
                        : entity.EncodedVideos.Select(v => new DomainProject.EncodedVideo
                        {
                            ProjectId = entity.Id,
                            FileId = v.FileId,
                            ContentType = v.ContentType,
                            Height = v.Height,
                            Width = v.Width
                        }).ToList(),
                    EncodedScreenshots = entity.EncodedScreenshots == null
                        ? new List<DomainProject.EncodedScreenshot>()
                        : entity.EncodedScreenshots.Select(v => new DomainProject.EncodedScreenshot
                        {
                            ProjectId = entity.Id,
                            FileId = v.FileId,
                            ContentType = v.ContentType,
                            Height = v.Height,
                            Width = v.Width
                        }).ToList(),
                    HitsCount = entity.HitsCount,
                    LikesCount = entity.LikesCount,
                    DislikesCount = entity.DislikesCount,
                    AbuseCount = entity.AbuseCount,
                    EnableComments = entity.EnableComments
                });

            Mapper.CreateMap<DomainProject, ProjectEntity>().ConvertUsing(
                project => new ProjectEntity
                {
                    Id = project.Id,
                    UserId = project.UserId,
                    Created = project.Created,
                    Modified = project.Modified,
                    Name = project.Name,
                    Description = project.Description,
                    Access = (int)project.Access,
                    ProjectType = (int)project.ProjectType,
                    ProjectSubtype = (int)project.ProjectSubtype,
                    ScreenshotFileId = project.ScreenshotFileId,
                    ProductId = (int)project.ProductType,
                    VideoType = (int)project.VideoType,
                    VideoSource = project.VideoSource,
                    VideoSourceProductName = project.VideoSourceProductName,
                    OriginalVideoFileId = project.OriginalVideoFileId,
                    AvsxFileId = project.AvsxFileId,
                    EncodedVideos = project.EncodedVideos == null
                        ? new ProjectEntity.EncodedVideo[] { }
                        : project.EncodedVideos.Select(v => new ProjectEntity.EncodedVideo
                        {
                            FileId = v.FileId,
                            ContentType = v.ContentType,
                            Width = v.Width,
                            Height = v.Height
                        }).ToArray(),
                    EncodedScreenshots = project.EncodedScreenshots == null
                        ? new ProjectEntity.EncodedScreenshot[] { }
                        : project.EncodedScreenshots.Select(v => new ProjectEntity.EncodedScreenshot
                        {
                            FileId = v.FileId,
                            ContentType = v.ContentType,
                            Width = v.Width,
                            Height = v.Height
                        }).ToArray(),
                    HitsCount = project.HitsCount,
                    LikesCount = project.LikesCount,
                    DislikesCount = project.DislikesCount,
                    AbuseCount = project.AbuseCount,
                    NameSort = project.Name == null ? null : project.Name.ToLowerInvariant(),
                    EnableComments = project.EnableComments
                });
        }
    }
}