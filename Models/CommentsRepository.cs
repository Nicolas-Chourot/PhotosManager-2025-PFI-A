using JSON_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace PhotosManager.Models
{
    public class CommentsRepository : Repository<Comment>
    {
        public override int Add(Comment comment)
        {
            Comment parent = DB.Comments.Get(comment.ParentId);
            if (parent != null)
            {
                string text = parent.Text;
                if (text.Length > 32)
                    text = text.Substring(0, 32) + "...";

            }

            return base.Add(comment);
        }
        public override bool Delete(int Id)
        {
            Comment comment = DB.Comments.Get(Id);
            if (comment == null) return false;

            List<Comment> responses = ToList().Where(c => c.ParentId == Id).ToList();
            foreach (Comment c in responses)
            {
                Delete(c.Id);
            }
            foreach (Like like in comment.Likes.ToList())
            {
                DB.Likes.Delete(like.Id);
            }
            return base.Delete(Id);
        }

        public void DeleteByPhotoId(int Id)
        {
            List<Comment> responses = ToList().Where(c => c.PhotoId == Id).ToList();
            foreach (Comment c in responses)
            {
                Delete(c.Id);
            }
        }
        public void DeleteByUserId(int Id)
        {
            List<Comment> responses = ToList().Where(c => c.OwnerId == Id).ToList();
            foreach (Comment c in responses)
            {
                Delete(c.Id);
            }
        }
    }
}