using System;
using System.Data.Entity;
using System.Web;
using RDH2.Web.DataModel.Configuration;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Context
{
    /// <summary>
    /// ModelContext is the Context for the Model that 
    /// contains the DbSets of the associated data classes
    /// and performs the configuration.
    /// </summary>
    public class ModelContext : DbContext
    {
        #region Constants
        private const String _sessionKey = "_RDH2_ModelContext";
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor for the ModelContext object.
        /// </summary>
        public ModelContext() : base("RDH2Web") 
        {
            //Setup the database so that it doesn't migrate
            Database.SetInitializer<ModelContext>(null);
        }


        /// <summary>
        /// Constructor that takes a connection String to
        /// initialize the object.
        /// </summary>
        /// <param name="connectionString">The connection string to use to connect to the data store</param>
        public ModelContext(String connectionString) : base(connectionString) 
        {
            //Setup the database so that it doesn't migrate
            Database.SetInitializer<ModelContext>(null);
        }
        #endregion


        #region Public Properties
        /// <summary>
        /// Activities gets and sets the DbSet of Activity items 
        /// from the data store.
        /// </summary>
        public DbSet<Activity> Activities { get; set; }


        /// <summary>
        /// Galleries gets and sets the DbSet of media Galleries
        /// from the data store.
        /// </summary>
        public DbSet<Gallery> Galleries { get; set; }


        /// <summary>
        /// Photos gets and sets the DbSet of Photo items from
        /// the data store.
        /// </summary>
        public DbSet<Photo> Photos { get; set; }


        /// <summary>
        /// Posts gets and sets the DbSet of Posts created from
        /// the data store.
        /// </summary>
        public DbSet<Post> Posts { get; set; }


        /// <summary>
        /// PostCounts gets and sets the DbSet of PostCounts 
        /// in the data store.
        /// </summary>
        public DbSet<PostCount> PostCounts { get; set; }


        /// <summary>
        /// Videos gets and sets the DbSet of Video items from
        /// the data store.
        /// </summary>
        public DbSet<Video> Videos { get; set; }
        #endregion


        #region OnModelCreating Override
        /// <summary>
        /// OnModelCreating is called when the Model is being
        /// created -- calls into the Configuration objects to
        /// do the real work.
        /// </summary>
        /// <param name="modelBuilder">The Builder used to create the Model in memory</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Call the base class event
            base.OnModelCreating(modelBuilder);

            //Create the new Configuration objects
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new ActivityConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new GalleryConfiguration());
            modelBuilder.Configurations.Add(new PhotoConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new PostCountConfiguration());
            modelBuilder.Configurations.Add(new VideoConfiguration());
        }
        #endregion


        #region ModelContext Current Property
        /// <summary>
        /// Current gets the current ModelContext from the Session
        /// variable or creates a new one for the Session.
        /// </summary>
        public static ModelContext Current
        {
            get
            {
                //Declare a variable to return
                ModelContext rtn = null;

                //If the HttpContext is valid, use it
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    //If the Items variable is found, get it
                    if (HttpContext.Current.Items[ModelContext._sessionKey] != null)
                    {
                        //Save the Context variable
                        rtn = HttpContext.Current.Items[ModelContext._sessionKey] as ModelContext;
                    }
                    else
                    {
                        //Create a new ModelContext
                        rtn = new ModelContext();

                        //Set the value in Session
                        ModelContext.Current = rtn;
                    }
                }
                else
                {
                    //Create a new ModelContext
                    rtn = new ModelContext();
                }

                //Return the result
                return rtn;
            }
            set 
            {
                //If the HttpContext is valid, use it
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    //Save the Context variable
                    HttpContext.Current.Items[ModelContext._sessionKey] = value;
                }
            }
        }
        #endregion
    }
}
