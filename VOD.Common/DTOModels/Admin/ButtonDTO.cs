using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Common.DTOModels.Admin
{
    public class ButtonDTO
    {
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ItemId { get { return Id > 0 ? Id.ToString() : UserId; } }

        public ButtonDTO(int courseId, int moduleId, int id)
        {
            CourseId = courseId;
            ModuleId = moduleId;
            Id = id;
        }
        public ButtonDTO(int courseId, int id)
        {
            CourseId = courseId;
            Id = id;
        }
        public ButtonDTO(int id)
        {
            Id = id;
        }
        public ButtonDTO(string userId)
        {
            UserId = userId;
        }
    }
}
