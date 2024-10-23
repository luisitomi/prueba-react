using Backend.Infraestructure.Repository.UserRepository;
using System;

namespace Backend.Infraestructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        void Commit();
        void RollBack();
    }
}
