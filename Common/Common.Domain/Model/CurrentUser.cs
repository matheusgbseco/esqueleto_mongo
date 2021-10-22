using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Domain.Model
{

    public class CurrentUser
    {

        private string _token;

        public void Init(string token, Dictionary<string, object> claims)
        {
            this._token = token;
            this._claims = claims;
        }

        private Dictionary<string, object> _claims;

        public string GetToken()
        {
            return this._token;
        }

        public Dictionary<string, object> GetClaims()
        {
            return this._claims;
        }

        private TS GetUserId<TS>()
        {
            if (this._claims.IsNotAny())
                return default;

            var subClaims = this._claims
                .Where(_ => _.Key.ToLower() == "sub");

            if (subClaims.IsAny())
            {
                var subjectId = subClaims
                    .SingleOrDefault()
                    .Value;

                return (TS)Convert.ChangeType(subjectId, typeof(TS));
            }

            return default;
        }

        public Guid GetUserId()
        {
            return this.GetUserId<Guid>();
        }



    }
}
