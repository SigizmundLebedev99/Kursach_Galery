using AutoMapper;
using Dapper;
using Galery.Server.DAL;
using Galery.Server.DAL.Models;
using Galery.Server.Service.DTO.PictureDTO;
using Galery.Server.Service.Exceptions;
using Galery.Server.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    public class TagService : ITagService
    {
        readonly IUnitOfWork uow;
        readonly string _connectionString;
        readonly DbProviderFactory _factory;
        readonly IMapper _mapper;

        public TagService(IUnitOfWork uow,
            DbProviderFactory factory,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.uow = uow;
            _factory = factory;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }

        public async Task<Tag> CreateTag(string name)
        {
            try
            {
                var tag = new Tag { DateOfCreation = DateTime.Now, Name = name };
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    return await uow.Tags.CreateAsync(connection, tag);
                }
            }
            catch(Exception ex)
            {
                throw new DatabaseException("Не удалось удалить данные", ex.Message);
            }
        }

        public async Task DeleteTag(int id)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    var entity = uow.Pictures.FindByIdAsync(connection, id);
                    if (entity == null)
                        throw new NotFoundException($"Не удалось найти жанр с id = {id}");
                    await uow.Tags.DeleteAsync(connection, id);
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось добавить данные", ex.Message);
            }
        }

        public async Task<IEnumerable<TagDTO>> GetAllTags()
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                    return await connection.QueryAsync<TagDTO>("GetTagsWithPicCount", null, null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Не удалось извлечь данные", ex.Message);
            }
        }

        public Task<Tag> UpdateTag(int id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
