using DataAccess.Entities;
namespace BusinessLogic.Interfaces;

public interface IClientService
{
    IEnumerable<Client> GetClients();
    Client? GetClientById(int id);
    Client? GetClientByPhoneNumber(string phoneNumber);
    Client? GetClientByName(string name);
    Client? GetClientBySurname(string surname); 
    void AddClient(Client client);
    void UpdateClient(Client client);
    void DeleteClient(int id);
}