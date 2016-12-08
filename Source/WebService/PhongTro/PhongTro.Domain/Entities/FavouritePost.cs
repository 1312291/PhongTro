using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhongTro.Domain.Entities
{
    public class FavouritePost
    {
        public Guid FavouritePostID { get; set; }
        public Guid PhongTroUserID { get; set; }
        public Guid PostID { get; set; }

        public virtual PhongTroUser PhongTroUser { get; set; }
        public virtual Post Post { get; set; }
    }
}
