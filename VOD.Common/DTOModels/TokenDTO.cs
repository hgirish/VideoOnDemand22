using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Common.DTOModels
{
    public class TokenDTO
    {
        public string Token { get; set; } = "";
        public DateTime TokenExpires { get; set; } = default;
        public TokenDTO()
        {

        }
        public TokenDTO(string token, DateTime expires)
        {
            Token = token;
            TokenExpires = expires;
        }
        public bool TokenHasExpired
        {
            get
            {
                return TokenExpires == default ? true :
                    !(TokenExpires.Subtract(DateTime.UtcNow).Minutes > 0);
            }
        }
    }
}
