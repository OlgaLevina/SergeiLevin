using System.Collections.Generic;
using System.Security.Claims;

namespace SergeiLevin0.Domain.DTO.Identity
{
    public abstract class ClaimDTO: UserDTO //модель для хранения утверждений, с которыми ассоциируется пользователь, например, что можно установить, что он администратор без ассоциации с ролью администратора. С помощью этого можно организовать регистрацию на сайте с помощью сторонних поставщиков регистрации
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
    public class AddClaimDTO : ClaimDTO { }//модель для добавления клэймов

    public class RemoveClaimDTO : ClaimDTO { }

    public class ReplaceClaimDTO : UserDTO
    {
        public Claim Claim { get; set; }
        public Claim NewClaim { get; set; }
    }

}
