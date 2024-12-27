using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Project.Data;
using Project.Models;

public class ItemsController : Controller
{
    private readonly AppDbContext _db;
    private readonly SignInManager<Users> signInManager;
    private readonly UserManager<Users> userManager;

    public ItemsController(SignInManager<Users> signInManager, UserManager<Users> userManager, AppDbContext db)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        _db = db;
    }

    public IActionResult HomePage()
    {
        var itemList = _db.Items.Include(c => c.Category).ToList();
        return View(itemList);
    }

    [Authorize]
    public IActionResult Index()
    {
        var itemList = _db.Items.Include(c => c.Category).ToList();
        return View(itemList);
    }

    public IActionResult Product(int? Id)
    {
        if (Id != null || Id > 0)
        {
            var item = _db.Items.Include(c => c.Category).FirstOrDefault(c => c.Id == Id.Value);
            return View(item);
        }
        else
        {
            return RedirectToAction("HomePage", "Items");
        }
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Item Model)
    {
        if (ModelState.IsValid)
        {
            if (Model.clientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                Model.clientFile.CopyTo(stream);
                Model.dbImage = stream.ToArray();
            }

            _db.Items.Add(Model);
            _db.SaveChanges();
            return RedirectToAction("Index", "Items");
        }
        else
        {
            return View(Model);
        }
    }

    [Authorize]
    public IActionResult Edit(int Id)
    {
        if (Id <= 0)
        {
            return NotFound();
        }

        var ItemList = _db.Items.Find(Id);

        if (ItemList == null)
        {
            return NotFound();
        }
        return View(ItemList);
    }


    [HttpPost]
    public IActionResult Edit(Item Model)
    {
        if (ModelState.IsValid)
        {
            if (Model.clientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                Model.clientFile.CopyTo(stream);
                Model.dbImage = stream.ToArray();
            }
            _db.Items.Update(Model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View(Model);
        }

    }

    [Authorize]
    public IActionResult Delete(int Id)
    {
        if (Id <= 0)
        {
            return NotFound();
        }
        var ItemList = _db.Items.Find(Id);
        if (ItemList == null)
        {
            return NotFound();
        }

        return View(ItemList);
    }


    [HttpPost]
    public IActionResult Delete(Item Model)
    {
        if (ModelState.IsValid)
        {
            if (Model.clientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                Model.clientFile.CopyTo(stream);
                Model.dbImage = stream.ToArray();
            }
            _db.Items.Remove(Model);
            _db.SaveChanges();
            return RedirectToAction("Index", "Items");
        }

        return View(Model);
    }

    [Authorize]
    public async Task<IActionResult> Cart()
    {
        var user = await userManager.GetUserAsync(User);
        var userId = user.UserName;
        var cartItems = _db.Carts
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Items)
            .ToList();

        ViewBag.Total = cartItems.Sum(ci => ci.Quantity * ci.Items.Price);

        return View(cartItems);
    }

    [Authorize]
    public async Task<IActionResult> AddToCart(int itemId, int quantity)
    {
        var user = await userManager.GetUserAsync(User);
        var userId = user.UserName;
        var existingCartItem = _db.Carts
            .FirstOrDefault(ci => ci.ItemId == itemId && ci.UserId == userId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            var cartItem = new Cart
            {
                ItemId = itemId,
                Quantity = quantity,
                UserId = userId
            };
            _db.Carts.Add(cartItem);
        }

        _db.SaveChanges();
        return RedirectToAction("Cart");
    }

    [Authorize]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var user = await userManager.GetUserAsync(User);
        var userId = user.UserName;
        var cartItem = _db.Carts
            .FirstOrDefault(ci => ci.Id == id && ci.UserId == userId);

        if (cartItem != null)
        {
            _db.Carts.Remove(cartItem);
            _db.SaveChanges();
        }

        return RedirectToAction("Cart");
    }

    public IActionResult Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return View(new List<Item>());
        }
        var searchResults = _db.Items
            .Where(i => i.Category.Name.Contains(query) || i.Name.Contains(query) || i.Description.Contains(query))
            .ToList();
        ViewBag.Message = $"{searchResults.Count} results are listed for '{query}' search.";
        return View(searchResults);
    }

}