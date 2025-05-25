namespace NaviriaAPI.DTOs.Task.Create
{
    public class TaskWithSubtasksCreateDto : TaskCreateDto
    {
        public TaskWithSubtasksCreateDto()
        {
            Type = "with_subtasks";
        }
    }
}
