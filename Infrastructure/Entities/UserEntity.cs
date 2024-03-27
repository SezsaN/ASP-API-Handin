﻿namespace Infrastructure.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<SavedCoursesEntity> SavedCourses { get; set; } = new List<SavedCoursesEntity>();
}