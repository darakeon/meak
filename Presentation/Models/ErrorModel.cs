using System;
using Structure.Data;

namespace Presentation.Models
{
    public class ErrorModel : BaseModel
    {
        public ErrorModel(Paths paths) : base(paths) { }

        public String Message { get; set; }
    }
}