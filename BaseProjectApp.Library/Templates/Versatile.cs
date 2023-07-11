using BaseProjectApp.Library.Templates.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates
{
    public class TextVarDto
    {
        public string? Data { get; set; }
        public string? Link { get; set; }
    }
    public class ListModelT
    {
        public int id { get; set; }
        public string? title { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

  
    public class ListItemN
    {
        public int Id { get; set; }
        public string? Value { get; set; }
    }
    public class ListItemN_Parent
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? Value { get; set; }
    }
    public class ListItemNN
    {
        public int? Id { get; set; }
        public string? Value { get; set; }
    }

    public class ListItemNUser
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
    }

    public class ListItemN_Cl
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public int? CLookupId { get; set; }
    }

    public class SaveFileResponse
    {
        public string Caption { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }


    public class CustomJWTClaimTypes
    {
        public const string Admin = "Admin";
        public const string UserID = "UserId";
        public const string Email = "UserEmail";
        public const string ClientId = "ClientId";
        public const string UserType = "usertype";
        public const string Permessions = "Permessions";
        public const string BusinessActivityId = "BusinessActivityId";
    }



    public class APIResponse<T>
    {
        public T content { get; set; }
        public bool Succeeded { get; set; }
        public string message { get; set; }
        public string code { get; set; }

        public static APIResponse<T> Fail(string errorMessage, T data)
        {
            return new APIResponse<T> { content = data, Succeeded = false, message = errorMessage };
        }
        public static APIResponse<T> Success(T data)
        {
            return new APIResponse<T> { Succeeded = true, content = data };
        }

        public static APIResponse<T> SuccessWithMessage(T data, string msg)
        {
            return new APIResponse<T> { Succeeded = true, content = data, message = msg };
        }
        public static APIResponse<T> NotFound(bool english)
        {
            return new APIResponse<T> { Succeeded = false, message = english ? "not found.." : "المعلومات غير متوفرة.." };
        }

        public static APIResponse<T> ServerError(bool english)
        {
            return new APIResponse<T> { Succeeded = false, message = english ? "Internal Server Error.." : "خطأ في العملية.." };
        }

        public static object? Success(ICollection<LookUpValueDTO> lookUpValueDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
