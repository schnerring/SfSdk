﻿using System;
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
                        return new SfResponse(TestConstants.ExistingError, TestConstants.ValidServerUri);
                    // 2013/04/27
                    const string loginResponse = "0021912230455/75298/1367106812/1362182092/837135956/4/0/5/572/2200/360/55499/-1/1204/14/4/5/3/103/103/4/103/3/1/1/1/0/6/1/2/17/21/30/20/24/0/0/0/0/0/0/0/2/0/0/2/2/1365774494/6/1001/3/0/5/2/3/0/0/0/26/0/0/0/0/0/0/0/0/0/0/0/0/0/5/1001/2/0/5/4/3/0/0/0/24/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1/1001/4/14/0/0/0/0/0/0/1/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1365774068/5/5/5/6/4/2/-98/-11/-64/17/11/7/105/420/105/0/1001/9/0/3/1/2/3/0/0/154/0/0/1001/2/0/2/1/4/2/0/0/25/0/0/1001/6/0/1/4/2/4/0/0/154/0/44/774/91/53/51/42/0/1363718552/5/1001/6/0/5/2/3/2/0/0/131/0/7/1001/9/0/1/5/4/3/0/0/141/0/4/1001/7/0/1/2/3/3/0/0/147/0/1/1002/11/33/1/4/3/0/0/0/96/0/4/1001/4/0/5/2/4/0/0/0/45/0/4/1003/21/0/1/4/3/3/0/0/126/1/1362943332/8/4/0/0/1/5/3/3/0/0/75/0/9/7/0/0/2/4/5/2/0/0/50/0/8/6/0/0/2/5/1/2/0/0/50/0/12/4/0/0/11/4/0/72/10/0/71/0/9/1/0/0/3/5/4/3/0/0/37/1/12/5/0/0/11/5/0/72/10/0/71/0/0/1/8601/3/0/0/0/0/0/0/1362946879/0/0/0/5/4/14/0/0/0/0/0/1365763903/5580/0/0/0/0/0/0/0/1362182092/5/0/3/13/0/1425/399/0/1/9/0/0/2/1362776202/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/98807380/98807380/98807380/0/0/0/0/1367106812/;0;8Lxm0i176U00216U74D2p0G38398R33f;0;452;0";
                    return new SfResponse(loginResponse, TestConstants.ValidServerUri);
                case SF.ActLogout:
                    // 2013/05/17
                    const string logoutResponse = "+187";
                    return new SfResponse(logoutResponse, TestConstants.ValidServerUri);
                case SF.ActScreenChar:
                    // 2013/05/17
                    const string respScreenBuildchar = "+0041912230455/75298/1368808222/1362182092/837135956/3/0/6/1196/2890/400/55861/-1/1438/15/5/5/3/103/103/4/103/3/1/1/1/0/6/1/2/17/21/30/20/24/0/0/0/0/0/0/0/2/0/0/0/2/0/6/1001/3/0/5/2/3/0/0/0/26/0/0/0/0/0/0/0/0/0/0/0/0/0/5/1001/2/0/5/4/3/0/0/0/24/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1/1001/4/14/0/0/0/0/0/0/1/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/6/6/6/6/3/4/-56/-45/-35/1/4/6/120/360/120/0/1001/9/0/3/1/2/3/0/0/154/0/0/1002/11/0/1/2/4/5/5/0/196/0/0/7/0/0/4/3/2/2/0/0/50/0/169/612/173/38/72/27/0/1363718552/5/1001/6/0/5/2/3/2/0/0/131/0/7/1001/9/0/1/5/4/3/0/0/141/0/4/1001/7/0/1/2/3/3/0/0/147/0/1/1002/11/33/1/4/3/0/0/0/96/0/4/1001/4/0/5/2/4/0/0/0/45/0/4/1003/21/0/1/4/3/3/0/0/126/1/1368799335/9/4/0/0/4/1/5/8/0/0/225/0/9/5/0/0/5/2/1/2/2/0/75/1/8/4/0/0/1/3/2/2/0/0/25/1/9/7/0/0/3/5/4/6/6/0/225/3/9/7/0/0/1/2/4/3/0/0/75/0/9/5/0/0/5/4/3/2/0/0/50/0/0/0/8601/3/0/0/0/0/0/0/1362946879/0/0/0/5/4/14/0/0/0/0/0/1368795021/4800/0/0/0/0/0/0/0/1362182092/6/0/3/17/0/1659/400/0/1/9/0/0/2/1362776202/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/-1838562723/-1838562723/-1838562723/0/0/0/0/1368808222;;";
                    return new SfResponse(respScreenBuildchar, TestConstants.ValidServerUri);
                case SF.ActRequestChar:
                    // 2013/05/17
                    const string actRequestChar = "+1110000000000/4296/0/0/0/0/0/231/76364148/110062180/47329/2/-1/0/0/0/0/7/509/507/2/507/5/1/9/1/0/4/257/3/1001/1810/1001/1700/1200/275/4469/275/3251/2261/915/1713/914/1610/1110/0/0/0/6/2050/795/0/2/4/5/290/290/290/9685227/0/3/2057/1596/0/2/4/5/280/280/280/8847416/0/5/2051/1308/0/2/4/5/292/292/292/5241891/0/4/2057/1360/0/2/4/5/286/286/286/15595112/0/8/57/0/0/2/4/5/278/278/278/4381832/0/7/2050/1161/0/2/4/5/283/283/283/24626499/0/9/57/0/0/2/4/5/277/277/277/1711336/0/10/54/0/0/6/0/0/275/0/0/0/0/1/2008/322/898/2/3/4/952/0/0/9985111/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/-42/-55/-130/15/9/6/450/450/300/0/2008/422/782/2/1/5/510/422/0/10494642/0/8/4/0/0/1/5/3/253/209/0/3147157/0/0/2009/1295/0/3/1/2/488/0/0/6772186/0/1016200/516100/505400/10839800/10243100/9458500/2621444/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/8/3/0/11669/0/0/12/12/0/32/0/0/6220/322/898/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/231/100/4677/5203/1529/1000000000/47659/0/1/77/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/123/11/150/14/12/16/1370299355/1369802949/1372446217/25/25/25/25/0/0/0/0/0/0/0/1368809900;Der erste lvl 100 am 23.02.13 um 16:39##geb: 08.02.2013     18:00 Uhr#s1##;Shades of Night";
                    return new SfResponse(actRequestChar, TestConstants.ValidServerUri);
                case SF.ActScreenEhrenhalle:
                    // 2013/05/17
                    const string actScreenEhrenhalle = "+007-1/Hadrians S25/Shades of Night/231/47970/-2/brigada00/Shades of Night/231/47329/-3/seelenjäger/Insane/219/46668/-4/tofo/Shades of Night/224/46381/-5/VerführunG/Shades of Night/223/46293/-6/Pad/Shades of Night/221/46259/-7/Tenegros/Shades of Night/227/46228/-8/Xayne/Shades of Night/224/46186/-9/Araton/Shades of Night/227/46125/10/Blutseele/Insane/-221/46109/-11/Pizza/Insane/219/46107/-12/Susanno/Tigers and Dragons/221/45993/-13/MissGeschick/Insane/221/45967/14/Delphi/Shades of Night/-220/45946/-15/Pfledex/100 Fäuste/213/45945/;";
                    return new SfResponse(actScreenEhrenhalle, TestConstants.ValidServerUri);
                case SF.ActAlbum:
                    // 2014/05/19
                    // monsters     36/ 252
                    // valuables    18/ 246
                    // warrior      19/ 506
                    // mage         20/ 348
                    // scout        10/ 348
                    // --------------------
                    // total       103/1700
                    const string actAlbum = "192wOBBQAKgSYIELIQEBACJdE8AAAAAAAAAAAAAAAAAAAAAAAAAAAACAEhAIwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgRAGlAAAAAAAAAAAAAAAAAAAAAAAAIoAAAAAAAAAAAAAAAAAAgABAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAyAAAAAAAAAAAAAAAACEgAAAAAAAAAAAAAAAAAAAEQAAAAAAAAAAAAAAABYAAAAAAAAAAAAAAAAAAAYAAAAAAAAAAAAAAAAIEIAAAAAAAAAAAAAAAAAQsAAAAAAAAAAAAAAAAAAIIIAAAAAAAAAAAAAAAAIMAIAAAAAAAAAAAAAAAoAAAAAAAAAAAAAAAAAAEjAEAAAAAAAAAAAAAAAAKAAAAAAAAAAAAAAAAAAAEAIAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAAAAAAEAIAAAAAAAAAAAAAABAAACAAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAAAEBAAAAAAAAAAAAAAAAAAA==";
                    return new SfResponse(actAlbum, TestConstants.ValidServerUri);
                default:
                    throw new NotImplementedException(action.ToString());
            }
        }
    }
}