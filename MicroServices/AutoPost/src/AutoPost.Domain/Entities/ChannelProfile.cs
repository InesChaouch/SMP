using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.ChannelEntities;
using SMP.Domain.Entities.PostEntities;

namespace SMP.Domain.Entities
{
    public class ChannelProfile
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Headline { get; set; }
        public required string Description { get; set; }
        public required string CoverPhoto { get; set; }
        public  required string Photo { get; set; }
        public required string ProfileId { get; set; }
        public required string ProfileLink { get; set; }

        public string ChannelName { get; set; } = null!;
        public Channel Channel { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        //better add this to dto   
        //public required string AccessToken { get; set; }
        //public DateTime ExpiresIn { get; set; }
        //public required string RefreshToken { get; set; }
        //public DateTime RefreshTokenExpiresIn { get; set; }
    }
}
