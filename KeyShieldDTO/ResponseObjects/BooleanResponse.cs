using System;

namespace KeyShieldDTO.ResponseObjects
{
    public class BooleanResponse
    {
        public bool Value { get; set; }
        
        public BooleanResponse() { }
        
        public BooleanResponse(bool value)
        {
            Value = value;
        }
    }
}
