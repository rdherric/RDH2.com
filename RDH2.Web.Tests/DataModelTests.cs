using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.Tests
{
    /// <summary>
    /// DataModelTests contains tests for the DataModel
    /// part of the Web application.
    /// </summary>
    [TestFixture]
    public class DataModelTests
    {
        #region Constants
        private const String _connectionString = "Data Source=localhost;Initial Catalog=RDH2Web_DEV;User ID=RDH2Web;Password=RDH2W3BP@ssw0rd";
        #endregion


        /// <summary>
        /// CanConnectToDatabase tests that the DataModel
        /// classes can even connect to the database.
        /// </summary>
        [Test]
        public void CanConnectToDatabase()
        {
            //Arrange:  Given a Post Repository...
            //Act:  ...when created and accessed...
            using (Repository<Post> repository = new Repository<Post>(DataModelTests._connectionString))
            {
                //Assert:  ...there are objects in the Context.
                Assert.AreNotEqual(0, repository.GetAll().Count());
            }
        }


        /// <summary>
        /// CanAddPostToDatabase tests that a Post can be added
        /// to the data store.
        /// </summary>
        [Test]
        public void CannotAddExistingPostToDatabase()
        {
            //Arrange:  Given a Post Repository and an Account
            Account account = null;
            using (Repository<Account> accounts = new Repository<Account>(DataModelTests._connectionString))
            {
                account = accounts.GetBy(a => a.ID == 1).FirstOrDefault();
            }

            using (Repository<Post> repository = new Repository<Post>(DataModelTests._connectionString))
            {
                //Act:  ...when a Post is created...
                Post post = new Post
                {
                    Title = "Test Post",
                    ReducedTitle = "test-post",
                    Body = "Body",
                    CreatedBy = account
                };

                post = repository.AddUnique(post, p => post.ReducedTitle == p.ReducedTitle);

                //Assert:  ...the Post is added to the data store
                Assert.AreEqual(-1, post.ID);
            }
        }


        /// <summary>
        /// CanRetrieveExistingPostCounts tests whether the PostCount
        /// Repository works.
        /// </summary>
        [Test]
        public void CanRetrieveExistingPostCounts()
        {
            //Arrange:  Given a Repository of PostCounts...
            using (Repository<PostCount> counts = new Repository<PostCount>(DataModelTests._connectionString))
            {
                //Act:  ...when all of them are retrieved...
                List<PostCount> countList = counts.GetAll()
                    .OrderByDescending(pc => pc.Bucket)
                    .ToList();

                //Assert:  ...there are PostCounts in the List.
                Assert.AreNotEqual(0, countList.Count);
            }
        }
 
        
        /// <summary>
        /// GalleryHasFullNameAndPath tests that a Gallery has 
        /// a FullName and FullPath property based on the ID.
        /// </summary>
        [Test]
        public void GalleryHasFullNameAndPath()
        {
            //Arrange:  Given a Gallery Repository...
            using (Repository<Gallery> galleries = new Repository<Gallery>(DataModelTests._connectionString))
            {
                //Act:  ...when the Galleries are retrieved...
                Gallery g = galleries.GetAll().FirstOrDefault();

                //Assert:  ...the first Gallery has a FullName and FullPath
                Assert.IsNotNullOrEmpty(g.FullName);
                Assert.IsNotNullOrEmpty(g.FullPath);
            }
        }


        /// <summary>
        /// ParentGalleryHasChildGalleries tests that a Gallery
        /// that acts as a Parent has access to its associated
        /// child Galleries.
        /// </summary>
        [Test]
        public void ParentGalleryHasChildGalleries()
        {
            //Arrange:  Given a Gallery...
            using (Repository<Gallery> galleries = new Repository<Gallery>(DataModelTests._connectionString))
            {
                //Act:  ...when the child Galleries are retrieved...
                Gallery parent = galleries.GetBy(g => g.ID == 13)
                    .Include(g => g.Galleries)
                    .FirstOrDefault();
                
                //Assert:  ...there are Galleries in the Collection.
                Assert.AreNotEqual(0, parent.Galleries.Count());
            }
        }


        /// <summary>
        /// CanAddUniqueGalleryToDatabase tests that a new Gallery can be 
        /// added to the database.
        /// </summary>
        [Test]
        public void CannotAddExistingGalleryToDatabase()
        {
            //Arrange:  Given a User Account and a new Gallery with a Parent...
            Account account = null;
            using (Repository<Account> accounts = new Repository<Account>(DataModelTests._connectionString))
            {
                account = accounts.GetBy(a => a.ID == 1).FirstOrDefault();
            }

            Gallery parent = null;
            using (Repository<Gallery> getParent = new Repository<Gallery>(DataModelTests._connectionString))
            {
                parent = getParent.GetBy(g => g.ID == 13).FirstOrDefault();
            }

            Gallery gallery = new Gallery
            {
                CreatedBy = account,
                Name = "Test Gallery",
                Parent = parent,
                ReducedName = "test-gallery"
            };

            //Act:  ...when added to the Repository...
            using (Repository<Gallery> galleries = new Repository<Gallery>(DataModelTests._connectionString))
            {
                galleries.AddUnique(gallery, g => gallery.ReducedName == g.ReducedName);
            }

            //Assert:  ...a new Gallery is created.
            Assert.AreEqual(-1, gallery.ID);
        }


        /// <summary>
        /// GalleryContainsPhotos tests that a Gallery that
        /// should contain Photos does.
        /// </summary>
        [Test]
        public void GalleryContainsPhotos()
        {
            //Arrange:  Given a Gallery Repository and a Gallery...
            using (Repository<Gallery> galleries = new Repository<Gallery>(DataModelTests._connectionString))
            {
                Gallery gallery = galleries.GetBy(g => g.ID == 14)
                    .Include(g => g.Photos)
                    .FirstOrDefault();

                //Act:  ...when the Gallery Photos are accessed...
                //Assert:  ...Photos are found.
                Assert.AreNotEqual(0, gallery.Photos.Count);
            }
        }

        
        /// <summary>
        /// CanRetrieveActivities tests that the Activities can 
        /// be retrieved from the database.
        /// </summary>
        [Test]
        public void CanRetrieveActivities()
        {
            //Arrange:  Given an Activity repository...
            using (Repository<Activity> activities = new Repository<Activity>(DataModelTests._connectionString))
            {
                //Act:  ...when the Activities are retrieved...
                List<Activity> activityList = activities.GetAll()
                    .Take(10)
                    .OrderByDescending(a => a.ActivityDate)
                    .Include(a => a.CreatedBy)
                    .ToList();

                //Assert:  ...there are items in the List.
                Assert.AreEqual(68, activityList[0].ID);
                Assert.AreEqual(1, activityList[0].CreatedBy.ID);
                Assert.AreEqual(ActivityType.Gallery, activityList[0].ActivityType);
            }
        }
    }
}
