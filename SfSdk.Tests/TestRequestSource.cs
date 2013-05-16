using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.Tests
{
    internal class TestRequestSource : IRequestSource
    {
#pragma warning disable 1998
        public async Task<ISfResponse> RequestAsync(string sessionId, SF action, IEnumerable<string> args = null)
#pragma warning restore 1998
        {
            switch (action)
            {
                case SF.ActLogin:
                    if (args == null) throw new ArgumentException("Logging in requires the args: username, passwordHash, serverUrl");
                    var argsList = args.ToList();
                    if (argsList[0] == TestConstants.InvalidUsername || argsList[1] == TestConstants.InvalidPasswordHash)
                        return new SfResponse(TestConstants.ExistingError);

                    // 2013/04/27
                    const string loginResponseString = "0021912230455/75298/1367106812/1362182092/837135956/4/0/5/572/2200/360/55499/-1/1204/14/4/5/3/103/103/4/103/3/1/1/1/0/6/1/2/17/21/30/20/24/0/0/0/0/0/0/0/2/0/0/2/2/1365774494/6/1001/3/0/5/2/3/0/0/0/26/0/0/0/0/0/0/0/0/0/0/0/0/0/5/1001/2/0/5/4/3/0/0/0/24/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1/1001/4/14/0/0/0/0/0/0/1/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1365774068/5/5/5/6/4/2/-98/-11/-64/17/11/7/105/420/105/0/1001/9/0/3/1/2/3/0/0/154/0/0/1001/2/0/2/1/4/2/0/0/25/0/0/1001/6/0/1/4/2/4/0/0/154/0/44/774/91/53/51/42/0/1363718552/5/1001/6/0/5/2/3/2/0/0/131/0/7/1001/9/0/1/5/4/3/0/0/141/0/4/1001/7/0/1/2/3/3/0/0/147/0/1/1002/11/33/1/4/3/0/0/0/96/0/4/1001/4/0/5/2/4/0/0/0/45/0/4/1003/21/0/1/4/3/3/0/0/126/1/1362943332/8/4/0/0/1/5/3/3/0/0/75/0/9/7/0/0/2/4/5/2/0/0/50/0/8/6/0/0/2/5/1/2/0/0/50/0/12/4/0/0/11/4/0/72/10/0/71/0/9/1/0/0/3/5/4/3/0/0/37/1/12/5/0/0/11/5/0/72/10/0/71/0/0/1/8601/3/0/0/0/0/0/0/1362946879/0/0/0/5/4/14/0/0/0/0/0/1365763903/5580/0/0/0/0/0/0/0/1362182092/5/0/3/13/0/1425/399/0/1/9/0/0/2/1362776202/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/98807380/98807380/98807380/0/0/0/0/1367106812/;0;8Lxm0i176U00216U74D2p0G38398R33f;0;452;0";
                    return new SfResponse(loginResponseString);
                case SF.ActLogout:
                default:
                    throw new NotImplementedException(action.ToString());
            }
        }
    }
}