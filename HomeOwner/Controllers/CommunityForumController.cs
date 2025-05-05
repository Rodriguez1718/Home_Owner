using Microsoft.AspNetCore.Mvc;
using HomeOwner.Data;
using HomeOwner.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HomeOwner.Controllers
{
    public class CommunityForumController : Controller
    {
        private readonly AppDbContext _context;

        public CommunityForumController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var forums = _context.CommunityForums.ToList();
            return View(forums);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommunityForum forum)
        {
            if (ModelState.IsValid)
            {
                _context.CommunityForums.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forum);
        }

        public IActionResult ManagePosts(int id)
        {
            var posts = _context.ForumPosts.Where(p => p.ForumId == id).ToList();
            ViewBag.ForumId = id;
            return View(posts);
        }

        public IActionResult CreatePost(int forumId)
        {
            ViewBag.ForumId = forumId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(ForumPost post)
        {
            if (ModelState.IsValid)
            {
                _context.ForumPosts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManagePosts), new { id = post.ForumId });
            }
            return View(post);
        }

        // Homeowner: View all forums
        public IActionResult Forums()
        {
            var forums = _context.CommunityForums.ToList();
            return View(forums);
        }

        // Homeowner: View posts in a specific forum
        public IActionResult ForumPosts(int id)
        {
            var forum = _context.CommunityForums.FirstOrDefault(f => f.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            var posts = _context.ForumPosts.Where(p => p.ForumId == id).ToList();
            ViewBag.ForumTitle = forum.Title;
            ViewBag.ForumId = id;
            return View(posts);
        }

        // Homeowner: View a specific post and its comments
        public IActionResult PostDetails(int id)
        {
            var post = _context.ForumPosts
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    Post = p,
                    Comments = _context.Comments
                        .Where(c => c.PostId == id)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToList()
                })
                .FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.PostTitle = post.Post.Title;
            ViewBag.PostContent = post.Post.Content;
            ViewBag.PostId = id;

            return View(post.Comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PostDetails), new { id = comment.PostId });
            }

            return RedirectToAction(nameof(PostDetails), new { id = comment.PostId });
        }

        // Homeowner: Add a new post to a forum
        public IActionResult AddPost(int forumId)
        {
            ViewBag.ForumId = forumId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(ForumPost post)
        {
            if (ModelState.IsValid)
            {
                _context.ForumPosts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ForumPosts), new { id = post.ForumId });
            }
            return View(post);
        }
    }
}
