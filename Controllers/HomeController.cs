using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FindARoomate.Models;
using FindARoomate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyMvcApp.Data;
using FindARoomate.Areas.Identity.Pages.Account;

namespace FindARoomate.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context1;
    private readonly UserManager<ApplicationUser> _userManager;


    public HomeController(ILogger<HomeController> logger, AppDbContext context, ApplicationDbContext context1, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _context1 = context1;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var postT = _context.posts.Include(p => p.Images).ToList();
        return View(postT);
    }

    [Authorize]
    // Show existing conversation or start a new one
    public async Task<IActionResult> StartConversation(string targetUserId)
    {
        var currentUserId = _userManager.GetUserId(User);

        // Check if conversation exists
        var convT = _context1.conversations
            .Include(c => c.messages)
            .FirstOrDefault(c =>
                (c.User1Id == currentUserId && c.User2Id == targetUserId) ||
                (c.User2Id == currentUserId && c.User1Id == targetUserId)
            );
        if (convT == null)
        {
            var conv = new Conversation
            {
                User1Id = currentUserId,
                User2Id = targetUserId,
                CreatedAt = DateTime.Now,
                messages = new List<Message>(),
            };
            _context1.conversations.Add(conv);
            await _context1.SaveChangesAsync();

            convT = conv;
        }

        return RedirectToAction("Chat", new { id = convT.Id });
    }

    // Show messages in conversation
    public IActionResult Chat(int id)
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

        return RedirectToAction("Chat", new { id = conversationId });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
