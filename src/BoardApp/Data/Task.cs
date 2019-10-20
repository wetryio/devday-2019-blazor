﻿namespace BoardApp.Data
{
    public enum Status
    {
        TODO,
        INPROGRESS,
        DONE
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}
