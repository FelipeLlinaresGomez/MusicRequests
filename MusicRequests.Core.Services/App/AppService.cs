
//using MusicRequests.Core.Models;
//using MusicRequests.Core.Services.Dtos;
//using System.Threading.Tasks;

//namespace MusicRequests.Core.Services
//{
//	public class AppService : WebApiBaseService, IAppService
//	{

//        public Task<HomeVersionResponse> GetMinimumVersion()
//        {
//            return this.HttpCall<HomeVersionResponse>("api/home/version",
//                                                      HttpMethodEnum.GET,
//                                                      "1");
//        }

//        public Task<HomeContactosResponse> GetContacto(string ticket, string userCode, string contrato)
//		{
//			return this.SecureHttpCall<HomeContactosResponse>("api/home/contactos",
//			                                                    HttpMethodEnum.GET,
//								                                ticket,
//								                                userCode,
//			                                                   "1",
//			                                                    contrato);
//		}

//        public Task<HomeFaqResponse> GetFaq()
//        {
//            return this.HttpCall<HomeFaqResponse>("api/home/faq",
//                                                   HttpMethodEnum.GET,
//                                                   "1");
//        }

//        public Task<HomeAvisoLegalResponse> GetAvisoLegal()
//        {
//            return this.HttpCall<HomeAvisoLegalResponse>("api/home/avisolegal",
//                                                                 HttpMethodEnum.GET,
//                                                                "1");
//        }

//        public Task<HomeTerminosGdprResponse> GetTerminosGdpr()
//        {
//            string endpoint = $"api/home/terminosGdpr";

//            return this.HttpCall<HomeTerminosGdprResponse>(endpoint,
//                                                           HttpMethodEnum.GET);
//        }
//    }
//}
