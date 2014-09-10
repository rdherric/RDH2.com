using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;
using RDH2.Web.UI.Models;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// PostController is the controlling entity between the
    /// Post entities and the User.
    /// </summary>
    public class PostController : Controller
    {
        #region Constants
        private const Int32 _postCount = 10;
        #endregion


        #region Member Variables
        private readonly Repository<Post> _posts = null;
        private readonly Repository<PostCount> _postCounts = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default constructor for the PostController object.
        /// </summary>
        public PostController()
        {
            //Save the input in the member variables
            this._posts = new Repository<Post>();
            this._postCounts = new Repository<PostCount>();
        }
        #endregion


        #region MVC View Methods
        /// <summary>
        /// Index shows the base page of the Posts.
        /// </summary>
        /// <returns>ActionResult of Post data</returns>
        public ActionResult Index()
        {
            //Get the list of Posts
            List<Post> posts = this._posts
                .GetAll()
                .Where(this.GetPublishedLambda())
                .OrderByDescending(p => p.Created)
                .Take(PostController._postCount)
                .ToList();

            //Return the list of all Posts
            return View("Index", this.GetPostViewModel(posts));
        }


        /// <summary>
        /// Show shows Post data based on the input that 
        /// came to the method.
        /// </summary>
        /// <param name="input">The reduced title of the Post to show to the User</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Show(String input)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Try to get the Post by title
            rtn = this.GetPostByTitle(input);

            //If the post wasn't returned by title, this might
            //be a tag, so try to get the posts by tag
            if (rtn == null)
            {
                rtn = this.GetPostsByTag(input);
            }

            //If the tag wasn't valid, this might be a date
            //range, so try to get the posts by date
            if (rtn == null)
            {
                rtn = this.GetPostsByDate(input);
            }

            //If the return still is not valid, just return the
            //Index page
            if (rtn == null)
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region MVC Edit Methods
        /// <summary>
        /// Create allows a User to create a new Post.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Create(Account account)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Return the base View with a new Post if the
            //User is logged in.  Otherwise, show the Index.
            if (account != null)
            {
                rtn = this.View("EditPost", new Post());
            }
            else
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Edit allows a User to edit an existing Post.
        /// </summary>
        /// <param name="id">The ID of the Post to edit</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Edit(Account account, Int32 id)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the existing Post
            Post post = this._posts
                .GetBy(p => p.ID == id)
                .FirstOrDefault();

            //If the Post came back and the User is logged in, allow
            //the edit.  Otherwise, go to the Index.
            if (post != null && account != null)
            {
                rtn = this.View("EditPost", post);
            }
            else
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Save saves a Post in the data store.
        /// </summary>
        /// <param name="account">The Account used to create the Post</param>
        /// <param name="post">The Post that was created</param>
        /// <returns>JSON of success data</returns>
        [ValidateInput(false)]
        public JsonResult Save(Account account, Post post)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //If the Account is valid, try to save the data
            if (account != null)
            {
                //Setup the Post
                post.Body = HttpUtility.HtmlDecode(post.Body);
                post.CreatedBy = account;
                post.Modified = DateTime.Now;
                post.ReducedTitle = post.Title.Reduce();

                //Try to save the Post to the data store
                try
                {
                    if (post.ID == -1)
                    {
                        post = this._posts.AddUnique(post, p => post.ReducedTitle == p.ReducedTitle);
                    }
                    else
                    {
                        //Get the Post from the Repository 
                        Post toUpdate = this._posts
                            .GetBy(p => p.ID == post.ID)
                            .FirstOrDefault();

                        //Set the properties on the Post
                        toUpdate.Body = post.Body;
                        toUpdate.Modified = post.Modified;
                        toUpdate.Published = post.Published;
                        toUpdate.ReducedTitle = post.ReducedTitle;
                        toUpdate.Tags = post.Tags;
                        toUpdate.Title = post.Title;

                        //Update the Post
                        post = this._posts.Update(toUpdate);
                    }
                }
                catch (Exception e)
                {
                    rtn = this.Json(new { success = false, message = e.Message });
                }

                //If the Post was saved properly, return a success message
                if (post.ID > 0)
                {
                    rtn = this.Json(new 
                        { 
                            success = true, 
                            id = post.ID, 
                            url = this.Url.Action("Show", new { input = post.ReducedTitle })
                        });
                }
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Delete deletes a Post from the data store.
        /// </summary>
        /// <param name="account">The Account of the person deleting the data</param>
        /// <param name="ID">The Post to delete</param>
        /// <returns>JsonResult with result data in it</returns>
        public JsonResult Delete(Account account, Post post)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //Get the Post by ID
            Post toDelete = this._posts
                .GetBy(p => p.ID == post.ID)
                .FirstOrDefault();

            //If the post comes back, delete it
            if (account != null && post != null)
            {
                //Delete the Post
                this._posts.Delete(toDelete);

                //Setup the JSON
                rtn = this.Json(new { success = true, url = this.Url.Action("Index") });
            }
            else
            {
                //Setup the JSON 
                rtn = this.Json(new { success = false });
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// GetPostsByTitle retrieves the Post by the reduced
        /// title that is passed in.
        /// </summary>
        /// <param name="input">The reduced title of the Post</param>
        /// <returns>ActionResult with View data if successful, null otherwise</returns>
        private ActionResult GetPostByTitle(String input)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the Post from the data store
            Post post = this._posts
                .GetBy(p => p.ReducedTitle == input)
                .Where(this.GetPublishedLambda())
                .FirstOrDefault();

            //If the Post came back successfully, show it.  Otherwise,
            //just return to the home page
            if (post != null)
            {
                //Return the View to show the Post
                rtn = this.View("Show", this.GetPostViewModel(new List<Post> { post }));
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// GetPostsByTag gets a List of Posts based on the 
        /// Tag that is passed in.
        /// </summary>
        /// <param name="input">The tag to use to search for Posts</param>
        /// <returns>ActionResult with View data if successful, null otherwise</returns>
        private ActionResult GetPostsByTag(String input)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Try to get the Posts by tag
            List<Post> posts = this._posts
                .GetBy(p => p.Tags.Contains(input) == true)
                .Where(this.GetPublishedLambda())
                .OrderByDescending(p => p.Created)
                .ToList();

            //If the List has items in it, return a View
            if (posts.Count > 0)
            {
                //Get the PostViewModel
                PostViewModel vm = this.GetPostViewModel(posts);
                vm.CurrentTag = input;

                //Create the View
                rtn = this.View("Index", vm);
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// GetPostsByDate gets a List of Posts based on the 
        /// date that was passed in.
        /// </summary>
        /// <param name="input">The String representation of a month</param>
        /// <returns>ActionResult with View data if successful, null otherwise</returns>
        private ActionResult GetPostsByDate(String input)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //If the input is valid, try to use it
            if (String.IsNullOrEmpty(input) == false)
            {
                //Try to turn the input into a month and year
                Int32 month = Int32.MinValue;
                Int32 year = Int32.MinValue;

                //Split the String on the hyphen
                String[] nums = input.Split('-');

                //If there are sufficient values, parse them
                if (nums.GetLength(0) == 2)
                {
                    Int32.TryParse(nums[0], out month);
                    Int32.TryParse(nums[1], out year);
                }

                //If the values are valid, try to get the Posts
                if (month != Int32.MinValue && year != Int32.MinValue)
                {
                    //Get the List of Posts
                    List<Post> posts = this._posts
                        .GetBy(p => p.Created.Month == month && p.Created.Year == year)
                        .Where(this.GetPublishedLambda())
                        .OrderByDescending(p => p.Created)
                        .ToList();

                    //If there are any Posts, create an ActionResult
                    if (posts.Count > 0)
                    {
                        rtn = this.View("Index", this.GetPostViewModel(posts));
                    }
                }
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// GetPostViewModel gets a PostViewModel with the 
        /// specified Posts in it.
        /// </summary>
        /// <param name="posts">The List of Posts to add to the Model</param>
        /// <returns>PostViewModel with data in it</returns>
        private PostViewModel GetPostViewModel(List<Post> posts)
        {
            //Create a View Model with Post data
            return new PostViewModel
            {
                Tags = this.GetTagCloudList(),
                Posts = posts,
                PostCounts = this._postCounts.GetAll().OrderByDescending(pc => pc.Bucket).ToList()
            };
        }


        /// <summary>
        /// GetTagCloudList retrieves the List of Tags that have
        /// been applied to the Posts.
        /// </summary>
        /// <returns>List of Tags on the Posts</returns>
        private List<String> GetTagCloudList()
        {
            //Declare a variable to return
            List<String> rtn = new List<String>();
            
            //Get the list of Posts
            List<Post> posts = this._posts.GetAll().ToList();

            //Add the Lists to the return
            foreach (Post post in posts)
            {
                rtn.AddRange(post.TagList);
            }

            //Return the result
            return rtn.Distinct().OrderBy(s => s).ToList();
        }


        /// <summary>
        /// GetPublishedLambda retrieves a lambda expression 
        /// that will determine if the unpublished items will
        /// be shown.
        /// </summary>
        /// <returns>Lambda of Published logic</returns>
        private Func<Post, Boolean> GetPublishedLambda()
        {
            //Declare a Func to return
            Func<Post, Boolean> rtn = (p => p.Published == true);

            //If the User is logged in, show all posts
            if (AccountModelBinder.Current != null)
            {
                rtn = (p => true);
            }

            //Return the result
            return rtn;
        }
        #endregion
    }
}
