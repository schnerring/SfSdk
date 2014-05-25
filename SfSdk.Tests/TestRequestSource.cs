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
                        return new SfResponse(TestConstants.ExistingError, TestConstants.ValidServerUri);
                    // 2013/04/27
                    const string loginResponse = "0021912230455/75298/1367106812/1362182092/837135956/4/0/5/572/2200/360/55499/-1/1204/14/4/5/3/103/103/4/103/3/1/1/1/0/6/1/2/17/21/30/20/24/0/0/0/0/0/0/0/2/0/0/2/2/1365774494/6/1001/3/0/5/2/3/0/0/0/26/0/0/0/0/0/0/0/0/0/0/0/0/0/5/1001/2/0/5/4/3/0/0/0/24/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1/1001/4/14/0/0/0/0/0/0/1/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/1365774068/5/5/5/6/4/2/-98/-11/-64/17/11/7/105/420/105/0/1001/9/0/3/1/2/3/0/0/154/0/0/1001/2/0/2/1/4/2/0/0/25/0/0/1001/6/0/1/4/2/4/0/0/154/0/44/774/91/53/51/42/0/1363718552/5/1001/6/0/5/2/3/2/0/0/131/0/7/1001/9/0/1/5/4/3/0/0/141/0/4/1001/7/0/1/2/3/3/0/0/147/0/1/1002/11/33/1/4/3/0/0/0/96/0/4/1001/4/0/5/2/4/0/0/0/45/0/4/1003/21/0/1/4/3/3/0/0/126/1/1362943332/8/4/0/0/1/5/3/3/0/0/75/0/9/7/0/0/2/4/5/2/0/0/50/0/8/6/0/0/2/5/1/2/0/0/50/0/12/4/0/0/11/4/0/72/10/0/71/0/9/1/0/0/3/5/4/3/0/0/37/1/12/5/0/0/11/5/0/72/10/0/71/0/0/1/8601/3/0/0/0/0/0/0/1362946879/0/0/0/5/4/14/0/0/0/0/0/1365763903/5580/0/0/0/0/0/0/0/1362182092/5/0/3/13/0/1425/399/0/1/9/0/0/2/1362776202/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/98807380/98807380/98807380/0/0/0/0/1367106812/;0;8Lxm0i176U00216U74D2p0G38398R33f;0;452;0";
                    return new SfResponse(loginResponse, TestConstants.ValidServerUri);
                case SF.ActLogout:
                    // 2013/05/17
                    const string logoutResponse = "+187";
                    return new SfResponse(logoutResponse, TestConstants.ValidServerUri);
                case SF.ActScreenChar:
                    // 2014/05/25
                    const string respScreenBuildchar = "0041629341313/64210/1401043089/1400859115/1102867789/0/0/17/13629/17745/1964/10388/-1/71/0/11/36/1/206/207/6/1/8/2/5/1/0/2/2/3/41/129/42/99/84/0/79/0/68/41/6/85/6/61/47/2/2/1401039258/6/2005/95/0/4/3/2/14/0/0/235/0/3/2006/65/0/2/3/4/18/0/0/183/0/5/2003/44/0/5/4/3/4/4/0/129/0/4/2003/66/0/4/2/5/9/9/0/208/0/8/4/0/0/2/5/1/16/0/0/157/0/7/2002/41/0/2/3/5/8/0/0/90/0/9/5/0/0/2/1/3/2/0/0/10/0/0/0/0/0/0/0/0/0/0/0/0/0/1/2051/26/50/2/4/5/26/26/26/6308/0/0/0/0/0/0/0/0/0/0/0/0/0/0/2004/27/49/5/4/3/18/0/0/0/0/0/2002/47/0/4/5/1/2/2/0/0/0/0/4/0/0/2/1/3/2/0/0/0/0/0/2002/28/0/2/4/3/2/2/0/0/0/0/5/0/0/11/5/0/72/10/0/57/0/1401033532/17/17/17/4/3/6/-126/-39/-129/10/12/13/60/60/60/0/2005/32/44/5/4/2/26/18/0/858/0/0/2006/107/0/3/5/4/16/14/0/757/0/0/2003/27/0/4/2/1/8/6/0/163/0/72/138/102/66/26/45/3/1400979454/3/2003/43/0/3/2/4/10/0/0/655/0/7/2005/67/0/2/1/3/9/7/0/902/4/7/2004/55/0/3/4/1/5/3/0/479/2/6/2005/78/0/4/2/1/15/0/0/1387/0/3/2004/54/0/4/2/1/10/10/0/959/5/4/2005/41/0/5/3/4/1/1/0/376/0/1400976025/9/2/0/0/3/5/4/2/0/0/50/0/12/16/0/0/11/12/0/168/25/0/520/15/8/9/0/0/1/5/2/3/3/0/112/1/8/4/0/0/2/3/1/1/1/0/38/0/8/1/0/0/2/1/3/10/10/0/488/5/9/1/0/0/1/3/2/6/6/0/225/3/0/0/1502/3/2/10186/0/0/0/0/1400958006/0/0/0/311/26/50/0/1402130440/0/0/0/1400976123/1170/9/4/1401041958/1401039532/72/54/1/1400946467/17/4/23/118/16/34100/2005/0/0/25/0/0/1/1401038932/3/6/0/0/0/0/0/0/0/0/0/120/0/0/4/5/0/1401122745/1401206209/0/10/10/0/0/1102867789/1102867789/1102867789/1/1401039183/0/80/1401043089;;";
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