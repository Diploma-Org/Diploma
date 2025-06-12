using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class ClientService : IClientService
{
    private readonly IRepository<Client> _clientRepository;
    public ClientService(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public void AddClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null.");
        if (string.IsNullOrWhiteSpace(client.Name) || string.IsNullOrWhiteSpace(client.Surname) || string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new ArgumentException("Client must have a valid name, surname, and phone number.");
        _clientRepository.Insert(client);
        _clientRepository.Save();
    }

    public void DeleteClient(int id)
    {
        var client = _clientRepository.GetById(id);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID {id} not found.");
        _clientRepository.Delete(client);
        _clientRepository.Save();
    }

    public Client? GetClientById(int id)
    {
        var client = _clientRepository.GetById(id);
        if (client == null)
            throw new KeyNotFoundException($"Client with ID {id} not found.");
        return client;
    }

    public Client? GetClientByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        return _clientRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public Client? GetClientByPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
        
        return _clientRepository.GetAll().FirstOrDefault(c => c.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase));
    }

    public Client? GetClientBySurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Surname cannot be null or empty.", nameof(surname));
        
        return _clientRepository.GetAll().FirstOrDefault(c => c.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Client> GetClients()
    {
        return _clientRepository.GetAll();
    }

    public void UpdateClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null.");
        if (string.IsNullOrWhiteSpace(client.Name) || string.IsNullOrWhiteSpace(client.Surname) || string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new ArgumentException("Client must have a valid name, surname, and phone number.");
        _clientRepository.Update(client);
        _clientRepository.Save();
    }
}