using Microsoft.AspNetCore.Mvc;

namespace Quiz.App.ResponseModels
{
    public class DataResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }



        public DataResponseModel(T data, bool success):base(success)
        {
            this.Data = data;
        }


        public DataResponseModel(T data, bool success, string message):base(success, message)
        {
            this.Data = data;
        }



        public DataResponseModel(bool success):base(success)
        {
            this.Data = default;
        }


        public DataResponseModel(bool success, string message):base(success, message)
        {
            this.Data = default;
        }
    }
}