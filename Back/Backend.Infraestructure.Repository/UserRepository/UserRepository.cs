using Backend.CrossCuting.Helpers;
using Backend.CrossCuting.Helpers.Enum;
using Backend.Domain.Entities.Entities.User;
using Backend.Domain.Entities.Util;
using Backend.Infraestructure.Repository.Repository;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Backend.Infraestructure.Repository.UserRepository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly AppConfiguration _configuration;
        public UserRepository(IDbTransaction transaction) : base(transaction)
        {
            _configuration = new AppConfiguration();
        }

        public async Task<Return> Add(UserModel request)
        {
            var response = new Return()
            {
                Data = null,
                Message = request.Id > 0 ? ReturnMessage.PatchError : ReturnMessage.PostError,
                Valid = false,
            };

            IDbConnection connection = Connection;
            {
                DynamicParameters parameters = new();
                var sql = @"[procesar_usuario_add]";
                if (request.Id > 0)
                {
                    sql = @"[procesar_usuario_edit]";
                    parameters.Add("@Id", request.Id, DbType.Int32, ParameterDirection.Input);
                }
                parameters.Add("@Nombres", request.Nombres, DbType.String, ParameterDirection.Input);
                parameters.Add("@Apellidos", request.Apellidos, DbType.String, ParameterDirection.Input);
                parameters.Add("@User", request.User, DbType.String, ParameterDirection.Input);
                parameters.Add("@Password", request.Password, DbType.String, ParameterDirection.Input);
                parameters.Add("@Success", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync(sql, parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);

                if (parameters.Get<int>("@Success") == 1)
                {
                    response.Data = ReturnMessage.Ok;
                    response.Message = request.Id > 0 ? ReturnMessage.Patch : ReturnMessage.Post;
                    response.Valid = true;
                }
            }

            return response;
        }

        public async Task<Return> Delete(Entity request)
        {
            var response = new Return()
            {
                Data = null,
                Message = ReturnMessage.DeleteError,
                Valid = false,
            };

            IDbConnection connection = Connection;
            {
                DynamicParameters parameters = new();
                var sql = @"[procesar_usuario_delete]";
                parameters.Add("@Id", request.Id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Success", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync(sql, parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);

                if (parameters.Get<int>("@Success") == 1)
                {
                    response.Data = ReturnMessage.Ok;
                    response.Message = ReturnMessage.Delete;
                    response.Valid = true;
                }
            }

            return response;
        }

        public async Task<Return> Login(UserDTO request)
        {
            var response = new Return()
            {
                Data = null,
                Message = ReturnMessage.OkError,
                Valid = false,
            };

            using IDbConnection con = Connection;
            DynamicParameters parameters = new();
            parameters.Add("@user", request.User, DbType.String, ParameterDirection.Input);
            parameters.Add("@password", request.Password, DbType.String, ParameterDirection.Input);
            var result = (List<UserModel>)await con.QueryAsync<UserModel>(@"[validar_usuarios]", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);

            if (result.Count == 0)
            {
                response.Message = ReturnMessage.NoUser;
                return response;
            }

            string jwtToken = GenerateJwtToken(result[0]);

            response.Data = jwtToken;
            response.Message = ReturnMessage.Ok;
            response.Valid = true;

            return response;
        }

        public async Task<Return> Search(UserSearchDTO request)
        {
            var response = new Return()
            {
                Data = null,
                Message = ReturnMessage.OkError,
                Valid = false,
            };

            using IDbConnection con = Connection;
            DynamicParameters parameters = new();
            parameters.Add("@search", request.Search, DbType.String, ParameterDirection.Input);
            var result = (List<UserModel>)await con.QueryAsync<UserModel>(@"[consultar_usuarios]", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);

            UserPagination userPagination = new UserPagination()
            {
                TotalElementos = result.Count,
                Users = result.Skip(request.Page).Take(request.Total).ToList(),
            };

            response.Data = userPagination;
            response.Message = ReturnMessage.Ok;
            response.Valid = true;


            return response;
        }

        private string GenerateJwtToken(UserModel tokens)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_configuration._TokenKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", tokens.Id.ToString()),
                    new Claim("User", tokens.User),
                    new Claim("Apellidos", tokens.Apellidos),
                    new Claim("Nombres", tokens.Nombres),
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration._AppName,
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
