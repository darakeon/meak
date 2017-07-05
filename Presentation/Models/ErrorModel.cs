using System;

namespace Presentation.Models
{
    public class ErrorModel : BaseModel
    {
        public ErrorModel(BaseModel.Paths paths) : base(paths) { }

        public String Message { get; set; }
    }
}