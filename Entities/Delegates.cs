using System.Threading.Tasks;

namespace HSNXT.Unirest.Net.Entities
{
    public delegate Task OnSuccessAsync<in T>(T body);
    public delegate Task OnFailAsync<T>(PartialHttpResponse<T> response);
}