using System.Threading.Tasks;
using HSNXT.Unirest.Net.Http;

namespace HSNXT.Unirest.Net.Unirest
{
    public delegate Task OnSuccessAsync<in T>(T body);
    public delegate Task OnFailAsync<T>(PartialHttpResponse<T> response);
}