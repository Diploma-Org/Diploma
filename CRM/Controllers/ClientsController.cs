using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interfaces;
using DataAccess.Entities;

namespace WebApp.Controllers;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }
    public IActionResult Index()
    {
        var clients = _clientService.GetClients();
        return View(clients);
    }
    public IActionResult AddClient(Client client)
    {
        _clientService.AddClient(client);
        return RedirectToAction("Index");
    }
    public IActionResult DeleteClient(int id)
    {
        _clientService.DeleteClient(id);
        return RedirectToAction("Index");
    }
    public IActionResult EditClient(Client client)
    {
        if (client == null)
            return BadRequest("Client cannot be null.");
        _clientService.UpdateClient(client);
        return RedirectToAction("Index");
    }
}