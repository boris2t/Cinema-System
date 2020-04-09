﻿namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Repositories;
    using THECinema.Web.ViewModels.Comments;

    using Xunit;

    public class CommentsServiceTests
    {
        private readonly AddCommentInputModel comment;

        public CommentsServiceTests()
        {
            this.comment = new AddCommentInputModel
            {
                Content = "Thank you!",
                MovieId = 1,
                ReviewId = 1,
            };
        }

        [Fact]
        public async Task AddCommentShouldAddCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);

            await commentsService.AddAsync(this.comment);
            Assert.Equal(1, commentsRepository.All().Count());
        }

        [Fact]
        public async Task AddCommentShouldAddCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);

            var dbComment = await commentsService.AddAsync(this.comment);

            Assert.Equal("Thank you!", dbComment.Content);
            Assert.Equal(1, commentsRepository.All().FirstOrDefault().Id);
            Assert.Equal(commentsRepository.All().FirstOrDefault().CreatedOn, dbComment.CreatedOn);
        }

        [Fact]
        public async Task EditCommentShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);
 
            await commentsService.AddAsync(this.comment);

            var diffComment = new AddCommentInputModel
            {
                Id = 1,
                Content = "123",
                MovieId = 1,
                ReviewId = 1,
            };

            var dbComment = await commentsService.EditAsync(diffComment);

            Assert.Equal("123", dbComment.Content);
        }

        [Fact]
        public async Task EditCommentShouldThrowWithInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);

            await commentsService.AddAsync(this.comment);

            var diffComment = new AddCommentInputModel
            {
                Id = 5,
                Content = "123",
                MovieId = 1,
                ReviewId = 1,
            };

            Assert.Throws<ArgumentNullException>(() => commentsService.EditAsync(diffComment).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteCommentShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);

            await commentsService.AddAsync(this.comment);
            await commentsService.DeleteAsync(1);

            var dbComment = await commentsRepository.GetByIdWithDeletedAsync(1);

            Assert.True(dbComment.IsDeleted);
        }

        [Fact]
        public void DeleteCommentShouldThrowWithInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(context);
            var commentsService = new CommentsService(commentsRepository);

            Assert.Throws<ArgumentNullException>(() => commentsService.DeleteAsync(1).GetAwaiter().GetResult());
        }
    }
}