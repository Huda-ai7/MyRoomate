using FindARoomate.Data;
using FindARoomate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using FindARoomate.Areas.Identity.Pages.Account;
using MyMvcApp.Data;

namespace MyApp.Namespace
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context1;

        public UserController(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context1)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _context1 = context1;
        }
        // GET: UserControllers
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var userPosts = _context.posts
            .Include(p => p.Images)
            .Where(p => p.UserId == userId)
            .ToList();
            return View(userPosts);
        }

        [HttpPost]
        public async Task<IActionResult> Add_Post(CreatePostView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var postT = new Post
            {
                Content = model.Content,
                UserId = _userManager.GetUserId(User),
                UserName = User.Identity.Name,
                Images = new List<PostImage>()
            };

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            foreach (var file in model.Images ?? new List<IFormFile>())
            {
                if (file.Length > 0)
                {
                    var uniqeFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsPath, uniqeFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    postT.Images.Add(new PostImage
                    {
                        ImagePath = "/uploads/" + uniqeFileName,
                    });
                }
            }
            _context.posts.Add(postT);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index",model);
        }

        // public async Task<IActionResult> Add_Post_Ajax()
        // {
        //     var content = Request.Form["Content"];
        //     var files = Request.Form.Files;

        //     var post = new Post
        //     {
        //         Content = content,
        //         UserId = User.Identity.Name,
        //         UserName = User.Identity.Name,
        //         Images = new List<PostImage>()
        //     };

        //     var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //     Directory.CreateDirectory(uploadPath);

        //     foreach (var file in files)
        //     {
        //         if (file.Length > 0)
        //         {
        //             var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //             var filePath = Path.Combine(uploadPath, fileName);

        //             using var stream = new FileStream(filePath, FileMode.Create);
        //             await file.CopyToAsync(stream);

        //             post.Images.Add(new PostImage
        //             {
        //                 ImagePath = "/uploads/" + fileName
        //             });
        //         }
        //     }

        //     _context.posts.Add(post);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction("Index");

        //     // return Ok(); // ✅ AJAX will receive 200 OK
        // }


        // public async Task<IActionResult> UpdateImgeList(List<IFormFile> imgList)
        // {

        //     var uploadImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//uploads");
        //     if (!Directory.Exists(uploadImg))
        //         Directory.CreateDirectory(uploadImg);

        //     var imageEntities = new List<ImageList>();
        //     foreach (var file in imgList)
        //     {
        //         var uniqeFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //         var filePath = Path.Combine(uploadImg, uniqeFileName);
        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             file.CopyTo(stream);
        //         }
        //         imageEntities.Add(new ImageList
        //         {
        //             ImagePath = "/uploads/" + uniqeFileName,
        //         });
        //     }

        //     _context.imageLists.AddRange(imageEntities);

        //     _context.SaveChanges();
        //     var imgL = _context.imageLists.ToList();
        //     return View(imgL);
        // }
        public async Task<IActionResult> Delete_Post(int id)
        {
            var postD = await _context.posts.FindAsync(id);
            if (postD != null)
            {
                _context.posts.Remove(postD);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Chat List
        public IActionResult MyConversations()
        {
            var userId = _userManager.GetUserId(User);

            var conv = _context1.conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToList();
            
                return View(conv);
        }

        // Show messages in conversation
        public IActionResult ChatU(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var conversation = _context1.conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.messages)
                .ThenInclude(m => m.Sender)
                .FirstOrDefault(c => c.Id == id &&
                    (c.User1Id == currentUserId || c.User2Id == currentUserId));

            if (conversation == null) return NotFound();

            // Get target user
            var targetUser = conversation.User1Id == currentUserId ? conversation.User2 : conversation.User1;
            // You can pass the target user using ViewBag or a ViewModel
            ViewBag.TargetUserName = targetUser.UserName;
            ViewBag.TargetId = targetUser.Id;

            return View(conversation);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int conversationId, string text)
        {
            var currentUserId = _userManager.GetUserId(User);

            var message = new Message
            {
                ConversationId = conversationId,
                SenderId = currentUserId,
                Text = text,
                SentAt = DateTime.Now
            };

            _context1.messages.Add(message);
            await _context1.SaveChangesAsync();

            return RedirectToAction("ChatU", new { id = conversationId });
        }




    }
}
